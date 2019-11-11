using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Emgu.CV;
using SensorManager.Processing;
using Sensors.Sensors;


namespace SensorManager.Manager
{
    //The main Sensor managing class that creates, and combines necessary resources for calculation.
    //End results will end up here, the manager will either map or wrap them and then writes the results in the appropriate object store for rendering 
    public class SensorHubManager:IDisposable
    {

        public PushBPushE BtoE;
      
        public SensorHubManager()
        {
            this.BtoE = new PushBPushE(0);
        }

        public void Start()
        {
           BtoE.Start();
        }


        public void Dispose()
        {
        }
    }
}
