using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Connectome.Twitter.API
{
    public abstract class TwitterObjectNavigatable<T>
    {
        protected Thread twitterThread;

        protected List<T> twitterObjects;

        protected T currentTwitterObject;

        protected TwitterAuthenticator twitterAuth;

        protected bool shouldContinueThread = true;

        protected TwitterObjectNavigatable(TwitterAuthenticator authenticator)
        {
            twitterAuth = authenticator;
        }

        public abstract void startThread();

        public void endThread()
        {
            shouldContinueThread = false;
            twitterThread.Abort();
        }

        public abstract T getNewerObject();

        public abstract T getOlderObject();

        public bool hasNewerObject()
        {
            int index = twitterObjects.IndexOf(currentTwitterObject);
            if (index == -1)
                throw new Exception("Error! Can't find newer object.");

            return index < (twitterObjects.Count - 1) && twitterObjects.Count > 0;
        }

        public bool hasOlderObject()
        {
            int index = twitterObjects.IndexOf(currentTwitterObject);
            if (index == -1)
                throw new Exception("Error! Can't find older object.");

            return index > 0 && twitterObjects.Count > 0;
        }

        public abstract int getNumNewerObjects();

        public abstract int getNumOlderObjects();

        public void resetCurrentObject()
        {
            currentTwitterObject = default(T);
        }
    }
}
