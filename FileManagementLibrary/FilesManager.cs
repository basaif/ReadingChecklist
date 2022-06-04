namespace FileManagementLibrary
{
    public class FilesManager
    {
        public FilesManager(string location)
        {
            _location = location;
        }

        private string _location;

        public void ChangeLocation(string newLocation)
        {
            _location = newLocation;
        }

        public bool DoesLocationExist(string location)
        {
            return Directory.Exists(location);
        }

        public List<string> GetAllFilePathsInLocation()
        {
            string[] files = Directory.GetFiles(_location, "*", SearchOption.AllDirectories);

            return files.ToList();
        }

        public string GetFileNameFromFilePath(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public List<string> GetFoldersFromFilePath(string filePath)
        {
            List<string> folders = new();

            string fileName = GetFileNameFromFilePath(filePath);

            List<string> tokens = GetListPathParts(filePath);

            foreach (string t in tokens)
            {
                if (!(_location.Contains(t) || t == fileName))
                {
                    if (!folders.Contains(t))
                    {
                        folders.Add(t);
                    }
                }
            }
            return folders;
        }

        public List<(List<string> Folders, string FileName)> GetAllFoldersFileNamePairsInLocation()
        {
            List<(List<string> Folders, string FileName)> foldersNamePairs = new();

            List<string> files = GetAllFilePathsInLocation();

            foreach (string file in files)
            {
                foldersNamePairs.Add(GetFoldersFileNamePairs(file));
            }

            return foldersNamePairs;
        }

        public (List<string> Folders, string FileName) GetFoldersFileNamePairs(string filePath)
        {
            return (GetFoldersFromFilePath(filePath), GetFileNameFromFilePath(filePath));
        }

        public List<string> GetListPathParts(string filePath)
        {
            return filePath.Split('\\').ToList();
        }

    }
}