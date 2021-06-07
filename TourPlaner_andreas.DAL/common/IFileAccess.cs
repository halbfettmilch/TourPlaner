

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using TourPlaner_andreas.Models;

namespace TourPlaner_andreas.DAL.common
{
    public interface IFileAccess
    {
        public bool CreateTourItemLog(ObservableCollection<TourLog> Logs, TourItem item, string path);
        public string[] ImportFile(string path);
        public bool DeletePictureOfTour(int id);
        public string GetImagePath(int id);
        public bool checkStringsEmptyTour(TourItem tourItem);
        public bool checkLogsEmpty(TourLog tourLog);

    }
}
