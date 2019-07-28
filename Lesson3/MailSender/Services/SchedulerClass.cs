using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using WpfEmailSendServiceDLL;
using System.Collections.ObjectModel;


namespace MailSender
{
    /// <summary>
    /// Класс-планировщик, который создает расписание, следит за его выполнением и напоминает о событиях
    /// Также помогает автоматизировать рассылку писем в соответствии с расписанием
    /// </summary>
    class SchedulerClass
    {
        DispatcherTimer timer = new DispatcherTimer();  // таймер 
        WpfEmailSendService emailSender;                // экземпляр класса, отвечающего за отправку писем
        DateTime dtSend;                                // дата и время отправки
        ObservableCollection<Emails> emails;            // коллекция email-ов адресатов

        /// <summary>
        /// Преобразует string в TimeSpan
        /// </summary>
        /// <param name="strSendTime"></param>
        /// <returns></returns>
        public TimeSpan GetSendTime(string strSendTime)
        {
            TimeSpan tsSendTime = new TimeSpan();
            try
            {
                tsSendTime = TimeSpan.Parse(strSendTime);
            }
            catch { }
            return tsSendTime;
        }

        /// <summary>
        /// Отправка писем
        /// </summary>
        /// <param name="dtSend"></param>
        /// <param name="emailSender"></param>
        /// <param name="emails"></param>
        public void SendMails(DateTime dtSend, WpfEmailSendService emailSender, ObservableCollection<Emails> emails)
        {
            this.emailSender = emailSender; // Экземпляр класса, отвечающего за отправку писем, присваиваем 
            this.dtSend = dtSend;
            this.emails = emails;
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        /// <summary>
        /// Таймер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (dtSend.ToShortTimeString() == DateTime.Now.ToShortTimeString())
            {
                List<string> listEmails = new List<string>();
                foreach (Emails em in emails) listEmails.Add(em.Value);

                emailSender.SendMails(WpfMailSender.MailSubject, WpfMailSender.MailBody, listEmails);
                timer.Stop();
                MessageBox.Show("Отправка писем завершена");
            }
        }
    }
}

