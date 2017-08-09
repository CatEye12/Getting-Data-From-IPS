using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intermech.Interfaces;
using Intermech.Interfaces.Projects;
using System.Runtime.Remoting;
using Intermech.Kernel;

namespace ComApi
{
    class Program
    {

        static void Main(string[] args)
        { 
            Program obj = new Program();
            IUserSession session = obj.Connect("kb82", "");

          

            Console.WriteLine(session.UserName + "\n" + session.IsAdmin.ToString());
            Console.ReadLine();

            IDBObject my_obj = (IDBObject)session;
              
            
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
    }
}