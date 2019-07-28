using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MailSender.Services;
using System.ComponentModel;
using MailSender.DataClasses;
using MailSender.ViewModel;
using System.Windows;

namespace MailSender.ViewModel 
{
    public class SaveEmailModel: INotifyPropertyChanged
    {
        DataAccessService _dataAccessService;
        private string _strEmail;
        private string _strName;
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand SaveCommand { get; set; }
        public Emails EmailInfo { get; set; }

        public SaveEmailModel()
        {
            _dataAccessService = MainWindowModel.DAS;
            SaveCommand = new RelayCommand(SaveEmail);
            EmailInfo = new Emails();
        }

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

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

