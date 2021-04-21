using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace UrbanRFP.Helpers
{
    public class EmailManager
    {
        /// <summary>
        /// Send user new credentials
        /// </summary>
        /// <param name="email">Email address</param>
        /// <param name="password">User Password</param>
        /// <param name="displayName">User Full Name</param>
        public static void SendWelcome(string email, string displayName, string activation_code)
        {
            StringBuilder emailBody = new StringBuilder();
            emailBody.Append($"Dear {displayName},<br/><br/>");
            emailBody.Append("Welcome to the UrbanRFP.  You have been registered successfully.<br/><br/>");
            emailBody.Append("Please click the link below to activate your account.<br/><br/>");
            emailBody.Append(ConfigHelper.DomainURL + "account/activate/" + activation_code + "<br/><br/>");
            emailBody.Append("If you have any problems accessing the system, please email support@urbanrfp.com<br/><br/>");
            emailBody.Append("Thank You,<br/>UrbanRFP Support Team");


            SendEmail(email, displayName, "Welcome to the UrbanRFP!", emailBody.ToString());
        }

        /// <summary>
        /// Send user new credentials
        /// </summary>
        /// <param name="email">Email address</param>
        /// <param name="password">User Password</param>
        /// <param name="displayName">User Full Name</param>
        public static void SendResetPassword(string email, string displayName, string secure_key)
        {
            StringBuilder emailBody = new StringBuilder();
            emailBody.Append($"Dear {displayName},<br/><br/>");
            emailBody.Append("Please click the link below to reset your password.<br/><br/>");
            emailBody.Append(ConfigHelper.DomainURL + "account/resetpassword/" + secure_key + "<br/><br/>");
            emailBody.Append("If you have any problems accessing the system, please email support@urbanrfp.com<br/><br/>");
            emailBody.Append("Thank You,<br/>UrbanRFP Support Team");


            SendEmail(email, displayName, "Reset your password on UrbanRFP!", emailBody.ToString());
        }

        public static void SendResetPasswordConfirmation(string email, string displayName)
        {
            StringBuilder emailBody = new StringBuilder();
            emailBody.Append($"Dear {displayName},<br/><br/>");
            emailBody.Append("Your password has been changed successfully.<br/><br/>");
            emailBody.Append("If you didn't change your password, please contact us immediately.<br/><br/>");
            emailBody.Append("If you have any problems accessing the system, please email support@urbanrfp.com<br/><br/>");
            emailBody.Append("Thank You,<br/>UrbanRFP Support Team");


            SendEmail(email, displayName, "Password changed successfully!", emailBody.ToString());
        }

        /// <summary>
        /// Send successfully generated report to employer.
        /// </summary>
        /// <param name="reportPath">Report path</param>
        /// <param name="email">Employer email address</param>
        /// <param name="displayName">Employer name</param>
        public static void SendReport(string reportPath, string reportTitle, string email, string displayName)
        {
            StringBuilder emailBody = new StringBuilder();
            emailBody.Append("<h1>" + reportTitle + "</h1>");
            emailBody.Append("<p>Your report has been generated successfully</p><b/>");
            emailBody.Append("<a href='" + reportPath + "'>Download here</a><b/>");
            emailBody.Append("<p>If you have any problems accessing the system, please email login@acataxtrack.com</p>");

            SendEmail(email, displayName, "ACATaxTrack - " + reportTitle, emailBody.ToString());
        }

        /////// <summary>
        /////// Send any error message to administrator
        /////// </summary>
        /////// <param name="errorMessage">List of error messages</param>
        /////// <param name="description">Error description</param>
        ////public static void SendError(List<string> errorMessages, string description)
        ////{
        ////    StringBuilder emailBody = new StringBuilder();
        ////    emailBody.Append("<h1 style='color:firebrick;'>Error</h1>");
        ////    emailBody.Append("<p><b>MESSAGE: </b></p><br/>");
        ////    string errorlist = "";
        ////    errorMessages.ForEach(e =>
        ////    {
        ////        errorlist += "<li>" + e + "<li>";
        ////    });
        ////    emailBody.Append("<ul>" + errorlist + "</ul>");
        ////    emailBody.Append("<p><b>DESCRIPTION: </b></p>" + description + "<br/>");

        ////    SendEmail("uzair.akram@innovativewhiz.com", "Uzair Akram", "ACATaxTrack program!", emailBody.ToString());
        ////}


        /// <summary>
        /// Function to send the new credentials to the email id of the user
        /// </summary>
        /// <param name="toEmailAddress">To Email</param>
        /// <param name="userName">To Display Name</param>
        /// <param name="aMailSubject">Email Subject</param>
        /// <param name="aEmailBody">Email Body</param>
        private static void SendEmail(string toEmailAddress, string toDisplayName, string aMailSubject, string aEmailBody)
        {
            //to and from of the mail
            MailAddress toAddress = new MailAddress(toEmailAddress, toDisplayName);
            MailAddress fromAddress = new MailAddress(ConfigHelper.GetSettingAsString("FromEmail"), "UrbanRFP");
            var message = new MailMessage(fromAddress, toAddress);

            //the body of the message
            var strEmailMessage = new StringBuilder();
            strEmailMessage.Append(
                "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head runat='server'><title></title></head><body><form id='form1' runat='server'><div>" +
                aEmailBody + "</div></form></body></html>");

            //subject
            message.Subject = aMailSubject;
            message.Body = strEmailMessage.ToString();
            //set id the body is in html format
            message.IsBodyHtml = true;

            //sent the mail
            var smtp = new SmtpClient();
            try
            {
                smtp.Send(message);
            }
            catch(Exception ex)
            {

            }
        }
    }
}