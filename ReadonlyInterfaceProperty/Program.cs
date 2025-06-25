using System;

namespace ReadonlyInterfaceProperty
{
    public interface IReadOnlyProperty
    {
        public string Name { get; }
    }

    public class ReadonlyProperty: IReadOnlyProperty
    {
        private string _name;
        public string Name { get { return _name; } }

        public ReadonlyProperty(string name) => _name = name;
        public void ChangeName(string name) => _name = name;
       
    }
    class Program
    {
        static void Main(string[] args)
        {
            ReadonlyProperty foo = new ReadonlyProperty("I'm Foo");
            Console.WriteLine(foo.Name);
            foo.ChangeName("I'm Foobar");
            Console.WriteLine(foo.Name);

        }
    }
}
