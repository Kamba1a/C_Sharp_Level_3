using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Security;

namespace MailSender.Services
{
    public class EmailSendService
    {
        private SecureString _myPassword;
        private string _myEmail;

        public Action<string> MessageMailSend;
        public Action<string> MessageBeforeSendMail;

        public EmailSendService(string myEmail, SecureString myPassword)
        {
            _myEmail = myEmail;
            _myPassword = myPassword;
            SmtpServer = "smtp." + new Regex("(?<=@).*").Match(_myEmail).Value;
            SmtpPort = 587;
        }

        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }

        public void SendMails(string mailSubject, string mailBody, ICollection<string> sendEmailsList)
        {
            foreach (string mail in sendEmailsList)
            {
                using (MailMessage mm = new MailMessage(_myEmail, mail))
                {
                    mm.Subject = mailSubject;
                    mm.Body = mailBody;
                    mm.IsBodyHtml = false;

                    using (SmtpClient sc = new SmtpClient(SmtpServer, SmtpPort))
                    {
                        sc.EnableSsl = true;
                        sc.Credentials = new NetworkCredential(_myEmail, _myPassword);
                        sc.Timeout = 5000;
                        try
                        {
                            MessageBeforeSendMail?.Invoke("Отправляем письмо..");
                            sc.Send(mm);
                            MessageMailSend?.Invoke($"Письмо отправлено.\nАдресат: {mail};\nСервер: {SmtpServer}, порт: {SmtpPort}");
                        }
                        catch (Exception ex)
                        {
                            MessageMailSend?.Invoke("Невозможно отправить письмо:\n" + ex.ToString());
                        }
                    }
                }
            }
            _myPassword.Dispose();
        }
    }
}

