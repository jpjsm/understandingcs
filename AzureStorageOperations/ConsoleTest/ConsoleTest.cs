using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace ConsoleTest
{
    using AzureStorageOperations;

    public class TestEntity : TableEntity
    {
        public string PartitionId { get { return PartitionKey; } }
        public string RowId { get { return RowKey; } }
        public TestEntity()
        {
            PartitionKey = "1";
            RowKey = "1";
        }

        public TestEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
    }
    class ConsoleTest
    {
        const string TestConnection = "DefaultEndpointsProtocol=https;AccountName=jpjofrestorage;AccountKey=URbOxBi0ckYU7XTxV4OvKhAJV5Qhcerf0mexOIG2GDrPsaLkExj2ZGyvNsQxxfpt7ZGQCxZ3py4Lt5y77DLmVQ==";
        const int TotalPartitions = 100;
        const int RowsPerPartition = 500;
        static void Main(string[] args)
        {
            DateTime stopwatch;
            AzureStorageOperations storage = new AzureStorageOperations(TestConnection);

            string tablename = "T" + Guid.NewGuid().ToString("N");
            List<TestEntity> expected = new List<TestEntity>();

            for (int i = 0; i < TotalPartitions; i++)
            {
                for (int j = 0; j < RowsPerPartition; j++)
                {
                    expected.Add(new TestEntity(i.ToString("D3"), j.ToString("D3")));
                }
            }

            stopwatch = DateTime.Now;
            if (!storage.TryBatchInsertRows(tablename, expected))
            {
                throw new ApplicationException("TryBatchInsertRows failed.");
            }

            TimeSpan insertRowsTime = DateTime.Now - stopwatch;

            Console.WriteLine("TryBatchInsertRows Succeeded; elapsed time: {0}", insertRowsTime);
            Console.WriteLine("   Total Insert  time {0}. Average insert time {1}. Speed: {2} rows/sec", insertRowsTime, TimeSpan.FromTicks(insertRowsTime.Ticks / expected.Count), expected.Count / insertRowsTime.TotalSeconds);

            stopwatch = DateTime.Now;
            List<TestEntity> received = storage.GetAllRows<TestEntity>(tablename).ToList();
            TimeSpan getRowsTime = DateTime.Now - stopwatch;

            Console.WriteLine("Received Succeeded. Total rows: {0:N0}. Elapsed time: {1}", received.Count, getRowsTime);
            Console.WriteLine("   Total Receive time {0}. Average get    time {1}. Speed: {2} rows/sec", getRowsTime, TimeSpan.FromTicks(getRowsTime.Ticks / received.Count), received.Count / getRowsTime.TotalSeconds);

            if (expected.Count != received.Count)
            {
                throw new ApplicationException(string.Format("Count: Expected:{0} != Received:{1}", expected.Count, received.Count));
            }

            bool[] checkArray = new bool[TotalPartitions * RowsPerPartition];
            foreach (TestEntity expectedItem in received)
            {
                int p = int.Parse(expectedItem.PartitionId);
                int r = int.Parse(expectedItem.RowId);
                checkArray[(p * RowsPerPartition) + r] = true;
            }

            List<int> missingItems = new List<int>();
            for (int i = 0; i < (TotalPartitions * RowsPerPartition); i++)
            {
                if (!checkArray[i])
                {
                    missingItems.Add(i);
                }
            }

            if (missingItems.Count > 0)
            {
                Console.WriteLine("Missing values:");
                foreach (int item in missingItems)
                {
                    Console.WriteLine("Partition: {0,5:N0}, Row: {1,5:N0}", item / RowsPerPartition, item % RowsPerPartition);
                }

                throw new ApplicationException("Received values different than expected.");
            }

            Console.WriteLine("Test Succeeded!!");
        }
    }
}
