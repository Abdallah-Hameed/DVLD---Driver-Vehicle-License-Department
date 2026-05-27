using DVLDtraining_BusinessLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDtraining.Global
{
    public class clsUtil
    {
        static public clsUser CurrentUser = new clsUser();

        static string GeneerateGUID()
        {
            Guid g = new Guid();

            return g.ToString();
        }

        static bool CreateFolderIfDoesNotExist(string FolderPath)
        {
            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);

                    return true;
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }
            }

            return true;
        }

        static string ReplaceFileNameWithGUID(string SourceFile)
        {
            FileInfo fi = new FileInfo(SourceFile);

            return GeneerateGUID() + fi.Extension;
        }

        public static bool CopyImageToProjectImagesFolder(ref string SourceFile)
        {
            if (!CreateFolderIfDoesNotExist(@"C:\DVLD-People-Images\"))
                return false;

            try
            {
                File.Copy(SourceFile, @"C:\DVLD-People-Images\" + ReplaceFileNameWithGUID(SourceFile), true);
            }

            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            SourceFile = @"C:\DVLD-People-Images\" + ReplaceFileNameWithGUID(SourceFile);

            return true;
        }

        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            try
            {
                //in case the username is empty, delete the file
                if (Username == "" && File.Exists(Directory.GetCurrentDirectory() + "\\remember.txt"))
                {
                    File.Delete(Directory.GetCurrentDirectory() + "\\remember.txt");

                    return true;
                }

                using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + "\\remember.txt"))
                {
                    // Write the data to the file
                    writer.WriteLine(Username + "#//#" + Password);

                    return true;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");

                return false;
            }
        }

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            try
            {
                if (File.Exists(Directory.GetCurrentDirectory() + "\\remember.txt"))
                {
                    using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "\\remember.txt"))
                    {
                        string Line;

                        while ((Line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(Line); // Output each line of data to the console

                            string[] result = Line.Split(new string[] { "#//#" }, StringSplitOptions.None);

                            Username = result[0];

                            Password = result[1];
                        }

                        return true;
                    }
                }

                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");

                return false;
            }
        }
    }
}
