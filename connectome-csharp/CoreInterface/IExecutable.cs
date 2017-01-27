using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Interface
{
    public interface IExecutable
    {
        event Action OnExecute; 
    }
}
