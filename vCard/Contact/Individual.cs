using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contact
{
    public class Individual : Contact
    {
        private static readonly char[] whitespace = { '\u0020', '\u00A0', '\u1680', '\u2000', '\u2001',
                                                      '\u2002', '\u2003', '\u2004', '\u2005', '\u2006',
                                                      '\u2007', '\u2008', '\u2009', '\u200A', '\u202F',
                                                      '\u2007', '\u2008', '\u2009', '\u200A', '\u202F',
                                                      '\u2007', '\u205F', '\u3000', '\u2028', '\u2029',
                                                      '\u0009', '\u000A', '\u000B', '\u000C', '\u000D',
                                                      '\u0085'
                                                    };
        private string[] names;
        private string[] surnames;
        private List<PointOfContact> pointsOfContact;

        public override ContactType ContactType => ContactType.Individual;

        public override string FullName => string.Join("{0}  {1}", Names, Surnames);

        public override PointOfContact[] PointsOfContact => pointsOfContact.ToArray();

        public string FirstName { get { return names[0]; } }

        public string MiddleNames { get { return names.Length > 1 ? string.Join(" ", names.Skip(1)) : null; } }

        public string Names { get { return string.Join(" ", names); } }

        public string LastName { get { return surnames[0]; } }

        public string OtherLastNames { get { return surnames.Length > 1 ? string.Join(" ", surnames.Skip(1)) : null; } }

        public string Surnames { get { return string.Join(" ", surnames); } }

        public Individual(IEnumerable<string> names, IEnumerable<string> surnames, IEnumerable<PointOfContact> pointsOfContact = null)
        {
            if (names == null || names.Count() == 0 || names.All(n => String.IsNullOrWhiteSpace(n)))
                throw new ArgumentNullException(nameof(names));

            if (surnames == null || surnames.Count() == 0 || surnames.All(n => String.IsNullOrWhiteSpace(n)))
                throw new ArgumentNullException(nameof(surnames));

            this.names = names.Where(n => !String.IsNullOrWhiteSpace(n)).Select(n => n.Trim()).ToArray();

            this.surnames = surnames.Where(n => !String.IsNullOrWhiteSpace(n)).Select(n => n.Trim()).ToArray();

            if (pointsOfContact == null)
            {
                this.pointsOfContact = new List<PointOfContact>();
            }
            else
            {
                this.pointsOfContact = pointsOfContact.Where(p => p != null).ToList();
            }
        }

        public Individual(string names, string surnames)
            : this(names.Split(whitespace, StringSplitOptions.RemoveEmptyEntries), surnames.Split(whitespace, StringSplitOptions.RemoveEmptyEntries))
        {
        }
    }
}
