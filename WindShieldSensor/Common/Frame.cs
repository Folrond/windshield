using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Frame<T> : ICloneable, IDisposable
    {
        public Frame()
        {
            Data = default(T);
        }

        public Frame(T data)
        {
            Data = data;
            TimeStamp = DateTime.Now;
        }

        public Frame(T data, DateTime timeStamp)
        {
            Data = data;
            TimeStamp = timeStamp;
        }

        public DateTime TimeStamp { get;}

        public T Data { get; }

        //TODO Implement deep copy
        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        //TODO Dispose Data if possible
        public virtual void Dispose()
        {
            if(Data is IDisposable disposable)
                disposable.Dispose();
        }

        public bool IsEmpty => Data == null;
    }
}
    