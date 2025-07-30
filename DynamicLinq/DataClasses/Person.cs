using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClasses
{
    public enum Gender
    {
        Male, Female
    }

    public enum EmailType
    {
        Personal,
        Work,
        Education,
    }



    public class Person
    {
        public string FirstName { get; set; }
        public string[] MiddleNames { get; set; } 
        public string LastName { get; set; }
        public string[] OtherLastNames { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string[] EmailAddresses { get; set; }


    }
}
