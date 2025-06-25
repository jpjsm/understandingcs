using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListingBrhHierarchies
{
    using BrhServiceReference;

    class Program
    {
        public const string DefaultBrhEndpoint = "http://businesshierarchy.api.msdial.com/brhdataservice";
        static void Main(string[] args)
        {
            List<string> brhData = new List<string>();
            MFxBRHEntities brhContext = BrhContext(new Uri(DefaultBrhEndpoint));
            var treeData = brhContext.vwBrhTreeDatas.ToList();
            string brhHierarchiesDataHeader = "GroupId\tGroupName\tGroupState\tGroupType\tGroupTypeId\tParentGroupId";
            Console.WriteLine(brhHierarchiesDataHeader);
            brhData.Add(brhHierarchiesDataHeader);

            foreach (vwBrhTreeData treeElement in treeData)
            {
                string[] elements = new string[]
                {
                    treeElement.GroupId.ToString(),
                    treeElement.GroupName,
                    treeElement.GroupState,
                    treeElement.GroupType,
                    treeElement.GroupTypeId.ToString(),
                    treeElement.ParentGroupId
                };

                string brhDataRow = string.Join("\t", elements);
                Console.WriteLine(brhDataRow);
                brhData.Add(brhDataRow);
            }

            File.WriteAllLines("BrhData.tsv", brhData);
        }

        public static MFxBRHEntities BrhContext(Uri brhUri)
        {
            return new MFxBRHEntities(brhUri)
            {
                Credentials = CredentialCache.DefaultNetworkCredentials,
                Timeout = 300,
                MergeOption = MergeOption.OverwriteChanges,
                UsePostTunneling = true,
                IgnoreMissingProperties = true,
                IgnoreResourceNotFoundException = true
            };
        }
    }
}
