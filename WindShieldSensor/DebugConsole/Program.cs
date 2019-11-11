using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SensorManager.Manager;

namespace DebugConsole
{
    class Program
    {

        private static SensorHubManager manager;

        static void Main(string[] args)
        {
            manager = new SensorHubManager();
            manager.Start();
            while (true)
            {
                //Console.WriteLine(manager.RecievedA?.TimeStamp.ToString("yyyy MMMM, dd HH:mm:ss:fff")+" "+manager.RecievedA?.Data?.Height);
                //Console.WriteLine(manager.RecievedB?.TimeStamp.ToString("yyyy MMMM, dd HH:mm:ss:fff"));
                //Console.WriteLine(manager.RecievedC?.TimeStamp.ToString("yyyy MMMM, dd HH:mm:ss:fff"));
                //Console.WriteLine(manager.RecievedD?.TimeStamp.ToString("yyyy MMMM, dd HH:mm:ss:fff"));
            }
            
        }
    }
}
