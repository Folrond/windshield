using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    //This class can start a background thread to do work.
    //If a new result is produced the subscribers will get this new data.
    public abstract class RunnableDataProvider<T>
    {
        protected event Action<Frame<T>> ActualFramePushEvent;
        protected event Action<Frame<T>> CopiedFramePushEvent;
        
        private CancellationTokenSource source;
        private CancellationToken token;
        private Task serviceTask;

        public TaskStatus ServiceStatus => serviceTask?.Status ?? TaskStatus.Created;

        protected RunnableDataProvider()
        {
            source = new CancellationTokenSource();
            token = source.Token;
        }

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
            ActualFramePushEvent?.Invoke(newFrame);
            CopiedFramePushEvent?.Invoke((Frame<T>)newFrame.Clone());
        }


        public virtual void RegisterForActualFramePush(Action<Frame<T>> eventHandler)
        {
            ActualFramePushEvent += eventHandler;
        }

        public virtual void RegisterForCopiedFramePush(Action<Frame<T>> eventHandler)
        {
            CopiedFramePushEvent += eventHandler;
        }

        public virtual void UnregisterFromActualFramePush(Action<Frame<T>> eventHandler)
        {
            ActualFramePushEvent -= eventHandler;
        }

        public virtual void UnregisterFromCopiedFramePush(Action<Frame<T>> eventHandler)
        {
            CopiedFramePushEvent -= eventHandler;
        }
    }
}
