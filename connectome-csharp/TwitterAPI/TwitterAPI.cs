using CoreTweet;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;

namespace Connectome.Twitter.API
{
    public class TwitterAPI
    {
        private static TwitterAPI instance;
        private OAuth.OAuthSession session;
        private Tokens tokens; // This is where the OAuth tokens are stored

        private int refillTweetsNumber = 3;
        private int initialTweetsNumber = 3;

        // Singleton object for the API
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
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            Configuration config = null;
            try
            {
                // Open the config file for the dll
                string exeConfigPath = this.GetType().Assembly.Location;
                config = ConfigurationManager.OpenExeConfiguration(exeConfigPath);
            }
            catch(Exception ex)
            {
                // handle error
            }

            if (config != null)
            {
                // Grab the keys from the config file
                string consumerKey = GetAppSetting(config, "consumerKey");
                string consumerSecret = GetAppSetting(config, "consumerSecret");

                // Store the keys for the app, these should probably be stored in a file
                session = OAuth.Authorize(consumerKey, consumerSecret);
            } 
        }

        // Function to grab a specific app setting from a configuration file
        private string GetAppSetting(Configuration config, string key)
        {
            KeyValueConfigurationElement element = config.AppSettings.Settings[key];
            if (element != null)
            {
                string value = element.Value;
                if (!string.IsNullOrEmpty(value))
                    return value;
            }
            return string.Empty;
        }

        // Politely ignore the below code :-)
        #region CertificateFix 
        public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;
            // If there are errors in the certificate chain, look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                        bool chainIsValid = chain.Build((X509Certificate2)certificate);
                        if (!chainIsValid)
                        {
                            isOk = false;
                        }
                    }
                }
            }
            return isOk;
        }
        #endregion

        // Return the url that the user needs to authorize twitter for the application
        public String getAuthorizationURL()
        {
            // Make sure the user has a valid session
            if (session != null)
            {
                return session.AuthorizeUri.ToString();
            }

            throw new TwitterAuthException("Unable to get session for user.");
        }

        // Return the access token that was generated during authentication
        public string getAccessToken()
        {
            if (!string.IsNullOrEmpty(tokens.AccessToken))
            {
                return tokens.AccessToken;
            }

            throw new TwitterAuthException("Unable to get the tokens for user.");
        }

        // Return the access token that was generated during authentication
        public string getAccessTokenSecret()
        {
            if (!string.IsNullOrEmpty(tokens.AccessTokenSecret))
            {
                return tokens.AccessTokenSecret;
            }

            throw new TwitterAuthException("Unable to get the tokens for user.");
        }

        // Set the tokens. Used by the able to set the tokens from a previous session
        public void setTokens(string accessToken, string accessTokenSecret)
        {
            tokens = Tokens.Create("AeMBFSekBw8CiP19URpCeMsMy", "GhfHgUVq6i69VM1PaAQtZdnFH7eVLhlXcL9oAc6hnbnM2nOntf", accessToken, accessTokenSecret);
        }

        public Boolean enterPinCode(string pinCode)
        {
            tokens = session.GetTokens(pinCode); // get the oauth tokens for the entered pin code

            if (tokens != null)
                return true;
            else
                throw new TwitterAuthException("Unable to get the tokens for user.");
        }

        // publish a tweet
        public void publishTweet(String tweet)
        {
            tokens.Statuses.Update(new { status = tweet });
        }

        // Get the user's home time line
        public List<Status> getHomeTimeLine()
        {
            List<Status> list = new List<Status>();
            foreach (var status in tokens.Statuses.HomeTimeline(count => initialTweetsNumber)) {
                list.Add(status);
            }

            initialTweetsNumber += refillTweetsNumber;
            return list;
        }

        public List<Status> getConversation(string username, string tweetid) {
            List<Status> list = new List<Status>();
            foreach (var status in tokens.Search.Tweets(q => "to:"+username, since_id => tweetid, count => 100)) {
                if (status.InReplyToStatusId.ToString().Contains(tweetid)) {
                    list.Add(status);
                }
            }

            return list;
        }

        public string getTop5HomeTimeLineTweets()
        {
            string timeLineString = "";

            foreach (var status in tokens.Statuses.HomeTimeline(count => 5))
            {
                timeLineString += "Tweet by " + status.User.ScreenName + " : " + status.Text + "\n";
                //Console.WriteLine("long tweet by {0}: {1}", status.User.ScreenName, status.Text);
            } 

            return timeLineString;
        }

        public List<DirectMessage> getReceivedDMs()
        {
            List<DirectMessage> list = new List<DirectMessage>();
            foreach (var dm in tokens.DirectMessages.Received(count => 100))
            {
                list.Add(dm);
            }

            return list;
        }

        public List<DirectMessage> getSentDMs()
        {
            List<DirectMessage> list = new List<DirectMessage>();
            foreach (var dm in tokens.DirectMessages.Sent(count => 100))
            {
                list.Add(dm);
            }

            return list;
        }

        public DirectMessage getDM(string dmID)
        {
            return tokens.DirectMessages.Show(id => dmID);
        }

        public void createDM(string screenName, string reply)
        {
            tokens.DirectMessages.New(screen_name => screenName, text => reply);
        }

        // Need to create a method to get the DM conversation
    }
}
