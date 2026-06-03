using DVLDtraining_BusinessLogic;
using Microsoft.Win32;
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
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\DVLDtraining"))
                {
                    if (Username == "")
                    {
                        key.DeleteValue("Credentials", false);
                        
                        return true;
                    }

                    key.SetValue("Credentials", Username + "#//#" + Password);

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
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DVLDtraining"))
                {
                    if (key == null) return false;

                    string value = key.GetValue("Credentials") as string;

                    if (value == null) return false;

                    string[] result = value.Split(new string[] { "#//#" }, StringSplitOptions.None);

                    Username = result[0];

                    Password = result[1];

                    return true;
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
