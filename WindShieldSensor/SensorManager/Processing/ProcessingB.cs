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
    class ProcessingB : RunnableDataProvider<Bitmap>
    {
        private readonly RgbCamera leftCamera;
        private readonly RgbCamera rightCamera;

        public Frame<Mat> RecievedLeftFrame => leftCamera.QueryFrame();

        public Frame<Mat> RecievedRightFrame => rightCamera.QueryFrame();


        public ProcessingB(RgbCamera cameraLeft, RgbCamera cameraRight)
        {
            this.leftCamera = cameraLeft;
            this.rightCamera = cameraRight;
        }

        public override Frame<Bitmap> QueryFrame()
        {
            var leftFrame = RecievedLeftFrame;
            var rightFrame = RecievedRightFrame;

            if (!CanExecute(leftFrame, rightFrame))
                return null;

            //TODO DO Work here like yolo calculation and other stuff
            Thread.Sleep(new Random().Next(50, 70));

            //var image1 = leftFrame.Data;
            //var image2 = leftFrame.Data;

            //Image<Gray, Byte> imageResult = new Image<Gray, Byte>(image1.Width, image1.Height*2);


            //imageResult.ROI = new Rectangle(0, 0, image1.Width, image1.Height);
            //image1.CopyTo(imageResult);
            //imageResult.ROI = new Rectangle(0, image1.Height, image2.Width, image2.Height);
            //image2.CopyTo(imageResult);
            //imageResult.ROI = Rectangle.Empty;
            //CreateResult Frame
            //var newFrame = new Frame<Bitmap>(imageResult.Bitmap);

            //CreateResult Frame
            var newFrame = new Frame<Bitmap>(leftFrame.Data.Bitmap);

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
        
    }
}
