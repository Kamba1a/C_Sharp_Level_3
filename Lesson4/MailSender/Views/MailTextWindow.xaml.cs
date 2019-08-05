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
using System.Windows.Shapes;

namespace MailSender.Views
{
    /// <summary>
    /// Логика взаимодействия для MailText.xaml
    /// </summary>
    public partial class MailTextWindow : Window
    {
        /// <summary>
        /// Привязка окна к конкретному элементу списка
        /// </summary>
        DateTime SendDateTime {get; set;}

        public MailTextWindow(object obj)
        {
            InitializeComponent();
            SendDateTime = (DateTime)obj;
        }

        /// <summary>
        /// Кнопка "Отмена" - закрыть окно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
