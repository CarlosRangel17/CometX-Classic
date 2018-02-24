using System;
using System.IO;

namespace CometX.Application.Models.FileExplorer
{
    public class File
    {
        public long DirectoryId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public string DateCreated { get; set; }
        public string DunningLetterType { get; set; }
        public string ReportType { get; set; }

        public File()
        {

        }

        public File(FileInfo fileInfo)
        {
            DirectoryId = DateTime.Now.Ticks;
            FileName = fileInfo.Name;
            ReportType = FileName.Split('_')[1];
            FileExtension = fileInfo.Extension;
            FilePath = fileInfo.FullName;
            FileSize = fileInfo.Length.ToString() + " bytes";
            DateCreated = fileInfo.CreationTime.ToString();
        }
    }
}
