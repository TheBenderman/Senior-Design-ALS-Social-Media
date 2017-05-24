using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CoreTweet;
using System.Threading;

namespace Connectome.Twitter.API.NavigatableTwitterObjects
{
    public class HomeTimelineNavigatable : TwitterObjectNavigatable<Status>
    {
        private int refreshTime = 60000;

        private int currentFirstTweetIndex = -1;
        private int currentLastTweetIndex = -1;
        
        public HomeTimelineNavigatable(TwitterAuthenticator authenticator) : base(authenticator)
        {
            twitterObjects = new List<Status>();
            refresh();
        }

        public bool hasNewerObject()
        {
            return currentFirstTweetIndex != -1 && currentFirstTweetIndex < twitterObjects.Count - 1;
        }

        public bool hasOlderObject()
        {
            return currentFirstTweetIndex != -1 && currentLastTweetIndex > 0;
        }

        public List<Status> getFourOlderTweets()
        {
            List<Status> tweets = new List<Status>();
            for (int i = 0; i < 4; i++)
            {
                if (currentLastTweetIndex == -1)
                    currentLastTweetIndex = twitterObjects.Count;

                if ((currentLastTweetIndex - i - 1) >= 0)
                    tweets.Add(twitterObjects.ElementAt(currentLastTweetIndex - i - 1));
                else
                    tweets.Add(null);
            }

            currentFirstTweetIndex = twitterObjects.FindIndex(x => x.Id == tweets.ElementAt(0).Id);
            currentLastTweetIndex = twitterObjects.FindIndex(x => x.Id == tweets.ElementAt(3).Id);

            return tweets;
        }

        public List<Status> getFourNewerTweets()
        {
            List<Status> tweets = new List<Status>();
            for (int i = 0; i < 4; i++)
            {
                if (currentLastTweetIndex == -1)
                    return getFourOlderTweets();

                if ((currentFirstTweetIndex + i + 1) <= (twitterObjects.Count - 1))
                    tweets.Add(twitterObjects.ElementAt(currentFirstTweetIndex + i + 1));
                else
                    tweets.Add(null);
            }

            currentFirstTweetIndex = twitterObjects.FindIndex(x => x.Id == tweets.ElementAt(3).Id);
            currentLastTweetIndex = twitterObjects.FindIndex(x => x.Id == tweets.ElementAt(0).Id);

            return tweets;
        }

        public override void refresh()
        {
            currentTwitterObject = null;
            twitterObjects = new List<Status>();
            currentTwitterObjectIndex = -1;

            foreach (var status in twitterAuth.getTokens().Statuses.HomeTimeline(count => 200).OrderBy(x => x.CreatedAt.DateTime))
                twitterObjects.Add(status);

            /*currentTwitterObject = twitterObjects.Last(); // Get the last tweet, which is the newest
            currentTwitterObjectIndex = twitterObjects.Count - 1;*/
            currentFirstTweetIndex = -1;
            currentLastTweetIndex = -1;
        }

        /* UNUSED */
        public override void startThread()
        {
            /*twitterThread = new Thread(() => {
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
            while (!twitterThread.IsAlive) ;*/
        }

        public void setCurrentFirstTweetIndex(int index)
        {
            currentFirstTweetIndex = index;
        }

        public int getCurrentFirstTweetIndex()
        {
            return currentFirstTweetIndex;
        }

        public void setCurrentTwitterItemNum(int index)
        {
            currentTwitterObjectIndex = index;
        }

        public int getCurrentTweetIndex()
        {
            return currentTwitterObjectIndex;
        }

        /* UNUSED */
        public override Status getNewerObject()
        {
            if (twitterObjects.Count == 0)
                return null;

            if (currentTwitterObject == null)
            {
                currentTwitterObject = twitterObjects.Last(); // Get the last tweet, which is the newest
                currentTwitterObjectIndex = twitterObjects.Count - 1;
                currentFirstTweetIndex = currentTwitterObjectIndex;
                return currentTwitterObject;
            }

            /*int index = twitterObjects.FindIndex(x => x.Id == currentTwitterObject.Id);
            if (index == -1)
                throw new Exception("Error! Can't find current timeline tweet.");*/

            if (currentTwitterObjectIndex >= (twitterObjects.Count - 1))
                return null;
            else
            {
                currentTwitterObjectIndex++;
                currentTwitterObject = twitterObjects.ElementAt(currentTwitterObjectIndex);
                return currentTwitterObject;
            }
        }

        /* UNUSED */
        public override Status getOlderObject()
        {
            if (twitterObjects.Count == 0)
                return null;

            if (currentTwitterObject == null)
            {
                currentTwitterObject = twitterObjects.Last(); // Get the last tweet, which is the newest
                currentTwitterObjectIndex = twitterObjects.Count - 1;
                currentFirstTweetIndex = currentTwitterObjectIndex;
                return currentTwitterObject;
            }

            /*int index = twitterObjects.FindIndex(x => x.Id == currentTwitterObject.Id);
            if (index == -1)
                throw new Exception("Error! Can't find current timeline tweet.");*/

            if (currentTwitterObjectIndex <= 0)
                return null;
            else
            {
                currentTwitterObjectIndex--;
                currentTwitterObject = twitterObjects.ElementAt(currentTwitterObjectIndex);
                return currentTwitterObject;
            }
        }

        /* UNUSED */
        public override int getNumOlderObjects()
        {
            /*int index = twitterObjects.FindIndex(x => x.Id == currentTwitterObject.Id);*/

            return currentTwitterObjectIndex;
        }

        /* UNUSED */
        public override int getNumNewerObjects()
        {
            /*int index = twitterObjects.IndexOf(currentTwitterObject);*/

            return twitterObjects.Count - currentTwitterObjectIndex - 1;
        }

        #region private methods
        /* UNUSED */
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
