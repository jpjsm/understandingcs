using System;

namespace Contact
{
    public abstract class PointOfContact
    {
        public abstract PointOfContanctType PointOfContanctType { get; }
        public abstract string PointOfContactData { get; }

        public override string ToString()
        {
            return PointOfContactData;
        }

        public override int GetHashCode()
        {
            return PointOfContactData.GetHashCode() << Convert.ToInt32(PointOfContanctType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            //Check for null and compare run-time types.
            PointOfContact p = obj as PointOfContact;



            if (p == null)
            {
                return false;
            }

            if (this.PointOfContactData != p.PointOfContactData)
            {
                return false;
            }

            return true;
        }

    }
}
