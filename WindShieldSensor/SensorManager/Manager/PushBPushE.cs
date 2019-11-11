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
    class PushBPushE: IDisposable
    {
        private RgbCamera leftCamera;
        private RgbCamera rightCamera;

        private ProcessingB processingB;
        private ProcessingE processingE;

        private Action<Frame<Mat>> recievedBHandler;


        //DEBUG
        object lockB = new object();

        private Frame<Mat> recievedB;

        public Frame<Mat> RecievedB
        {
            get
            {
                lock (lockB)
                {
                    return new Frame<Mat>(recievedB?.Data);
                }

            }
            set
            {
                lock (lockB)
                {
                    recievedB = value;
                }
            }
        }


        private void RegisterForPushEvents()
        {

            recievedBHandler = (frame) => { RecievedB = frame; };

            processingB.RegisterForActualFramePush(recievedBHandler);

        }


        public void Dispose()
        {

            processingB.UnregisterFromActualFramePush(recievedBHandler);
        }


    }
}
