using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MDClone.GeneralClasses.Email
{
    public static class EmailExecution
    {

        /// <summary>
        /// get the details to send the email cause no need to implement the "sent email" - just return always the message 
        /// </summary>
        /// <param name="fromAddressString"></param>
        /// <param name="toAddressString"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="message"> the messgae that will display at messageBox</param>
        public static bool SendEmail(string fromAddressString, string toAddressString, string subject, string body, GeneralClass.ShowDataToUser messageToUser)
        {

            var message = default(MailMessage);
            SmtpClient smtpClient = null;
            bool isMailSentSuccssefully;

            try
            {
                var fromAddress = new MailAddress(fromAddressString, "Tal Kh");
                var toAddress = new MailAddress(toAddressString, toAddressString);
                string fromPassword = "999999";
                smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                };

                smtpClient.SendMailAsync(message);
                isMailSentSuccssefully = true;
            }
            catch (Exception ex)
            {
                // in this case not to use messageToUser cause the sending email dose not work
                Console.WriteLine(ex.Message);
            }
            finally
            {
                message?.Dispose();
                smtpClient?.Dispose();
            }

            // patch just to get that message sent 
            messageToUser("Email has been sent successfully");
            isMailSentSuccssefully = true;

            return isMailSentSuccssefully;

        }
    }
}
