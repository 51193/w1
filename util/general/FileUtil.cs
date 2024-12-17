using Godot;
using System.IO;

namespace MyGame.Util
{
    public static class FileUtil
    {
        public static void EnsureDirectoryExists(string relativeFilePath)
        {
            string directoryPath = Path.GetDirectoryName(relativeFilePath);

            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                GD.Print($"Created directory: {directoryPath}");
            }
            else
            {
                GD.Print($"Directory already exists: {directoryPath}");
            }
        }

        public static void WriteToFile(string filePath, string data)
        {
            EnsureDirectoryExists(filePath);
            File.WriteAllText(filePath, data);
        }

        public static string ReadFromFile(string filePath)
        {
            EnsureDirectoryExists(filePath);
            return File.ReadAllText(filePath);
        }
    }
}
