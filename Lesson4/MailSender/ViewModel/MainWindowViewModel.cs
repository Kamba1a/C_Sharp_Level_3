using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using MailSender.Services;
using System.ComponentModel;
using MailSender.DataClasses;
using System.Security;
using System;
using System.Linq;

namespace MailSender.ViewModel
{
    /// <summary>
    /// ViewModel для MainWindowView (главное окно приложения)
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;                       //реализация интерфейса INotifyPropertyChanged
        public ObservableCollection<SmtpServers> SmtpServersCol { get; set; }           //коллекция Smtp-серверов
        public ObservableCollection<SenderEmails> SenderEmailsCol { get; set; }         //коллекция емейлов отправителя
        public string MailSubject { get; set; }                                         //заголовок письма
        public string MailBody { get; set; }                                            //текст письма
        public string Login { get; set; }                                               //емейл-адрес отправителя
        public string SmtpServer { get; set; }                                          //Smtp-сервер для отправки
        public string SendTime { get; set; }                                            //время отправки почты
        public DateTime SendDate { get; set; }                                          //дата отправки почты
        public RelayCommand BtnSendAtOnceClickCommand { get; set; }                     //команда для отправки почты сразу
        public RelayCommand BtnSendScheduledClickCommand { get; set; }                  //команда для отправки почты отложено
        EmailSendService emailSender;                                                   //класс для отправки почты

        public MainWindowViewModel()
        {
            SmtpServersCol = new ObservableCollection<SmtpServers>();
            SenderEmailsCol = new ObservableCollection<SenderEmails>();
            BtnSendAtOnceClickCommand = new RelayCommand(SendEmailsAtOnce);
            BtnSendScheduledClickCommand = new RelayCommand(SendEmailsScheduled);
            GetSenderEmails();
            GetSmtpServers();
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
        /// Проверка на заполненность обязательных полей
        /// </summary>
        bool FieldsIsEmpty()
        {
            if (string.IsNullOrEmpty(Login))
            {
                MessageBox.Show("Выберите отправителя");
                return true;
            }
            //if ((obj as SecureString).Length==0)                                  //непонятно, как проверить SecureString на null, Length всегда =0
            //{
            //    MessageBox.Show("Укажите пароль отправителя");
            //    return true;
            //}
            if (string.IsNullOrEmpty(MailSubject) || string.IsNullOrEmpty(MailBody))
            {
                MessageBox.Show("Заголовок и тело письма не должны быть пустыми");
                //tabControl.SelectedItem = tbitmMail;                              //как сделать переключение вкладок из ViewModel?
                return true;
            }
            return false;
        }

        /// <summary>
        /// передача данных в класс, отвечающий за отправку почты
        /// </summary>
        void EmailSenderCreate(SecureString password)
        {
            emailSender = new EmailSendService(Login, password);
            emailSender.MessageBeforeSendMail += msg => MessageBox.Show(msg);       //подписка на вывод сообщения непосредственно перед отправкой писем
            emailSender.MessageMailSend += msg => MessageBox.Show(msg);             //подписка на вывод сообщений после отправки писем

            emailSender.SmtpServer = SmtpServer;
            emailSender.SmtpPort = (from f in SmtpServersCol
                                    where f.Server == SmtpServer
                                    select f.Port).ToList<int>()[0];
        }

        /// <summary>
        /// возвращает список емейл-адресов получателей
        /// </summary>
        List<string> ListEmails()
        {
            List<string> listEmails = new List<string>();
            //foreach (Emails em in EmailInfoViewModel.EmailsCol) listEmails.Add(em.Email);
            foreach (Emails em in EmailInfoViewModel.EmailsCol) listEmails.Add(em.Email);
            return listEmails;
        }

        /// <summary>
        /// мгновенная отправка почты
        /// </summary>
        void SendEmailsAtOnce(object obj)
        {
            if (FieldsIsEmpty()) return;
            EmailSenderCreate(obj as SecureString);
            emailSender.SendMails(MailSubject, MailBody, ListEmails());
            MessageBox.Show("Отправка писем завершена");
        }

        /// <summary>
        /// отложенная отправка почты
        /// </summary>
        void SendEmailsScheduled(object obj)
        {
            SchedulerClass sc = new SchedulerClass(MailSubject, MailBody, ListEmails());
            TimeSpan tsSendTime = sc.GetSendTime(SendTime);

            if (tsSendTime == new TimeSpan())
            {
                MessageBox.Show("Некорректный формат даты");
                return;
            }

            DateTime dtSendDateTime = (SendDate!=new DateTime() ? SendDate: DateTime.Today).Add(tsSendTime);

            if (dtSendDateTime < DateTime.Now)
            {
                MessageBox.Show("Дата и время отправки писем не могут быть раньше, чем настоящее время");
                return;
            }

            if (FieldsIsEmpty()) return;
            EmailSenderCreate(obj as SecureString);
            sc.SendMails(dtSendDateTime, emailSender);
            MessageBox.Show($"Отправка писем запланирована на {dtSendDateTime}");
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
