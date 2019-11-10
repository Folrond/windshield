using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class ProcessingA : RunnableDataProvider<Bitmap>, IDisposable
    {
        private readonly RgbCamera leftCamera;
        private readonly RgbCamera rightCamera;

        //TODO Enrich get/set to produce statistics about dropped (unused) frames, and latency
        //TODO Enrich get/set to introduce Epsilon distance
        private Frame<Mat> leftFrame = new Frame<Mat>();
        private Frame<Mat> rightFrame = new Frame<Mat>();

        public Frame<Mat> RecievedLeftFrame
        {
            get
            {
                if (leftFrame == null)
                    return null;
                return InterlockedHelper.SafeRead(ref leftFrame);
            }
            set => InterlockedHelper.SafeWrite(ref leftFrame, value);
        }

        public Frame<Mat> RecievedRightFrame
        {
            get
            {
                if (rightFrame == null)
                    return null;
                return InterlockedHelper.SafeRead(ref rightFrame);
            }
            set => InterlockedHelper.SafeWrite(ref rightFrame, value);
        }
        
        private Action<Frame<Mat>> recievedLeftFrameAction;
        private Action<Frame<Mat>> recievedRightFrameAction;

        public ProcessingA(RgbCamera cameraLeft, RgbCamera cameraRight)
        {
            this.leftCamera = cameraLeft;
            this.rightCamera = cameraRight;
            RegisterForPushEvents();
        }

        public override Frame<Bitmap> QueryFrame()
        {
            var leftFrame = RecievedLeftFrame;
            var rightFrame = RecievedRightFrame;

            if (!CanExecute(leftFrame, rightFrame))
                return null;

            //TODO DO Work here like yolo calculation and other stuff
            Thread.Sleep(new Random().Next(50,70));

            var image1 = leftFrame.Data;
            var image2 = leftFrame.Data;

            //CreateResult Frame
            var newFrame = new Frame<Bitmap>(image1.Bitmap);

            //Push data
            OnFrameChanged(newFrame);


            return newFrame;
        }

        private bool CanExecute(Frame<Mat> frame1, Frame<Mat> frame2)
        {
            if (frame1 == null || frame2 == null)
                return false;

            if (frame1.IsEmpty || frame2.IsEmpty)
                return false;

            return true;
        }

        public void Dispose()
        {
            leftCamera.UnregisterFromActualFramePush(recievedLeftFrameAction);
            rightCamera.UnregisterFromActualFramePush(recievedRightFrameAction);
        }

        private void RegisterForPushEvents()
        {
            recievedLeftFrameAction = (frame) =>
            {
                RecievedLeftFrame = frame;
            };
            recievedRightFrameAction = (frame) =>
            {
                RecievedRightFrame = frame;
            };

            leftCamera.RegisterForActualFramePush(recievedLeftFrameAction);
            rightCamera.RegisterForActualFramePush(recievedRightFrameAction);

        }
    }
}
