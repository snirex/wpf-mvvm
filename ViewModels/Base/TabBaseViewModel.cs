using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MDClone.ViewModels
{
    public class TabBaseViewModel:BaseViewModel
    {
        public delegate void delClosed();
        public event delClosed CloseInitiated;
        private void CloseCommandAction()
        {
            CloseInitiated?.Invoke();
            MainWindowViewModel.Inst.DeleteTab();
        }

        private ICommand _CloseTabCommand;
        public ICommand CloseTabCommand
        {
            get
            {
                return _CloseTabCommand ?? (_CloseTabCommand = new CommandHandler(() => CloseCommandAction(), () => true));
            }
        }
    }
}
