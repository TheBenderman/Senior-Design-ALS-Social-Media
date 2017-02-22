using System;
using System.Collections.Generic;
using System.Linq;
using CoreTweet;

namespace Connectome.Twitter.API
{
	public class TwitterInteractor
	{

        private int refillTweetsNumber = 20;
		private int initialTweetsNumber = 3;
		private TwitterAuthenticator authenticator;

		public TwitterInteractor(TwitterAuthenticator twitauth){
            authenticator = twitauth;
		}

        public TwitterAuthenticator getA() {
            return this.authenticator;
        }

		#region TwitterInteraction
		// publish a tweet
		public void publishTweet(String tweet)
		{
			authenticator.getTokens().Statuses.Update(new { status = tweet });
		}

		// Get the user's home time line
		public List<Status> getHomeTimeLine()
		{
			List<Status> list = new List<Status>();
			foreach (var status in authenticator.getTokens().Statuses.HomeTimeline(count => initialTweetsNumber))
			{
				list.Add(status);
			}

			initialTweetsNumber += refillTweetsNumber;
			return list;
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

		public string getTop5HomeTimeLineTweets()
		{
			string timeLineString = "";

			foreach (var status in authenticator.getTokens().Statuses.HomeTimeline(count => 5))
			{
				timeLineString += "Tweet by " + status.User.ScreenName + " : " + status.Text + "\n";
				Console.WriteLine("long tweet by {0}: {1}", status.User.ScreenName, status.Text);
			}

			return timeLineString;
		}

		public List<DirectMessage> getReceivedDMs()
		{
			List<DirectMessage> list = new List<DirectMessage>();
			foreach (var dm in authenticator.getTokens().DirectMessages.Received(count => 100))
			{
				list.Add(dm);
			}

			return list;
		}

		public List<DirectMessage> getSentDMs()
		{
			List<DirectMessage> list = new List<DirectMessage>();
			foreach (var dm in authenticator.getTokens().DirectMessages.Sent(count => 100))
			{
				list.Add(dm);
			}

			return list;
		}

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
			foreach (var dm in authenticator.getTokens().DirectMessages.Sent(count => 100).Where(x => x.Recipient.Equals(sender)))
				list.Add(dm);

			foreach (var dm in authenticator.getTokens().DirectMessages.Received(count => 100).Where(x => x.Sender.Equals(sender)))
				list.Add(dm);

			return list.OrderBy(x => x.CreatedAt).ToList();
		}

		public List<Status> search(string text)
		{
			List<Status> statuses = new List<Status>();
			foreach (var status in authenticator.getTokens().Search.Tweets(q => text))
				statuses.Add(status);

			return statuses.OrderBy(x => x.CreatedAt).ToList();
		}

        public List<User> getUniqueDMs()
        {
            List<DirectMessage> sentList = new List<DirectMessage>();
            Tokens tokens = authenticator.getTokens();
            var users = tokens.DirectMessages.Sent(count => 100).OrderBy(x => x.CreatedAt)
                .Select(x => x.Recipient);
            users = users.Union(tokens.DirectMessages.Received(count => 100).OrderBy(x => x.CreatedAt)
                .Select(x => x.Sender));
            users = users.GroupBy(x => x.Name).First();

            return users.ToList();
        }
		#endregion

	}
}
