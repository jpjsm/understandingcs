abstract class AbstractIntegerContainer<T>where T : struct
{
    public static HashSet<Type> ValidIntegerTypes = new HashSet<Type>{
        typeof(long),
        typeof(int),
        typeof(short),
        typeof(sbyte),

        typeof(ulong),
        typeof(uint),
        typeof(ushort),
        typeof(byte)
    };
    private List<T> integers = [];

    public AbstractIntegerContainer()
    {
        if (!ValidIntegerTypes.Contains(typeof(T)))
        {
            throw new ArgumentException("Class only supports integer data types.");
        }
    }

    public abstract T this[int i] { get; set; }
}