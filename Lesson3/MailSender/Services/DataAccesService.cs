using System.Collections.ObjectModel;
using MailSender.DataClasses;

namespace MailSender.Services
{
    public class DataAccessService
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

        public string CreateEmail(Emails email)
        {
            try
            {
                emailsDataContext.Emails.InsertOnSubmit(email);
                emailsDataContext.SubmitChanges();
                return email.Email;
            }
            catch (System.Data.Linq.DuplicateKeyException ex)
            {
                return ex.Message;
            }
        }

        public void DeleteEmail(Emails email)
        {
            emailsDataContext.Emails.DeleteOnSubmit(email);
            emailsDataContext.SubmitChanges();
        }

        public ObservableCollection<Emails> GetEmails()
        {
            ObservableCollection<Emails> Emails = new ObservableCollection<Emails>();
            foreach (var item in emailsDataContext.Emails) Emails.Add(item);
            return Emails;
        }

        public ObservableCollection<SenderEmails> GetSenderEmails()
        {
            ObservableCollection<SenderEmails> senderEmails = new ObservableCollection<SenderEmails>();
            foreach (var item in senderEmailsDataContext.SenderEmails) senderEmails.Add(item);
            return senderEmails;
        }

        public ObservableCollection<SmtpServers> GetSmtpServers()
        {
            ObservableCollection<SmtpServers> smtpServers = new ObservableCollection<SmtpServers>();
            foreach (var item in SmtpServersDataContext.SmtpServers) smtpServers.Add(item);
            return smtpServers;
        }
    }
}
