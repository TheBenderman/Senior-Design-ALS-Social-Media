using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Connectome.Twitter.API.NavigatableTwitterObjects;
using CoreTweet;
using CoreTweet.Core;

namespace Connectome.Twitter.API
{
	public class TwitterInteractor
	{
        private int refillTweetsNumber = 20;
		private int initialTweetsNumber = 3;
        private int initialProfileTweetsNumber = 3;

		private TwitterAuthenticator authenticator;

	    private Thread timelineThread;
	    private Thread dmThread;

	    private DMUsersNavigatable dmUsersNavigatable;
	    private HomeTimelineNavigatable homeTimelineNavigatable;

		public TwitterInteractor(TwitterAuthenticator twitauth){
            authenticator = twitauth;

            dmUsersNavigatable = new DMUsersNavigatable(authenticator);
            homeTimelineNavigatable = new HomeTimelineNavigatable(authenticator);

		    dmUsersNavigatable.startThread();
            homeTimelineNavigatable.startThread();
		}

	    public DMUsersNavigatable getDmUsersNavigatable()
	    {
	        return dmUsersNavigatable;
	    }

	    public HomeTimelineNavigatable getHomeTimelineNavigatable()
	    {
	        return homeTimelineNavigatable;
	    }

        public TwitterAuthenticator getA() {
            return this.authenticator;
        }

	    public string getCurrentUser()
	    {
	        ListedResponse<Status> statuses = authenticator.getTokens().Statuses.UserTimeline(count => 1);
	        return statuses.First().User.ScreenName;
	    }

		// publish a tweet
		public void publishTweet(String tweet)
		{
			authenticator.getTokens().Statuses.Update(new { status = tweet });
		}

		public List<Status> getConversation(string screenName, string id)
		{
			List<Status> list = new List<Status>();
			foreach (var status in authenticator.getTokens().Search.Tweets(q => "to:" + screenName, since_id => id, count => 100))
			{
				if (status.InReplyToStatusId.ToString().Contains(id))
				{
					list.Add(status);
				}
			}

			return list;
		}

		#region DM Functions
		public DirectMessage getDM(string dmID)
		{
			return authenticator.getTokens().DirectMessages.Show(id => dmID);
		}

		public void createDM(string screenName, string reply)
		{
			authenticator.getTokens().DirectMessages.New(screen_name => screenName, text => reply);
		}

		// Need to create a method to get the DM conversation
		public List<DirectMessage> buildDMConversation(string sender)
		{
			List<DirectMessage> list = new List<DirectMessage>();
			foreach (var dm in authenticator.getTokens().DirectMessages.Sent(count => 100).Where(x => x.Recipient.ScreenName.Equals(sender)))
				list.Add(dm);

			foreach (var dm in authenticator.getTokens().DirectMessages.Received(count => 100).Where(x => x.Sender.ScreenName.Equals(sender)))
				list.Add(dm);

		    list = list.GroupBy(x => x.Id).Select(x => x.First()).
                OrderByDescending(x => x.CreatedAt.DateTime).ToList();

		    return list;
		}

		#endregion

		public List<Status> search(string text)
		{
			List<Status> statuses = new List<Status>();
			foreach (var status in authenticator.getTokens().Search.Tweets(q => text))
				statuses.Add(status);

			return statuses.OrderBy(x => x.CreatedAt).ToList();
		}

        #region profile functions
        public string getLoggedInUserScreenName() {
            return authenticator.getTokens().ScreenName.ToString();
        }

        public List<Status> getLoggedInUserTimeline() {
            List<Status> list = new List<Status>();

            foreach (var status in authenticator.getTokens().Statuses.UserTimeline(count => initialProfileTweetsNumber)) {
                list.Add(status);
                //Console.Write(status.Text + "\n" );
            }
            initialProfileTweetsNumber += refillTweetsNumber;

            return list;
        }

        public List<Status> getTweetsFromUser(string user) {
            List<Status> list = new List<Status>();
            foreach (var status in authenticator.getTokens().Search.Tweets(q => user, count => 100))
            {
                list.Add(status);
                //Console.Write(status.Text + "\n");
            }
            return null;
        }

        public List<User> getFollowing(string user_name) {
            List<User> list = new List<User>();
            foreach (var user in authenticator.getTokens().Friends.List(screen_name => user_name))
            {
                list.Add(user);
                //Console.Write(user.ScreenName.ToString() + "\n");
            }
            return list;
        }

        public List<User> getFollowers(string user_name)
        {
            List<User> list = new List<User>();
            foreach (var user in authenticator.getTokens().Followers.List(screen_name => user_name))
            {
                list.Add(user);
            }
            return list;
        }
        #endregion
    }
}