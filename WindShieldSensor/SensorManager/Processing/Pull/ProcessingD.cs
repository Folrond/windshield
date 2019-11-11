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
using OpenTK.Graphics.OpenGL;

namespace SensorManager.Processing
{
    public class ProcessingD:RunnableProcessing<Bitmap>
    {
        private ProcessingC processingC;


        public ProcessingD(ProcessingC c)
        {
            processingC = c;
        }

        public override Frame<Bitmap> QueryFrame()
        {

            var latestFrame = processingC.ActualFrame;

            if (!CanExecute(latestFrame))
                return null;

            //TODO DO Work here like yolo calculation and other stuff
            Thread.Sleep(new Random().Next(50, 70));

            var img = latestFrame.Data.ToImage<Bgr,byte>();
            var rotated = img.Rotate(20, new Bgr(Color.Gray));

            //CreateResult Frame
            var newFrame = new Frame<Bitmap>(rotated.Bitmap);

            //Push data
            OnFrameChanged(newFrame);

            return newFrame;


        }

        private bool CanExecute(Frame<Mat> frame1)
        {
            if (frame1 == null )
                return false;

            if (frame1.IsEmpty)
                return false;

            return true;
        }
    }
}
