using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureStorageOperationsUnitTest
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
    [TestClass]
    public class TablesUnitTest
    {
        const string TestConnection = "DefaultEndpointsProtocol=https;AccountName=jpjofrestorage;AccountKey=URbOxBi0ckYU7XTxV4OvKhAJV5Qhcerf0mexOIG2GDrPsaLkExj2ZGyvNsQxxfpt7ZGQCxZ3py4Lt5y77DLmVQ==";
        [TestMethod]
        public void TestConnectivity()
        {
            AzureStorageOperations storage = new AzureStorageOperations(TestConnection);
            string errmsg;
            Assert.IsTrue(storage.TestConnection(out errmsg), errmsg);
        }

        [TestMethod]
        public void TestInsert()
        {
            AzureStorageOperations storage = new AzureStorageOperations(TestConnection);

            TestEntity expected = new TestEntity();
            string tablename = "T" + Guid.NewGuid().ToString("N");
            Assert.IsTrue(storage.TryInsertRow(tablename, new TestEntity()));

            CloudTable cloudTableReference;
            Assert.IsTrue(storage.TryCreateTable(tablename, out cloudTableReference));
            TableOperation retrieveOperation = TableOperation.Retrieve<TestEntity>("1", "1");
            TableResult retrievedResult = cloudTableReference.Execute(retrieveOperation);
            TestEntity received = retrievedResult.Result as TestEntity;
            Assert.IsNotNull(received);
            Assert.IsTrue(received.PartitionId == expected.PartitionId && received.RowId == expected.RowId);
        }

        [TestMethod]
        public void TestBatchInsert()
        {
            AzureStorageOperations storage = new AzureStorageOperations(TestConnection);

            string tablename = "T" + Guid.NewGuid().ToString("N");
            List<TestEntity> expected = new List<TestEntity>();

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 2000; j++)
                {
                    expected.Add(new TestEntity(i.ToString("D3"), j.ToString("D3")));
                }
            }

            Assert.IsTrue(storage.TryBatchInsertRows(tablename, expected), "TryBatchInsertRows failed.");

            List<TestEntity> received = storage.GetAllRows<TestEntity>(tablename).ToList();

            Assert.IsTrue(expected.Count == received.Count, string.Format("Count: Expected:{0} != Received:{1}", expected.Count, received.Count));
            foreach (TestEntity expectedItem in expected)
            {
                Assert.IsTrue(received.Any(r => r.PartitionId == expectedItem.PartitionId && r.RowId == expectedItem.RowId));
            }
        }
    }
}
