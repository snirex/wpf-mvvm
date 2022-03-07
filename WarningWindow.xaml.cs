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
using System.Windows.Shapes;

namespace MDClone
{
    /// <summary>
    /// Interaction logic for WarningWindow.xaml
    /// </summary>
    public partial class WarningWindow : Window
    {
        public bool IsOK { get; set; }


        public WarningWindow()
        {
            InitializeComponent();
           // this.Resources = GeneralViewModel.Inst.CurrentResourceDictionary;
        }
        public static void ShowText(string text, bool isTwoButtons = false, string title = "")
        {

            Application.Current.Dispatcher.Invoke(delegate
            {
                WarningWindow warningWindow = new WarningWindow();
                warningWindow.Title = title;
                warningWindow.SetText(text, isTwoButtons);
                warningWindow.ShowDialog();
            });
        }
        public void SetText(string text, bool isTwoButtons)
        {
            textLb.Content = text;
            if (!isTwoButtons)
            {
                lblInvisible.Width = 220;
                btnCancel.Visibility = Visibility.Collapsed;
                btnClose.Visibility = Visibility.Hidden;
                btnOk.Content = "Ok";
            }
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            IsOK = true;
            Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsOK = false;
            Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Cancel_Click(sender, e);
        }
    }
}
