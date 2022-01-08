using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

namespace NTPrincipalParts
{
    public class classGlobal
    {
        /*============================================================================================*
         *                                                                                            *
         *                                       classGlobal                                          *
         *                                       ===========                                          *
         *                                                                                            *
         *   Variables that will need to be accessed in several other classes or forms.               *
         *                                                                                            *
         *   Since Registry values will also need to be available globally, Registry management has   *
         *   been included in the global class.                                                       *
         *                                                                                            *
         *============================================================================================*/

        /*--------------------------------------------------------------------------------------------*
         *                                                                                            *
         *  These variables control the default settings for frmDetails and relate to the registry    *
         *    keys with titles held in registryKeys.                                                  *
         *                                                                                            *
         *  participleDisplay:  Whether participles will be included in the display or not:           *
         *     0   Do not show any participles                                                        *
         *     1   Show only the masculine singular nominative participle                             *
         *     2   Show all                                                                           *
         *                                                                                            *
         *  subjunctiveDisplay: Whether subjunctives and optatives will be included in the display:   *
         *     0   Do not show subjunctives and optatives                                             *
         *     1   Show both                                                                          *
         *                                                                                            *
         *  printSelection: When creating a CSV of the entry, which principal parts should be         *
         *                  included.  This is an accumulated value                                   *
         *     bit set        meaning                                                                 *
         *        0       Present and imperfect active, middle and passive                            *
         *        1       Future active and middle                                                    *
         *        2       Aorist active and middle                                                    *
         *        3       Perfect and pluperfect active and middle                                    *
         *        4       Perfect and pluperfect passive                                              *
         *        5       Aorist and future passive                                                   *
         *                                                                                            *
         *--------------------------------------------------------------------------------------------*/
        int participleDisplay = 0, subjunctiveDisplay = 0, printSelection = 63;
        String RegistryKeyString = @"software\LFCConsulting\NTPrincipalParts";
        String[] registryKeys = { "Participles", "Subjunctives", "Print selection" };
        object regValue;
        RegistryKey baseKey;

        /*--------------------------------------------------------------------------------------------*
         *                                                                                            *
         *  File location                                                                             *
         *  -------------                                                                             *
         *                                                                                            *
         *  baseDirectory:    The root directory for files, referenced by the registry                *
         *  altBaseDirectory: The root directory for files when in development mode                   *
         *                                                                                            *
         *--------------------------------------------------------------------------------------------*/

        String baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LFCConsulting\NTPrincipalParts\Source\";
        String altBaseDirectory = Path.GetFullPath(@"..\Source");
        String helpDirectory = "Help", ntTextFolder = "NTText", lxxTextFolder = "LXX_Text", helpFile = @"Help\Help.html",
            ntTitlesFile = "NTTitles.txt", lxxTitlesFile = "LXX_Titles.txt";

        public string BaseDirectory { get => baseDirectory; set => baseDirectory = value; }
        public string HelpDirectory { get => helpDirectory; set => helpDirectory = value; }
        public string NtTextFolder { get => ntTextFolder; set => ntTextFolder = value; }
        public string LxxTextFolder { get => lxxTextFolder; set => lxxTextFolder = value; }
        public string HelpFile { get => helpFile; set => helpFile = value; }
        public string NtTitlesFile { get => ntTitlesFile; set => ntTitlesFile = value; }
        public string LxxTitlesFile { get => lxxTitlesFile; set => lxxTitlesFile = value; }

        public void initialiseRegistry()
        {
            altBaseDirectory = Path.GetFullPath(altBaseDirectory);
            openRegistry();
            regValue = baseKey.GetValue(registryKeys[0]);
            if (regValue != null) participleDisplay = (int)regValue;
            regValue = baseKey.GetValue(registryKeys[1]);
            if (regValue != null) subjunctiveDisplay = (int)regValue;
            regValue = baseKey.GetValue(registryKeys[2]);
            if (regValue != null) printSelection = (int)regValue;
            regValue = baseKey.GetValue("Base Directory");
            if (regValue == null) manageSourceFiles();
            else
            {
                baseDirectory = regValue.ToString();
//                if (Directory.Exists(baseDirectory)) baseDirectory += @"\";
//                else manageSourceFiles();
                if (! Directory.Exists(baseDirectory)) manageSourceFiles();
            }
        }

        public void updateRegSetting( int whichKey, int keyValue)
        {
            openRegistry();
            baseKey.SetValue(registryKeys[whichKey], keyValue, RegistryValueKind.DWord);
            closeRegistryKey();
        }

        public int getRegSetting( int whichKey )
        {
            int regKeyValue = 0;

            openRegistry();
            regValue = baseKey.GetValue(registryKeys[whichKey]);
            if (regValue != null) regKeyValue = (int)regValue;
            closeRegistryKey();
            return regKeyValue;
        }

        private void manageSourceFiles()
        {
            DirectoryInfo diSource, diTarget;

            Directory.CreateDirectory(baseDirectory);
            // Make sure the target directory is empty
            diTarget = new DirectoryInfo(baseDirectory);
            try
            {
                foreach (DirectoryInfo diName in diTarget.GetDirectories()) diName.Delete(true);
            }
            catch
            {
                // Do nothing
            }
            foreach (FileInfo fiFile in diTarget.GetFiles()) fiFile.Delete();
            // Now copy data from the current location
            diSource = new DirectoryInfo(altBaseDirectory);
            foreach (DirectoryInfo diName in diSource.GetDirectories()) cloneSingleDirectory(diTarget, diName);
            // Now get any files in the current directory
            foreach (FileInfo fiFile in diSource.GetFiles()) fiFile.CopyTo(diTarget.FullName + @"\" + fiFile.Name);
            openRegistry();
            if (baseKey != null) baseKey.SetValue("Base Directory", baseDirectory, RegistryValueKind.String);
            closeRegistryKey();
            baseDirectory += @"\";
        }

        public void cloneSingleDirectory(DirectoryInfo diTargetDirectory, DirectoryInfo diSourceDirectory)
        {
            DirectoryInfo diTarget;

            // Create the new directry
            if (!Directory.Exists(diTargetDirectory.FullName + @"\" + diSourceDirectory.Name)) Directory.CreateDirectory(diTargetDirectory.FullName + @"\" + diSourceDirectory.Name);
            // Get info for the new directory
            diTarget = new DirectoryInfo(diTargetDirectory.FullName + @"\" + diSourceDirectory.Name);
            // Now go down a level and do the same
            foreach (DirectoryInfo diName in diSourceDirectory.GetDirectories()) cloneSingleDirectory(diTarget, diName);
            // Now get any files in the current directoy
            foreach (FileInfo fiFile in diSourceDirectory.GetFiles()) fiFile.CopyTo(diTargetDirectory.FullName + @"\" + diSourceDirectory.Name + @"\" + fiFile.Name);
        }

        public void openRegistry()
        {
            baseKey = Registry.CurrentUser.OpenSubKey(RegistryKeyString, true);
            if (baseKey == null) baseKey = Registry.CurrentUser.CreateSubKey(RegistryKeyString, true);
        }

        public void closeRegistryKey()
        {
            if (baseKey != null) baseKey.Close();
        }
    }
}
