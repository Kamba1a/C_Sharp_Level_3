using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestMailSender
{
    /// <summary>
    /// Статические переменные по заданию(1а)
    /// </summary>
    public static class StaticVars
    {
        public static string MyEMailRambler { get; } = "tst_for_gkbrns@rambler.ru";
        public static string RamblerSmtpServer { get; } = "smtp.rambler.ru";
        public static int RamblerSmtpPort { get; } = 587; //В инструкции Рамблера указан порт 465, но с ним почему-то не работает
        public static string MailSubject { get; } = "Привет-привет! =)";
        public static string MailBody { get; } = "Просто тестовое письмо";
        public static List<string> SendEmailsList { get; set; } = new List<string> { "tst_for_gkbrns1@inbox.ru", "tst_for_gkbrns2@list.ru", "tst_for_gkbrns3@mail.ru" };
        public static string ErrorText { get; } = "Невозможно отправить письмо";
    }
}
