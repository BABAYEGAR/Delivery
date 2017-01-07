using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web;
using Delivery.Data.Objects.Entities;

namespace Delivery.Data.Service.EmailService
{
    public class MailerDaemon
    {
        /// <summary>
        ///     This method sends an email containing a username and password to a newly created user
        /// </summary>
        /// <param name="user"></param>
        public void NewUser(AppUser user)
        {
            var message = new MailMessage
            {
                From = new MailAddress(""),
                Subject = "New User Details",
                Priority = MailPriority.High,
                SubjectEncoding = Encoding.UTF8,
                Body = GetEmailBody_NewUserCreated(user),
                IsBodyHtml = true
            };
            //message.To.Add(Config.DevEmailAddress);
            message.To.Add(user.Email);
            try
            {
                new SmtpClient().Send(message);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///     Html page content for the new user email
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private static string GetEmailBody_NewUserCreated(AppUser user)
        {
            return
                new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/NewUserCreated.html")).ReadToEnd()
                    .Replace("DISPLAYNAME", user.Firstname)
                    .Replace("USERNAME", user.Email)
                    .Replace("PASSWORD", user.Password)
                    .Replace("URL", "http://10.10.15.77/bhuinfo/Account/Login")
                    .Replace("ROLE", user.Role)
                    .Replace("FROM", "");
        }
        /// <summary>
        /// This method sends an email for new orders
        /// </summary>
        /// <param name="order"></param>
        public void NewOrder(Order order)
        {
            var message = new MailMessage
            {
                From = new MailAddress("salxsaa@gmail.com"),
                Subject = "Order Confirmation",
                Priority = MailPriority.High,
                SubjectEncoding = Encoding.UTF8,
                Body = GetEmailBody_NewOrder(order),
                IsBodyHtml = true
            };
            //message.To.Add(Config.DevEmailAddress);
            message.To.Add("salxsaa@gmail.com");
            message.To.Add(order.Email);
            try
            {
                new SmtpClient().Send(message);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///     Html page content for the new user email
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private static string GetEmailBody_NewOrder(Order order)
        {
            
            return
                new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/NewOrder.html")).ReadToEnd()
                    .Replace("DISPLAYNAME", order.Name)
                    .Replace("CODE", order.OrderCode)
                    .Replace("CONTACT1", order.Mobile)
                    .Replace("CONTACT2", order.Email)
                    .Replace("FROM", "support@shisha.com");
        }
        
    }
}