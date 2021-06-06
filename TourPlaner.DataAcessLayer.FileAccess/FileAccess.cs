using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;
using log4net;
using TourPlaner_andreas.DAL.common;
using TourPlaner_andreas.Models;

namespace TourPlaner.DataAcessLayer.FileAccess
{
    public class FileAccess : IFileAccess
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string filePath;
       

        public FileAccess(string filePath)
        {
            this.filePath = filePath;
        }

        private IEnumerable<FileInfo> getFileInfos(string startFolder, FileTypes searchType)
        {
            DirectoryInfo dir = new DirectoryInfo(startFolder);
            return dir.GetFiles("*" + searchType.ToString() + ".txt", SearchOption.AllDirectories);
        }

        private string GetFullPath(string fileName)
        {
            return "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\TourFiles\\" + fileName;

        }
        public bool CreateTourItemLog(ObservableCollection<TourLog> Logs, TourItem item,string path)
        {
            var id = Guid.NewGuid().GetHashCode();
            var fileName = item.TourID + "_TourItem.txt";
           // var path = GetFullPath(fileName);
            try
            {
                if (item.Name != "\"\"" && item.CreationTime.ToString() != "\"\"" &&
                    item.Fromstart != "\"\"" && item.To != "\"\"" && item.TourLength.ToString() != "\"\"" &&
                    item.Duration.ToString() != "\"\"" && item.Description != "\"\"")
                {
                    using (var writer = File.CreateText(path))
                    {
                        writer.WriteLine(item.TourID);
                        writer.WriteLine(item.Name);
                        writer.WriteLine(item.Fromstart);
                        writer.WriteLine(item.To);
                        writer.WriteLine(item.CreationTime);
                        writer.WriteLine(item.TourLength);
                        writer.WriteLine(item.Duration);
                        writer.WriteLine(item.Description);
                        foreach (var log in Logs)
                        {
                            writer.WriteLine(log.LogId);
                            writer.WriteLine(log.Date);
                            writer.WriteLine(log.MinVelocity);
                            writer.WriteLine(log.MinVelocity);
                            writer.WriteLine(log.AvVelocity);
                            writer.WriteLine(log.CaloriesBurnt);
                            writer.WriteLine(log.Duration);
                            writer.WriteLine(log.Author);
                            writer.WriteLine(log.Comment);
                            writer.WriteLine(log.LogTourItem.TourID);
                        }


                    }
                }
                else
                {
                    throw new ArgumentException("Some Value of " + item.Name + " are empty");
                }
            }
            catch (Exception ex)
            {

                log.Error(ex.Message);
                return false;
            }
            return true;
        }

        public string[] ImportFile(string path)
        {
            return System.IO.File.ReadAllLines(path);
        }

        string GetFileText(FileInfo file)
        {
            using (StreamReader reader= file.OpenText())
            {
                StringBuilder sb = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    sb.Append(reader); // wenn fehler hier auftaucht: Video anschauen
                }

                return sb.ToString();
            }
            
        }
        
    }
}
