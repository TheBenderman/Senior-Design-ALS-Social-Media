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
        protected volatile bool _shouldStop;
        public Action<Exception> OnExp;

        protected List<T> twitterObjects;

        protected T currentTwitterObject;

        protected int currentTwitterObjectIndex;

        protected TwitterAuthenticator twitterAuth;

        protected bool shouldContinueThread = true;

        protected TwitterObjectNavigatable(TwitterAuthenticator authenticator)
        {
            twitterAuth = authenticator;
        }

        public abstract void startThread();
        public abstract void refresh();

        public void RequestStop()
        {
            _shouldStop = true;
        }

        public void stopThread() {
            if (twitterThread.IsAlive)
            {
                RequestStop();
                twitterThread.Join();
            }
        }

        public void endThread()
        {
            shouldContinueThread = false;
            twitterThread.Abort();
        }

        public abstract T getNewerObject();

        public abstract T getOlderObject();

        public bool hasNewerObject()
        {
            if (currentTwitterObject == null)
                return false;

            /*int index = twitterObjects.IndexOf(currentTwitterObject);
            if (index == -1)
                throw new Exception("Error! Can't find newer object.");*/

            return currentTwitterObjectIndex < (twitterObjects.Count - 1) && twitterObjects.Count > 0;
        }

        public bool hasOlderObject()
        {
            if (currentTwitterObject == null)
                return false;

            /*int index = twitterObjects.IndexOf(currentTwitterObject);
            if (index == -1)
                throw new Exception("Error! Can't find older object.");*/

            return currentTwitterObjectIndex > 0 && twitterObjects.Count > 0;
        }

        public abstract int getNumNewerObjects();

        public abstract int getNumOlderObjects();

        public void resetCurrentObject()
        {
            currentTwitterObject = default(T);
            currentTwitterObjectIndex = twitterObjects.Count - 1;
        }
    }
}
