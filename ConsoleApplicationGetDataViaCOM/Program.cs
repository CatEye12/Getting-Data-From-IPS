using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intermech.Interfaces;
using Intermech.Interfaces.Projects;
using System.Runtime.Remoting;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using System.Data;

namespace ComApi
{
    class Program
    {
        public static IUserSession session;
        
        static void Main(string[] args)
        { 
            Program obj = new Program();
            session = obj.Connect("kb82", "");
            Console.WriteLine(session.UserName);
            obj.Method();
            Console.ReadLine();
            
            Console.WriteLine("the type is : " + MetaDataHelper.GetObjectTypeName(1033));
         
            // Console.WriteLine("the type is : " + ;
            Console.ReadLine();

        }
        public IUserSession Connect(string loginName, string password)
        {
            string serverURL = "tcp://PDMSRV:8008/IntermechRemoting/Server.rem";
            
            // Подключаемся к серверу и получаем сессию
            IMServer server = (IMServer)Activator.GetObject(typeof(IMServer), serverURL);
            IUserSession session = server.CreateSession();
            // Получаем смещение для текущего часового пояса и вызываем у сессии функцию авторизации
            DateTime now = DateTime.Now;
            TimeSpan ts = now - now.ToUniversalTime();
            session.Login(loginName, password, Environment.MachineName, ts, 0);
            return session;
        }

        public void Method()
        {
            //    IDBObject obj = session.GetObject(748547);
            //    Console.WriteLine("ProjectID of obj = " + obj.ProjectID.ToString() + "\n" + obj.Caption + "\n" + obj.SiteID + "\n" + obj.SubjectAreas);
            //    Console.ReadLine();
            //    IDBObjectCollection objCollection = session.GetObjectCollection(1052); // выбрать детали



            IDBObjectCollection objCollection = session.GetObjectCollection(1033); //1052 выбрать все объекты

            ConditionStructure[] conditions = new ConditionStructure[]
            {
                new ConditionStructure("Заголовок объекта", RelationalOperators.StartString, "", LogicalOperators.NONE, 0, true)
            };

            object[] columns = new object[]
            {
                ObligatoryObjectAttributes.F_OBJECT_ID, "Заголовок объекта"
            };
            //object[] columns = null;
            object[] sortColumns = new object[]
            {
                ObligatoryObjectAttributes.F_OBJECT_ID, "Заголовок объекта"
            };

            SortOrders[] order = new SortOrders[] { SortOrders.ASC, SortOrders.DESC };

            DBRecordSetParams pars = new DBRecordSetParams(conditions, columns, sortColumns, order, 0, null, QueryConsts.Default, true, "MyTable");
            
            DataTable dt = objCollection.Select(pars);
            for (int i = 0; i < dt.Rows.Count;  i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Console.WriteLine( dt.Rows[i][j].ToString());
                }
            }
            Console.ReadLine();
        }
    }
}