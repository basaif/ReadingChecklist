using Xunit;

namespace FileSystemUtilities.Library.Tests
{
	public class FolderFileNamePairsTests
	{
		[Fact]
		public void GetFoldersFromFilePath_ReturnsListOfAllFolderNamesInFileRelativePath()
		{
			string filePath = @"F:\Books\Psychology\Social Psychology\Introduction to Social Psychology.pdf";
			FoldersFileNamePairs filesManager = new("F:\\Books");

			List<string> expected = new() { "Psychology", "Social Psychology" };
			List<string> actual = filesManager.GetFoldersFromFilePath(filePath);

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void GetFoldersFromFilePath_ReturnsEmptyListIfFileIsInRootLocation()
		{
			string filePath = @"F:\Books\Introduction to Social Psychology.pdf";
			FoldersFileNamePairs filesManager = new("F:\\Books");

			List<string> expected = new();
			List<string> actual = filesManager.GetFoldersFromFilePath(filePath);

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void GetFoldersFileNamePairs_ShouldReturnTupleWithListOfFoldersFileIsInAndFileName()
		{
			string filePath = @"F:\Books\Psychology\Social Psychology\Introduction to Social Psychology.pdf";
			FoldersFileNamePairs filesManager = new("F:\\Books");

			(List<string> Folders, string FileName) expected = (new()
			{
				"Psychology",
				"Social Psychology"
			},
				"Introduction to Social Psychology.pdf");
			(List<string> Folders, string FileName) actual = filesManager.GetFoldersFileNamePairs(filePath);

			Assert.Equal(expected.Folders, actual.Folders);
			Assert.Equal(expected.FileName, actual.FileName);
		}
	}
}
