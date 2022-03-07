using MDClone.GeneralClasses;
using MDClone.GeneralClasses.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MDClone.ViewModels
{
    public class DataTableViewModel: TabBaseViewModel
    {
        private readonly DBManipulation dBManipulation = new DBManipulation();
     

        public DataTableViewModel()
        {
            dBManipulation.OnDataTableHanlder += DBManipulation_OnDataTableHanlder;
            CloseInitiated += DataTableViewModel_CloseInitiated;
        }

        #region Handlers of events

        private void DBManipulation_OnDataTableHanlder(DataTable dataTable)
        {
            Clear();
            Application.Current.Dispatcher.Invoke(() =>
            {
                CylinderBatchRecordsData = dataTable;
            });
        }
       
        private void DataTableViewModel_CloseInitiated()
        {
            Clear();
        }
        #endregion


        #region properties
        private DataTable _CylinderBatchRecordsData;
        public DataTable CylinderBatchRecordsData
        {
            get { return _CylinderBatchRecordsData; }
            set
            {
                _CylinderBatchRecordsData = value;
                NotifyPropertyChanged("CylinderBatchRecordsData");
            }
        }

        private string _FilePath;
        public string FilePath
        {
            get { return _FilePath; }
            set
            {
                _FilePath = value;
                NotifyPropertyChanged("FilePath");
            }
        }
        #endregion

        #region private methods

        private void Clear()
        {
           
            CylinderBatchRecordsData?.Dispose();
            CylinderBatchRecordsData = null;

        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private async Task ChooseFileCommandAction()
        {
            FilePath = GeneralClass.ChooseFile("csv");
            Clear();
            await dBManipulation.StartReadFile(FilePath);
        }
        #endregion

        #region Commands

        private ICommand _ChooseFileCommand;
        public ICommand ChooseFileCommand => _ChooseFileCommand ?? (_ChooseFileCommand = new CommandHandler(async() => await ChooseFileCommandAction(), () => true));

        #endregion

       
    }
}
