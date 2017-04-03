using System;
namespace Connectome.Twitter.API
{
	/// <summary>
	/// Represent an authentication process for a social media API wrapper
	/// </summary>
	public interface IAuthentication
	{

		/// <summary>
		/// Returns an access token required to authenticate to an API
		/// </summary>
		/// <returns>string representing an access token</returns>
		string getAccessToken();

		/// <summary>
		/// Returns an access token secret required to authenticate to an API
		/// </summary>
		/// <returns>string representing an access token secret</returns>
		string getAccessTokenSecret();

		/// <summary>
		/// Returns a specific setting saved for the specific instance of the app
		/// </summary>
		/// <returns>string representing a value to a setting</returns>
		string getLocalAppSetting(Object config, string key);

		/// <summary>
		/// Conducts neccessary steps to authenticate against an API
		/// </summary>
		void authenticate();
	}
}
