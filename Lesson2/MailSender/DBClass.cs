using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender
{
    /// <summary>
    /// Класс для работы с базой данных
    /// </summary>
    public static class DBClass
    {
        private static EmailsDataContext emails = new EmailsDataContext();

        public static IQueryable<Emails> Emails
        {
            get
            {
                return from s in emails.Emails select s;
            }
        }

        private static SenderEmailsDataContext senderEmails = new SenderEmailsDataContext();

        public static IQueryable<SenderEmails> SenderEmails
        {
            get
            {
                return from s in senderEmails.SenderEmails select s;
            }
        }

        private static SmtpServersDataContext smtpServers = new SmtpServersDataContext();

        public static IQueryable<SmtpServers> SmtpServers
        {
            get
            {
                return from s in smtpServers.SmtpServers select s;
            }
        }
    }
}
