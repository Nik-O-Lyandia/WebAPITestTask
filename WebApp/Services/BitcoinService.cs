using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using WebApp.Models;

namespace WebApp.Services
{
    public class BitcoinService
    {
        private const string _emailsFilePath = "Src/emails.txt";
        IHttpClientFactory _clientFactory;

        public BitcoinService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public decimal Rate()
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://bitpay.com/api/rates");
            var response = client.Send(request);

            var contentStream = response.Content.ReadAsStream();
            var currencies = JsonSerializer.Deserialize<List<Currency>>(contentStream);

            Currency finalCurrency = new Currency();

            if (currencies != null)
            {
                foreach (Currency cur in currencies)
                {
                    if (cur.Code == "UAH")
                    {
                        finalCurrency = cur;
                    }
                }
            }

            return finalCurrency.Rate;
        }

        public void Subscribe(string email)
        {
            Regex rgx = new Regex("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~]+[@][a-z]+[.][a-z]+");

            if (rgx.IsMatch(email))
            {
                var allEmails = File.ReadAllLines(_emailsFilePath);

                if (!allEmails.Contains(email))
                {
                    File.AppendAllText(_emailsFilePath, email + "\n");
                }
                else
                {
                    throw new InvalidOperationException($"Address '{email}' is already subscribed");
                }
            }
            else
            {
                throw new ArgumentException("Wrong email address");
            }
        }

        public void SendEmails()
        {
            var allEmails = File.ReadAllLines(_emailsFilePath);

            if (allEmails != null)
            {
                var rate = Rate();
                //string to = "toaddress@gmail.com"; //To address    
                //string from = "fromaddress@gmail.com"; //From address    
                //MailMessage message = new MailMessage(from, to);

                //string mailbody = "In this article you will learn how to send a email using Asp.Net & C#";
                //message.Subject = "Sending Email Using Asp.Net & C#";
                //message.Body = mailbody;
                //message.BodyEncoding = Encoding.UTF8;
                //message.IsBodyHtml = true;
                //SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                //System.Net.NetworkCredential basicCredential1 = new
                //System.Net.NetworkCredential("yourmail id", "Password");
                //client.EnableSsl = true;
                //client.UseDefaultCredentials = false;
                //client.Credentials = basicCredential1;
                //try
                //{
                //    client.Send(message);
                //}

                //catch (Exception ex)
                //{
                //    throw ex;
                //}
            }
        }
    }
}
