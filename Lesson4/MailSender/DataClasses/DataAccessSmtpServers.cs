using System.Collections.ObjectModel;
using MailSender.DataClasses;

namespace MailSender.DataClasses
{
    public static class DataAccessSmtpServers
    {
        static SmtpServersDataContext smtpServersDataContext = new SmtpServersDataContext();

        /// <summary>
        /// Получить список smtp-серверов
        /// </summary>
        static public ObservableCollection<SmtpServers> GetSmtpServers()
        {
            ObservableCollection<SmtpServers> smtpServers = new ObservableCollection<SmtpServers>();
            foreach (var item in smtpServersDataContext.SmtpServers) smtpServers.Add(item);
            return smtpServers;
        }
    }
}
