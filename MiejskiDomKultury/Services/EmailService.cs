using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Services
{
    public class EmailService
    {

        string apiKey = Environment.GetEnvironmentVariable("GMAIL_KEY");

        public void SendEmail(string email)
        {
            try
            {
              

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("polichronowe@gmail.com"),
                    Subject = "Miejski Dom Kultury",
                    Body = $@"
                <html>
                    <body>
                       Treść maila jako html
                    </body>
                </html>",
                    IsBodyHtml = true
                };

                mail.To.Add(email);




                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential("polichronowe@gmail.com", apiKey)
                };

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas wysyłania e-maila: {ex.Message}");
            }
        }

      

    }
}
