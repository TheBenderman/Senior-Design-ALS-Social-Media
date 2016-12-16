using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotivWrapperInterface
{
    public interface IEmotivDevice
    {
       bool Connect(out string errorMsg);
       void Disconnect();

       bool ConnectionSetUp(out string errorMsg);
       bool DisconnectionSetUp();

       IEmotivState Read();
    }
}
