using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{

    /// <summary>
    /// <h4>BudgetFiles class is used to manage the files used in the Budget project</h4>
    /// </summary>
    public class BudgetFiles
    {
        private static String DefaultSavePath = @"Budget\";
        private static String DefaultAppData = @"%USERPROFILE%\AppData\Local\";

        // ====================================================================
        // verify that the name of the file exists, or set the default file, and 
        // is it readable?
        // throws System.IO.FileNotFoundException if file does not exist
        // ====================================================================
        /// <summary>
        /// Verifies that the name of a file exists (sets a default file in AppData if not), and if it's readable
        /// 
        /// <para>
        /// For the examples below, assume that <i>readable.txt</i> and <i>default.txt</i> are both existing files:
            /// <example>
                /// <code>
                /// string filePath = "./readable.txt";
                /// string defaultName = "default.txt";
                /// 
                /// string readablePath = VerifyReadFromFileName(filePath, defaultName);
                /// Console.WriteLine(readablePath);
                /// </code>
            /// Output if the file path exists:
                /// <code>
                /// ./readable.txt
                /// </code>
            /// Output if the file path doesn't exist (Pretend this is the absolute path to the default file):
                /// <code>
                /// %USERPROFILE%\AppData\Local\Budget\default.txt
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// <param name="FilePath">The path of the file to verify</param>
        /// <param name="DefaultFileName">The path of a file to set as the default in AppData, should the FilePath not exist</param>
        /// <returns>The path of the existing file</returns>
        /// <exception cref="FileNotFoundException">Throws if neither the FilePath nor the default file exist</exception>
        public static String VerifyReadFromFileName(String FilePath, String DefaultFileName)
        {

            // ---------------------------------------------------------------
            // if file path is not defined, use the default one in AppData
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does FilePath exist?
            // ---------------------------------------------------------------
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("ReadFromFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ----------------------------------------------------------------
            // valid path
            // ----------------------------------------------------------------
            return FilePath;

        }

        // ====================================================================
        // verify that the name of the file exists, or set the default file, and 
        // is it writable
        // ====================================================================
        /// <summary>
        /// Verifies that the name of a file exists (sets a default file in AppData if not), and if it's writable
        /// 
        /// <para>
        /// For the examples below, assume that <i>writable.txt</i> and <i>default.txt</i> are both existing files:
            /// <example>
                /// <code>
                /// string filePath = "./writable.txt";
                /// string defaultName = "default.txt";
                /// 
                /// string writablePath = VerifyWriteToFileName(filePath, defaultName);
                /// Console.WriteLine(writablePath);
                /// </code>
            /// Output if the file path exists:
                /// <code>
                /// ./writable.txt
                /// </code>
                /// Output if the file path doesn't exist (Pretend this is the absolute path to the default file):
                /// <code>
                /// %USERPROFILE%\AppData\Local\Budget\default.txt
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// <param name="FilePath">The path of the file to verify</param>
        /// <param name="DefaultFileName">The path of a file to set as the default in AppData, should the FilePath not exist</param>
        /// <returns>The path of the existing file</returns>
        /// <exception cref="Exception">Throws if both files or their directories don't exist</exception>
        public static String VerifyWriteToFileName(String FilePath, String DefaultFileName)
        {
            // ---------------------------------------------------------------
            // if the directory for the path was not specified, then use standard application data
            // directory
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                // create the default appdata directory if it does not already exist
                String tmp = Environment.ExpandEnvironmentVariables(DefaultAppData);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                // create the default Budget directory in the appdirectory if it does not already exist
                tmp = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does directory where you want to save the file exist?
            // ... this is possible if the user is specifying the file path
            // ---------------------------------------------------------------
            String folder = Path.GetDirectoryName(FilePath);
            String delme = Path.GetFullPath(FilePath);
            if (!Directory.Exists(folder))
            {
                throw new Exception("SaveToFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ---------------------------------------------------------------
            // can we write to it?
            // ---------------------------------------------------------------
            if (File.Exists(FilePath))
            {
                FileAttributes fileAttr = File.GetAttributes(FilePath);
                if ((fileAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    throw new Exception("SaveToFileException:  FilePath(" + FilePath + ") is read only");
                }
            }

            // ---------------------------------------------------------------
            // valid file path
            // ---------------------------------------------------------------
            return FilePath;

        }



    }
}
