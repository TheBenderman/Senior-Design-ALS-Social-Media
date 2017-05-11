using CoreTweet;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Connectome.Twitter.API
{
	public class TwitterAuthenticator : IAuthentication
	{
		#region Variables
		private static TwitterAuthenticator AuthenticatorInstance;
		private OAuth.OAuthSession Session;
		private Tokens Tokens; // This is where the OAuth tokens are stored
        private string screenName;
		#endregion

		#region Preamble
		// Singleton object for the API
		public static TwitterAuthenticator Instance
		{
			get
			{
				if (AuthenticatorInstance == null)
					return AuthenticatorInstance = new TwitterAuthenticator();
				else
					return AuthenticatorInstance;
			}
		}

		public TwitterAuthenticator()
		{
			authenticate();
		}

		public Tokens getTokens()
		{
			return this.Tokens;
		}
		#endregion

		#region Authentication
		// Return the access token that was generated during authentication
		public string getAccessToken()
		{
			if (!string.IsNullOrEmpty(Tokens.AccessToken))
			{
				return Tokens.AccessToken;
			}

			throw new Exception("Unable to get the tokens for user.");
		}

        public string getLoggedInUserScreenName() {
            return screenName;
        }

		// Return the access token that was generated during authentication
		public string getAccessTokenSecret()
		{
			if (!string.IsNullOrEmpty(Tokens.AccessTokenSecret))
			{
				return Tokens.AccessTokenSecret;
			}

			throw new Exception("Unable to get the tokens for user.");
		}


		public string getLocalAppSetting(Object conf, string key)
		{
			Configuration config = (Configuration)conf;
			KeyValueConfigurationElement element = config.AppSettings.Settings[key];
			if (element != null)
			{
				string value = element.Value;
				if (!string.IsNullOrEmpty(value))
					return value;
			}
			return string.Empty;
		}

		public void authenticate()
		{
			ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

			Configuration config = null;
			try
			{
				// Open the config file for the dll
				string exeConfigPath = this.GetType().Assembly.Location;
				config = ConfigurationManager.OpenExeConfiguration(exeConfigPath);
			}
			catch (Exception ex)
			{
				// handle error
			}

			if (config != null)
			{
				// Grab the keys from the config file
				string consumerKey = getLocalAppSetting(config, "consumerKey");
				string consumerSecret = getLocalAppSetting(config, "consumerSecret");

				// Store the keys for the app, these should probably be stored in a file
				Session = OAuth.Authorize(consumerKey, consumerSecret);
			}
		}

		// Politely ignore the below code :-)
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

		// Return the url that the user needs to authorize twitter for the application
		public String getAuthorizationURL()
		{
			// Make sure the user has a valid session
			if (Session != null)
			{
				return Session.AuthorizeUri.ToString();
			}

			throw new Exception("Unable to get session for user.");
		}


		// Set the tokens. Used by the able to set the tokens from a previous session
		public void setTokens(string accessToken, string accessTokenSecret)
		{
			Tokens = Tokens.Create("AeMBFSekBw8CiP19URpCeMsMy", "GhfHgUVq6i69VM1PaAQtZdnFH7eVLhlXcL9oAc6hnbnM2nOntf", accessToken, accessTokenSecret);
		}

		public Boolean enterPinCode(string pinCode)
		{
			Tokens = Session.GetTokens(pinCode); // get the oauth tokens for the entered pin code

            if (Tokens != null)
            {
                screenName = Tokens.ScreenName.ToString();
                return true;
            }
            else
                throw new Exception("Unable to get the tokens for user.");
		}
		#endregion

	}
}
