﻿using System.Net;
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
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;

        public BitcoinService(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _clientFactory = clientFactory;
            _config = config;
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
            Regex rgx = new Regex("^[a-zA-Z0-9.]+[@][a-z]+[.][a-z]+");

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
                var fromEmailAddress = _config.GetValue<string>("Email");
                var fromEmailAppPassword = _config.GetValue<string>("EmailAppPassword");

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailAppPassword);
                client.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmailAddress, "BTC to UAH Service");

                foreach (var email in allEmails)
                {
                    mail.To.Add(new MailAddress(email));
                }

                mail.Subject = "BTC to UAH";
                mail.SubjectEncoding = Encoding.UTF8;

                mail.Body = rate.ToString();
                mail.BodyEncoding = Encoding.UTF8;

                try
                {
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new InvalidOperationException("No subscribed addresses availible");
            }
        }
    }
}
