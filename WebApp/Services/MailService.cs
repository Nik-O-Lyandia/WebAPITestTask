﻿/*
 * MailService provides logic oparating with emails
 */

using WebApp.Interfaces;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace WebApp.Services
{
    public class MailService : IMailService
    {
        private readonly IDataService _dataService;
        private readonly IConfiguration _configuration;
        private readonly IBitcoinService _bitcoinService;

        public MailService(IDataService dataService, IConfiguration configuration, IBitcoinService bitcoinService)
        {
            _dataService = dataService;
            _configuration = configuration;
            _bitcoinService = bitcoinService;
        }

        public void Subscribe(string email)
        {
            if (EmailIsValid(email)) throw new ArgumentException("Wrong email address. Allowed only letters, numbers & periods");

            string[]? emails;
            try
            {
                emails = _dataService.ReadEmails(_configuration.GetValue<string>("EmailsFilePath"));

                if (emails.Contains(email)) throw new InvalidOperationException($"Address '{email}' is already subscribed");

                _dataService.WriteEmail(email, _configuration.GetValue<string>("EmailsFilePath"));
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Folder 'Data/' is missed. Creating a new one");
                Directory.CreateDirectory(Path.GetDirectoryName(_configuration.GetValue<string>("EmailsFilePath")));
                _dataService.WriteEmail(email, _configuration.GetValue<string>("EmailsFilePath"));
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File emails.txt is missed. Creating a new one");
                _dataService.WriteEmail(email, _configuration.GetValue<string>("EmailsFilePath"));
            }
        }

        public void SendEmails()
        {
            string[] emails = _dataService.ReadEmails(_configuration.GetValue<string>("EmailsFilePath"));

            var rate = _bitcoinService.Rate();
            // Using Secrets.json for hide important data such as passwords
            var fromEmailAddress = _configuration.GetValue<string>("Email");
            var fromEmailAppPassword = _configuration.GetValue<string>("EmailAppPassword");

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailAppPassword);
            client.EnableSsl = true;

            foreach (var email in emails)
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmailAddress, "BTC to UAH Service");

                mail.To.Add(new MailAddress(email));


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
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private bool EmailIsValid(string email)
        {
            // Using Regex to accept emails names that compatible with gmail standard
            Regex rgx = new Regex(@"^[a-zA-Z0-9.]+[@][a-z]+[.][a-z.]+");

            return rgx.IsMatch(email);
        }
    }
}
