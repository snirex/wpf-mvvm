using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MDClone.GeneralClasses.DB
{
    public class DBManipulation
    {
        private readonly List<string[]> dataRows = new List<string[]>();
        private DataTable dataTable;

        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private Task _LastTask;

        #region event and delegate
        public delegate void OnDataTable(DataTable dataTable);
        public event OnDataTable OnDataTableHanlder;
        #endregion

        #region public methods
        /// <summary>
        /// The methods read the file in chunks and make notifications when finish to read full chunk or some part,
        /// if user ask to read new file  before the previously is done, so stop the reading previously file.
        /// </summary>
        /// <param name="csv_file_path">the full path to file</param>
        /// <returns></returns>
        public async Task StartReadFile(string csv_file_path)
        {
            //check goes here - abort if running 
            if (_LastTask != null && !_LastTask.IsCompleted)
            {
                tokenSource.Cancel();
            }

            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Dispose();
                tokenSource = new CancellationTokenSource();
            }

            Task task = Task.Run(() => ReadCSVFile(csv_file_path,tokenSource.Token));
            _LastTask = task;
            await _LastTask;
            if (task == _LastTask)
            {
                // Only reset _task value if it's the one we created in this
                // method call
                _LastTask = null;
            }
        }

        #endregion

        #region private methods
        /// <summary>
        /// read file in chunks at notify user
        /// </summary>
        /// <param name="csv_file_path"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private bool ReadCSVFile(string csv_file_path, CancellationToken ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                {
                    ct.ThrowIfCancellationRequested();
                }

                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(GeneralClass.DELIMITER);
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] headers = csvReader.ReadFields();
                    if (headers == null) return false;
                    dataTable = new DataTable();
                    foreach (string header in headers)
                    {
                        dataTable.Columns.Add(new DataColumn(header));
                    }

                    OnDataTableHanlder(dataTable.Copy());

                    dataRows.Clear();
                    while (!csvReader.EndOfData)
                    {
                        //Thread.Sleep(100); // only for test
                        string[] data = csvReader.ReadFields();
                        dataRows.Add(data);

                        // if num of rows is equal to GeneralClass.NUM_OF_ROWS, make noitifcation to viewmodel
                        if (dataRows.Count == GeneralClass.NUM_OF_ROWS)
                        {
                            // alloc of data 
                            List<string[]> rowAllocate = dataRows.Select(s => s).ToList();
                            NotifyOfDataChunks(rowAllocate); // notify the viewModel of new rowData and clear
                        }

                        if (ct.IsCancellationRequested)
                        {
                            ct.ThrowIfCancellationRequested();
                        }
                    }

                    // in the end of while if still some rows, make noitifcation to viewmodel
                    if (dataRows.Count > 0)
                    {
                        NotifyOfDataChunks(dataRows);
                    }
                }
            }
            catch (OperationCanceledException oex)
            {
                GeneralClass.ShowMessageToUser($"The previous file reading canceled \n{oex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                GeneralClass.ShowMessageToUser($"Please check the file \n{ex.Message}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// make rows for datatable and notify viewmodel
        /// </summary>
        /// <param name="data">the data of fields</param>
        private void NotifyOfDataChunks(List<string[]> data)
        {
            foreach (string[] row in data)
            {
                int index = 0;
                DataRow r = dataTable.NewRow();
                foreach (string col in row)
                {
                    r[index++] = col;
                }
                dataTable.Rows.Add(r);
            }
            OnDataTableHanlder(dataTable.Copy());
            dataRows.Clear();
        }
        #endregion
    }
}
