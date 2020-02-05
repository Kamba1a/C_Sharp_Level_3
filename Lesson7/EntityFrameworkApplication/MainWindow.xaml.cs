using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Words.NET;


/*
====================================================
Практическое задание из методички:
"Приложение с использованием Entity Framework.
Формирование отчетов в виде документов Word и Excel"
====================================================
Мои примечания:
Библиотека Microsoft Report Viewer, используемая в методичке, похоже не устанавливается в последних версиях VS.
Преподаватель тоже упомянул о том, что не смог поставить эту библиотеку.
В итоге он показал 2 других способа, которыми он пользовался в реальных проектах для формирования отчетов:
1. Программа Open XML SDK 2.5 for Microsoft Office
2. Библиотека Xceed DocX
Я выбрала второй способ, но вместо использования контрола winForms:ReportViewer просто сделала кнопки для формирования и просмотра отчета в формате *.docx
Проблема только в том, что эти библиотеки работают только с Word, а по заданию, нужен также Excel

Нашла статью на Хабре про библиотеки для работы с Excel https://habr.com/ru/post/235903/ :
1. Microsoft Office Interop Excel - устарела? 
2. Microsoft Open XML SDK
3. EPPlus - как я поняла, наиболее удобная и простая библиотека
В задании не стала реализовывать работу с этими библиотеками, думаю при необходимости разберусь, главное суть отчетности уловила.

Кроме всего, в процессе поиска инфы, нашла легчайший способ напрямую распечатать любой XAML элемент, что тоже можно использовать, как простой вид отчета.
Но у этого способа есть недостатки - нет контроля над полями страницы при печати, и нет возможности разбить содержимое на страницы.
Подробнее - https://professorweb.ru/my/WPF/documents_WPF/level28/28_9.php
*/


namespace EntityFrameworkApplication
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseContainer database;
        public List<Track> TracksList;
        public MainWindow()
        {
            InitializeComponent();
            database = new DatabaseContainer();
            Note.Text = "Библиотека Microsoft Report Viewer, используемая в методичке, не устанавливается в последних версиях VS,"
                + "поэтому использовалась другая библиотека, с помощью которой таблица выгружается в документ Word";
        }

        /// <summary>
        /// Событие после загрузки окна - загрузить из БД список треков
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ReloadTracksList();
        }

        /// <summary>
        /// Кнопка "Добавить" - добавляет трек в БД и обновляет список
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ArtistNameTxt.Text) && !string.IsNullOrWhiteSpace(TrackNameTxt.Text))
            {
                AddNewTrack();
                ReloadTracksList();
            }
        }

        /// <summary>
        /// Добавляет новый трек в БД
        /// </summary>
        private void AddNewTrack()
        {
            database.Tracks.Add(
                new Track
                {
                    ArtistName = ArtistNameTxt.Text,
                    TrackName = TrackNameTxt.Text
                });
            database.SaveChanges();     //ОБЯЗАТЕЛЬНО для сохранения изменений в БД!
        }

        /// <summary>
        /// Загрузка из БД списка треков в список List
        /// </summary>
        private void ReloadTracksList()
        {
            TracksList = database.Tracks.ToList();
            Grid.ItemsSource = TracksList;      //в конструкторе и так прописан ItemsSource, но без этой строки не отображает данные - нужно просто для обновления? 
        }

        /// <summary>
        /// Кнопка "Сформировать отчет"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            using (DocX document = DocX.Create("CsharpReportFile.docx"))
            {
                //document.MarginBottom = 28.3f; //1cm (необязательно - для примера)

                //Заголовок
                document.InsertParagraph("Треклист");
                document.Paragraphs[0].FontSize(16);
                document.Paragraphs[0].Bold();
                document.Paragraphs[0].Alignment = Xceed.Document.NET.Alignment.center;
                document.Paragraphs[0].AppendLine("");

                //Таблица
                Xceed.Document.NET.Table table = document.AddTable(database.Tracks.Count(), 3);
                table.Alignment = Xceed.Document.NET.Alignment.center;
                table.AutoFit = Xceed.Document.NET.AutoFit.Contents;
                int i = 0;
                foreach (Track track in database.Tracks)
                {
                    table.Rows[i].Cells[0].Paragraphs[0].Append(track.TrackId.ToString());
                    table.Rows[i].Cells[1].Paragraphs[0].Append(track.TrackName.ToString());
                    table.Rows[i].Cells[2].Paragraphs[0].Append(track.ArtistName.ToString());
                    i++;
                }
                document.InsertTable(table);

                document.Save();
                MessageBox.Show("Отчет сформирован");
                
                OpenReportButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Кнопка "Открыть сформированный отчет"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenReportButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("CsharpReportFile.docx");
        }


        /// <summary>
        /// Кнопка "Распечатать таблицу" - выводит на печать XAML элемент x:Name="Grid"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintTableButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(this.Grid, "Печать трек-листа");
            }
        }

        /// <summary>
        /// Вручную загружает данные из БД, если вдруг не подгрузилось
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadTracksList();
        }
    }
}
