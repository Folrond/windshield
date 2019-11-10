using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SensorManager.Helpers;

namespace Common
{
    public abstract class RunnableProcessing<T>
    {
        private Frame<T> actualFrame;
        
        public Frame<T> ActualFrame
        {
            get => InterlockedHelper.SafeRead(ref actualFrame);
            protected set => InterlockedHelper.SafeWrite(ref actualFrame,value);
        }

        protected RunnableProcessing()
        {
            source = new CancellationTokenSource();
            token = source.Token;
        }

        public Frame<T> CopiedFrame => ActualFrame.Clone() as Frame<T>;

        private CancellationTokenSource source;
        private CancellationToken token;
        private Task serviceTask;

        public TaskStatus ServiceStatus => serviceTask?.Status ?? TaskStatus.Created;

        public virtual void StartQuery()
        {
            if (ServiceStatus == TaskStatus.Canceled || ServiceStatus == TaskStatus.Faulted || ServiceStatus == TaskStatus.Created)
            {
                token = source.Token;

                this.serviceTask = Task.Run(() =>
                {
                    try
                    {
                        while (!token.IsCancellationRequested)
                        {
                            this.QueryFrame();
                        }
                    }
                    finally
                    {
                        GC.Collect();
                    }

                }, token);

            }
        }

        //This is method will do the actual work.
        public abstract Frame<T> QueryFrame();

        public virtual void StopQuery()
        {
            source.Cancel();
        }


        protected virtual void OnFrameChanged(Frame<T> newFrame)
        {
            this.ActualFrame = newFrame;
        }

    }
}
