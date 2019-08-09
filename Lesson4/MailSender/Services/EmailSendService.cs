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
        string _myPassword;         //пароль
        string _myEmail;            //емейл-адрес отправителя

        /// <summary>
        /// Сообщение после отправки почты
        /// </summary>
        public Action<string> MessageMailSend;
        /// <summary>
        /// Сообщение перед отправкой почты
        /// </summary>
        public Action<string> MessageBeforeSendMail;
        /// <summary>
        /// Smtp-сервер
        /// </summary>
        public string SmtpServer { get; set; }
        /// <summary>
        /// Порт
        /// </summary>
        public int SmtpPort { get; set; }

        public EmailSendService(string myEmail, string myPassword)
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
            _myPassword = null;
        }
    }
}

