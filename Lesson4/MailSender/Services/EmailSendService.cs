using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Security;

namespace MailSender.Services
{
    /// <summary>
    /// Класс для отправки электронной почты
    /// </summary>
    public class EmailSendService
    {
        public Action<string> MessageMailSend;          //сообщение об отправке почты
        public Action<string> MessageBeforeSendMail;    //сообщение перед отправкой почты
        private SecureString _myPassword;               //пароль
        private string _myEmail;                        //емейл-адрес отправителя
        public string SmtpServer { get; set; }          //smtp-сервер
        public int SmtpPort { get; set; }               //порт

        public EmailSendService(string myEmail, SecureString myPassword)
        {
            _myEmail = myEmail;
            _myPassword = myPassword;
            SmtpServer = "smtp." + new Regex("(?<=@).*").Match(_myEmail).Value;
            SmtpPort = 587;
        }

        /// <summary>
        /// Отправка почты
        /// </summary>
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

