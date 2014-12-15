using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace TranslConsole
{
	public class TranslBaidu : TranslationEngine
	{
		private const string clientID = "4e5nLMnxIEkKVS5fTPvtMUYU";
		private const string reg_result = @"(?<=""dst"":"").*?(?=""}])";
		private const string url_translator = "http://openapi.baidu.com/public/2.0/bmt/translate?";
		private Dictionary<string, string> dict_lang = new Dictionary<string, string>()
		{
			{ "chinese", "zh" }, 
			{ "english",  "en" }
		};

		public TranslBaidu(string input, string transl_from, string transl_to)
		{
			this.input = input;
			this.transl_from = transl_from;
			this.transl_to = transl_to;
		}

		/// <summary>
		///	translate input with baidu engine
		/// </summary>
		/// <param name="input"></param>
		/// <param name="transl_from"></param>
		/// <param name="transl_to"></param>
		/// <returns></returns>
		public override void Translate()
		{
			try
			{
				///	create baidu engine's url
				string url = string.Format("{0}client_id={1}&q={2}&from={3}&to={4}", url_translator, clientID, HttpUtility.UrlEncode(input), dict_lang[transl_from], dict_lang[transl_to]);
				HttpWebRequest httpRequest = WebRequest.Create(url) as HttpWebRequest;
				HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse;

				///	get output from response
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
	}
}