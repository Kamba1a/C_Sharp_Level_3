using System.Collections.ObjectModel;
using MailSender.DataClasses;

namespace MailSender.DataClasses
{
    /// <summary>
    /// Класс для работы с таблицей Emails из БД
    /// </summary>
    public static class DataAccessEmails
    {
        static EmailsDataContext emailsDataContext = new EmailsDataContext();

        /// <summary>
        /// Получить список получателей
        /// </summary>
        public static ObservableCollection<Emails> GetEmails()
        {
            ObservableCollection<Emails> Emails = new ObservableCollection<Emails>();
            foreach (var item in emailsDataContext.Emails) Emails.Add(item);
            return Emails;
        }

        /// <summary>
        /// Добавить нового получателя
        /// </summary>
        public static string CreateEmail(Emails email)
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
        public static void DeleteEmail(Emails email)
        {
            emailsDataContext.Emails.DeleteOnSubmit(email);
            emailsDataContext.SubmitChanges();
        }


    }
}
