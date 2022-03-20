using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ReadingChecklistModels
{
    public class TagModel : IEquatable<TagModel>, IComparable<TagModel>
    {
        public int Id { get; set; }
        public string TagName { get; set; } = string.Empty;

        public TagModel(long id, string tagName)
        {
            Id = (int)id;
            TagName = tagName;
        }

        public TagModel(string tagName)
        {
            TagName = tagName;
        }

        public bool Equals(TagModel? other)
        {
            return IsEqual(other);
        }

        private bool IsEqual(TagModel? other)
        {
            bool output = false;

            if (other != null)
            {
                if (Id != other.Id)
                {
                    return output;
                }
                else if (!TagName.Equals(other.TagName))
                {
                    return output;
                } 
            }
            else
            {
                return false;
            }

            output = true;

            return output;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TagModel);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public int CompareTo(TagModel? other)
        {
            return TagName.CompareTo(other?.TagName);
        }
    }
}
