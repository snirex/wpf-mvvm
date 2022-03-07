using MDClone.GeneralClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace MDClone.ViewModels
{
    public class MainWindowViewModel:BaseViewModel
    {
        #region Instance and Constructor

        private static MainWindowViewModel inst;
        public static MainWindowViewModel Inst
        {
            get
            {
                if (inst == null)
                    inst = new MainWindowViewModel();
                return inst;
            }
            private set { }
        }

        private MainWindowViewModel()
        {
            Tabs = new ObservableCollection<TabItem>();
        }

        #endregion

        #region properties

        public Action CloseAction { get; set; }

        private ObservableCollection<TabItem> _Tabs;
        public ObservableCollection<TabItem> Tabs
        {
            get { return _Tabs; }
            set
            {
                _Tabs = value;
                NotifyPropertyChanged("Tabs");
            }
        }

        private TabItem _SelectedTab;
        public TabItem SelectedTab
        {
            get { return _SelectedTab; }
            set
            { _SelectedTab = value;
                NotifyPropertyChanged("SelectedTab");
            }
        }
        #endregion

        #region privatre methods

        private int FindLastNumberAtHeader()
        {
            try
            {
                if (Tabs.Count > 0)
                {
                    string[] data = Tabs.Last().Header.ToString().Split(GeneralClass.HEADER_DELIMITER);
                    int.TryParse(data[data.Count() - 1], out int res);
                    return res + 1;
                }
            }
            catch
            {
            }
            return 1;
        }

        private void AddNewTab(UserControl userControl, string headertab)
        {
            // create new tab item
            var header = headertab + FindLastNumberAtHeader();
            SelectedTab = new TabItem { Header = header, Content = userControl };
           
            Tabs.Add(SelectedTab);
            NotifyPropertyChanged("Tabs");           
        }


        private void AddNewTabAction(object param)
        {
            try
            {
                if (param != null)
                {
                    Enum.TryParse(param.ToString(), out TabTypesEnum tabTypes);
                    AddNewTab(TabManager.Inst.GetNewUserControl(tabTypes),GeneralClass.GetDescription(tabTypes));
                }
            }
            catch
            {
                throw new Exception($"Can not cast object to TabTypesEnum");
            }
        }

        private void ExitCommandAction()
        {
            // make user ask if to out 
            CloseAction();
        }
        #endregion

        public void DeleteTab()
        {
            Tabs.Remove(SelectedTab);
        }

        #region Commands

        private ICommand _ExitCommand;
        public ICommand ExitCommand
        {
            get
            {
                return _ExitCommand ?? (_ExitCommand = new CommandHandler(() => ExitCommandAction(), () => true));

            }
        }

        private ICommand _AddNewTabCommand;
        public ICommand AddNewTabCommand
        {
            get
            {
                return _AddNewTabCommand ?? (_AddNewTabCommand = new CommandHandlerWithParameter(param => AddNewTabAction(param)));

            }
        }
        #endregion
    }
}
