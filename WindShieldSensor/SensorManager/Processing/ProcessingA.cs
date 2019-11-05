using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Sensors.Sensors;

namespace SensorManager.Processing
{
    public class ProcessingA : RunnableDataProvider<string>, IDisposable
    {
        private readonly RgbCamera leftCamera;
        private readonly RgbCamera rightCamera;

        //TODO Enrich get/set to produce statistics about dropped (unused) frames, and lag
        //TODO Enrich get/set to introduce Epsilon distance
        public Frame<object> RecievedLeftFrame { get; set; }
        public Frame<object> RecievedRightFrame { get; set; }

        private Action recievedLeftFrameAction;
        private Action recievedRightFrameAction;

        private object recieveLockingObject = new object();

        //TODO This needs finishing, we'd like to prevent useless data recalculation (if this module is fast but modules above are not)
        //TODO At the begining we need to wait for booth required data
        AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        public ProcessingA()
        {
            leftCamera = new RgbCamera("ResourcePath to Left Camera");
            rightCamera = new RgbCamera("ResourcePath to Right Camera");
            RegisterForPushEvents();
        }

        public ProcessingA(RgbCamera cameraLeft, RgbCamera cameraRight)
        {
            this.leftCamera = cameraLeft;
            this.rightCamera = cameraRight;
            RegisterForPushEvents();
        }

        private void RegisterForPushEvents()
        {
            recievedLeftFrameAction = () =>
            {
                lock (recieveLockingObject)
                {
                    RecievedLeftFrame = leftCamera.ActualCurrentFrame;
                }
                
            };
            recievedRightFrameAction = () =>
            {
                lock (recieveLockingObject)
                {
                    RecievedRightFrame = rightCamera.ActualCurrentFrame;
                }
            };

            leftCamera.ActualFramePushEvent += recievedLeftFrameAction;
            rightCamera.ActualFramePushEvent += recievedRightFrameAction;
        }

        protected override void QueryFrame()
        {
            var frames = ReadFrames();
            //TODO DO Work here like yolo calculation and other stuff
            Thread.Sleep(new Random().Next(50,70));

            this.ActualCurrentFrame = new Frame<string>
            {
                TimeStamp = DateTime.Now,
                Data = "ProcessingA"
            };

            this.OnFrameChanged();
            
        }

        //TODO Return List, Dto, Whatever
        private object ReadFrames()
        {
            lock (recieveLockingObject)
            {
                var f1 = RecievedRightFrame;
                var f2 = RecievedLeftFrame;

                return new {f1,f2};
            }
            
        }


        public void Dispose()
        {
            leftCamera.ActualFramePushEvent -= recievedLeftFrameAction;
            rightCamera.ActualFramePushEvent -= recievedRightFrameAction;

        }
    }
}
