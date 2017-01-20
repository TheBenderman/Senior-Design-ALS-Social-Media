using System;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Connectome.Twitter.API
{
    public class TwitterAPI
    {
        private TwitterCredentials applicationCredentials;
        private IAuthenticationContext authenticationContext;
        private ITwitterCredentials userCredentials;
        private IAuthenticatedUser user;

        private static TwitterAPI instance;

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
          
        }

        public void initializeAuthentication()
        {
            applicationCredentials = new Tweetinvi.Models.TwitterCredentials("AeMBFSekBw8CiP19URpCeMsMy", "GhfHgUVq6i69VM1PaAQtZdnFH7eVLhlXcL9oAc6hnbnM2nOntf");
            authenticationContext = AuthFlow.InitAuthentication(applicationCredentials);
        }

        public String getAuthorizationURL()
        {
            return authenticationContext.AuthorizationURL;
        }

        public void enterPINCode(String pinCode)
        {
            userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(pinCode, authenticationContext);
            Auth.SetCredentials(userCredentials);

            user = User.GetAuthenticatedUser(userCredentials);
        }

        public IEnumerable<ITweet> getUserHomeTimeLine()
        {
            var homeTimeLineParameter = new HomeTimelineParameters
            {
                MaximumNumberOfTweetsToRetrieve = 100
            };

            return user.GetHomeTimeline(homeTimeLineParameter);
        }

        public void publishTweet(String tweetText)
        {
            var tweet = Tweet.PublishTweet(tweetText);
        }

        public IUser getUserById(int id)
        {
            return User.GetUserFromId(id);
        }

        public IUser getUserByScreenname(String username)
        {
            return User.GetUserFromScreenName(username);
        }

        public IEnumerable<IUser> getFriends()
        {
            return user.GetFriends();
        }

        public IEnumerable<IMessage> getReceivedMessages()
        {
            return user.GetLatestMessagesReceived();
        }

        public IEnumerable<IMessage> getSentMessages()
        {
            return user.GetLatestMessagesSent();
        }

        public Boolean sendMessage(string userIdentifier, string message)
        {
            var messageObj = Message.PublishMessage(message, userIdentifier);
            return messageObj.IsMessagePublished;
        }

        public IEnumerable<ITweet> searchForTweets(string text)
        {
            return Search.SearchTweets(text);
        }

        public IEnumerable<IUser> searchForUsers(string text)
        {
            return Search.SearchUsers(text);
        }
    }
}
