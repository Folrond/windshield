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
    public class SensorHubManager:IDisposable
    {
        
        private ProcessingB processingB;
        

        private Action<Frame<Bitmap>> recievedAHandler;
        
      
        
        public SensorHubManager()
        {
            
        }

        public void Start()
        {
            processingB.StartQuery();

        }


        public void Dispose()
        {
        }
    }
}
