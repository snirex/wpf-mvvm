using MDClone.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace MDClone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel inst = MainWindowViewModel.Inst;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = inst;
            if (inst.CloseAction == null) // for close the window 
                inst.CloseAction = new Action(this.Close);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Cursor = Cursors.Hand;
                DragMove();
            }
            else
                Cursor = Cursors.Arrow;
        }
    }
}
