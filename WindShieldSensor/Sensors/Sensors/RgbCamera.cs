using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Sensors.Interfaces;

namespace Sensors.Sensors
{

    public class RgbCamera:RunnableDataProvider<object>,IRgbCamera<object>
    {
        
        public object CameraObject { get; set; }

        
        public RgbCamera(string resourcePath)
        {
            //USE resourcePath to init actual camera
            CameraObject = new object();
        }


        protected override void QueryFrame()
        {
            var frame = new Frame<object>();
            
            //GET FRAME
            CameraObject.ToString();
            frame.Data = CameraObject.ToString();
            frame.TimeStamp = DateTime.Now;

            this.ActualCurrentFrame = frame;

            this.OnFrameChanged();

        }

    }
}
