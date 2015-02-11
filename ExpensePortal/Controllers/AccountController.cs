using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpensePortal.ExpensesServiceReference;
using ExpensePortal.Helpers;
using ExpensePortal.Models;

namespace ExpensePortal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private AuthorizationServiceClient _authorizationService;
        private BusinessServiceClient _businessService;

        private void OpenExpensePortal()
        {

            _authorizationService = new AuthorizationServiceClient();
            _businessService = new BusinessServiceClient();
       }



        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if(Request.UrlReferrer != null && Request.UrlReferrer.LocalPath.Contains("ResetPassword"))
            {
                ViewBag.FlashMessage = "Your password has been set. Please login.";
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            OpenExpensePortal();

            if(ModelState.IsValid)
            {
                try
                {
                    if(!IsActiveSession(model.UserName))
                    {
                        LoginImpl(model);
                    }

                    return RedirectToProducts(returnUrl);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            OpenExpensePortal();

            string userName = AuthenticationHelper.GetCookieValue(AuthenticationHelper.LoginCookieName, "username");
            string token = AuthenticationHelper.GetCookieValue(AuthenticationHelper.LoginCookieName, "auth");

            _authorizationService.Logout(userName, token);

            AuthenticationHelper.RemoveCookie(AuthenticationHelper.LoginCookieName);

            return RedirectToAction("Login");
        }

        // Get
        [AllowAnonymous]
        public ActionResult SetPassword(string e, string t)
        {
            OpenExpensePortal();
            var model = new SetPasswordViewModel
            {
                UserName = e,
                Token = t
            };

            GetEmployeeName(model);

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SetPassword(SetPasswordViewModel model)
        {
            OpenExpensePortal();

            if(ModelState.IsValid)
            {
                try
                {
                    _authorizationService.RegisterEmployee(model.UserName, model.Password, model.Token);

                    // TODO: If token is re-used above throws "invalid token" exception) redirect to invalid token page

                    return RedirectToAction("Login");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(model);
        }

        // Get
        [AllowAnonymous]
        public ActionResult ResetPassword(string e, string t)
        {
            OpenExpensePortal();
            var model = new SetPasswordViewModel();

            string userName = e;
            string token = t;

            model.UserName = userName;
            model.Token = token;

            GetEmployeeName(model);

            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(SetPasswordViewModel model)
        {
            OpenExpensePortal();

            if(ModelState.IsValid)
            {
                try
                {
                    _authorizationService.ResetEmployeePassword(model.UserName, model.Password, model.Token);

                    // TODO: If token is re-used above throws "invalid token" exception) redirect to invalid token page

                    return RedirectToAction("Login");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(model);
        }

        private void GetEmployeeName(SetPasswordViewModel model)
        {
            var name = GetEmployeeName(model.UserName);
            model.FirstName = name.Item1;
            model.LastName = name.Item2;
        }

        /// <summary>
        /// Gets the first and last name of the employee.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>First and last name of the employee.</returns>
        private Tuple<string, string> GetEmployeeName(string userName)
        {
            var employeeProducts = _businessService.GetEmployeeProducts(new List<string>(), new List<string> { userName });
            var employee = employeeProducts.First().Employee;
            
            var result = Tuple.Create(employee.FirstName, employee.LastName);

            return result;
        }

        private void LoginImpl(LoginViewModel model)
        {
            string userName = model.UserName;
            string password = model.Password;
            string webClient = Request.UserAgent;
            string token = _authorizationService.Login(userName, password, webClient);
            string adminVisible = (_authorizationService.IsExistingEmployeeRole(userName, "Admin") || _authorizationService.IsExistingEmployeeRole(userName, "Super Admin")).ToString();

            TryDeactivateOtherEmployee(userName);

            string firstName = GetEmployeeName(userName).Item1;

            AuthenticationHelper.CreateCookie(new EmployeeCookieInfo
            {
                Name = AuthenticationHelper.LoginCookieName,
                TokenKey = "auth",
                Token = token,
                UserNameKey = "username",
                UserName = userName,
                UserFirstNameKey = "firstname",
                UserFirstName = firstName,
                AdminVisibleKey = "adminvisible",
                AdminVisible = adminVisible
            });              
        }

        private bool IsActiveSession(string currentUserName)
        {
            string token = AuthenticationHelper.GetCookieValue(AuthenticationHelper.LoginCookieName, "auth");
            string otherUserName = AuthenticationHelper.GetCookieValue(AuthenticationHelper.LoginCookieName, "username");

            bool isCurrentUser = otherUserName.Equals(currentUserName, StringComparison.OrdinalIgnoreCase);

            bool result = (token != string.Empty && isCurrentUser) && _authorizationService.CanAuthenticateWithToken(token);

            return result;
        }


        /// <summary>
        /// Tries to deactivate other logged-in employee (if any).
        /// </summary>
        /// <param name="currentUserName">The user name of the current user.</param>
        /// <remarks>
        /// There's a scenario where current user logs in on the same box with the same browser while previous user has not logged out. Rare, but should be handled.
        /// </remarks>
        private void TryDeactivateOtherEmployee(string currentUserName)
        {
            string token = AuthenticationHelper.GetCookieValue(AuthenticationHelper.LoginCookieName, "auth");
            string otherUserName = AuthenticationHelper.GetCookieValue(AuthenticationHelper.LoginCookieName, "username");

            bool otherEmployeeIsActive = token != string.Empty && !string.IsNullOrEmpty(otherUserName) && !otherUserName.Equals(currentUserName, StringComparison.OrdinalIgnoreCase);

            if(otherEmployeeIsActive)
            {
                try
                {
                    _authorizationService.Logout(otherUserName, token);
                }
                finally
                {
                    // Enforce removal of cookie regardless
                    AuthenticationHelper.RemoveCookie(AuthenticationHelper.LoginCookieName);
                }
            }
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation(ForgotPasswordViewModel model)
        {
            OpenExpensePortal();

            var data= _authorizationService.GetResetPasswordData(model.UserName); // also writes token to database

            Debug.Assert(Request.Url != null, "If Request.Url is null there is probably a bug in the code somewhere");
            var url = Request.Url.Scheme + "://" + Request.Url.Authority +   (Request.ApplicationPath ?? "").TrimEnd('/') + "/Account/ResetPassword";
            url += "?e=" + HttpUtility.UrlEncode(model.UserName) + "&t=" + HttpUtility.UrlEncode(data.Token);            
            SmtpHelper.SendMail("EPR Portal Password Reset", Server.MapPath("~/App_Data/EmailTemplates/ResetPassword.txt"), data.Email, data.FirstName, data.Surname, url);
            
            return View(model);
        }

        #region Helpers


        private ActionResult RedirectToProducts(string returnUrl)
        {
            if(Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Products", "Expense");
        }

        #endregion
    }
}