using MDClone.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MDClone.ViewModels
{
    public class ClosableTabModel: TabItem
    {

        // Constructor
        public ClosableTabModel()
        {

            // Create an instance of the usercontrol
            ClosableHeaderControl closableTabHeader = new ClosableHeaderControl();

            // Assign the usercontrol to the tab header
            this.Header = closableTabHeader;
           // this.Content = "jjj";

            // Attach to the ClosableHeaderControl events (Mouse Enter/Leave, Button Click, and Label resize)
            closableTabHeader.button_close.MouseEnter += button_close_MouseEnter;
            closableTabHeader.button_close.MouseLeave += button_close_MouseLeave;
            closableTabHeader.button_close.Click += button_close_Click;
            closableTabHeader.label_TabTitle.SizeChanged += label_TabTitle_SizeChanged;
        }



        /// <summary>
        /// Property - Set the Title of the Tab
        /// </summary>
        public string ClosableTabTitle
        {
            set
            {
                ((ClosableHeaderControl)this.Header).label_TabTitle.Content = value;
            }
        }

        public UserControl ClosableTabContent
        {
            set
            {
                this.Content = value;
            }
        }


        //
        // - - - Overrides  - - -
        //


        // Override OnSelected - Show the Close Button
        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            ((ClosableHeaderControl)this.Header).button_close.Visibility = Visibility.Visible;
        }

        // Override OnUnSelected - Hide the Close Button
        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            ((ClosableHeaderControl)this.Header).button_close.Visibility = Visibility.Visible;
        }

        // Override OnMouseEnter - Show the Close Button
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            ((ClosableHeaderControl)this.Header).button_close.Visibility = Visibility.Visible;
        }

        // Override OnMouseLeave - Hide the Close Button (If it is NOT selected)
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!this.IsSelected)
            {
                ((ClosableHeaderControl)this.Header).button_close.Visibility = Visibility.Hidden;
            }
        }





        //
        // - - - Event Handlers  - - -
        //


        // Button MouseEnter - When the mouse is over the button - change color to Red
        void button_close_MouseEnter(object sender, MouseEventArgs e)
        {
            ((ClosableHeaderControl)this.Header).button_close.Foreground = Brushes.Red;
        }

        // Button MouseLeave - When mouse is no longer over button - change color back to black
        void button_close_MouseLeave(object sender, MouseEventArgs e)
        {
            ((ClosableHeaderControl)this.Header).button_close.Foreground = Brushes.Black;
        }


        // Button Close Click - Remove the Tab - (or raise an event indicating a "CloseTab" event has occurred)
        void button_close_Click(object sender, RoutedEventArgs e)
        {
           // ((TabControl)this.Parent).Items.Remove(this);

            MainWindowViewModel.Inst.Tabs.Remove(this);
        }


        // Label SizeChanged - When the Size of the Label changes (due to setting the Title) set position of button properly
        void label_TabTitle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((ClosableHeaderControl)this.Header).button_close.Margin = new Thickness(((ClosableHeaderControl)this.Header).label_TabTitle.ActualWidth + 5, 3, 4, 0);
        }



    }
}
