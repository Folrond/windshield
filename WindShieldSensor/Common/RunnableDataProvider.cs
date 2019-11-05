using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    //INotifyPropChanged can be considered
    public abstract class RunnableDataProvider<T> : IDataProvider<T>
    {
        public event Action ActualFramePushEvent;
        public event Action CopiedFramePushEvent;

        //TODO Not really necessary since the "readers" are actually running on our data producing thread after an event notification
        public ReaderWriterLockSlim ProviderLock = new ReaderWriterLockSlim();

        private Frame<T> currenFrame;

        //TODO We need to know when to dispose a Frame, disposing it when we get a new one is not an option since we don't know who's using it.
        //TODO Solution: Count ReadAttempts if no one read this prop until the next write occurs dispose the frame data.
        public Frame<T> ActualCurrentFrame
        {
            get
            {
                try
                {
                    ProviderLock.EnterReadLock();
                    return currenFrame;
                }
                finally
                {
                    ProviderLock.ExitReadLock();
                }
            }
            set
            {
                try
                {
                    ProviderLock.EnterWriteLock();
                    currenFrame = value;
                }
                finally
                {
                    ProviderLock.ExitWriteLock();
                }

            }
        }

        //Since this is a deep copied object caller is responsible for disposing it if possible
        public Frame<T> CopiedCurrentFrame => (Frame<T>)ActualCurrentFrame.Clone();


        protected CancellationTokenSource source;
        protected CancellationToken token;
        protected Task serviceTask;

        public TaskStatus ServiceStatus => serviceTask?.Status ?? TaskStatus.Created;

        public virtual void StartQuery()
        {
            if (ServiceStatus == TaskStatus.Canceled || ServiceStatus == TaskStatus.Faulted)
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

        protected abstract void QueryFrame();

        public virtual void StopQuery()
        {
            source.Cancel();
        }

        protected virtual void OnFrameChanged()
        {
            ActualFramePushEvent?.Invoke();
            CopiedFramePushEvent?.Invoke();
        }
    }
}
