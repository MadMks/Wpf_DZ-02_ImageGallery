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
    /// <summary>
    /// Делегат для progressBar.
    /// </summary>
    delegate void UpdateProgressBarDelegate(DependencyProperty dp, object value);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            using(FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.LocalizedResources;
                folderBrowserDialog.ShowNewFolderButton = false;


                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.imageSlider.Source = null;
                    this.progressBar.Visibility = Visibility.Visible;

                    AddToTheListFoundFilesImages(folderBrowserDialog.SelectedPath);

                    this.progressBar.Visibility = Visibility.Hidden;

                    if (this.listBox.Items.Count > 0)
                    {
                        SettingFirstImageInSlider();

                        if (this.expander.IsExpanded == true)
                        {
                            this.ComputeFileDataForExpander();
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Установка в слайдер первой загруженной картинки.
        /// </summary>
        private void SettingFirstImageInSlider()
        {
            imageSlider.Source = ((this.listBox.Items[0] as Wpf.Button).Content as Image).Source;
        }


        /// <summary>
        /// Добавление картинок в listBox из выбранной папки.
        /// </summary>
        /// <param name="selectedPath">Путь выбранной папки</param>
        private void AddToTheListFoundFilesImages(string selectedPath)
        {
            this.listBox.Items.Clear();
            this.progressBar.Value = 0;

            UpdateProgressBarDelegate updProgress = new UpdateProgressBarDelegate(progressBar.SetValue);

            DirectoryInfo directoryInfo = new DirectoryInfo(selectedPath);

            this.progressBar.Maximum = directoryInfo.GetFiles().Count();


            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (IsImageExtensions(fileInfo.Extension) == true)
                {
                    this.AddToListBoxImage(fileInfo);

                    Dispatcher.Invoke(updProgress,
                      System.Windows.Threading.DispatcherPriority.Background,
                      new object[] { Wpf.ProgressBar.ValueProperty, ++this.progressBar.Value });

                }
            }
        }

        /// <summary>
        /// Проверка расширения файла (картинка или нет).
        /// </summary>
        /// <param name="extension">Расширение файла</param>
        /// <returns>true если картинка</returns>
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

        /// <summary>
        /// Добавление в listBox одной картинки.
        /// </summary>
        /// <param name="fileInfo"></param>
        private void AddToListBoxImage(FileInfo fileInfo)
        {
            Wpf.Button button = new Wpf.Button();
            button.Click += Button_Click;
            button.Height = 100;
            button.Width = this.listBox.ActualWidth;    // TODO поправить listBox (vertical scroll)
            button.Margin = new Thickness(0, 2, 0, 2);

            Image image = new Image();
            image.Stretch = Stretch.UniformToFill;

            image.Source = new BitmapImage(new Uri(fileInfo.FullName, UriKind.Absolute));
            button.Content = image;

            this.listBox.Items.Add(button);
        }

        /// <summary>
        /// Обработка нажатия на картинку (кнопку в которой картинка).
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Установка выбранной картинки в слайдер.
            imageSlider.Source = ((sender as Wpf.Button).Content as Image).Source;

            // TODO FIX HACK
            //this.listBox.Items.

            if (this.expander.IsExpanded == true)
            {
                this.ComputeFileDataForExpander();
            }
        }

        private void ComputeFileDataForExpander()
        {
            this.fileNameTextBox.Text = System.IO.Path.GetFileName(this.imageSlider.Source.ToString());

            string fileExtension = System.IO.Path.GetExtension(this.imageSlider.Source.ToString());
            this.fileExtensionTextbox.Text = (fileExtension.Remove(0, 1)).ToUpper();

            this.fileSizeTextBox.Text =
                (this.imageSlider.Source as BitmapSource).PixelWidth.ToString()
                + " x "
                + (this.imageSlider.Source as BitmapSource).PixelHeight.ToString();
        }

        private void progressBar_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (progressBar.IsVisible)
            {
                blockControl.Visibility = Visibility.Hidden;
                expander.Visibility = Visibility.Hidden;
            }
            else
            {
                blockControl.Visibility = Visibility.Visible;
                expander.Visibility = Visibility.Visible;
            }
        }

        private void expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.ComputeFileDataForExpander();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
