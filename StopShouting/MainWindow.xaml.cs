using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StopShouting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            OutputTextBox.Text =
                TextNormalizer.Deshout(InputTextBox.Text);
        }
        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OutputTextBox.Text))
                return;

            Clipboard.SetText(OutputTextBox.Text);

            var button = sender as Button;

            var originalContent = button?.Content?.ToString() ?? "📄";

            button?.Content = "✓";

            CopiedToast.Visibility = Visibility.Visible;

            await Task.Delay(1000);

            CopiedToast.Visibility = Visibility.Collapsed;

            button?.Content = originalContent;
        }

    }
}