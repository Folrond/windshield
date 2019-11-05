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
            TimeStamp = DateTime.Now;
        }

        public DateTime TimeStamp { get; set; }

        public T Data { get; set; }

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
    }
}
    