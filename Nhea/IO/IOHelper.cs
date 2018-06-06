using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nhea.IO
{
    public static class IOHelper
    {
        private const string PathDelimeter = "\\";

        public static string PrepareDirectory(string directory)
        {
            return PrepareDirectory(directory, false);
        }

        public static string PrepareDirectory(string directory, bool createUniqueDirectory)
        {
            if (!directory.EndsWith(PathDelimeter))
            {
                directory += PathDelimeter;
            }

            if (createUniqueDirectory)
            {
                string uniqueDirectory = Guid.NewGuid().ToString();

                directory += uniqueDirectory + PathDelimeter;
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!directory.EndsWith(PathDelimeter))
            {
                directory += PathDelimeter;
            }

            return directory;
        }

        public static string ReadFileContent(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream);

            try
            {
                return streamReader.ReadToEnd();
            }
            finally
            {
                streamReader.Close();
                fileStream.Close();
            }
        }
    }
}
