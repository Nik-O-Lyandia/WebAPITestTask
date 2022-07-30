namespace WebApp.Interfaces
{
    public interface IMailService
    {
        void Subscribe(string email);
        void SendEmails();
    }
}
