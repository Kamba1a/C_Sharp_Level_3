using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using MailSender.Services;
using GalaSoft.MvvmLight.Command;

namespace MailSender.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>

        ObservableCollection<Emails> _Emails;
        ObservableCollection<SmtpServers> _smtpServers;
        ObservableCollection<SenderEmails> _senderEmails;
        IDataAccessService _serviceProxy;
        Emails _EmailInfo;

        public RelayCommand ReadEmailsCommand { get; set; }
        public RelayCommand ReadSenderEmailsCommand { get; set; }
        public RelayCommand ReadSmtpServersCommand { get; set; }
        public RelayCommand<Emails> SaveCommand { get; set; }

        public MainViewModel(IDataAccessService servProxy)
        {
            _serviceProxy = servProxy;
            Emails = new ObservableCollection<Emails>();
            SmtpServers = new ObservableCollection<SmtpServers>();
            SenderEmails = new ObservableCollection<SenderEmails>();
            ReadEmailsCommand = new RelayCommand(GetEmails);
            ReadSenderEmailsCommand = new RelayCommand(GetSenderEmails);
            ReadSmtpServersCommand = new RelayCommand(GetSmtpServers);
            ReadEmailsCommand = new RelayCommand(GetEmails);
            SaveCommand = new RelayCommand<Emails>(SaveEmail);
        }

        public Emails EmailInfo
        {
            get { return _EmailInfo; }
            set
            {
                _EmailInfo = value;
                RaisePropertyChanged(nameof(EmailInfo));
            }
        }

        public ObservableCollection<Emails> Emails
        {
            get { return _Emails; }
            set
            {
                _Emails = value;
                RaisePropertyChanged(nameof(Emails));
            }
        }

        public ObservableCollection<SmtpServers> SmtpServers
        {
            get
            {
                return _smtpServers;
            }
            set
            {
                _smtpServers = value;
                RaisePropertyChanged(nameof(SmtpServers));
            }
        }

        public ObservableCollection<SenderEmails> SenderEmails
        {
            get
            {
                return _senderEmails;
            }
            set
            {
                _senderEmails = value;
                RaisePropertyChanged(nameof(SenderEmails));
            }
        }

        /// <summary>
        /// данные читаются из БД и помещаются в наблюдаемую коллекцию Emails
        /// </summary>
        void GetEmails()
        {
            Emails.Clear();
            foreach (var item in _serviceProxy.GetEmails())
            {
                Emails.Add(item);
            }
        }

        void GetSenderEmails()
        {
            SenderEmails.Clear();
            foreach (var item in _serviceProxy.GetSenderEmails())
            {
                SenderEmails.Add(item);
            }
        }

        void GetSmtpServers()
        {
            SmtpServers.Clear();
            foreach (var item in _serviceProxy.GetSmtpServers())
            {
                SmtpServers.Add(item);
            }
        }

        void SaveEmail(Emails email)
        {
            EmailInfo.Id = _serviceProxy.CreateEmail(email);
            if (EmailInfo.Id != 0)
            {
                Emails.Add(EmailInfo);
                RaisePropertyChanged(nameof(EmailInfo));
            }
        }
    }
}