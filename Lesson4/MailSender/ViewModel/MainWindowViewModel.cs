using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using MailSender.Services;
using System.ComponentModel;
using MailSender.DataClasses;
using System.Security;
using System;
using System.Linq;
using MailSender.Views;
using System.Windows.Controls;

namespace MailSender.ViewModel
{
    /// <summary>
    /// ViewModel для MainWindowView (главное окно приложения)
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        EmailSendService _emailSender;                                                  //класс для отправки почты
        public event PropertyChangedEventHandler PropertyChanged;                       //реализация интерфейса INotifyPropertyChanged
        public ObservableCollection<SmtpServers> SmtpServersCol { get; set; }           //коллекция Smtp-серверов
        public ObservableCollection<SenderEmails> SenderEmailsCol { get; set; }         //коллекция емейлов отправителя
        public ObservableCollection<ListViewItemScheduler> SendMails { get; set; }      //коллекция писем для отложенной отправки
        public string MailSubject { get; set; }                                         //заголовок письма
        public string MailBody { get; set; }                                            //текст письма
        public string Login { get; set; }                                               //емейл-адрес отправителя
        public string SmtpServer { get; set; }                                          //Smtp-сервер для отправки
        public RelayCommand BtnSendAtOnceClickCommand { get; set; }                     //команда для отправки почты сразу
        public RelayCommand BtnSendScheduledClickCommand { get; set; }                  //команда для отправки почты отложено
        public RelayCommand BtnAddMail { get; set; }                                    //команда для добавления письма в список отложенной отправки

        public MainWindowViewModel()
        {
            SmtpServersCol = new ObservableCollection<SmtpServers>();
            SenderEmailsCol = new ObservableCollection<SenderEmails>();
            SendMails = new ObservableCollection<ListViewItemScheduler>();
            BtnSendAtOnceClickCommand = new RelayCommand(SendEmailsAtOnce);
            BtnSendScheduledClickCommand = new RelayCommand(SendEmailsScheduled);
            BtnAddMail = new RelayCommand(AddMail);
            GetSenderEmails();
            GetSmtpServers();
        }

        /// <summary>
        /// Добавляет новое письмо в список писем с запланированной отправкой
        /// </summary>
        void AddMail(object obj=null)
        {
            SendMails.Add(new ListViewItemScheduler(SendMails));
        }

        /// <summary>
        /// получение из БД емейлов отправителя
        /// </summary>
        void GetSenderEmails()
        {
            SenderEmailsCol.Clear();
            foreach (var item in DataAccessSenderEmails.GetSenderEmails())
            {
                SenderEmailsCol.Add(item);
            }
        }

        /// <summary>
        /// получение из БД списка Smtp-серверов
        /// </summary>
        void GetSmtpServers()
        {
            SmtpServersCol.Clear();
            foreach (var item in DataAccessSmtpServers.GetSmtpServers())
            {
                SmtpServersCol.Add(item);
            }
        }

        /// <summary>
        /// передача данных в класс, отвечающий за отправку почты
        /// </summary>
        void EmailSenderCreate(object obj)
        {
            var passwordBox = obj as PasswordBox;
            var password = passwordBox.Password;

            _emailSender = new EmailSendService(Login, password);
            _emailSender.MessageBeforeSendMail += MessageBoxShow;
            _emailSender.MessageMailSend += MessageBoxShow;

            _emailSender.SmtpServer = SmtpServer;
            _emailSender.SmtpPort = (from f in SmtpServersCol
                                    where f.Server == SmtpServer
                                    select f.Port).ToList<int>()[0];
        }

        /// <summary>
        /// возвращает список емейл-адресов получателей
        /// </summary>
        List<string> ListEmails()
        {
            List<string> listEmails = new List<string>();
            foreach (Emails em in EmailInfoViewModel.EmailsCol) listEmails.Add(em.Email);
            return listEmails;
        }

        /// <summary>
        /// мгновенная отправка почты
        /// </summary>
        void SendEmailsAtOnce(object obj)
        {
            if (!IsLoginFill()) return;
            if (string.IsNullOrEmpty(MailSubject) || string.IsNullOrEmpty(MailBody))
            {
                MessageBox.Show("Заголовок и тело письма не должны быть пустыми");
                return;
            }
            EmailSenderCreate(obj);
            _emailSender.SendMails(MailSubject, MailBody, ListEmails());
            MessageBox.Show("Отправка писем завершена");
        }

        bool IsLoginFill()
        {
            if (string.IsNullOrEmpty(Login))
            {
                MessageBox.Show("Выберите отправителя");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверяет даты в SendMails, если находит некорректную, возвращает дату string, иначе null 
        /// </summary>
        string SendDatesCorrect()
        {
            foreach (ListViewItemScheduler mail in SendMails)
            {
                if (mail.SendDateTime < DateTime.Now) return mail.SendDateTime.ToString();
            }
            return null;
        }

        /// <summary>
        /// отложенная отправка почты
        /// </summary>
        void SendEmailsScheduled(object obj)
        {
            if (SendMails.Count==0)
            {
                MessageBoxShow("Для запланированной отправки добавьте хотя бы одно письмо");
                return;
            }
            if (!IsLoginFill()) return;
            if (SendDatesCorrect()!=null)
            {
                MessageBoxShow($"Задайте для письма {SendDatesCorrect()} правильную дату и время \n(дата и время отправки не может быть раньше, чем настоящее время)");
                return;
            }

            SchedulerClass sc = new SchedulerClass(ListEmails());
            sc.MessageAfterOneSend += MessageBoxShow;
            sc.MessageAfterSendAll += MessageBoxShow;
            sc.MessageAfterPlanning += MessageBoxShow;

            try
            {
                //заполняем словарь
                sc.DatesEmailTexts = SendMails.ToDictionary(mail => mail.SendDateTime, mail => mail.MailText);
                #region
                //так тоже можно заполнить словарь, но тогда нет обращения к сеттеру словаря, где настроена сортировка:
                //foreach (ListViewItemScheduler mail in SendMails)
                //{
                //    if (mail.SendDateTime < DateTime.Now)
                //    {
                //        MessageBoxShow($"Для письма {mail.SendDateTime} заданы не верные дата и время \n(дата и время отправки писем не могут быть раньше, чем настоящее время). \nДанное письмо не будет отправлено.");
                //        continue;
                //    }
                //    sc.DatesEmailTexts.Add(mail.SendDateTime, mail.MailText);
                //}
                #endregion
            }
            catch (ArgumentException)
            {
                MessageBoxShow("Нельзя запланировать два письма на одинаковое время");
                return;
            }

            EmailSenderCreate(obj);
            sc.SendMails(_emailSender);
        }

        /// <summary>
        /// Вывод сообщения
        /// </summary>
        void MessageBoxShow(string msg)
        {
            MessageBox.Show(msg);
        }

        /// <summary>
        /// реализация интерфейса INotifyPropertyChanged
        /// </summary>
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
