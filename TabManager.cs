using MDClone.UserControls;
using MDClone.ViewModels;
using System.ComponentModel;
using System.Windows.Controls;

namespace MDClone
{
    public enum TabTypesEnum
    {
        [Description("Table Form ")]
        DataTable,
        [Description("Email Form ")]
        Email,
    }
    class TabManager
    {
        private static TabManager inst;
        public static TabManager Inst
        {
            get 
            {
                if (inst == null)
                    inst = new TabManager();
                return inst;
            }
            private set { }
        }

        private TabManager()
        {

        }


        public UserControl GetNewUserControl(TabTypesEnum tabTypesEnum)
        {
            switch (tabTypesEnum)   
            {
                case TabTypesEnum.DataTable:
                    return new DataTableUserControl();
                case TabTypesEnum.Email:
                    return new EmailFormUserControl();
                default:
                    return null;
            }
        }
    }
}
