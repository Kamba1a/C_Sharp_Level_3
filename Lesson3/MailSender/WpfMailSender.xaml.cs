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
using WpfEmailSendServiceDLL;
using System.Collections.ObjectModel;
using MailSender.ViewModel;

namespace MailSender
{
    /// <summary>
    /// Логика взаимодействия для WpfMailSender.xaml
    /// </summary>
    public partial class WpfMailSender : Window
    {
        public static string MailSubject { get; set; }
        public static string MailBody { get; set; }

        public WpfMailSender()
        {
            InitializeComponent();
            DataContext = App.MVM;
        }

        private void MiClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnClock_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabPlanner;
        }

        private void BtnSendAtOnce_Click(object sender, RoutedEventArgs e)
        {
            string strLogin = cbSenderSelect.Text;
            string strPassword = txtbxPassword.Password;

            if (string.IsNullOrEmpty(strLogin))
            {
                MessageBox.Show("Выберите отправителя");
                return;
            }

            if (string.IsNullOrEmpty(strPassword))
            {
                MessageBox.Show("Укажите пароль отправителя");
                return;
            }

            if (txtbxSubject.Text == "" || txtbxBody.Text == "")
            {
                MessageBox.Show("Заголовок и тело письма не должны быть пустыми");
                tabControl.SelectedItem = tbitmMail;
                return;
            }

            WpfEmailSendService emailSender = new WpfEmailSendService(strLogin, strPassword);
            emailSender.MessageAuthorization += msg=>MessageBox.Show(msg);
            emailSender.MessageMailSend += msg => MessageBox.Show(msg);

            var locator = (ViewModelLocator)FindResource("Locator");

            List<string> listEmails = new List<string>();
            foreach (Emails em in locator.Main.Emails) listEmails.Add(em.Value);

            emailSender.SmtpServer = txtbxSmtpServer.Text;
            emailSender.SmtpPort = (from f in locator.Main.SmtpServers
                                    where f.Server == txtbxSmtpServer.Text
                                    select f.Port).ToList<int>()[0];

            emailSender.SendMails(MailSubject, MailBody, listEmails);
            MessageBox.Show("Отправка писем завершена");
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (txtbxSubject.Text == "" || txtbxBody.Text == "")
            {
                MessageBox.Show("Заголовок и тело письма не должны быть пустыми");
                tabControl.SelectedItem = tbitmMail;
                return;
            }

            SchedulerClass sc = new SchedulerClass();
            TimeSpan tsSendTime = sc.GetSendTime(tbTimePicker.Text);
            if (tsSendTime == new TimeSpan())
            {
                MessageBox.Show("Некорректный формат даты");
                return;
            }
            DateTime dtSendDateTime = (cldSchedulDateTimes.SelectedDate ?? DateTime.Today).Add(tsSendTime);
            if (dtSendDateTime < DateTime.Now)
            {
                MessageBox.Show("Дата и время отправки писем не могут быть раньше, чем настоящее время");
                return;
            }

            WpfEmailSendService emailSender = new WpfEmailSendService(cbSenderSelect.Text, cbSenderSelect.SelectedValue.ToString());
            emailSender.MessageAuthorization += msg => MessageBox.Show(msg);
            emailSender.MessageMailSend += msg => MessageBox.Show(msg);

            var locator = (ViewModelLocator)FindResource("Locator");

            emailSender.SmtpServer = txtbxSmtpServer.Text;
            sc.SendMails(dtSendDateTime, emailSender, locator.Main.Emails);
        }
    }
}
