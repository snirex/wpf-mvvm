using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MDClone.GeneralClasses
{
    public static class GeneralClass
    { 
        public  static string[] DELIMITER = new string[] { "," };
        public  const ushort NUM_OF_ROWS = 5;
        public const char HEADER_DELIMITER = ' ';

        public delegate void ShowDataToUser(string data);

        // need to do it better
        public static string GetInitialDirectoryPath()
        {
            var strExeFilePath = Assembly.GetExecutingAssembly().Location;
            return  new DirectoryInfo(strExeFilePath).Parent.Parent.Parent.FullName;
        }

        public static void ShowMessageToUser(string message)
        {
            WarningWindow.ShowText(message);
        }

        // diplay the Desciption attribute of enum
        public static string GetDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (_Attribs != null && _Attribs.Count() > 0)
                {
                    return ((DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }

        public static string ChooseFile(string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Files (*." + filter + ")|*." + filter
            };

            openFileDialog.InitialDirectory = GetInitialDirectoryPath() + @"\FilesToRead";

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return "";
        }
    }
}
