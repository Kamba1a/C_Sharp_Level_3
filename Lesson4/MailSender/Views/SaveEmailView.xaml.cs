using System.Windows.Controls;


namespace MailSender.Views
{
    /// <summary>
    /// Логика взаимодействия для SaveEmailView.xaml
    /// </summary>
    public partial class SaveEmailView : UserControl
    {
        public SaveEmailView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// для валидации вводимых данных
        /// </summary>
        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                ((Control)sender).ToolTip = e.Error.ErrorContent.ToString();
                btnSave.IsEnabled = false;
            }
            else
            {
                ((Control)sender).ToolTip =null;
                btnSave.IsEnabled = true;
            }
        }

        /// <summary>
        /// кнопка "Сохранить" (получателя) - неактивна при пустом TextBox "Email"
        /// </summary>
        //private void TxtbxEmail_TextChanged(object sender, TextChangedEventArgs e)        //конфликтует с TextBox_Error
        //{
        //    if (string.IsNullOrEmpty(txtbxEmail.Text)) btnSave.IsEnabled = false;
        //    else btnSave.IsEnabled = true;
        //}
    }
}
