using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace MyProjectUniversityPanel.Helpers
{
    public static class Helper
    {
        public enum Roles {

            SuperAdmin,
            Admin,
            Teacher,
            Student,
        }
        public static async Task SendMessage(string messageSubject, string messageBody, string mailTo)
        {


            SmtpClient client = new SmtpClient("smtp.yandex.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("nigarkhanim.a@itbrains.edu.az", "burhphattpriyhqd");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;


            MailMessage message = new MailMessage("nigarkhanim.a@itbrains.edu.az", mailTo);
            message.Subject = messageSubject;
            message.Body = messageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;


            await client.SendMailAsync(message);

        }


    }
}
