using System.Collections.ObjectModel;
using MailSender.Services;
using MailSender.DataClasses;
using System.Windows;
using System.ComponentModel;

namespace MailSender.ViewModel
{
    /// <summary>
    /// ViewModel для EmailInfoView (UserControl для работы со списком получателей)
    /// </summary>
    public class EmailInfoModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        DataAccessService _dataAccessService;                                   //класс для работы с БД
        public static ObservableCollection<Emails> EmailsCol { get; set; }      //коллекция емейлов получателей (всех)
        public static ObservableCollection<Emails> EmailsSearch { get; set; }   //коллекция емейлов получателей (только искомых)
        public RelayCommand SearchCommand { get; set; }                         //команда для поиска в списке получателей
        public RelayCommand DeleteEmailCommand { get; set; }                    //команда для удаления получателя из списка (из базы и коллекции)
        public RelayCommand TempDeleteEmailCommand { get; set; }                //команда для удаления получателя из списка (только из коллекции, не из БД)
        public RelayCommand GetEmailsCommand { get; set; }                      //команда получения списка получателей из БД
        string _name;                                                           //принимает имя получателя для поиска в списке получателей

        public EmailInfoModel()
        {
            _dataAccessService = MainWindowModel.DAS;
            EmailsCol = new ObservableCollection<Emails>();
            EmailsSearch = new ObservableCollection<Emails>();
            SearchCommand = new RelayCommand(SearchEmail);
            DeleteEmailCommand = new RelayCommand(DeleteEmail);
            TempDeleteEmailCommand = new RelayCommand(TempDeleteEmail);
            GetEmailsCommand = new RelayCommand(GetEmails);
            GetEmails();
        }

        /// <summary>
        /// принимает имя получателя для поиска в списке получателей
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Получение из БД списка всех получателей
        /// </summary>
        void GetEmails(object obj=null)
        {
            EmailsCol.Clear();
            foreach (var item in _dataAccessService.GetEmails())
            {
                EmailsCol.Add(item);
            }
            EmailsSearch.Clear();
            CopyEmails();
            Name = null;
        }

        /// <summary>
        /// Копирование списка получателей из коллекции EmailsCol в EmailsSearch
        /// </summary>
        void CopyEmails()
        {
            foreach (Emails e in EmailsCol)
            {
                EmailsSearch.Add(e);
            }
        }

        /// <summary>
        /// Наполнение коллекции EmailsSearch только искомыми получателями
        /// </summary>
        void SearchEmail(object obj=null)
        {
            EmailsSearch.Clear();
            if (string.IsNullOrEmpty(Name)) CopyEmails();
            else
            {
                foreach (Emails s in EmailsCol)
                {
                    if (string.IsNullOrEmpty(s.Name)) continue;
                    if (s.Name.ToLower().Contains(Name.ToLower()))
                    {
                        EmailsSearch.Add(s);
                    }
                }
            }
        }

        /// <summary>
        /// Удаление получателя из БД и коллекций
        /// </summary>
        void DeleteEmail(object obj)
        {
            if (obj == null) MessageBox.Show("Выберите получателя");
            else
            {
                _dataAccessService.DeleteEmail(obj as Emails);
                GetEmails();
                SearchEmail();
            }
        }

        /// <summary>
        /// Удаление получателя только из коллекции (не из БД)
        /// </summary>
        void TempDeleteEmail(object obj)
        {
            if (obj == null) MessageBox.Show("Выберите получателя");
            else
            {
                EmailsCol.Remove(obj as Emails);
                SearchEmail();
            }
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
