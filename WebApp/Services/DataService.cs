/*
 * DataService provides logic oparating with data (specifically with emails.txt file in this situation)
 */

using WebApp.Interfaces;

namespace WebApp.Services
{
    public class DataService : IDataService
    {
        public string[] ReadEmails(string path)
        {
            return File.ReadAllLines(path);
        }

        public void WriteEmail(string email, string path)
        {
            File.AppendAllText(path, email + "\n");
        }
    }
}
