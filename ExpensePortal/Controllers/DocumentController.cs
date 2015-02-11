
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
    public class DocumentController : Controller
    {

        private AuthorizationServiceClient _authorizationService = new AuthorizationServiceClient();
        private DocumentServiceClient _documentService = new DocumentServiceClient();
        private DocumentHelper _documentHelper = new DocumentHelper();
        string fileName;

        public ActionResult Index()
        {
            if (!AuthenticationHelper.IsActiveSession(_authorizationService))
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Overview");
        }

        public ActionResult Documents(string CategoryType)
        {
            if (!AuthenticationHelper.IsActiveSession(_authorizationService))
            {
                // If not logged in this will redirect to Login
                return RedirectToAction("Login", "Account");
            }
            var model = new DocumentViewModel();
            _documentHelper.SetProductNameAndPayrollNumber();
            model.CategoryType = CategoryType;
            return View(model);
        }
        public ActionResult DocumentView(string Name)
        {
            if (!AuthenticationHelper.IsActiveSession(_authorizationService)) return RedirectToAction("Login", "Account");
            string userName = AuthenticationHelper.GetCookieValue(AuthenticationHelper.LoginCookieName, "username");
            var model = new DocumentViewModel();
            var documentDetails = _documentService.GetDocumentDescriptions(DocumentHelper.ProductType, DocumentHelper.EmployeePayrollNumber, DocumentHelper.CategoryList);
            fileName = documentDetails.Where(x => x.FileName == Name).FirstOrDefault().Descriptor;
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            Response.AppendHeader("Content-Disposition", "inline; filename=" + Name);
            return File(fileStream, "application/pdf");
        }


    }
}