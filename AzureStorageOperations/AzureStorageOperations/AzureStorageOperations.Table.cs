using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;


namespace AzureStorageOperations
{
    public partial class AzureStorageOperations
    {
        public bool TryCreateTable(string tableName, out CloudTable cloudTableReference)
        {
            cloudTableReference = null;
            try
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    throw new ArgumentNullException("tableName");
                }

                CloudTable t = TableClient.GetTableReference(tableName);
                t.CreateIfNotExists();
                cloudTableReference = t;
                return true;
            }
            catch (Exception)
            {
                return false; ;
            }
        }

        public bool TryInsertRow<T>(string tableName, T data) where T : TableEntity
        {
            try
            {
                CloudTable table;

                if (string.IsNullOrWhiteSpace(tableName))
                {
                    throw new ArgumentNullException("tableName");
                }

                if (!TryCreateTable(tableName, out table))
                {
                    throw new ApplicationException("Unable to create or retrieve table: " + tableName);
                }

                TableOperation insertData = TableOperation.Insert(data);
                table.Execute(insertData);
                return true;
            }
            catch (Exception)
            {
                return false; ;
            }
        }

        public bool TryBatchInsertRows<T>(
            string tableName,
            IEnumerable<T> dataCollection) where T : TableEntity
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    throw new ArgumentNullException("tableName");
                }

                CloudTable table;
                if (!TryCreateTable(tableName, out table))
                {
                    throw new ApplicationException("Unable to create or retrieve table: " + tableName);
                }

                Dictionary<string, List<T>> dataByPartition = new Dictionary<string, List<T>>();
                foreach (T dataItem in dataCollection)
                {
                    if (!dataByPartition.ContainsKey(dataItem.PartitionKey))
                    {
                        dataByPartition.Add(dataItem.PartitionKey, new List<T>());
                    }

                    dataByPartition[dataItem.PartitionKey].Add(dataItem);
                }

                Parallel.ForEach(
                    dataByPartition.Keys,
                    (k) =>
                    {
                        CloudTable t = TableClient.GetTableReference(tableName);
                        TableBatchOperation batch = new TableBatchOperation();
                        int batchCount = 0;
                        foreach (var dataItem in dataByPartition[k])
                        {
                            batch.Insert(dataItem);
                            if (batch.Count == 100)
                            {
                                t.ExecuteBatch(batch);
                                batch.Clear();
                                batchCount++;
                            }
                        }

                        if (batch.Count > 0)
                        {
                            t.ExecuteBatch(batch);
                            batchCount++;
                        }

                    });

                return true;
            }
            catch (Exception ex)
            {
                throw   ex;
            }
        }

        public IEnumerable<T> GetAllRows<T>(string tableName) where T : TableEntity, new()
        {

            CloudTable table;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            if (!TryCreateTable(tableName, out table))
            {
                throw new ApplicationException("Unable to create or retrieve table: " + tableName);
            }

            TableContinuationToken token = null;
            int segments = 0;
            int totalRows = 0;
            int rowsInSegment;
            do
            {
                TableQuerySegment<T> querySegment = table.ExecuteQuerySegmented(new TableQuery<T>(), token);
                segments++;
                rowsInSegment = 0;
                foreach (var row in querySegment)
                {
                    totalRows++;
                    rowsInSegment++;
                    yield return row;
                }

                token = querySegment.ContinuationToken;
            } while (token != null);
        }

        public IEnumerable<T> GetAllRows<T>(
            string tableName, 
            TableQuery<T> tableQuery) where T : TableEntity, new()
        {

            CloudTable table;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            if (!TryCreateTable(tableName, out table))
            {
                throw new ApplicationException("Unable to create or retrieve table: " + tableName);
            }

            if (tableQuery == null)
            {
                tableQuery = new TableQuery<T>();
            }

            TableContinuationToken token = null;

            do
            {
                foreach (var row in table.ExecuteQuerySegmented(tableQuery, token))
                {
                    yield return row;
                }
            } while (token != null);
        }

        public IEnumerable<T> GetAllRowsInPartition<T>(
            string tableName,
            string partitionValue) where T : TableEntity, new()
        {

            CloudTable table;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            if (!TryCreateTable(tableName, out table))
            {
                throw new ApplicationException("Unable to create or retrieve table: " + tableName);
            }

            TableContinuationToken token = null;
            TableQuery<T> tableQuery = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionValue));

            do
            {
                foreach (var row in table.ExecuteQuerySegmented(tableQuery, token))
                {
                    yield return row;
                }
            } while (token != null);
        }

        public T RetrieveByPartitionRowKey<T>(
            string tableName,
            string partitionValue,
            string rowKey) where T : TableEntity, new()
        {

            CloudTable table;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            if (!TryCreateTable(tableName, out table))
            {
                throw new ApplicationException("Unable to create or retrieve table: " + tableName);
            }

            TableOperation tableOperation = TableOperation.Retrieve<T>(partitionValue, rowKey);

            TableResult retrievedRow = table.Execute(tableOperation);

            return retrievedRow.Result as T;
        }
    }
}
