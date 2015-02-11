using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpensePortal.ExpensesServiceReference;
using ExpensePortal.DocumentServiceReference;


namespace ExpensePortal.Helpers
{
    public class DocumentHelper
    {
        private  DocumentServiceClient _documentService = new DocumentServiceClient();
        private  BusinessServiceClient _businessService = new BusinessServiceClient(); 
        private AuthorizationServiceClient _authorizationService = new AuthorizationServiceClient();
        public static  string ProductType { get; set; }
        public static int ProductId { get; set; }
        public static  string EmployeePayrollNumber { get; set; }
        public static string[] CategoryList
        {
            get
            {
                string[] _categories = { "Payslip" };//, "Invoices", "WelcomePack", "Miscellaneous" };
                return _categories;
            }
        }

        public  List<DocumentTypeAndCount> DocumentCountAndProductType()
        {
            List<DocumentTypeAndCount> _documentCount = new List<DocumentTypeAndCount>();
            DocumentTypeAndCount documentCount;
            SetProductNameAndPayrollNumber();
            var documentItems = _documentService.GetDocumentDescriptions(ProductType, EmployeePayrollNumber, CategoryList);

            foreach (var item in DocumentHelper.CategoryList)
            {
                documentCount = new DocumentTypeAndCount();
                documentCount.DocumentCount = documentItems.Where(e => e.Category == item).Count();
                documentCount.DocumentType = item;
                _documentCount.Add(documentCount);
            }
            return _documentCount;
        }

        public   void SetProductNameAndPayrollNumber()
        {

            if (AuthenticationHelper.IsActiveSession(_authorizationService))
            {
               
            string userName = AuthenticationHelper.GetCookieValue(AuthenticationHelper.LoginCookieName, "username");
            var userNames = new List<string> { userName };
            List<EmployeeProductData> employeeProducts = _businessService.GetEmployeeProducts(new List<string>(), userNames);
            var employeeProduct = employeeProducts.Single(e => e.Employee.UserName.ToLower() == userName.ToLower());
            var productPayrolls = employeeProduct.ProductPayrolls.ToList();
            var productPayroll = productPayrolls.SingleOrDefault();
              EmployeePayrollNumber = productPayroll.PayrollNumber;
            if (productPayroll.ProductName == "BetterPayments")
            {
                ProductType = "BetterPay";
            }
            else
            {
                ProductType = productPayroll.ProductName;
            }
            ProductId = GetProductId();
            }
        }


        public int GetProductId()
        {
            var allProducts = _businessService.GetProducts();
            string ProductType;
            if (DocumentHelper.ProductType == "BetterPay")
            {
                ProductType = "BetterPayments";
            }
            else
            {
                ProductType = DocumentHelper.ProductType;
            }
            var productsForEmployee =
              from product in allProducts

              where product.Name == ProductType
              orderby product.Name
              select product;

            int ProductIdByProduct = productsForEmployee.FirstOrDefault().ProductID;
            return ProductIdByProduct;
        }


    }

    public class DocumentTypeAndCount
    {
        public string DocumentType { get; set; }
        public int DocumentCount { get; set; }
    }
}