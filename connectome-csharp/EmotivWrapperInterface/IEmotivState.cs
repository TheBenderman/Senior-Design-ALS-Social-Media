using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotivWrapperInterface
{
    public interface IEmotivState
    {
       EmotivStateType command { set; get; }
       float power { set; get; }
       long time { set; get; }
    }
}
