using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Library
{
	public class BookEqualityComparer : IEqualityComparer<BookModel>
	{
		public bool Equals(BookModel? x, BookModel? y)
		{
			if (x is not null)
			{
				return x.Equals(y);
			}
			return false;
		}

		public int GetHashCode([DisallowNull] BookModel obj)
		{
			return obj.GetHashCode();
		}
	}
}
