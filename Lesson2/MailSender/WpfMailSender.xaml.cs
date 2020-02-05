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

            tbitmMail.DataContext = this;   //Когда пытаюсь задать DataContext через Xaml, выдает ошибку, что свойство должно быть членом класса. 
                                            //Грубый пример:
                                            //< Element >
                                            //  < Element.DataContext >
                                            //      <local: MyClassInCodeBehind />                  - указываю имя класса
                                            //  < Element.DataContext >
                                            //  < TextBox Text =”{Binding MyStaticProperty}”/>      - здесь подчеркивает ошибку
                                            //</ Element >
                                            //Как можно задать DataContext через Xaml?

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
            //string strPassword = cbSenderSelect.SelectedValue.ToString();

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

            List<string> listEmails = new List<string>();
            foreach (Emails em in dgEmails.ItemsSource) listEmails.Add(em.Value);

            WpfEmailSendService emailSender = new WpfEmailSendService(strLogin, strPassword);
            emailSender.ShowMessage += msg=>MessageBox.Show(msg);

            //emailSender.SmtpServer = cbSmtpServer.Text;
            emailSender.SmtpServer = txtbxSmtpServer.Text;
            emailSender.SmtpPort = (from f in DBClass.SmtpServers
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
            emailSender.ShowMessage += msg=>MessageBox.Show(msg);
            emailSender.SmtpServer = txtbxSmtpServer.Text;
            //emailSender.SmtpServer = cbSmtpServer.Text;
            sc.SendMails(dtSendDateTime, emailSender, (IQueryable<Emails>)dgEmails.ItemsSource);
        }
    }
}
