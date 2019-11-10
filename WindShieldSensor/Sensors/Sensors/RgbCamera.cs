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

namespace Sensors.Sensors
{

    public class RgbCamera:RunnableDataProvider<Mat>
    {

        private Capture capture;

        public RgbCamera()
        {   //This capture will use the first (and only in my demo) camera.
            //You can specify the camera index.
            capture = new Capture();
        }

        public RgbCamera(int resourcePath)
        {
            capture = new Capture(resourcePath);
        }


        public override Frame<Mat> QueryFrame()
        {
            var img = capture.QueryFrame();
            var frame = new Frame<Mat>(img);
            OnFrameChanged(frame);
            return frame;
            //var bmp = img.Bitmap;
            //.ToImage<Bgr, byte>();
        }

    }
}
