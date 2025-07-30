using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetReconciliationStartAzureWorkflow
{
    public partial class Main : Form
    {
        private bool connectionOK = false;
        private AzureStorageOperations storage;
        public Main()
        {
            InitializeComponent();
            defaultEndpointProtocols.SelectedIndex = 0;
            accountNames.SelectedIndex = 0;
            accountKeys.SelectedIndex = 0;
            UpdateStorageConnectionString();
            connectionOK = TestConnection();
            //if (connectionOK)
            //{
            //    storage = new AzureStorageOperations(storageConnectionString.Text);
            //    UpdateStorageContainers(storage);
            //}
        }

        private void UpdateStorageConnectionString()
        {
            storageConnectionString.Text =
                string.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2}",
                defaultEndpointProtocols.Items[defaultEndpointProtocols.SelectedIndex],
                accountNames.Items[accountNames.SelectedIndex],
                accountKeys.Items[accountKeys.SelectedIndex]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string errorMessage;
            if (TestConnection(out errorMessage))
            {
                MessageBox.Show("Connection successful", "Test Storage Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(string.Format("Connection error: {0}", errorMessage), "Test Storage Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool TestConnection(out string errorMessage)
        {
            AzureStorageOperations testContainer = new AzureStorageOperations(storageConnectionString.Text);

            return testContainer.TestConnection(out errorMessage);
        }

        private bool TestConnection()
        {
            string errmsg;

            return TestConnection(out errmsg);
        }

        private void UpdateStorageContainers(AzureStorageOperations storage)
        {
            storageHierarchy.Nodes.Clear();
            foreach (AzureBlobStorageReference containerReference in storage.GetContainersReferences())
            {
                TreeNode node = new TreeNode(containerReference.Name);
                node.Tag = containerReference;

                storageHierarchy.Nodes.Add(node);
            }
        }
    }
}
