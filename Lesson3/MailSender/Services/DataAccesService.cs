using System.Collections.ObjectModel;

namespace MailSender.Services
{
    public interface IDataAccessService
    {
        ObservableCollection<Emails> GetEmails();
        ObservableCollection<SenderEmails> GetSenderEmails();
        ObservableCollection<SmtpServers> GetSmtpServers();
        int CreateEmail(Emails email);
    }

    public class DataAccessService : IDataAccessService
    {

        EmailsDataContext emailsDataContext;
        SenderEmailsDataContext senderEmailsDataContext;
        SmtpServersDataContext SmtpServersDataContext;

        public DataAccessService()
        {
            emailsDataContext = new EmailsDataContext();
            senderEmailsDataContext = new SenderEmailsDataContext();
            SmtpServersDataContext = new SmtpServersDataContext();
        }

        public int CreateEmail(Emails email)
        {
            emailsDataContext.Emails.InsertOnSubmit(email);
            emailsDataContext.SubmitChanges();
            return email.Id;
        }

        public ObservableCollection<Emails> GetEmails()
        {
            ObservableCollection<Emails> Emails = new ObservableCollection<Emails>();
            foreach (var item in emailsDataContext.Emails)
            {
                Emails.Add(item);
            }
            return Emails;
        }

        public ObservableCollection<SenderEmails> GetSenderEmails()
        {
            ObservableCollection<SenderEmails> senderEmails = new ObservableCollection<SenderEmails>();
            foreach (var item in senderEmailsDataContext.SenderEmails)
            {
                senderEmails.Add(item);
            }
            return senderEmails;
        }

        public ObservableCollection<SmtpServers> GetSmtpServers()
        {
            ObservableCollection<SmtpServers> smtpServers = new ObservableCollection<SmtpServers>();
            foreach (var item in SmtpServersDataContext.SmtpServers)
            {
                smtpServers.Add(item);
            }
            return smtpServers;
        }
    }
}
