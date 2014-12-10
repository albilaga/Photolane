using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace Photolane.Droid.Helper
{
    public static class IsoStorage
    {
        public static void Save<T>(string fileName, T item)
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists(fileName))
                {
                    storage.DeleteFile(fileName);
                }
                using (var fileStream = new IsolatedStorageFileStream(fileName, FileMode.CreateNew, storage))
                {
                    using (var writer = new StreamWriter(fileStream))
                    {
                        writer.Write(item);
                    }
                }
            }
        }

        public static string Load(string fileName)
        {
            try
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!storage.FileExists(fileName)) return "";
                    using (var fileStream = new IsolatedStorageFileStream(fileName, FileMode.Open, storage))
                    {
                        using (var reader = new StreamReader(fileStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}