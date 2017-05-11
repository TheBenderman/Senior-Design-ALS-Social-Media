using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Calibration.API.Interfaces
{
    public interface LoggerInterface
    {
        void add(String unit);

        void write();
    }
}
