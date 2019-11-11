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
    public class PullCPullD:IDisposable
    {
        private RgbCamera leftCamera;
        private RgbCamera rightCamera;
        
        private ProcessingC processingC;
        private ProcessingD processingD;

        public PullCPullD(int camId)
        {
            leftCamera =new RgbCamera(camId);
            rightCamera = new RgbCamera(camId);


            processingC = new ProcessingC(leftCamera, rightCamera);
            processingD = new ProcessingD(processingC);
        }

        public PullCPullD(RgbCamera left, RgbCamera right)
        {
            leftCamera = left;
            rightCamera = right;


            processingC = new ProcessingC(leftCamera, rightCamera);
            processingD = new ProcessingD(processingC);
        }

        //DEBUGGING
        object lockA =new object();
        object lockB = new object();

        public Frame<Bitmap> RecievedC
        {
            get
            {
                lock (lockA)
                {
                    return new Frame<Bitmap>(processingC.ActualFrame?.Data?.Bitmap.Clone() as Bitmap);
                }

            }
        }

        public Frame<Bitmap> RecievedD
        {
            get
            {
                lock (lockB)
                {
                    return new Frame<Bitmap>(processingD.ActualFrame?.Data?.Clone() as Bitmap);
                }

            }
        }

        public void Start()
        {
            processingC.StartQuery();
            processingD.StartQuery();
        }

        public void Dispose()
        {
        }
    }
}
