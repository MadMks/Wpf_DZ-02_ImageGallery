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

namespace Task_ImageGallery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Путь поиска файлов.
        /// </summary>
        private string path;    // TODO delete

        public MainWindow()
        {
            InitializeComponent();
        }


        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            // TODO диалог открытия папки
            using(FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.LocalizedResources;
                folderBrowserDialog.ShowNewFolderButton = false;

                //DialogResult result = folderBrowserDialog.ShowDialog();

                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // TODO получаем путь к папке
                    this.path = folderBrowserDialog.SelectedPath;   // TODO delete

                    AddToTheListFoundFilesImages(folderBrowserDialog.SelectedPath);     // TODO (string path)
                }
            }
        }

        private void AddToTheListFoundFilesImages(string selectedPath)
        {
            this.listBox.Items.Clear();

            DirectoryInfo directoryInfo = new DirectoryInfo(selectedPath);

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                System.Windows.Controls.Button button = new System.Windows.Controls.Button
                {
                    Content = "test btn"
                };
                this.listBox.Items.Add(button);
            }
        }
    }
}
