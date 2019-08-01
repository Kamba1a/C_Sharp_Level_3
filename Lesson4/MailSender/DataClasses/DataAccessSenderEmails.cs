using System.Collections.ObjectModel;
using MailSender.DataClasses;

namespace MailSender.DataClasses
{
    /// <summary>
    /// Класс для работы с таблицей SenderEmails в БД
    /// </summary>
    public static class DataAccessSenderEmails
    {
        static SenderEmailsDataContext senderEmailsDataContext = new SenderEmailsDataContext();

        /// <summary>
        /// Получить список емейлов отправителя
        /// </summary>
        public static ObservableCollection<SenderEmails> GetSenderEmails()
        {
            ObservableCollection<SenderEmails> senderEmails = new ObservableCollection<SenderEmails>();
            foreach (var item in senderEmailsDataContext.SenderEmails) senderEmails.Add(item);
            return senderEmails;

        }
    }
}
