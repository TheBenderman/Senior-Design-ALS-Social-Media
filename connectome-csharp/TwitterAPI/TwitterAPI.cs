using CoreTweet;
using System;
using System.Collections.Generic;
using System.IO;

namespace Connectome.Twitter.API
{
    public class TwitterAPI
    {

        private static TwitterAPI instance;
        private OAuth.OAuthSession session;
        private Tokens tokens;

        public static TwitterAPI Instance
        {
            get
            {
                if (instance == null)
                    return instance = new TwitterAPI();
                else
                    return instance;
            }
        }

        public TwitterAPI()
        {
            session = OAuth.Authorize("AeMBFSekBw8CiP19URpCeMsMy", "GhfHgUVq6i69VM1PaAQtZdnFH7eVLhlXcL9oAc6hnbnM2nOntf");  
        }

        public String getAuthorizationURL()
        {
            if (session != null)
            {
                return session.AuthorizeUri.ToString();
            }

            return null;
        }

        public Boolean enterPinCode(String pinCode)
        {
            tokens = session.GetTokens(pinCode);

            if (tokens != null)
                return true;
            else
                return false;
        }

        public void publishTweet(String tweet)
        {
            tokens.Statuses.Update(new { status = tweet });
        }

        public CoreTweet.Rest.Statuses getHomeTimeLine()
        {
            return tokens.Statuses;
        }

        public void publishTweetWithPicture(String tweet, String picPath)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            tokens.Statuses.UpdateWithMedia(new { status = tweet, media = new FileInfo(picPath) });
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public void printHomeTimeline()
        {
            foreach(var status in tokens.Statuses.HomeTimeline(count => 50))
            {
                Console.WriteLine("long tweet by {0}: {1}", status.User.ScreenName, status.Text);
            }
        }
    }
}
