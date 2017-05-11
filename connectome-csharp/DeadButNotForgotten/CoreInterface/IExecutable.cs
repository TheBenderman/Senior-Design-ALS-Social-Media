using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Interface
{
    public interface IExecutable<T>
    {
        event Action<T> OnExecute; 
    }
}
