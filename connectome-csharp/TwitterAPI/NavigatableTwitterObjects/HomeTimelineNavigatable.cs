using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreTweet;
using System.Threading;

namespace Connectome.Twitter.API.NavigatableTwitterObjects
{
    public class HomeTimelineNavigatable : TwitterObjectNavigatable<Status>
    {
        private int refreshTime = 60000;
        
        public HomeTimelineNavigatable(TwitterAuthenticator authenticator) : base(authenticator)
        {
            twitterObjects = new List<Status>();
        }

        public override void startThread()
        {
            twitterThread = new Thread(() => {
                try
                {
                    fetchHomeTimelineTweets();
                }
                catch (Exception e) {
                    OnExp?.Invoke(e);
                }
            });
            shouldContinueThread = true;
            twitterThread.Start();
            while (!twitterThread.IsAlive) ;
        }

        public override Status getNewerObject()
        {
            if (twitterObjects.Count == 0)
                return null;

            if (currentTwitterObject == null)
            {
                currentTwitterObject = twitterObjects[twitterObjects.Count - 1]; // Get the last tweet, which is the newest
                return currentTwitterObject;
            }

            int index = twitterObjects.FindIndex(x => x.Id == currentTwitterObject.Id);
            if (index == -1)
                throw new Exception("Error! Can't find current timeline tweet.");

            if (index >= (twitterObjects.Count - 1))
                return null;
            else
            {
                currentTwitterObject = twitterObjects.ElementAt(index + 1);
                return currentTwitterObject;
            }
        }

        public override Status getOlderObject()
        {
            if (twitterObjects.Count == 0)
                return null;

            if (currentTwitterObject == null)
            {
                currentTwitterObject = twitterObjects[twitterObjects.Count - 1]; // Get the last tweet, which is the newest
                return currentTwitterObject;
            }

            int index = twitterObjects.FindIndex(x => x.Id == currentTwitterObject.Id);
            if (index == -1)
                throw new Exception("Error! Can't find current timeline tweet.");

            if (index <= 0)
                return null;
            else
            {
                currentTwitterObject = twitterObjects.ElementAt(index - 1);
                return currentTwitterObject;
            }
        }

        public override int getNumOlderObjects()
        {
            int index = twitterObjects.FindIndex(x => x.Id == currentTwitterObject.Id);

            return index;
        }

        public override int getNumNewerObjects()
        {
            int index = twitterObjects.IndexOf(currentTwitterObject);

            return twitterObjects.Count - index - 1;
        }

        #region private methods
        private void fetchHomeTimelineTweets()
        {
            long last_id = 0;

            do
            {
                if (last_id != 0)
                {
                    foreach (var status in twitterAuth.getTokens().Statuses.HomeTimeline(count => 100, since_id => last_id).OrderBy(x => x.CreatedAt.DateTime))
                        twitterObjects.Add(status);
                }

                if (twitterObjects.Count > 0)
                    last_id = twitterObjects.First().Id;
                else
                {
                    foreach (var status in twitterAuth.getTokens().Statuses.HomeTimeline(count => 100).OrderBy(x => x.CreatedAt.DateTime))
                        twitterObjects.Add(status);
                }

                Thread.Sleep(refreshTime);
            }
            while (!_shouldStop);
        }
        #endregion
    }
}
