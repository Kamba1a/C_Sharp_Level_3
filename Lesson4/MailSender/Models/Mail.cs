using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Models
{
    /// <summary>
    /// Класс "Письмо для отложенной отправки"
    /// </summary>
    public class SendMailClass
    {
        /// <summary>
        /// Дата/время отправки
        /// </summary>
        public DateTime SendDateTime { get; set; }

        /// <summary>
        /// Текст письма
        /// </summary>
        public string MailText { get; set; }

        public SendMailClass()
        {
            SendDateTime = DateTime.Parse(DateTime.Now.ToString());
        }
    }
}
