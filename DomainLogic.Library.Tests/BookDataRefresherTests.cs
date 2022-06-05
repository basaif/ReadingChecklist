using FileSystemUtilities.Library;
using Models.Library;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DomainLogic.Library.Tests
{
	public class BookDataRefresherTests
	{
		[Fact]
		public void AreTagsInBook_ReturnsTrueIfBookContiansAllAndOnlyTagsProvided()
		{
			List<TagModel> tags = new()
			{
				new TagModel("Science"),
				new TagModel("Text Books"),
				new TagModel("Earth Science")
			};

			List<TagModel> sameTags = new()
			{
				new TagModel("Science"),
				new TagModel("Text Books"),
				new TagModel("Earth Science")
			};

			BookModel book = new("Introduction to Geology", false, DateTime.Now, tags);

			List<TagModel> differentTags = new()
			{
				new TagModel("Psychology"),
				new TagModel("PDFs"),
				new TagModel("Ebooks")
			};

			List<TagModel> missingTags = new()
			{
				new TagModel("Science"),
				new TagModel("Earth Science")
			};

			List<TagModel> addedTags = new()
			{
				new TagModel("Science"),
				new TagModel("Text Books"),
				new TagModel("Earth Science"),
				new TagModel("Introductions"),
			};

			Mock<IFoldersFileNamePairs> mockFoldersFileNamePairs = new();
			Mock<ITagsCreator> mockTagsCreator = new();

			mockTagsCreator.Setup(t => t.LoadTags()).Returns(new List<TagModel>() {new TagModel("Science"),
				new TagModel("Text Books"),
				new TagModel("Earth Science"),
				new TagModel("Introductions") });

			IBooksDataRefresher bookDataRefresher = new BooksDataRefresher(mockFoldersFileNamePairs.Object, mockTagsCreator.Object);

			Assert.True(bookDataRefresher.AreTagsInBook(book, sameTags));
			Assert.True(!bookDataRefresher.AreTagsInBook(book, differentTags));
			Assert.True(!bookDataRefresher.AreTagsInBook(book, addedTags));
			Assert.True(!bookDataRefresher.AreTagsInBook(book, missingTags));
		}
	}
}
