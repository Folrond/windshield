using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Emgu.CV;
using Emgu.CV.Structure;
using SensorManager.Helpers;
using Sensors.Sensors;

namespace SensorManager.Processing
{
    class ProcessingE : RunnableDataProvider<Bitmap>,IDisposable
    {
        private ProcessingB b;
        private Action<Frame<Mat>> pushEventHandler;
        private Frame<Mat> recievedFrame = new Frame<Mat>();
        public Frame<Mat> RecievedFrame
        {
            get
            {
                if (recievedFrame == null)
                    return null;
                return InterlockedHelper.SafeRead(ref recievedFrame);
            }
            private set => InterlockedHelper.SafeWrite(ref recievedFrame, value);
        }

        public ProcessingE(ProcessingB b)
        {
            this.b = b;
            this.RegisterForPushEvents();
        }

        public override Frame<Bitmap> QueryFrame()
        {
            var lastFrame = RecievedFrame;
            if (!CanExecute(lastFrame))
                return null;

            //TODO DO Work here like yolo calculation and other stuff
            Thread.Sleep(new Random().Next(50, 70));

            var img = lastFrame.Data.ToImage<Bgr, byte>();
            var rotated = img.Rotate(20, new Bgr(Color.Gray));

            //CreateResult Frame
            var newFrame = new Frame<Bitmap>(rotated.Bitmap);

            //Push data
            OnFrameChanged(newFrame);

            return newFrame;
        }

        private bool CanExecute(Frame<Mat> frame1)
        {
            if (frame1 == null)
                return false;

            if (frame1.IsEmpty)
                return false;

            return true;
        }

        private void RegisterForPushEvents()
        {
            pushEventHandler = (frame) =>
            {
                RecievedFrame = frame;
            };
            b.RegisterForActualFramePush(pushEventHandler);
            
        }

        public void Dispose()
        {
            recievedFrame?.Dispose();
            b.UnregisterFromActualFramePush(pushEventHandler);
        }
    }
}
