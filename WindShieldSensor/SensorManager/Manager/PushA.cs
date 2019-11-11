using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using SensorManager.Processing;
using Sensors.Sensors;

namespace SensorManager.Manager
{
    public class PushA:IDisposable
    {
        private RgbCamera leftCamera1;
        private RgbCamera rightCamera1;

        private ProcessingA processingA;

        private Action<Frame<Bitmap>> recievedAHandler;

        //Only for debugging;
        object lockA = new object();

        private Frame<Bitmap> recievedA;
        public Frame<Bitmap> RecievedA
        {
            get
            {
                lock (lockA)
                {
                    return new Frame<Bitmap>(recievedA?.Data?.Clone() as Bitmap);
                }
            }
            set
            {
                lock (lockA)
                {
                    recievedA = value;
                }
            }
        }

        public PushA(int camId)
        {
            leftCamera1 = new RgbCamera(camId);
            rightCamera1 = new RgbCamera(camId);

            processingA = new ProcessingA(leftCamera1, rightCamera1);
        }

        public PushA(RgbCamera left,RgbCamera right)
        {
            leftCamera1 = left;
            rightCamera1 = right;

            processingA = new ProcessingA(leftCamera1, rightCamera1);
        }


        public void Start()
        {
            leftCamera1.StartQuery();
            rightCamera1.StartQuery();

            processingA.StartQuery();
        }

        private void RegisterForPushEvents()
        {
            recievedAHandler = (frame) => { RecievedA = frame; };
            processingA.RegisterForActualFramePush(recievedAHandler);
        }

        public void Dispose()
        {
            processingA.UnregisterFromActualFramePush(recievedAHandler);
        }
    }
}
