using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ExpensePortal.ExpensesServiceReference;


namespace ExpensePortal.Helpers
{
    public class AuthenticationHelper
    {

        public const string LoginCookieName = "Login";

        /// <summary>
        /// Creates the cookie.
        /// </summary>
        /// <param name="info">Employee information.</param>
        public static void CreateCookie(EmployeeCookieInfo info)
        {
            var cookie = new HttpCookie(info.Name);
            cookie.Values[info.TokenKey] = info.Token;
            cookie.Values[info.UserNameKey] = info.UserName;
            cookie.Values[info.UserFirstNameKey] = info.UserFirstName;
            cookie.Values[info.AdminVisibleKey] = info.AdminVisible;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Gets the value of the specified key in the specified cookie.
        /// </summary>
        /// <param name="name">The cookie name.</param>
        /// <param name="key">The cookie key.</param>
        /// <returns>
        /// The cookie value.
        /// </returns>
        public static string GetCookieValue(string name, string key)
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(name);

            string result = cookie != null ?
                cookie.Values[key] ?? string.Empty :
                String.Empty;

            return result;
        }

        /// <summary>
        /// Removes the cookie.
        /// </summary>
        /// <param name="name">The cookie name.</param>
        public static void RemoveCookie(string name)
        {
            bool cookieExists = HttpContext.Current.Request.Cookies[name] != null;

            if(cookieExists)
            {
                var cookie = new HttpCookie(name)
                {
                    Expires = DateTime.Now.AddDays(-1d)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// Adds a key and value to an existing cookie.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static void AddToCookie(string name, string key, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(name);
            if(cookie == null) throw new InvalidOperationException(string.Format("Could not find cookie named {0}.", name));

            cookie.Values[key] = value;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Determines whether the current user has an active session.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>true if user has active session; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">service;service is null.</exception>
        public static bool IsActiveSession(AuthorizationServiceClient service)
        {
            if(service == null) throw new ArgumentNullException("service", "service is null.");

            string token = GetCookieValue(LoginCookieName, "auth");

            bool result = token != string.Empty && service.CanAuthenticateWithToken(token);

            return result;
        }

        /// <summary>
        /// Determines whether the current user has an active session for the product and payroll number.
        /// </summary>
        /// <param name="businessService">The business service.</param>
        /// <param name="authorizationService"></param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="payrollNumber">The payroll number.</param>
        /// <returns>
        /// true if user has active session; otherwise false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">service;service is null.</exception>
        /// <remarks>
        /// The product name and payroll numbers are checked to prevent browsing to another user's expense record on the same browser.
        /// E.g., pasting in ...Expense/Expenditure/E_124/2014/34/Esentual where E_124 is not your payroll number should be prohibited.
        /// The Expense/Expenditure controller action calls this.
        /// </remarks>
        public static bool IsActiveSession(BusinessServiceClient businessService, AuthorizationServiceClient authorizationService, string productName, string payrollNumber)
        {
            if(businessService == null) throw new ArgumentNullException("businessService", "service is null.");

            bool result = false;

            string userName = GetCookieValue(LoginCookieName, "username");

            bool userSessionExists = !string.IsNullOrEmpty(userName);

            if(userSessionExists)
            {
                string token = GetCookieValue(LoginCookieName, "auth");

                if(token != string.Empty)
                {
                    var employeeProducts = businessService.GetEmployeeProducts(new List<string> { productName }, new List<string> { userName });

                    bool isValidProductPayroll = employeeProducts
                        .Any(e => e.ProductPayrolls.Any(p => p.ProductName == productName && p.PayrollNumber == payrollNumber));

                    result = isValidProductPayroll && authorizationService.CanAuthenticateWithToken(token);
                }
            }

            return result;
        }

        public static bool IsExistingEmployeeRole(AuthorizationServiceClient service, string role)
        {
            if(service == null) throw new ArgumentNullException("service", "service is null.");

            string userName = GetCookieValue(LoginCookieName, "username");

            bool result = !string.IsNullOrEmpty(userName) && service.IsExistingEmployeeRole(userName, role);

            return result;
        }

        public static bool IsInAdminRole(AuthorizationServiceClient service)
        {
            if(service == null) throw new ArgumentNullException("service", "service is null.");

            string token = GetCookieValue(LoginCookieName, "auth");
            string userName = GetCookieValue(LoginCookieName, "username");

            bool result =
                token != string.Empty &&
                service.CanAuthenticateWithToken(token) &&
                !string.IsNullOrEmpty(userName) &&
                (service.IsExistingEmployeeRole(userName, "Admin") || service.IsExistingEmployeeRole(userName, "Super Admin"));

            return result;
        }
    }

    public class EmployeeCookieInfo
    {
        public string Name { get; set; }
        public string TokenKey { get; set; }
        public string Token { get; set; }
        public string UserNameKey { get; set; }
        public string UserName { get; set; }
        public string UserFirstNameKey { get; set; }
        public string UserFirstName { get; set; }
        public string ProductNameKey { get; set; }
        public string ProductName { get; set; }
        public string AdminVisibleKey { get; set; }
        public string AdminVisible { get; set; }
    }
}