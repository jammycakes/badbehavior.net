using System;

namespace BadBehavior
{
    /// <summary>
    ///  A case-insensitive string comparer that also ignores the distinction
    ///  between underscores and hyphens.
    /// </summary>
    /// <remarks>
    ///  This emulates how headers_mixed works in reference BB.
    /// </remarks>

    public class HeadersMixedComparer : StringComparer
    {
        public static readonly HeadersMixedComparer Instance = new HeadersMixedComparer();

        public override int Compare(string x, string y)
        {
            return StringComparer.InvariantCultureIgnoreCase.Compare
                (x.Replace('_', '-'), y.Replace('_', '-'));
        }

        public override bool Equals(string x, string y)
        {
            return this.Compare(x, y) == 0;
        }

        public override int GetHashCode(string obj)
        {
            return obj.Replace('_', '-').GetHashCode();
        }
    }
}
