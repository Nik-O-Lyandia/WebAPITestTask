/*
 * DataService provides logic oparating with data (specifically with emails.txt file in this situation)
 */

using WebApp.Interfaces;

namespace WebApp.Services
{
    public class DataService : IDataService
    {
        private readonly IConfiguration _configuration;
        public DataService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string[]? ReadEmails()
        {
            return File.ReadAllLines(_configuration.GetValue<string>("EmailsFilePath"));
        }

        public void WriteEmail(string email)
        {
            File.AppendAllText(_configuration.GetValue<string>("EmailsFilePath"), email + "\n");
        }
    }
}
