using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Css
{
    public class CssVariable: IEquatable<CssVariable>
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string CurrentValue { get; set; }

        public CssVariable()
        {
            Id = string.Empty;
            Name = string.Empty;            
            CurrentValue = string.Empty;
        }

        public static bool TagContainsName(string tag, string delimiterStart, string delimiterEnd, char delimiterName)
        {
            bool result = false;

            string fullTagData = GetTagDataWithoutDelimiterers(tag,delimiterStart, delimiterEnd);
            if (fullTagData.Contains(delimiterName))
            {
                result = true;
            }
            return result;
        }

        private static string GetTagDataWithoutDelimiterers(string fullTag, string delimiterStart, string delimiterEnd)
        {
            string result = string.Empty;

            if (fullTag.StartsWith(delimiterStart))
            {
                if (fullTag.EndsWith(delimiterEnd))
                {
                    int delimiterTotalLength = delimiterStart.Length + delimiterEnd.Length;

                    if (fullTag.Length > delimiterTotalLength)
                    {
                        result = fullTag.Substring(delimiterStart.Length, fullTag.Length - delimiterTotalLength);
                        result = result.Trim();
                    }
                }
            }

            return result;
        }

        public static CssVariable ParseFromTag(string tag, string delimiterStart, string delimiterEnd, char delimiterName)
        {
            CssVariable result = new CssVariable();

            string tagDataNoDelimiters = GetTagDataWithoutDelimiterers(tag, delimiterStart, delimiterEnd);

            if (TagContainsName(tag, delimiterStart, delimiterEnd, delimiterName))
            {
                string[] tagparts = tagDataNoDelimiters.Split(delimiterName);
                if (tagparts.Count() > 1)
                {
                    result.Id = tagparts[0];
                    int splitterLocation = tagDataNoDelimiters.IndexOf(delimiterName);
                    result.Name = tagDataNoDelimiters.Substring(splitterLocation + 1,
                                                                tagDataNoDelimiters.Length - result.Id.Length - 1);
                }
            }
            else
            {
                result.Id = tagDataNoDelimiters;
            }           

            return result;
        }

        #region IEquatable<CssVariable> Members

        public bool Equals(CssVariable other)
        {            

            if (other.Name == Name)
            {
                if (other.CurrentValue == CurrentValue)
                {
                    if (other.Id == Id)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null) return base.Equals(obj);

            if (!(obj is CssVariable))
                throw new InvalidCastException("The 'obj' argument is not a CssVariable object.");
            else
                return Equals(obj as CssVariable);
        }

        public override int GetHashCode()
        {
            string hash = this.Id + this.CurrentValue + this.Name;
            return hash.GetHashCode();
        }

        public static bool operator ==(CssVariable var1, CssVariable var2)
        {
            return var1.Equals(var2);
        }

        public static bool operator !=(CssVariable var1, CssVariable var2)
        {
            return (!var1.Equals(var2));
        }


        #endregion
    }
}
