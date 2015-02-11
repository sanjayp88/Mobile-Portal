using System;
using System.IO;
using System.Net.Mail;

namespace ExpensePortal.Helpers
{
    /// <summary>
    /// This class could be made a lot more generic. For example by using reflection compared to the parameters in the templates. 
    /// But since at the moment there's only the one email to send I'll leave it like this for now. 
    /// </summary>
    public class SmtpHelper
    {
        public static void SendMail(string subject, string template, string email, string firstName, string surname, string url)
        {
            try
            {
                using(var msg = new MailMessage())
                {
                    msg.To.Add(email);
                    msg.Priority = MailPriority.High;
                    msg.IsBodyHtml = false;
                    msg.Subject = subject;
                    msg.Body = CreateEmailBody(LoadTemplate(template), firstName, surname, url);
                    var smtp = new SmtpClient();
                    smtp.Send(msg);
                }
            }
            catch(Exception ex)
            {
                throw; // TODO: Add error handling
            }
        }

        private static string CreateEmailBody(string templateBody, string firstName, string surname, string url)
        {
            string body = templateBody.Replace("[FirstName]", firstName).Replace("[Surname]", surname).Replace("[Url]", url);
            return body;
        }

        private static string LoadTemplate(string template)
        {
            using(var sr = new StreamReader(template))
            {                
                return sr.ReadToEnd();
            }
        }
    }
}