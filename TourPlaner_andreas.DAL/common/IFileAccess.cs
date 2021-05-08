

using System;
using System.Collections.Generic;
using System.IO;
using TourPlaner_andreas.Models;

namespace TourPlaner_andreas.DAL.common
{
    public interface IFileAccess
    {
        int CreateNewTourItemFile(int tourId, string name, string url, DateTime creationTime, int tourLength, int duration);
        int CreateNewTourLogFile(int logId, DateTime date, int maxVelocity, int minVelocity, int avVelocity, int caloriesBurnt, int duration, TourItem loggedItem);

        IEnumerable<FileInfo> SearchFiles(string searchTerm, FileTypes searchType); //Infos über eine Datei wie z.B. Text und man kann Streams zum einlesen und auslesen starten
        IEnumerable<FileInfo> GetAllFiles(FileTypes searchType);

    }
}
