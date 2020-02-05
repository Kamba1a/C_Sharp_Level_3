using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace WpfTestMailSender
{
    /// <summary>
    /// Класс для рассылки писем по заданию(1b)
    /// </summary>
    public class EmailSendServiceClass
    {
        private string _myPassword;
        private string _myEmail;

        public EmailSendServiceClass(string myEmail, string myPassword)
        {
            _myEmail = myEmail;
            _myPassword = myPassword;
            SmtpServer = "smtp." + new Regex("(?<=@).*").Match(_myEmail).Value;
            SmtpPort = 587;
        }

        public string SmtpServer { get; set;}
        public int SmtpPort { get; set;}

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
                            sc.Send(mm);
                        }
                        catch (Exception ex)
                        {
                            ErrorWindow ew = new ErrorWindow("E-mail получателя: " + mail + "\n" + ex.ToString());
                            ew.Owner = WpfMailSender.ThisWindow;
                            ew.ShowDialog();
                        }
                    }
                }
            }
        }

    }
}
