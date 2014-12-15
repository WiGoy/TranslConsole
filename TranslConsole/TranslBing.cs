using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;


namespace TranslConsole
{
	public class TranslBing : TranslationEngine
	{
		private string accessToken = "";
		private const string browser = @"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6";
		private const string clientID = "WiGoy";
		private const string clientSecret = "YUHK8beVEyRMcVFuivskOcwJ3gUzlwzhtMiBhTVDBMk=";
		private const string grantType = "client_credentials";
		private const string reg_accessToken = @"(?<=""access_token"":"").*?(?="")";
		private const string reg_result = @"(?<=/"">).*?(?=</string>)";
		private const string url_scope = "http://api.microsofttranslator.com";
		private const string url_token = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
		private const string url_translator = "http://api.microsofttranslator.com/v2/Http.svc/Translate?";
		private Dictionary<string, string> dict_lang = new Dictionary<string, string>()
		{
			{ "chinese", "zh-CHS" }, 
			{ "english",  "en" }
		};

		public TranslBing(string input, string transl_from, string transl_to)
		{
			this.input = input;
			this.transl_from = transl_from;
			this.transl_to = transl_to;
		}

		/// <summary>
		/// translate input with bing engine
		/// </summary>
		/// <param name="input"></param>
		/// <param name="transl_from"></param>
		/// <param name="transl_to"></param>
		/// <returns></returns>
		public override void Translate()
		{
			try
			{
				//	create bing engine's url
				string url = string.Format("{0}text={1}&From={2}&To={3}", url_translator, HttpUtility.UrlEncode(input), dict_lang[transl_from], dict_lang[transl_to]);
				//	get auth token
				AccessToken();
				string authToken = "Bearer " + accessToken;
				HttpWebRequest httpRequest = WebRequest.Create(url) as HttpWebRequest;
				httpRequest.Headers.Add("Authorization", authToken);
				HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse;

				//	get output from response
				using (StreamReader sr = new StreamReader(httpResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")))
				{
					Regex reg = new Regex(reg_result);
					output = reg.Match(sr.ReadToEnd()).Value;
				}
			}
			catch (ThreadAbortException) { }
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		/// <summary>
		///	get access token
		/// </summary>
		/// <returns></returns>
		private void AccessToken()
		{
			try
			{
				HttpWebRequest httpRequest = WebRequest.Create(url_token) as HttpWebRequest;
				httpRequest.ContentType = "application/x-www-form-urlencoded";
				httpRequest.Method = "POST";

				///	prepare post data
				string str_params = string.Format(@"client_id={0}&client_secret={1}&grant_type={2}&scope={3}", HttpUtility.UrlEncode(clientID), HttpUtility.UrlEncode(clientSecret), grantType, url_scope);
				byte[] byt_params = Encoding.ASCII.GetBytes(str_params);
				using (Stream stream = httpRequest.GetRequestStream())
				{
					stream.Write(byt_params, 0, byt_params.Length);
				}
				HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse;

				//	get access token from response
				using (StreamReader sr = new StreamReader(httpResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")))
				{
					Regex reg = new Regex(reg_accessToken);
					accessToken = reg.Match(sr.ReadToEnd()).Value;
				}
			}
			catch (ThreadAbortException) { }
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}
	}
}
