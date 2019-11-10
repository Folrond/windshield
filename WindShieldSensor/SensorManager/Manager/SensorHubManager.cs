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
        private RgbCamera leftCamera1;
        private RgbCamera rightCamera1;

        private RgbCamera leftCamera2;
        private RgbCamera rightCamera2;


        private ProcessingA processingA;
        private ProcessingB processingB;


        private Action<Frame<Bitmap>> recievedAHandler;
        private Action<Frame<Bitmap>> recievedBHandler;

        //Only for debugging;
        object lockA = new object();
        object lockB = new object();

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

        public Frame<Bitmap> RecievedB
        {
            get
            {
                lock (lockB)
                {
                    return new Frame<Bitmap>(recievedB?.Data?.Clone() as Bitmap);
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

        private Frame<Bitmap> recievedA;
        private Frame<Bitmap> recievedB;


        public SensorHubManager()
        {
            leftCamera1 = new RgbCamera(1);
            rightCamera1= new RgbCamera(1);
            leftCamera2 = new RgbCamera(1);
            rightCamera2 = new RgbCamera(1);

            processingA = new ProcessingA(leftCamera1, rightCamera1);
            processingB = new ProcessingB(leftCamera2, rightCamera2);
            RegisterForPushEvents();
        }

        public void Start()
        {
            leftCamera1.StartQuery();
            rightCamera1.StartQuery();

            processingA.StartQuery();
            processingB.StartQuery();
        }


        public void Dispose()
        {
            processingA.UnregisterFromActualFramePush(recievedAHandler);
            processingB.UnregisterFromActualFramePush(recievedBHandler);
        }

        private void RegisterForPushEvents()
        {
            recievedAHandler = (frame) => { RecievedA = frame; };
            recievedBHandler = (frame) => { RecievedB = frame; };

            processingA.RegisterForActualFramePush(recievedAHandler);
            processingB.RegisterForActualFramePush(recievedBHandler);

        }


    }
}
