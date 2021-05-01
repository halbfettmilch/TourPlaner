using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;
using TourPlaner_andreas.DAL.common;
using TourPlaner_andreas.Models;

namespace TourPlaner.DataAcessLayer.FileAccess
{
    public class FileAccess : IFileAccess
    {

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
            return Path.GetFullPath(filePath,fileName);

        }
        public int CreateNewTourItemFile(int tourId, string name, string url, DateTime creationTime, int tourLength, int duration)
        {
            int id = Guid.NewGuid().GetHashCode();
            string fileName = id + "_TourITem.txt";
            string path = GetFullPath(fileName);

            using (StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine(tourId);  
                writer.WriteLine(name); 
                writer.WriteLine(url);
                writer.WriteLine(creationTime);
                writer.WriteLine(tourLength);
                writer.WriteLine(duration);
            }

            return id;
        }

        public int CreateNewTourLogFile(string logText, TourItem logTourItem)
        {

            int id = Guid.NewGuid().GetHashCode();
            string fileName = id + "_TourLog.txt";
            string path = GetFullPath(fileName);

            using (StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine(id);
                writer.WriteLine(logText);
                writer.WriteLine(logTourItem.TourID);
               
            }

            return id;
        }

        public IEnumerable<FileInfo> GetAllFiles(FileTypes searchType)
        {
            return getFileInfos(filePath, searchType);
        }

        public IEnumerable<FileInfo> SearchFiles(string searchTerm, FileTypes searchType)
        {
            IEnumerable<FileInfo> fileList = getFileInfos(filePath, searchType);
            IEnumerable<FileInfo> queryMatchingFiles= 
                from file in fileList
                let fileText = GetFileText(file) 
                where fileText.Contains(searchTerm) 
                select file;
            return queryMatchingFiles;
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
