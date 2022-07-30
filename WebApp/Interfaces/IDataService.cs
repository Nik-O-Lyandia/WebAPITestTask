namespace WebApp.Interfaces
{
    public interface IDataService
    {
        public string[]? ReadEmails();
        public void WriteEmail(string email);
    }
}
