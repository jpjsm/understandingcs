using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingMatrixes
{
    public class Matrix2D<T>
    {
        private T[] data;
        private int rows;
        private int columns;

        public Matrix2D(int rows, int columns)
        {
            if (1 > rows)
            {
                throw new ArgumentOutOfRangeException(nameof(rows), $"{nameof(rows)} must be greater or equal to 1");
            }

            if (1 > columns)
            {
                throw new ArgumentOutOfRangeException(nameof(columns), $"{nameof(columns)} must be greater or equal to 1");
            }

            this.rows = rows;
            this.columns = columns;
            this.data = new T[rows * columns];
        }

        public Matrix2D(T[] data, int columns) : this(columns, data.Length / columns + 1)
        {
            data.CopyTo(this.data, 0);
        }

        public Matrix2D(T[,] data)
        {
            if (data.Rank != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(data), $"needs to be a 2 dimensional array; instead {data.Rank} were given.");
            }
            this.rows = data.GetLength(0);
            this.columns = data.GetLength(1);

            this.data = new T[this.rows * this.columns];

            for (int r = 0; r < this.rows; r++)
            {
                for (int c = 0; c < this.columns; c++)
                {
                    this.data[r * this.columns + c] = data[r, c];
                }
            }
        }

        public T this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= this.rows)
                {
                    throw new ArgumentOutOfRangeException(nameof(row), $"{row} must be greter equal to zero and less than {this.rows}");
                }

                if (column < 0 || column >= this.columns)
                {
                    throw new ArgumentOutOfRangeException(nameof(column), $"{column} must be greter equal to zero and less than {this.columns}");
                }
                int i = row * this.columns + column;

                return this.data[i];
            }
            set
            {
                if (row < 0 || row >= this.rows)
                {
                    throw new ArgumentOutOfRangeException(nameof(row), $"{row} must be greter equal to zero and less than {this.rows}");
                }

                if (column < 0 || column >= this.columns)
                {
                    throw new ArgumentOutOfRangeException(nameof(column), $"{column} must be greter equal to zero and less than {this.columns}");
                }
                int i = row * this.columns + column;

                this.data[i] = value;
            }
        }

        public void HorizontalFlip()
        {
            for (int r = 0; r < this.rows - 1 - r; r++)
            {
                for (int c = 0; c < this.columns; c++)
                {
                    (data[r * this.columns + c], data[(this.rows - 1 - r) * this.columns + c]) = (data[(this.rows - 1 - r) * this.columns + c], data[r * this.columns + c]);
                }
            }
        }

        public void VerticalFlip()
        {
            for (int r = 0; r < this.rows; r++)
            {
                for (int c = 0; c < this.columns - 1 - c; c++)
                {
                    (data[r * this.columns + c], data[r * this.columns + this.columns - 1 - c]) =
                        (data[r * this.columns + this.columns - 1 - c], data[r * this.columns + c]);
                }
            }
        }

        public void Right90()
        {
            Matrix2D<T> tmp = new(this.columns, this.rows);
            for (int r = 0; r < this.rows; r++)
            {
                for (int c = 0; c < this.columns; c++)
                {
                    tmp[c, r] = this[r, c];
                }
            }

            tmp.VerticalFlip();

            tmp.data.CopyTo(this.data, 0);
            (this.columns, this.rows) = (this.rows, this.columns);
        }

        public string ToMatrixString(int itemWidth = 1)
        {
            StringBuilder result = new StringBuilder();
            for (int r = 0; r < this.rows; r++)
            {
                result.Append(string.Join(", ", Enumerable.Range(0, this.columns).Select(i => $"{this.data[r * this.columns + i]}".PadLeft(itemWidth)).ToArray()) + Environment.NewLine);
            }

            return result.ToString();
        }

        public override string ToString()
        {
            return $"Rows: {this.rows}, Columns: {this.columns}, data:  {String.Join(", ", this.data.Select(d => $"{d}").ToArray())}";
        }
    }
}
