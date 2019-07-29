using System.Collections.ObjectModel;
using MailSender.DataClasses;

namespace MailSender.Services
{
    /// <summary>
    /// Класс для работы с БД
    /// </summary>
    public class DataAccessService
    {
        EmailsDataContext emailsDataContext;
        SenderEmailsDataContext senderEmailsDataContext;
        SmtpServersDataContext smtpServersDataContext;

        public DataAccessService()
        {
            emailsDataContext = new EmailsDataContext();
            senderEmailsDataContext = new SenderEmailsDataContext();
            smtpServersDataContext = new SmtpServersDataContext();
        }

        /// <summary>
        /// Добавить нового получателя
        /// </summary>
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

        /// <summary>
        /// Удалить получателя
        /// </summary>
        public void DeleteEmail(Emails email)
        {
            emailsDataContext.Emails.DeleteOnSubmit(email);
            emailsDataContext.SubmitChanges();
        }

        /// <summary>
        /// Получить список получателей
        /// </summary>
        public ObservableCollection<Emails> GetEmails()
        {
            ObservableCollection<Emails> Emails = new ObservableCollection<Emails>();
            foreach (var item in emailsDataContext.Emails) Emails.Add(item);
            return Emails;
        }

        /// <summary>
        /// Получить список емейлов отправителя
        /// </summary>
        public ObservableCollection<SenderEmails> GetSenderEmails()
        {
            ObservableCollection<SenderEmails> senderEmails = new ObservableCollection<SenderEmails>();
            foreach (var item in senderEmailsDataContext.SenderEmails) senderEmails.Add(item);
            return senderEmails;
        }

        /// <summary>
        /// Получить список smtp-серверов
        /// </summary>
        public ObservableCollection<SmtpServers> GetSmtpServers()
        {
            ObservableCollection<SmtpServers> smtpServers = new ObservableCollection<SmtpServers>();
            foreach (var item in smtpServersDataContext.SmtpServers) smtpServers.Add(item);
            return smtpServers;
        }
    }
}
