using System.IO;
using System.Text;
using System.Threading;
using System.Windows;

namespace WindowsInput.Tests.UnicodeTestSurface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyTextBox.Focus();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllText(App.Filename, MyTextBox.Text, Encoding.UTF32);
            Thread.Sleep(1000);
            base.OnClosing(e);
        }
    }
}
