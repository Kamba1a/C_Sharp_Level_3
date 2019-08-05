using System.Windows;
using System.Windows.Controls;

// ПОКА ЧТО НЕ ИСПОЛЬЗУЕМЫЙ UserControl
// ПОКА ЧТО НЕ ИСПОЛЬЗУЕМЫЙ UserControl
// ПОКА ЧТО НЕ ИСПОЛЬЗУЕМЫЙ UserControl

namespace MailSender.Views
{
    /// <summary>
    /// Логика взаимодействия для ListViewItemScheduler.xaml
    /// </summary>
    public partial class ListViewItemScheduler : UserControl
    {
        public ListViewItemScheduler()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return datePickerTextBox.Text; }
            set { datePickerTextBox.Text = value; }
        }

        /// <summary>
        /// Открыть окно редактирования текста письма
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditEmailText_Click(object sender, RoutedEventArgs e)
        {
            //MailText mailText = new MailText();
            //mailText.ShowDialog();
        }
    }
}
