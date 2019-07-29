using MailSender.Services;
using System.ComponentModel;
using MailSender.DataClasses;
using System.Windows;

namespace MailSender.ViewModel 
{
    /// <summary>
    /// ViewModel для SaveEmailView (UserControl для добавления нового емейла получателя)
    /// </summary>
    public class SaveEmailModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;   //реализация интерфейса INotifyPropertyChanged
        DataAccessService _dataAccessService;                       //класс для работы с БД
        private string _strEmail;                                   //принимает емейл-адрес нового получателя (Emails.Email)
        private string _strName;                                    //принимает имя нового получателя (Emails.Name)
        public Emails EmailInfo { get; set; }                       //объединяет в себе  адрес/имя получателя для добавления в список получателей
        public RelayCommand SaveCommand { get; set; }               //команда для добавления нового получателя

        public SaveEmailModel()
        {
            _dataAccessService = MainWindowModel.DAS;
            SaveCommand = new RelayCommand(SaveEmail);
            EmailInfo = new Emails();
        }

        /// <summary>
        /// принимает имя нового получателя (Emails.Name)
        /// </summary>
        public string StrEmail
        {
            get
            {
                return _strEmail;
            }
            set
            {
                _strEmail = value;
                EmailInfo.Email = value;
                OnPropertyChanged(nameof(StrEmail));
            }
        }

        /// <summary>
        /// принимает имя нового получателя (Emails.Name)
        /// </summary>
        public string StrName
        {
            get
            {
                return _strName;
            }
            set
            {
                _strName = value;
                EmailInfo.Name = value;
                OnPropertyChanged(nameof(StrName));
            }
        }

        /// <summary>
        /// Добавляет новый емейл в список получателей (в БД и коллекции из класса EmailInfoModel)
        /// </summary>
        void SaveEmail(object obj=null)
        {
            if (string.IsNullOrEmpty(EmailInfo.Email))
            {
                MessageBox.Show("Введите Email получателя");
                return;
            }
            if (!CheckEmail.IsEmail(EmailInfo.Email))
            {
                MessageBox.Show("Введенный Email некорректен");
                return;
            }
            string res=_dataAccessService.CreateEmail(EmailInfo);
            if (res == EmailInfo.Email)
            {
                EmailInfoModel.EmailsCol.Add(EmailInfo);
                EmailInfoModel.EmailsSearch.Add(EmailInfo);
                EmailInfo = new Emails();
                StrEmail = null;
                StrName = null;
            }
            else MessageBox.Show(res);
        }

        /// <summary>
        /// Реализация интерфейса INotifyPropertyChanged
        /// </summary>
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

