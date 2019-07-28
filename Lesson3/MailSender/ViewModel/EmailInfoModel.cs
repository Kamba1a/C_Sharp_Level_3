using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MailSender.ViewModel;
using MailSender.Services;
using System.ComponentModel;
using MailSender.DataClasses;
using System.Windows;

namespace MailSender.ViewModel
{
    public class EmailInfoModel
    {
        DataAccessService _dataAccessService;
        public static ObservableCollection<Emails> EmailsCol { get; set; }
        public static ObservableCollection<Emails> EmailsSearch { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand DeleteEmailCommand { get; set; }
        public string Name { get; set; }
        public Emails Email { get; set; }

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

        void GetEmails()
        {
            EmailsCol.Clear();
            foreach (var item in _dataAccessService.GetEmails())
            {
                EmailsCol.Add(item);
            }
        }

        void CopyEmails()
        {
            foreach (Emails e in EmailsCol)
            {
                EmailsSearch.Add(e);
            }
        }

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
