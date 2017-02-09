using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Twitter.API
{
    class TwitterAuthException : Exception
    {
        public TwitterAuthException()
        {
        }

        public TwitterAuthException(string message)
        : base(message)
        {
        }
    }
}
