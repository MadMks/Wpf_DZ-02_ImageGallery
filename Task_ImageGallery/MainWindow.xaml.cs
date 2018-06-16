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
using System.Windows.Forms;
using WinForms = System.Windows.Forms;
using System.IO;
using Wpf = System.Windows.Controls;
using System.ComponentModel;

namespace Task_ImageGallery
{

    delegate void UpdateProgressBarDelegate(DependencyProperty dp, object value);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Путь поиска файлов.
        /// </summary>
        private string path;    // TODO delete
        //BackgroundWorker worker;

        public MainWindow()
        {
            InitializeComponent();

            //worker = new BackgroundWorker();
            //worker.DoWork += Worker_DoWork;
        }

        //private void Worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    AddToTheListFoundFilesImages();
        //}

        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            // TODO диалог открытия папки
            using(FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.LocalizedResources;
                folderBrowserDialog.ShowNewFolderButton = false;


                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    // TODO считаю кол-во картинок в папке для прогресБара

                    this.path = folderBrowserDialog.SelectedPath;

                    AddToTheListFoundFilesImages(folderBrowserDialog.SelectedPath);     // TODO (string path)


                    if (this.listBox.Items.Count > 0)
                    {
                        SettingFirstImageInSlider();
                    }
                }
            }
        }

        private void SettingFirstImageInSlider()
        {
            imageSlider.Source = ((this.listBox.Items[0] as Wpf.Button).Content as Image).Source;
        }


        private void AddToTheListFoundFilesImages(string selectedPath)
        {
            this.listBox.Items.Clear();
            this.progressBar.Value = 0;

            UpdateProgressBarDelegate updProgress = new UpdateProgressBarDelegate(progressBar.SetValue);
            //double value = 0;

            

            DirectoryInfo directoryInfo = new DirectoryInfo(this.path);

            this.progressBar.Maximum = directoryInfo.GetFiles().Count();    // pb


            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (IsImageExtensions(fileInfo.Extension) == true)
                {
                    this.AddToListBoxImage(fileInfo);

                    //this.progressBar.Value++;
                    //System.Threading.Thread.Sleep(1000);
                    //value++;
                    //Dispatcher.Invoke(updProgress, new object[] { Wpf.ProgressBar.ValueProperty, ++this.progressBar.Value });
                    Dispatcher.Invoke(updProgress,
                      System.Windows.Threading.DispatcherPriority.Background, new object[] { Wpf.ProgressBar.ValueProperty, ++this.progressBar.Value });

                }
            }
        }


        //private void AddToTheListFoundFilesImages(string selectedPath)
        //{
        //    this.listBox.Items.Clear();

        //    DirectoryInfo directoryInfo = new DirectoryInfo(selectedPath);

        //    this.progressBar.Maximum = directoryInfo.GetFiles().Count();    // pb


        //    UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(progressBar.SetValue);



        //    foreach (FileInfo fileInfo in directoryInfo.GetFiles())
        //    {
        //        if (IsImageExtensions(fileInfo.Extension) == true)
        //        {
        //            this.AddToListBoxImage(fileInfo);

        //            this.progressBar.Value++;
        //            System.Threading.Thread.Sleep(1000);


        //        }
        //    }
        //}

        private bool IsImageExtensions(string extension)
        {
            if (extension == ".bmp"
                || extension == ".png"
                || extension == ".gif"
                || extension == ".jpg")
            {
                return true;
            }

            return false;
        }

        private void AddToListBoxImage(FileInfo fileInfo)
        {
            Wpf.Button button = new Wpf.Button
            {
                Content = "test btn"
            };
            button.Click += Button_Click;
            button.Height = 100;
            button.Width = this.listBox.ActualWidth;    // TODO перерисовывать!?
                                                        //button.Margin = new Thickness(10);


            Image image = new Image();
            image.Stretch = Stretch.UniformToFill;

            image.Source = new BitmapImage(new Uri(fileInfo.FullName, UriKind.Absolute));
            button.Content = image;

            this.listBox.Items.Add(button);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO установка выбранной картинки в слайдер
            imageSlider.Source = ((sender as Wpf.Button).Content as Image).Source;
        }

        private void progressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //progressBar.Value++;
        }






    }
}
