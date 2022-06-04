namespace FileManagementLibrary
{
    public class FilesManager
    {
        private string _location;
        public void ChangeLocation(string newLocation)
        {
            _location = newLocation;
        }

        public FilesManager(string location)
        {
            _location = location;
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
        private List<string> GetAllFilePathsInLocation()
        {
            string[] files = Directory.GetFiles(_location, "*", SearchOption.AllDirectories);

            return files.ToList();
        }
        public (List<string> Folders, string FileName) GetFoldersFileNamePairs(string filePath)
        {
            return (GetFoldersFromFilePath(filePath), GetFileNameFromFilePath(filePath));
        }
        public List<string> GetFoldersFromFilePath(string filePath)
        {
            List<string> folders = new();

            string fileName = GetFileNameFromFilePath(filePath);

            List<string> tokens = GetListPathParts(filePath);

            AddTokensToFolders(tokens, folders, fileName);

            return folders;
        }
        private string GetFileNameFromFilePath(string filePath)
        {
            return Path.GetFileName(filePath);
        }
        private List<string> GetListPathParts(string filePath)
        {
            return filePath.Split('\\').ToList();
        }
        private void AddTokensToFolders(List<string> tokens, List<string> folders, string fileName)
        {
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
        }
    }
}