using System;
using System.Collections.Generic;
using System.Windows.Threading;
using MailSender.Services;


namespace MailSender
{
    /// <summary>
    /// Класс-планировщик, который создает расписание, следит за его выполнением и напоминает о событиях
    /// Также помогает автоматизировать рассылку писем в соответствии с расписанием
    /// </summary>
    class SchedulerClass
    {
        DispatcherTimer timer;              //таймер 
        DateTime dtSend;                    //дата и время отправки
        EmailSendService emailSender;       //экземпляр класса, отвечающего за отправку писем
        List<string> _listEmails;           //емейл адреса получателей
        string _mailSubject;                //заголовок письма                     
        string _mailBody;                   //текст письма


        public SchedulerClass(string mailSubject, string mailBody, List<string> emails)
        {
            timer = new DispatcherTimer();
            this._mailSubject = mailSubject;
            this._mailBody = mailBody;
            _listEmails = emails;
        }

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
        public void SendMails(DateTime dtSend, EmailSendService emailSender)
        {
            this.dtSend = dtSend;
            this.emailSender = emailSender; 

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
                emailSender.SendMails(_mailSubject, _mailBody, _listEmails);
                timer.Stop();
            }
        }
    }
}

