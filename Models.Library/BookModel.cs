namespace Models.Library
{
	public class BookModel : IEquatable<BookModel>, IComparable<BookModel>
	{
		public int Id
		{
			get; set;
		}
		public string BookName { get; set; } = string.Empty;

		public bool IsRead
		{
			get; set;
		}

		public DateTime DateRead
		{
			get; set;
		}

		public List<TagModel> Tags { get; set; } = new();

		public BookModel(long id, string bookName, long isRead, string dateRead)
		{
			Id = (int)id;

			BookName = bookName;

			IsRead = isRead == 1;

			DateRead = DateTime.Parse(dateRead);
		}

		public BookModel(string bookName, bool isRead, DateTime dateRead, List<TagModel> tags)
		{
			BookName = bookName;

			IsRead = isRead;

			DateRead = dateRead;

			Tags = tags;
		}

		public bool Equals(BookModel? other)
		{
			return IsEqual(other);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public int CompareTo(BookModel? other)
		{
			return BookName.CompareTo(other?.BookName);
		}

		public override bool Equals(object? obj)
		{
			return Equals(obj as BookModel);
		}

		public bool IsSameBook(BookModel other)
		{
			return BookName == other?.BookName;
		}

		private bool IsEqual(BookModel? other)
		{
			bool output = false;

			if (other != null)
			{
				if (Id != other.Id)
				{
					return output;
				}
				else if (!BookName.Equals(other.BookName))
				{
					return output;
				}
				else if (!DateRead.Equals(other.DateRead))
				{
					return output;
				}
				else if (!IsRead.Equals(other.IsRead))
				{
					return output;
				}
				else if (Tags.Count != other.Tags.Count)
				{
					return output;
				}
				else if (Tags.Count == other.Tags.Count)
				{
					for (int i = 0; i < Tags.Count; i++)
					{
						if (!Tags[i].Equals(other.Tags[i]))
						{
							return output;
						}
					}
				}
			}
			else
			{
				return false;
			}

			output = true;

			return output;
		}
	}
}