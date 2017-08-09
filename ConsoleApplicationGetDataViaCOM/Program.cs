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
            IDBObject obj = session.GetObject(748547);
            Console.WriteLine("ProjectID of obj = " + obj.ProjectID.ToString() + "\n" + obj.Caption + "\n" + obj.SiteID + "\n" + obj.SubjectAreas);
           // Console.ReadLine();

            //IDBObjectCollection objCollection = session.GetObjectCollection(1052); // выбрать детали
            IDBObjectCollection objCollection = session.GetObjectCollection(-1); // выбрать все объекты
            ConditionStructure[] conditions = new ConditionStructure[]
            {
                new ConditionStructure("Заголовок объекта", RelationalOperators.StartString, "Кольцо", LogicalOperators.NONE, 0, true)
            };

            ColumnDescriptor[] descriptor = new ColumnDescriptor[]
            {
                new ColumnDescriptor(ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Auto, ColumnContents.ID, ColumnNameMapping.Default, SortOrders.DESC, 0),
                new ColumnDescriptor(ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Auto, ColumnContents.Text, ColumnNameMapping.Default, SortOrders.ASC, 1),
            };
            //Console.WriteLine("ObjCollection:" + objCollection.Select();
            Console.ReadLine();
        }
    }
}