using MDClone.GeneralClasses;
using MDClone.GeneralClasses.Email;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MDClone.ViewModels
{
    public class EmailViewModel: TabBaseViewModel
    {
        // constructor
        public EmailViewModel()
        {
            this.CloseInitiated += EmailViewModel_CloseInitiated;
        }


        #region properties
        private string _FromAddress = "sce.talk@gmail.com";
        public string FromAddress
        {
            get {return  _FromAddress; }
            set
            {
                _FromAddress = value;
                NotifyPropertyChanged("FromAddress");
            }
        }

        private string _ToAddress = string.Empty;
        public string ToAddress
        {
            get { return _ToAddress; }
            set
            {
                _ToAddress = value;
                NotifyPropertyChanged("ToAddress");
            }
        }

        private string _Body = "write here you message" + Environment.NewLine + Environment.NewLine + "Best wishes Tal";
        public string Body
        {
            get { return _Body; }
            set
            {
                _Body = value;
                NotifyPropertyChanged("Body");
            }
        }

        private string _Subject = string.Empty;
        public string Subject
        {
            get { return _Subject; }
            set
            {
                _Subject = value;
                NotifyPropertyChanged("Subject");
            }
        }
        #endregion

        #region private methods

        /// <summary>
        /// Send mail async to prevent that gui will stop working
        /// </summary>
        /// <returns></returns>
        private async Task SendEmailCommandAction()
        {
            await Task.Run(() =>
            {
                EmailExecution.SendEmail(FromAddress, ToAddress, Subject, Body, GeneralClass.ShowMessageToUser);
            });
        }
       
        private bool SendEmailCommandExecute
        {
            get
            {
                // check if need to adding new Item
                return CheckIfNoEmpty(Subject) && CheckIfNoEmpty(Body) && CheckIfNoEmpty(ToAddress);
            }
        }

        private bool CheckIfNoEmpty(string data)
        {
            return !string.IsNullOrEmpty(data);
        }

        private void EmailViewModel_CloseInitiated()
        {
            // cancel to send email
        }

        #endregion

        #region Commands

        private ICommand _SendEmailCommand;
        public ICommand SendEmailCommand
        {
            get
            {
                return _SendEmailCommand ?? (_SendEmailCommand = new CommandHandler(async() => await SendEmailCommandAction(), () => SendEmailCommandExecute));

            }
        }
        #endregion

       
    }

    public class ValidationRuleOfStrudel : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
           
            if(value.ToString().Contains("@"))
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Please enter a valid E-mail.");
        }
    }
}
