using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace CometX.Application.Models.FileExplorer
{
    public class Folder
    {
        public double DirectoryId { get; set; }
        public string FolderName { get; set; }
        public List<Folder> Subfolders { get; set; }
        public List<File> Files { get; set; }

        public Folder()
        {
            FolderName = "";
            Subfolders = new List<Folder>();
            Files = new List<File>();
        }

        public Folder(string directoryItem)
        {
            FolderName = directoryItem;
            Subfolders = new List<Folder>();
            Files = new List<File>();
        }

        public void AddFileToLastSubFolder(File file, Folder folder)
        {
            if (folder.Subfolders.Any()) folder.AddFileToLastSubFolder(file, folder.Subfolders.First());
            else folder.Files.Add(file);
        }

        public void AddFolderToLastSubFolder(string folderName, Folder folder)
        {
            if (folder.Subfolders.Any()) folder.AddFolderToLastSubFolder(folderName, folder.Subfolders.First());
            else folder.Subfolders.Add(new Folder(folderName));
        }
    }

    public static class FolderConverter
    {
        public static void AssignFolderIdentifiers(this List<Folder> folders, ref int id)
        {
            foreach (var folder in folders)
            {
                folder.DirectoryId = id;

                id++;

                if (folder.Subfolders != null && folder.Subfolders.Any()) folder.Subfolders.AssignFolderIdentifiers(ref id);
            }
        }

        public static void SetRootIdentifier(this Folder folder)
        {
            //Try to convert Camel casing to have spaces in between the words
            // Example --> "TwoWords" to "Two Words" casing 
            switch (folder.FolderName)
            {
                case "Folder1":
                    folder.FolderName = "Folder 1";
                    break;
                case "Folder2":
                    folder.FolderName = "Folder 2";
                    break;
                case "Folder3":
                    folder.FolderName = "Folder 3";
                    break;
                default:
                    folder.FolderName = "Root";
                    break;
            }
        }

        public static List<Folder> SortFileStructure(this IEnumerable<Folder> folders)
        {
            if (!folders.Any()) return folders.ToList();

            var num = 0;
            var sortByMonth = !int.TryParse(folders.First().FolderName, out num);
            return sortByMonth ? folders.OrderBy(s => DateTime.ParseExact(s.FolderName, "MMMM", new CultureInfo("en-US"))).ToList() : folders.OrderBy(x => int.Parse(x.FolderName)).ToList();
        }

        public static List<Folder> ToFileStructure(this IEnumerable<Folder> folders, int num = 0)
        {
            return folders.GroupBy(x => x.FolderName, x => x).Select(x => new Folder
            {
                FolderName = x.Key,
                Subfolders = x.SelectMany(y => y.Subfolders).Any() ? x.SelectMany(y => y.Subfolders).ToFileStructure() : null,
                Files = x.SelectMany(y => y.Files).ToList()
            }).SortFileStructure();
        }
    }
}
