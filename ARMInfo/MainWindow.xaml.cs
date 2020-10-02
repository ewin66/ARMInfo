using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using InfoCollector.PersonalInformation;
using InfoCollector.SystemInformation;

namespace ARMInfo
{

    public partial class MainWindow : Window
    {
        public static bool MayIGoOut = true;

        public MainWindow(IPCInfo pc)
        {
            InitializeComponent();
            this.DataContext = (pc != null)
                ? new MainViewModel(pc)
                : new MainViewModel(new SystemInfo(), new PersonalInfo());

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!MayIGoOut)
            {
                MessageBox.Show("Сначала отправьте данные!", "Проверьте корректность данных", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Cancel = true;
            }
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            try { this.DragMove(); }
            catch { }
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { DragMove(); }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && MayIGoOut)
            {
                this.Close();
            }
        }

        private void ComboBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            var cmb = ((ComboBox)sender);
            cmb.Text = cmb.Text.ToUpper();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                comboBoxInventoryNumber.Focus();
            }
        }

    }

}
