using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using MailSender.Services;


namespace MailSender
{
    /// <summary>
    /// Класс-планировщик
    /// </summary>
    public class SchedulerClass
    {
        DispatcherTimer _timer;                         //таймер 
        EmailSendService _emailSender;                  //экземпляр класса, отвечающего за отправку писем
        List<string> _listEmails;                       //емейл адреса получателей
        string _mailSubject;                            //заголовок письма                     
        string _mailBody;                               //текст письма
        Dictionary<DateTime, string> _dicDates;         //список писем на отправку

        /// <summary>
        /// Сообщение после отправки каждого письма
        /// </summary>
        public Action<string> MessageAfterOneSend;
        /// <summary>
        /// Сообщение после отправки всех писем
        /// </summary>
        public Action<string> MessageAfterSendAll;
        /// <summary>
        /// Сообщение после запуска таймера на отправку
        /// </summary>
        internal Action<string> MessageAfterPlanning;

        public SchedulerClass(List<string> emails)
        {
            _timer = new DispatcherTimer();
            _listEmails = emails;
            _dicDates = new Dictionary<DateTime, string>();
        }

        /// <summary>
        /// Преобразует string в TimeSpan - не используется, оставлен для проведения юнит-теста по ДЗ
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
        /// Список запланированных на отправку писем (дата отправки-текст письма)
        /// </summary>
        public Dictionary<DateTime, string> DatesEmailTexts
        {
            get { return _dicDates; }
            set
            {
                _dicDates = value;
                _dicDates = _dicDates.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
        }

        /// <summary>
        /// Отправка писем - старт таймера
        /// </summary>
        /// <param name="dtSend"></param>
        /// <param name="emailSender"></param>
        /// <param name="emails"></param>
        public void SendMails(EmailSendService emailSender)
        {
            this._emailSender = emailSender; 

            _timer.Tick += TimerTick;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
            MessageAfterPlanning?.Invoke("Отправка писем запланирована");
        }

        /// <summary>
        /// Отправка письма - выполняется по тику таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, EventArgs e)
        {
            if (_dicDates.Count > 0)
            {
                DateTime _nextSend = _dicDates.Keys.First<DateTime>();
                if (_nextSend.ToShortDateString()== DateTime.Now.ToShortDateString() && _nextSend.ToShortTimeString() == DateTime.Now.ToShortTimeString())
                {
                    _mailSubject = $"Рассылка от {_nextSend} ";
                    _mailBody = _dicDates[_dicDates.Keys.First<DateTime>()];
                    _emailSender.SendMails(_mailSubject, _mailBody, _listEmails);
                    MessageAfterOneSend?.Invoke($"Письмо от {_nextSend} отправлено");
                    _dicDates.Remove(_dicDates.Keys.First<DateTime>());
                }
            }
            else if (_dicDates.Count == 0)
            {
                _timer.Stop();
                MessageAfterSendAll?.Invoke("Запланированная отправка писем завершена");
            }
        }
    }
}

