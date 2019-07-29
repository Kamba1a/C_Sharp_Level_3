using System.Collections.ObjectModel;
using MailSender.Services;
using MailSender.DataClasses;
using System.Windows;

namespace MailSender.ViewModel
{
    /// <summary>
    /// ViewModel для EmailInfoView (UserControl для работы со списком получателей)
    /// </summary>
    public class EmailInfoModel
    {
        DataAccessService _dataAccessService;                                   //класс для работы с БД
        public static ObservableCollection<Emails> EmailsCol { get; set; }      //коллекция емейлов получателей (всех)
        public static ObservableCollection<Emails> EmailsSearch { get; set; }   //коллекция емейлов получателей (только искомых)
        public string Name { get; set; }                                        //принимает имя получателя для поиска в списке получателей
        public RelayCommand SearchCommand { get; set; }                         //команда для поиска в списке получателей
        public RelayCommand DeleteEmailCommand { get; set; }                    //команда для удаления получателя из списка (из базы и коллекций)

        public EmailInfoModel()
        {
            _dataAccessService = MainWindowModel.DAS;
            EmailsCol = new ObservableCollection<Emails>();
            EmailsSearch = new ObservableCollection<Emails>();
            SearchCommand = new RelayCommand(SearchEmail);
            DeleteEmailCommand = new RelayCommand(DeleteEmail);
            GetEmails();
            CopyEmails();
        }

        /// <summary>
        /// Получение из БД списка всех получателей
        /// </summary>
        void GetEmails()
        {
            EmailsCol.Clear();
            foreach (var item in _dataAccessService.GetEmails())
            {
                EmailsCol.Add(item);
            }
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
    }
}
