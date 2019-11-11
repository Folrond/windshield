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
    public class PushBPushE : IDisposable
    {
        private RgbCamera leftCamera;
        private RgbCamera rightCamera;

        private ProcessingB processingB;
        private ProcessingE processingE;

        private Action<Frame<Mat>> recievedBHandler;
        private Action<Frame<Bitmap>> recievedEHandler;


        public PushBPushE(int camId)
        {
            leftCamera=new RgbCamera(camId);
            rightCamera = new RgbCamera(camId);
            
            processingB = new ProcessingB(leftCamera,rightCamera);
            processingE = new ProcessingE(processingB);

            RegisterForPushEvents();

        }

        #region ╰(✿˙ᗜ˙)੭━☆ﾟ.*･｡ﾟ
        
        //DEBUG SECTION
        //For Debugging we lock the read/writes
        //Also for debugging we sniff the results of B
        object lockB = new object();
        object lockE = new object();

        private Frame<Mat> recievedB;
        private Frame<Bitmap> recievedE;

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

        public Frame<Bitmap> RecievedE
        {
            get
            {
                lock (lockE)
                {
                    return new Frame<Bitmap>(recievedB?.Data?.Bitmap.Clone() as Bitmap);
                }

            }
            set
            {
                lock (lockE)
                {
                    recievedE = value;
                }
            }
        }
        #endregion


        private void RegisterForPushEvents()
        {

            recievedBHandler = (frame) => { RecievedB = frame; };
            recievedEHandler = (frame) => { RecievedE = frame; };

            processingB.RegisterForActualFramePush(recievedBHandler);
            processingE.RegisterForActualFramePush(recievedEHandler);

        }

        public void Start()
        {
            this.processingB.StartQuery();
            this.processingE.StartQuery();
        }


        public void Dispose()
        {

            processingB.UnregisterFromActualFramePush(recievedBHandler);
            processingE.RegisterForActualFramePush(recievedEHandler);

        }


    }
}
