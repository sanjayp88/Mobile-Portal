
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using ExpensePortal.DocumentServiceReference;
using ExpensePortal.ExpensesServiceReference;
using ExpensePortal.Helpers;
using ExpensePortal.Models;
using System.Data;
using System.IO;

namespace ExpensePortal.Controllers
{
    public class ExpenseController : Controller
    {
        private AuthorizationServiceClient _authorizationService = new AuthorizationServiceClient();
        private BusinessServiceClient _businessService = new BusinessServiceClient();
        private DocumentServiceClient _documentService = new DocumentServiceClient();
       
        public ActionResult Index()
        {
            if (!AuthenticationHelper.IsActiveSession(_authorizationService))
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Overview");
        }

        public JsonResult ShowDocument()
        {
             DocumentHelper _documentHelper=new DocumentHelper();
             var _documentCount = _documentHelper.DocumentCountAndProductType();
            return Json(_documentCount, JsonRequestBehavior.AllowGet);

        }
     
        public ActionResult GetProductId()
        {
            int ProductId = DocumentHelper.ProductId;
            return Json(new { ProductIDUser = ProductId }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Products()
        {
            if (!AuthenticationHelper.IsActiveSession(_authorizationService)) return RedirectToAction("Login", "Account");

            string userName = AuthenticationHelper.GetCookieValue(AuthenticationHelper.LoginCookieName, "username");

            var model = new ExpenditureViewModel();

            var userNames = new List<string> { userName };
            var employeeProducts = _businessService.GetEmployeeProducts(new List<string>(), userNames);

            var employeeProduct = employeeProducts.Single(e => e.Employee.UserName.ToLower() == userName.ToLower());

            var productNames = employeeProduct.ProductPayrolls.Select(p => p.ProductName).ToList();

            var allProducts = _businessService.GetProducts();

            var productsForEmployee =
                 from product in allProducts
                 from productName in productNames
                 where product.Name == productName
                 orderby product.Name
                 select product;
        
            
            model.SetProducts(productsForEmployee);

            return View(model);
        }

        // Get
        public ActionResult Overview(int productID = 0)
        {
            if (productID <= 0) return RedirectToAction("Products");

            if (!AuthenticationHelper.IsActiveSession(_authorizationService)) return RedirectToAction("Login", "Account");

            string userNames = AuthenticationHelper.GetCookieValue(AuthenticationHelper.LoginCookieName, "username");
            string productName = _businessService.GetProducts().Single(p => p.ProductID == productID).Name;

            var model =
                new ExpenditureViewModel
                {
                    UserName = userNames,
                    ProductID = productID,

                    Product = productName
                };

            return View(model);
        }

        [HttpPost]
        public ActionResult Overview(ExpenditureViewModel model, string date)
        {
            string userName = model.UserName;
            var userNames = new List<string> { userName };
            List<EmployeeProductData> employeeProducts = _businessService.GetEmployeeProducts(new List<string>(), userNames);

            var employeeProduct = employeeProducts.Single(e => e.Employee.UserName.ToLower() == userName.ToLower());
            var productPayrolls = employeeProduct.ProductPayrolls.ToList();
            string productName = _businessService.GetProducts().Single(p => p.ProductID == model.ProductID).Name;

            var productPayroll = productPayrolls.Single(p => p.ProductName == productName);
            string payrollNumber = productPayroll.PayrollNumber;

            var d = string.IsNullOrEmpty(date) ? DateTime.Now : DateTime.Parse(date);
            int year = d.Year;
            int week = model.GetIso8601WeekOfYear(d);

            RouteValueDictionary routeValues = model.GetRouteValues(payrollNumber, year, week, productName);

            return RedirectToAction("Expenditure", routeValues);
        }

        /// <summary>
        /// Displays record of expenditure for the payroll number, year, week, product combination.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <remarks>
        /// The URL looks like .../Expense/Expenditure/E_123/2014/35/Esentual - see RouteConfig.cs
        /// So, currently authorised user can browse to any week's expenses for any product they are assigned to.
        /// </remarks>
        /// <returns>Expenditure record.</returns>
        public ActionResult Expenditure(ExpenditureViewModel model)
        {
            if (!AuthenticationHelper.IsActiveSession(_businessService, _authorizationService, model.Product, model.PayrollNumber) && !AuthenticationHelper.IsInAdminRole(_authorizationService))
            {
                return RedirectToAction("Login", "Account");
            }

            // temporary hack to fix the issue with having to save the data before validating the data
            if (TempData["tempModel"] != null)
            {
                model = (ExpenditureViewModel)TempData["tempModel"];
            }
            else
            {
                GetExpenditure(model);
            }

            return View(model);
        }

        public ActionResult ConfirmExpenditure(ExpenditureViewModel model)
        {
            model.SetTravelModes(_businessService.GetTravelModes());
            GetEmployeeName(model);
            ViewBag.ModelWithErrors = model;
            return View(model);
        }

        public ActionResult SaveChanges(ExpenditureViewModel model)
        {
            if (!AuthenticationHelper.IsActiveSession(_authorizationService)) return RedirectToAction("Login", "Account");


            // temporary hack to fix the issue with having to save the data before validating the data
            if (TempData["tempModel"] != null)
            {
                model = (ExpenditureViewModel)TempData["tempModel"];
                GetEmployeeName(model);
            }
            else
            {
                GetExpenditure(model);
            }

            var expenditure = model.GetUpdatedExpenses();

            var productPayroll = new ProductPayrollData
            {
                PayrollNumber = model.PayrollNumber,
                ProductName = model.Product
            };

            // the split between Persist and Confirm is probably due to how the code was previously written to save expenses before validating them
            _businessService.PersistExpenditure(expenditure, productPayroll);
            _businessService.ConfirmExpenditure(expenditure, productPayroll);

            return View("Acknowledge", model);
        }

        // Get
        public ActionResult Employees()
        {
            if (!AuthenticationHelper.IsInAdminRole(_authorizationService))
            {
                // If not logged in this will redirect to Login
                return RedirectToAction("Products", "Expense");
            }

            var model = new ExpenditureViewModel();

            var products = _businessService.GetProducts();
            model.SetProducts(products.OrderBy(p => p.Name));

            return View(model);
        }

        [HttpPost]
        public ActionResult Employees(ExpenditureViewModel model)
        {
            if (!AuthenticationHelper.IsInAdminRole(_authorizationService))
            {
                return RedirectToAction("Login", "Account");
            }

            var products = _businessService.GetProducts();
            model.SetProducts(products.OrderBy(p => p.Name));

            var productData = products.SingleOrDefault(p => p.ProductID == model.ProductID);

            bool filterByProduct = productData != null;

            if (filterByProduct)
            {
                model.Product = productData.Name;
            }

            GetEmployeeProductsAndRoles(model);

            return View(model);
        }

        private void GetEmployeeName(ExpenditureViewModel model)
        {
            //sjp - retrieve this from model.Payroll/model.Product
            // get Employee by Payroll/Product
            var employeeData = _businessService.GetEmployeeByPayrollProduct(
                new ProductPayrollData
                {
                    PayrollNumber = model.PayrollNumber,
                    ProductName = model.Product
                });

            model.FirstName = employeeData.FirstName;
            model.LastName = employeeData.LastName;
            model.UserName = employeeData.UserName;

            //string email = string.IsNullOrEmpty(model.Email) ?
            //    AuthenticationHelper.GetCookieValue(AuthenticationHelper.LoginCookieName, "username") :
            //    model.Email;

            //var employeeProducts = _businessService.GetEmployeeProducts(new List<string>(), new List<string> { email });
            //var employee = employeeProducts.First().Employee;
            //model.FirstName = employee.FirstName;
            //model.LastName = employee.LastName;
        }

        private void GetEmployeeProductsAndRoles(ExpenditureViewModel model)
        {
            bool filterByProduct = !string.IsNullOrEmpty(model.Product);

            var productNames = filterByProduct ? new List<string> { model.Product } : new List<string>();

            var employeeProductsAndRoles = _businessService.GetEmployeeProductsAndRoles(productNames)
                .OrderBy(e => e.Employee.LastName)
                .ThenBy(e => e.Employee.FirstName);

            model.SetEmployeeProductsAndRoles(employeeProductsAndRoles);
        }


        private void GetExpenditure(ExpenditureViewModel model)
        {
            model.SetTravelModes(_businessService.GetTravelModes());

            GetEmployeeName(model);

            var productPayrolls = new List<ProductPayrollData> {
                new ProductPayrollData
                {
                    PayrollNumber = model.PayrollNumber,
                    ProductName = model.Product
                }
            };
            int year = model.Year;
            int week = model.Week;

            DateTime monday = model.GetIso8601FirstDateOfWeek(year, week);
            DateTime sunday = monday.AddDays(6);

            model.DateFrom = monday;
            model.DateTo = sunday;

            model.Expenditures = _businessService.GetExpendituresByPayroll(monday, sunday, productPayrolls);

            model.TryGetExpenses();
        }


    }



}