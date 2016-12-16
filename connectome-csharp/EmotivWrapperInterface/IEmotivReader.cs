using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotivWrapperInterface
{
    public interface IEmotivReader
    {
        bool isRunning { set; get; }

        Action<IEmotivState> OnRead { set;}
        Action<EmotivStateType?, EmotivStateType> OnStateChange { set;  }
        Action OnStart { set; }
        Action OnStop { set; }
        
        void Start();
        void Stop(); 
    }
}
