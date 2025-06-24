using System;

namespace UnderstandingClassInheritance
{
	public class Alpha
	{
		public string foo { get; set; }

		public override string ToString()
		{
			return $"Foo: {foo}";
		}

	}

	public class Beta : Alpha
	{
		public string bar { get; set; }

		public override string ToString()
		{
			return $"Foo: {foo}, Bar: {bar}";
		}
	}

	class Program
    {
        static void Main(string[] args)
        {
			Beta b = new Beta() { bar = "bar", foo = "foo" };
			Console.WriteLine(b);
        }
    }
}
