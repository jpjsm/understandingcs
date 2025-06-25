using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GeneratingClassesFromXmlFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string phyiscalModelFilename = @"C:\DCM-Uploads\DcmData\2015\10\14\231605452725\PhysicalModel.xml\DCM_Microsoft.Windows.Azure.Fabric.DataCenterManager.PhysicalModel.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(PhysicalModel));

            StreamReader reader = new StreamReader(phyiscalModelFilename);
            var physicalModel = (PhysicalModel)serializer.Deserialize(reader);
            reader.Close();

            Dictionary<string, List<PhysicalModelInstance>> instancesByType = new Dictionary<string, List<PhysicalModelInstance>>();
            string classname;
            foreach (var instance in physicalModel.Instances)
            {
                classname = instance.classname.Split('.').Last();
                if(!instancesByType.ContainsKey(classname))
                {
                    instancesByType.Add(classname, new List<PhysicalModelInstance>());
                }

                instancesByType[classname].Add(instance);
            }

            foreach (KeyValuePair<string, List<PhysicalModelInstance>> kvp in instancesByType)
            {
                Console.WriteLine("{0} -> {1:N0}", kvp.Key, kvp.Value.Count);
            }
        }
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "modelxml")]
public partial class PhysicalModel
{

    private PhysicalModelInstance[] instanceField;

    private string subSchemaNameField;

    private string subSchemaVersionField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("instance")]
    public PhysicalModelInstance[] Instances
    {
        get
        {
            return this.instanceField;
        }
        set
        {
            this.instanceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string SubSchemaName
    {
        get
        {
            return this.subSchemaNameField;
        }
        set
        {
            this.subSchemaNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string SubSchemaVersion
    {
        get
        {
            return this.subSchemaVersionField;
        }
        set
        {
            this.subSchemaVersionField = value;
        }
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PhysicalModelInstance
{

    private object[] itemsField;

    private string idField;

    private string classnameField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("property", typeof(PhysicalModelInstanceProperty))]
    [System.Xml.Serialization.XmlElementAttribute("relationship", typeof(PhysicalModelInstanceRelationship))]
    public object[] Items
    {
        get
        {
            return this.itemsField;
        }
        set
        {
            this.itemsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string classname
    {
        get
        {
            return this.classnameField;
        }
        set
        {
            this.classnameField = value;
        }
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PhysicalModelInstanceProperty
{

    private string nameField;

    private string valueField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PhysicalModelInstanceRelationship
{

    private string nameField;

    private string contained_idField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string contained_id
    {
        get
        {
            return this.contained_idField;
        }
        set
        {
            this.contained_idField = value;
        }
    }
}

