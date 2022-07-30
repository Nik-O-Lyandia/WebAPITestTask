namespace WebApp.Interfaces
{
    public interface IDataService
    {
        public string[] ReadEmails(string path);
        public void WriteEmail(string email, string path);
    }
}
