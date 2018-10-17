using System;
using System.IO;

namespace Nhea.Helper
{
    public static class IOHelper
    {
        public static byte[] Concat(params byte[][] buffers)
        {
            int totalLength = 0;

            for (int i = 0; i < buffers.Length; i++)
            {
                totalLength += buffers[i].Length;
            }

            byte[] concatData = new byte[totalLength];

            int index = 0;

            for (int i = 0; i < buffers.Length; i++)
            {
                System.Buffer.BlockCopy(buffers[i], index, concatData, 0, buffers[i].Length);

                index += buffers[i].Length;
            }

            return concatData;
        }

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
