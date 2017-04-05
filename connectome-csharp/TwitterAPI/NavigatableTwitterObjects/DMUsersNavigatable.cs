using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CoreTweet;
using CoreTweet.Core;

namespace Connectome.Twitter.API.NavigatableTwitterObjects
{
    public class DMUsersNavigatable : TwitterObjectNavigatable<User>
    {
        private int refreshTime = 60000;
        public DMUsersNavigatable(TwitterAuthenticator authenticator) : base(authenticator)
        {
            twitterObjects = new List<User>();
        }

        public override void startThread()
        {
            twitterThread = new Thread(() => {
                try
                {
                    fetchDMs();
                }
                catch (Exception e)
                {
                    OnExp?.Invoke(e);
                }
            });
            shouldContinueThread = true;
            twitterThread.Start();
            while (!twitterThread.IsAlive) ;
        }

        public override User getNewerObject()
        {
            if (twitterObjects.Count == 0)
                return null;

            if (currentTwitterObject == null)
            {
                currentTwitterObject = twitterObjects[twitterObjects.Count - 1]; // Get the last tweet, which is the newest
                return currentTwitterObject;
            }

            int index = twitterObjects.IndexOf(currentTwitterObject);
            if (index == -1)
                throw new Exception("Error! Can't find current direct message user.");

            if (index >= (twitterObjects.Count - 1))
                return null;
            else
            {
                currentTwitterObject = twitterObjects.ElementAt(index + 1);
                return currentTwitterObject;
            }
        }

        public override User getOlderObject()
        {
            if (twitterObjects.Count == 0)
                return null;

            if (currentTwitterObject == null)
            {
                currentTwitterObject = twitterObjects[twitterObjects.Count - 1]; // Get the last tweet, which is the newest
                return currentTwitterObject;
            }

            int index = twitterObjects.IndexOf(currentTwitterObject);
            if (index == -1)
                throw new Exception("Error! Can't find current direct message user.");

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

            return twitterObjects.Count - index + 1;
        }

        private void fetchDMs()
        {
            do
            {
                IEnumerable<User> users = twitterAuth.getTokens().DirectMessages.Sent(count => 100).OrderByDescending(x => x.CreatedAt.DateTime)
                    .Select(x => x.Recipient);
                users = users.Union(twitterAuth.getTokens().DirectMessages.Received(count => 100).OrderByDescending(x => x.CreatedAt.DateTime)
                    .Select(x => x.Sender));

                users = users.GroupBy(x => x.Id).Select(group => group.First());
                users = users.Where(x => x.ScreenName != getCurrentUser());

                twitterObjects.Intersect(users.ToList());

                Thread.Sleep(refreshTime);
            }
            while (!_shouldStop);
        }

        private string getCurrentUser()
        {
            ListedResponse<Status> statuses = twitterAuth.getTokens().Statuses.UserTimeline(count => 1);
            return statuses.First().User.ScreenName;
        }
    }
}
