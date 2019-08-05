using System.Windows;
using System.Windows.Controls;

namespace MailSender.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Пункт меню "Закрыть" - закрывает программу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MiClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Кнопка с часами - переход на вкладку планировщика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnClock_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabPlanner;
        }

        /// <summary>
        /// При выборе емейла отправителя в TextBox заполняется smtp-сервер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbSenderSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtbxServer.Text = cbSenderSelect.SelectedValue.ToString();
        }

        /// <summary>
        /// Кнопка "Редактировать письмо" элемента в списке писем для запланированной отправки
        /// </summary>
        public void BtnEditEmailText_Click(object sender, RoutedEventArgs e)
        {
            MailTextWindow mailText = new MailTextWindow((sender as Button).Tag);
            mailText.ShowDialog();
        }
    }
}
