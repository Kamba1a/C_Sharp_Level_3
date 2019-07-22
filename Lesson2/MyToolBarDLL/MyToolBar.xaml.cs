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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyToolBarDLL
{
    /// <summary>
    /// Логика взаимодействия для MyToolBar.xaml
    /// </summary>
    public partial class MyToolBar : UserControl            //попытка сделать задание 7
    {
        public MyToolBar()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return label.Content.ToString(); }
            set { label.Content = value; }
        }

        public System.Collections.IEnumerable ItemsSource       //не работает
        {
            get { return comboBox.ItemsSource; }
            set { comboBox.ItemsSource = value; }
        }

        public int SelectedIndex
        {
            get { return comboBox.SelectedIndex; }
            set { comboBox.SelectedIndex = value; }
        }

        public object SelectedItem
        {
            get { return comboBox.SelectedItem; }
            set { comboBox.SelectedItem = value; }
        }

        public object SelectedValue
        {
            get { return comboBox.SelectedValue; }
            set { comboBox.SelectedValue = value; }
        }

        public string SelectedValuePath
        {
            get { return comboBox.SelectedValuePath; }
            set { comboBox.SelectedValuePath = value; }
        }

        public string DisplayMemberPath
        {
            get { return comboBox.DisplayMemberPath; }
            set { comboBox.DisplayMemberPath = value; }
        }

        public event RoutedEventHandler btnAddClick;
        public event RoutedEventHandler btnEditClick;
        public event RoutedEventHandler btnDeleteClick;

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            btnAddClick?.Invoke(sender, e);
        }
        private void btnEditClick_Click(object sender, RoutedEventArgs e)
        {
            btnEditClick?.Invoke(sender, e);
        }
        private void btnDeleteClick_Click(object sender, RoutedEventArgs e)
        {
            btnDeleteClick?.Invoke(sender, e);
        }
    }
}
