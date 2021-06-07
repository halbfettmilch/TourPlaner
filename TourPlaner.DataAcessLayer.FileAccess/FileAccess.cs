using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;
using log4net;
using TourPlaner_andreas.DAL.common;
using TourPlaner_andreas.Models;
using Document = iText.Layout.Document;
using Image = System.Drawing.Image;
using Paragraph = iText.Layout.Element.Paragraph;
using PdfDocument = iText.Kernel.Pdf.PdfDocument;
using PdfWriter = iText.Kernel.Pdf.PdfWriter;
using iTextSharp;


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
                            writer.WriteLine(log.MaxVelocity);
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
        
        public bool DeletePictureOfTour(int id)
        {
            string path = "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Pics\\" + id +
                          ".jpg";
            
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                else
                {
                    throw new ArgumentException("File does not exsist");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

            }

            return false;

        }
        public string GetImagePath(int id) 
        {
            string path = "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Pics\\" + id +
                          ".jpg";
            try
            {
                if (File.Exists(path))
                {
                    return path;
                }
                else
                {
                    throw new ArgumentException("File does not exsist");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

            }

            return null;

        }
        public bool CreateTourPdf(TourItem tourItem)
        {
            try
            {
                if (checkStringsEmptyTour(tourItem))
                {
                    var writer = new PdfWriter("Reports.pdf");
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);


                    var header = new Paragraph("Tour Report: ")
                        .SetFontSize(30);

                    document.Add(header);


                    var lines1 =
                        new Paragraph("-------------------------------------------------------------------------- ")
                            .SetFontSize(20);
                    var tmpContent1 = new Paragraph("Name: " + tourItem.Name)
                        .SetFontSize(10);
                    var tmpContent2 = new Paragraph("Created on: " + tourItem.CreationTime)
                        .SetFontSize(10);
                    var tmpContent3 = new Paragraph("Tour length " + tourItem.TourLength + " km")
                        .SetFontSize(10);
                    var tmpContent4 = new Paragraph("Expected duration " + tourItem.Duration + " h")
                        .SetFontSize(10);
                    var tmpContent5 = new Paragraph("Description: " + tourItem.Description)
                        .SetFontSize(8);
                    var lines2 =
                        new Paragraph("-------------------------------------------------------------------------- ")
                            .SetFontSize(20);


                   // iTextSharp.text.Image newimage = iTextSharp.text.Image.GetInstance(GetImagePath(tourItem.TourID)); endless conversion tried did not work
                   // document.Add(newimage);
                    document.Add(lines1);
                    document.Add(tmpContent1);
                    document.Add(tmpContent2);
                    document.Add(tmpContent3);
                    document.Add(tmpContent4);
                    document.Add(tmpContent5);
                    document.Add(lines2);


                    document.Close();
                }


                else
                {
                    throw new ArgumentException("Tour length or Expected Duration of " + tourItem.Name +
                                                " in not in the right Format");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }

            return true;
        }

        public bool CreateTourLogsPdf(ObservableCollection<TourLog> tourLogs, TourItem tourItem)
        {
            var writer = new PdfWriter("Reports.pdf");
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            try
            {
                if (checkStringsEmptyTour(tourItem))
                {
                    var header = new Paragraph("Tour Report: ")
                        .SetFontSize(30);

                    document.Add(header);


                    var lines = new Paragraph(
                            "-------------------------------------------------------------------------- ")
                        .SetFontSize(20);
                    var tmpContent1 = new Paragraph("Name: " + tourItem.Name)
                        .SetFontSize(10);
                    var tmpContent2 = new Paragraph("Created on: " + tourItem.CreationTime)
                        .SetFontSize(10);
                    var tmpContent3 = new Paragraph("Tour length " + tourItem.TourLength + " km")
                        .SetFontSize(10);
                    var tmpContent4 = new Paragraph("Expected duration " + tourItem.Duration + " h")
                        .SetFontSize(10);
                    var tmpContent5 = new Paragraph("Description: " + tourItem.Description)
                        .SetFontSize(8);


                    document.Add(lines);
                    document.Add(tmpContent1);
                    document.Add(tmpContent2);
                    document.Add(tmpContent3);
                    document.Add(tmpContent4);
                    document.Add(tmpContent5);
                    document.Add(lines);
                    foreach (var tourLog in tourLogs)
                        try
                        {
                            if (checkLogsEmpty(tourLog))
                            {
                                var tmplog1 = new Paragraph("LogID: " + tourLog.LogId)
                                    .SetFontSize(20);
                                var tmplog2 = new Paragraph("Date: " + tourLog.Date)
                                    .SetFontSize(10);
                                var tmplog3 = new Paragraph("Max Velocity: " + tourLog.MaxVelocity)
                                    .SetFontSize(10);
                                var tmplog4 = new Paragraph("Min Velocity: " + tourLog.MinVelocity)
                                    .SetFontSize(10);
                                var tmplog5 = new Paragraph("Av Velocity: " + tourLog.AvVelocity)
                                    .SetFontSize(10);
                                var tmplog6 = new Paragraph("CaloriesBurnt: " + tourLog.CaloriesBurnt)
                                    .SetFontSize(10);
                                var tmplog7 = new Paragraph("Duration: " + tourLog.Duration)
                                    .SetFontSize(10);
                                var tmplog8 = new Paragraph("Author: " + tourLog.Author)
                                    .SetFontSize(10);
                                var tmplog9 = new Paragraph("Comment: " + tourLog.Comment)
                                    .SetFontSize(8);


                                document.Add(tmplog1);
                                document.Add(tmplog2);
                                document.Add(tmplog3);
                                document.Add(tmplog4);
                                document.Add(tmplog5);
                                document.Add(tmplog6);
                                document.Add(tmplog7);
                                document.Add(tmplog8);
                                document.Add(tmplog9);
                                document.Add(lines);
                            }
                            else
                            {
                                throw new NullReferenceException("Some Value of Log with ID: " + tourLog.LogId +
                                                                 " is empty");
                            }


                        }
                        catch (Exception ex)
                        {
                            document.Close();
                            log.Error(ex.Message);
                            return false;
                        }



                }
                else
                {
                    throw new NullReferenceException("Some Value of " + tourItem.Name + " is empty");
                }
            }
            catch (Exception ex)
            {
                document.Close();
                log.Error(ex.Message);
                return false;
            }




            document.Close();
            return true;
        }
        public bool checkStringsEmptyTour(TourItem tourItem)
        {
            if (tourItem.Name != "\"\"" && tourItem.CreationTime.ToString() != "\"\"" &&
                tourItem.Fromstart != "\"\"" && tourItem.To != "\"\"" && tourItem.TourLength.ToString() != "\"\"" &&
                tourItem.Duration.ToString() != "\"\"" && tourItem.Description != "\"\"")
            {
                return true;
            }

            else return false;
        }
        public bool checkLogsEmpty( TourLog tourLog)
        {
            if (tourLog.LogId.ToString() != "\"\"" && tourLog.Date.ToString() != "\"\"" &&
                tourLog.MaxVelocity.ToString() != "\"\"" && tourLog.MinVelocity.ToString() != "\"\"" &&
                tourLog.AvVelocity.ToString() != "\"\"" && tourLog.CaloriesBurnt.ToString() != "\"\"" &&
                tourLog.Duration.ToString() != "\"\"" && tourLog.Author != "\"\"" &&
                tourLog.Comment != "\"\"")
            {
                return true;
            }
            else return false;
        }



    }
}
