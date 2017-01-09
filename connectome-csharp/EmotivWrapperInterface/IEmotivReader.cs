using System;

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
