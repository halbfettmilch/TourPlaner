

using System;
using System.Collections.Generic;
using System.IO;
using TourPlaner_andreas.Models;

namespace TourPlaner_andreas.DAL.common
{
    public interface IFileAccess
    {
        int CreateNewTourItemFile(string namen, string url, DateTime creationTime);
        int CreateNewTourLogFile(string logText, TourItem logTourItem);

        IEnumerable<FileInfo> SearchFiles(string searchTerm, FileTypes searchType); //Infos über eine Datei wie z.B. Text und man kann Streams zum einlesen und auslesen starten
        IEnumerable<FileInfo> GetAllFiles(FileTypes searchType);

    }
}
