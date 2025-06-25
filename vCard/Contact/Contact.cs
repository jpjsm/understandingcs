using System;
using System.Collections.Generic;
using System.Text;
namespace Contact
{
    using PropertyBag;

    public abstract class Contact
    {
        public abstract ContactType ContactType { get; }
        public abstract string FullName { get; }
        public abstract IList<PointOfContact> PointsOfContact { get; }
        public abstract PropertyBag. Properties { get; }
        public override string ToString()
        {
            return FullName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            //Check for null and compare run-time types.
            Contact c = obj as Contact;



            if (c == null)
            {
                return false;
            }

            if (this.ContactType != c.ContactType ||
                this.FullName != c.FullName ||
                this.PointsOfContact.Count != c.PointsOfContact.Count)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode() << Convert.ToInt32(ContactType);
        }
    }
}
