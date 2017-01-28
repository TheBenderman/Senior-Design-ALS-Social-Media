using Connectome.Core.Int;
using Connectome.Core.Interface;
using Connectome.Core.Template;
using Connectome.Emotiv.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Implementation
{
    class ClickProcessee : Processee<Timeline<IEmotivState>>
    {
        protected override bool IsFulfilled(Timeline<IEmotivState> timeline)
        {
            throw new NotImplementedException();
        }
    }
}
