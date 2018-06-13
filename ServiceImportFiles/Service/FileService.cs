using ServiceImportFiles.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ServiceImportFiles.Service
{
    public class FileService
    {
        private string _path;

        public FileService(string path)
        {
            this._path = path;
        }

        public void Start()
        {
            while (true)
            {
                if (!Directory.Exists($"{_path}\\in") || !Directory.Exists($"{_path}\\out"))
                {
                    Thread.Sleep(10000);
                    continue;
                }
                string[] files = Directory.GetFiles($"{_path}\\in");
                foreach (string file in files)
                {
                    try
                    {
                        ConvertFile(file);
                        Console.WriteLine($"Read file: {file}");
                    }
                    catch (Exception ex)
                    {
                        //fake log
                        Console.WriteLine(ex.Message);
                    }
                }

                Thread.Sleep(5000);
            }
        }

        private void ConvertFile(string file)
        {
            string line = string.Empty;
            try
            {
                var salesService = new SalesService();
                using (StreamReader sr = new StreamReader(file))
                {
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        salesService.ReadLine(line);
                        line = sr.ReadLine();
                    }
                }
                SaveOutput(Path.GetFileName(file), salesService.GenerateOutput());
                File.Delete(file);
            }
            catch (IOException ioEx)
            {
                throw new IOException($"invalid file {Path.GetFileName(file)}", ioEx);
            }
        }

        private void SaveOutput(string fileName, OutputModel output)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine($"{_path}\\out", $"{fileName}.done.dat")))
            {
                outputFile.WriteLine(output.OutputContent);
            }
        }
    }
}
