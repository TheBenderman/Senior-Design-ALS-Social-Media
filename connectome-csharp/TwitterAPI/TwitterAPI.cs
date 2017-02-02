using CoreTweet;
using System;
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
        private Tokens tokens;
        private int refillTweetsNumber = 3;
        private int initialTweetsNumber = 3;

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

            session = OAuth.Authorize("AeMBFSekBw8CiP19URpCeMsMy", "GhfHgUVq6i69VM1PaAQtZdnFH7eVLhlXcL9oAc6hnbnM2nOntf");  
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

        public String getAuthorizationURL()
        {
            if (session != null)
            {
                return session.AuthorizeUri.ToString();
            }

            return null;
        }

        public string getAccessToken()
        {
            if (!string.IsNullOrEmpty(tokens.AccessToken))
            {
                return tokens.AccessToken;
            }

            return "";
        }

        public string getAccessTokenSecret()
        {
            if (!string.IsNullOrEmpty(tokens.AccessTokenSecret))
            {
                return tokens.AccessTokenSecret;
            }

            return "";
        }

        public void setTokens(string accessToken, string accessTokenSecret)
        {
            tokens = Tokens.Create("AeMBFSekBw8CiP19URpCeMsMy", "GhfHgUVq6i69VM1PaAQtZdnFH7eVLhlXcL9oAc6hnbnM2nOntf", accessToken, accessTokenSecret);
        }

        public Boolean enterPinCode(string pinCode)
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

        public List<Status> getHomeTimeLine()
        {
            List<Status> list = new List<Status>();
            foreach (var status in tokens.Statuses.HomeTimeline(count => initialTweetsNumber)) {
                list.Add(status);
            }

            initialTweetsNumber += refillTweetsNumber;
            return list;
        }
        
        public void publishTweetWithPicture(String tweet, String picPath)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            tokens.Statuses.UpdateWithMedia(new { status = tweet, media = new FileInfo(picPath) });
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public void printHomeTimeline()
        {
            //Console.WriteLine(tokens.Statuses.HomeTimeline(count => 2).Json);
            JArray results = JArray.Parse(tokens.Statuses.HomeTimeline(count => 3).Json);
            Console.WriteLine(results);
            //foreach(var status in tokens.Statuses.HomeTimeline(count => 50))
            //{
            //Console.WriteLine("long tweet by {0}: {1}", status.User.ScreenName, status.Text);
            //Console.WriteLine();
            //}
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
    }
}
