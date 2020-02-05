using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System;
using System.ComponentModel;

namespace MailSender.Views
{
    /// <summary>
    /// Логика взаимодействия для ListViewItemScheduler.xaml
    /// </summary>
    public partial class ListViewItemScheduler : UserControl, INotifyPropertyChanged
    {
        DateTime _sendDateTime;                                         //дата и время отправки письма
        ObservableCollection<ListViewItemScheduler> _mailsScheduler;    //коллекция писем для запланированной отправки
        public event PropertyChangedEventHandler PropertyChanged;       //реализация интерфейса INotifyPropertyChanged

        public ListViewItemScheduler(ObservableCollection<ListViewItemScheduler> mailsScheduler)
        {
            InitializeComponent();
            _mailsScheduler = mailsScheduler;
            SendDateTime = DateTime.Now;
        }

        /// <summary>
        /// Текст письма
        /// </summary>
        public string MailText { get; set; }

        /// <summary>
        /// Дата и время отправки письма
        /// </summary>
        public DateTime SendDateTime {
            get { return _sendDateTime; }
            set
            {
                _sendDateTime = value;
                OnPropertyChanged(nameof(SendDateTime));
            }
        }

        /// <summary>
        /// Кнопка "//" - открывает окно редактирования текста письма
        /// </summary>
        private void BtnEditMailText_Click(object sender, RoutedEventArgs e)
        {
            MailTextWindow mailTextWindow = new MailTextWindow(MailText);
            mailTextWindow.ShowDialog();
            if (mailTextWindow.DialogResult == true) MailText = mailTextWindow.MailText;
        }

        /// <summary>
        /// Кнопка "-" - удаляет письмо из коллекции запланированн
        /// </summary>
        private void BtnDelMail_Click(object sender, RoutedEventArgs e)
        {
            _mailsScheduler.Remove(this);
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
