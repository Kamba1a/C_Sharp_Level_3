using MailSender.Services;
using System.ComponentModel;
using MailSender.DataClasses;
using System.Windows;

namespace MailSender.ViewModel 
{
    /// <summary>
    /// ViewModel для SaveEmailView (UserControl для добавления нового емейла получателя)
    /// </summary>
    public class SaveEmailViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;   //реализация интерфейса INotifyPropertyChanged
        private string _strEmail;                                   //принимает емейл-адрес нового получателя (Emails.Email)
        private string _strName;                                    //принимает имя нового получателя (Emails.Name)
        public Emails EmailInfo { get; set; }                       //объединяет в себе адрес/имя получателя для добавления в список получателей
        public RelayCommand SaveCommand { get; set; }               //команда для добавления нового получателя

        public SaveEmailViewModel()
        {
            SaveCommand = new RelayCommand(SaveEmail); // StrEmail => !string.IsNullOrEmpty(StrEmail as string)); //bool параметр почему-то не работает (button всегда заблокирована)
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

        public string Error => throw new System.NotImplementedException();
        //public string Error { get { return null; } }

        public string this[string propertyName]
        {
            get
            {
                if (propertyName == "StrEmail")
                {
                    if (StrEmail != null && !CheckEmail.IsEmail(StrEmail)) return "Некорректный e-mail адрес";
                }
                return null;
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
            string res= DataAccessEmails.CreateEmail(EmailInfo);
            if (res == EmailInfo.Email)
            {
                EmailInfoViewModel.EmailsCol.Add(EmailInfo);
                EmailInfoViewModel.EmailsSearch.Add(EmailInfo);
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

