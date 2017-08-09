namespace ComApi
{
    using Intermech;
    using Intermech.Client.Core;
    using Intermech.Collections;
    using Intermech.Interfaces;
    using Intermech.Interfaces.Client;
    using Intermech.Interfaces.Data.Metadata;
    using Intermech.Kernel.Search;
    using Intermech.Navigator;
    using Intermech.Runtime.ComInterop.LocalServer;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("F8358B23-3E51-453C-8EF8-9ABD22525214"), ProgId("IPS.TestObject")]
    [ClassInterface(ClassInterfaceType.None), ComDefaultInterface(typeof(ITestObject))]
    public class TestObject : SingleThreadedObject, ITestObject
    {
        public string SayHello()
        {
            return "Hello from IPS!";//, "IPS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public long SelectArticle()
        {
            long[] selection = SelectionWindow.SelectObjects("Выбор изделия", "Выберите изделие для публикации его состава в вызывающем приложении.",
                ProductType.Id, SelectionOptions.DisableMultiselect | SelectionOptions.DisableSelectFromTree | SelectionOptions.SelectObjects);
            return (selection == null || selection.Length == 0) ? Intermech.Consts.UnknownObjectId : selection[0];
        }

        public string[] GetArticleStructure(int articleObjectId)
        {
            DBRecordSetParams queryParams = new DBRecordSetParams();
            queryParams.RecordCount = QueryConsts.All;
            queryParams.Columns = new object[] { ObligatoryObjectAttributes.CAPTION };

            DataTable tbl;
            using (SessionKeeper keeper = new SessionKeeper())
            {
                IDBRelationCollection dbCollection = keeper.Session.GetRelationCollection(ProductLinkType.Id);
                dbCollection.FiltrationOwnerID = SystemGUIDs.filtrationBaseVersions;
                dbCollection.ObjectTypeID = ProductType.Id; // только изделия, материалы не нужны
                tbl = dbCollection.ConsistFrom(queryParams, articleObjectId);
            }

            var result = new List<string>(tbl.Rows.Count);
            foreach (DataRow row in tbl.Rows)
                result.Add(Convert.ToString(row[0]));
            return result.ToArray();
        }
        public object[,] GetArticleStructure2(int articleObjectId, string[] attributes)
        {
            if (attributes == null) throw new ArgumentNullException("attributes");
            if (attributes.Length == 0) throw new ArgumentOutOfRangeException("attributes");

            DBRecordSetParams queryParams = new DBRecordSetParams();
            queryParams.RecordCount = QueryConsts.All;
            queryParams.Columns = (object[])attributes;
            queryParams.ColumnsInfo = CollectionUtils.ConvertAsArray(attributes, attr => new ColumnInfo(attr, AttributeSourceTypes.Object, null));

            DataTable tbl;
            using (SessionKeeper keeper = new SessionKeeper())
            {
                IDBRelationCollection dbCollection = keeper.Session.GetRelationCollection(ProductLinkType.Id);
                dbCollection.FiltrationOwnerID = SystemGUIDs.filtrationBaseVersions;
                dbCollection.ObjectTypeID = ProductType.Id; // только изделия, материалы не нужны
                tbl = dbCollection.ConsistFrom(queryParams, articleObjectId);
            }

            return DBHelper.ToObjectArray(tbl, attributes);
        }

        private static readonly ObjectTypeResolver ProductType = MetadataResolverFactory.ObjectType(new Guid(SystemGUIDs.objtypeProduct));
        private static readonly RelationTypeResolver ProductLinkType = MetadataResolverFactory.RelationType(new Guid(SystemGUIDs.reltypeSP));

    }
}