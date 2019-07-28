using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using MailSender.ViewModel;
using MailSender.Services;
using System.ComponentModel;
using MailSender.DataClasses;
using System.Security;

namespace MailSender.ViewModel
{
    public class MainWindowModel: INotifyPropertyChanged
    {
        public static DataAccessService DAS;
        public event PropertyChangedEventHandler PropertyChanged;

        public static ObservableCollection<SmtpServers> SmtpServersCol { get; set; }
        public static ObservableCollection<SenderEmails> SenderEmailsCol { get; set; }
        public RelayCommand BtnSendAtOnceClickCommand { get; set; }
        public RelayCommand BtnSendClickCommand { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public string Login { get; set; }
        public string SmtpServer { get; set; }

        public MainWindowModel()
        {
            DAS = new DataAccessService();
            SmtpServersCol = new ObservableCollection<SmtpServers>();
            SenderEmailsCol = new ObservableCollection<SenderEmails>();
            BtnSendAtOnceClickCommand = new RelayCommand(SendEmailsAtOnce);
            BtnSendClickCommand = new RelayCommand(SendEmails);
            GetSenderEmails();
            GetSmtpServers();
        }

        void GetSenderEmails()
        {
            SenderEmailsCol.Clear();
            foreach (var item in DAS.GetSenderEmails())
            {
                SenderEmailsCol.Add(item);
            }
        }

        void GetSmtpServers()
        {
            SmtpServersCol.Clear();
            foreach (var item in DAS.GetSmtpServers())
            {
                SmtpServersCol.Add(item);
            }
        }

        void SendEmailsAtOnce(object obj)
        {
            if (string.IsNullOrEmpty(Login))
            {
                MessageBox.Show("Выберите отправителя");
                return;
            }

            //if ((obj as SecureString).Length==0)    //непонятно, как проверить SecureString на null, Length тоже не работает
            //{
            //    MessageBox.Show("Укажите пароль отправителя");
            //    return;
            //}

            if (string.IsNullOrEmpty(MailSubject) || string.IsNullOrEmpty(MailBody))
            {
                MessageBox.Show("Заголовок и тело письма не должны быть пустыми");
                //tabControl.SelectedItem = tbitmMail;      //как сделать переключение вкладок?
                return;
            }

            EmailSendService emailSender = new EmailSendService(Login, obj as SecureString);
            emailSender.MessageBeforeSendMail += msg => MessageBox.Show(msg);
            emailSender.MessageMailSend += msg => MessageBox.Show(msg);

            List<string> listEmails = new List<string>();
            foreach (Emails em in EmailInfoModel.EmailsCol) listEmails.Add(em.Email);

            emailSender.SmtpServer = SmtpServer;
            //emailSender.SmtpPort = (from f in SmtpServersCol
            //                        where f.Server == SmtpServer
            //                        select f.Port).ToList<int>()[0];

            emailSender.SendMails(MailSubject, MailBody, listEmails);
            MessageBox.Show("Отправка писем завершена");
        }

        void SendEmails(object obj)
        {
            //if (txtbxSubject.Text == "" || txtbxBody.Text == "")
            //{
            //    MessageBox.Show("Заголовок и тело письма не должны быть пустыми");
            //    tabControl.SelectedItem = tbitmMail;
            //    return;
            //}

            //SchedulerClass sc = new SchedulerClass();
            //TimeSpan tsSendTime = sc.GetSendTime(tbTimePicker.Text);
            //if (tsSendTime == new TimeSpan())
            //{
            //    MessageBox.Show("Некорректный формат даты");
            //    return;
            //}
            //DateTime dtSendDateTime = (cldSchedulDateTimes.SelectedDate ?? DateTime.Today).Add(tsSendTime);
            //if (dtSendDateTime < DateTime.Now)
            //{
            //    MessageBox.Show("Дата и время отправки писем не могут быть раньше, чем настоящее время");
            //    return;
            //}

            //WpfEmailSendService emailSender = new WpfEmailSendService(cbSenderSelect.Text, cbSenderSelect.SelectedValue.ToString());
            //emailSender.MessageAuthorization += msg => MessageBox.Show(msg);
            //emailSender.MessageMailSend += msg => MessageBox.Show(msg);

            //var locator = (ViewModelLocator)FindResource("Locator");

            //emailSender.SmtpServer = txtbxSmtpServer.Text;
            //sc.SendMails(dtSendDateTime, emailSender, locator.Main.Emails);
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
