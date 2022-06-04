namespace FileSystemUtilities.Library
{
	public interface IFoldersFileNamePairs
	{
		void ChangeLocation(string newLocation);
		List<(List<string> Folders, string FileName)> GetAllFoldersFileNamePairsInLocation();
	}
}