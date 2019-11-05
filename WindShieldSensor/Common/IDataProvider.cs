using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Common
{
    public interface IDataProvider<T>
    {
        event Action ActualFramePushEvent;
        event Action CopiedFramePushEvent;

        Frame<T> ActualCurrentFrame { get; set; }
        Frame<T> CopiedCurrentFrame { get; }
    
    }
}
