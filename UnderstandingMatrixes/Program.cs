namespace UnderstandingMatrixes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            int rows = 6;
            int columns = 5;
            Matrix2D<int> M1 = new Matrix2D<int>(rows, columns);
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    M1[r, c] = (r * columns) + c;
                }
            }

            Console.WriteLine(M1);
            Console.WriteLine(M1.ToMatrixString(3));

            Matrix2D<int> M2 = new Matrix2D<int>(new int[] { 0,1,2,3,4,5,6,7,8,9, 10, 11}, columns);
            Console.WriteLine(M2);
            Console.WriteLine(M2.ToMatrixString(3));

            int[,] array2d = new int[3,4] {
                { 0, 1, 2, 3 },
                { 4, 5, 6, 7}, 
                { 8, 9, 10, 11},
            };

            Matrix2D<int> M3 = new Matrix2D<int>(array2d);
            Console.WriteLine(M3);
            Console.WriteLine(M3.ToMatrixString(3));
            M3.HorizontalFlip();
            Console.WriteLine(M3.ToMatrixString(3));
            M3.HorizontalFlip();
            Console.WriteLine(M3.ToMatrixString(3));
            M3.VerticalFlip();
            Console.WriteLine(M3.ToMatrixString(3));
            M3.VerticalFlip();
            Console.WriteLine(M3.ToMatrixString(3));

            M3.Right90();
            Console.WriteLine(M3.ToMatrixString(3));
            M3.Right90();
            Console.WriteLine(M3.ToMatrixString(3));
        }
    }
}