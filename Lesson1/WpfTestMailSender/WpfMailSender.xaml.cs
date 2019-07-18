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
using System.Text.RegularExpressions;

//Ольга Назарова

namespace WpfTestMailSender
{
    public partial class WpfMailSender : Window //пока с MVVM не особо знакома, поэтому вся основная логика здесь
    {
        public static WpfMailSender ThisWindow { get; private set; } //указатель на это окно для других классов (не знаю, как по-другому передать)
        ObservableCollection<string> _sendEmailsList = new ObservableCollection<string> { "tst_for_gkbrns1@inbox.ru", "tst_for_gkbrns2@list.ru", "tst_for_gkbrns3@mail.ru" };

        public WpfMailSender()
        {
            InitializeComponent();
            ThisWindow = this;
            lstbxSendMails.ItemsSource = _sendEmailsList;
        }

        /// <summary>
        /// Кнопка - отправка почты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            if (IsAllFieldsFilled())
            {
                if (IsEmail(txtbxSenderMail.Text))
                {
                    EmailSendServiceClass emailSendServiceClass = new EmailSendServiceClass(txtbxSenderMail.Text, passwordBox.Password);
                    if (txtbxSmtpServer.Text != "") emailSendServiceClass.SmtpServer = txtbxSmtpServer.Text;
                    if (txtbxSmtpPort.Text != "" && int.TryParse(txtbxSmtpPort.Text, out int p)) emailSendServiceClass.SmtpPort = p;
                    emailSendServiceClass.SendMails(txtbxSubject.Text, txtbxBody.Text, _sendEmailsList);
                    ShowSendEndWindow();
                }
                else MessageBox.Show("Некорректный e-mail отправителя");
            }
            else MessageBox.Show("Заполнены не все обязательные поля");
        }

        /// <summary>
        /// Проверка на заполненность обязательных полей в окне отправки
        /// </summary>
        /// <returns></returns>
        bool IsAllFieldsFilled()
        {
            return txtbxSubject.Text != "" && txtbxBody.Text != "" && txtbxSenderMail.Text != "" && passwordBox.Password != "" && lstbxSendMails.Items.Count > 0;
        }

        /// <summary>
        /// Показ окна "Работа завершена"
        /// </summary>
        void ShowSendEndWindow()
        {
            SendEndWindow sew = new SendEndWindow();
            sew.Owner = this;
            sew.ShowDialog();
        }

        /// <summary>
        /// Проверка на корректность E-mail адреса
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool IsEmail(string email)
        {
            return new Regex(@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$").IsMatch(email);
        }

        /// <summary>
        /// Кнопка - добавляет введенный адрес в общий список рассылки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddSendMail_Click(object sender, RoutedEventArgs e)
        {
            if (IsEmail(txtbxSendMail.Text)) _sendEmailsList.Add(txtbxSendMail.Text);
            else MessageBox.Show("Некорректный e-mail");
        }

        /// <summary>
        /// Кнопка - удаляет выбранный адрес из общего списка рассылки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelSendMail_Click(object sender, RoutedEventArgs e)
        {
            _sendEmailsList.Remove(lstbxSendMails.SelectedItem as string);
        }
    }
}
