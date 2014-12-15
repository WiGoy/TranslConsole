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
	public class TranslGoogle : TranslationEngine
	{
		private const string browser = @"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6";
		private const string reg_result = @"(?<=TRANSLATED_TEXT=').*?(?=')";
		private const string url_translator = "http://translate.google.cn/";
		private Dictionary<string, string> dict_lang = new Dictionary<string, string>()
		{
			{ "chinese", "zh-CN" }, 
			{ "english",  "en" }
		};

		public TranslGoogle(string input, string transl_from, string transl_to)
		{
			this.input = input;
			this.transl_from = transl_from;
			this.transl_to = transl_to;
		}

		/// <summary>
		///	translate input with google engine
		/// </summary>
		/// <param name="input"></param>
		/// <param name="transl_from"></param>
		/// <param name="transl_to"></param>
		/// <returns></returns>
		public override void Translate()
		{
			try
			{
				HttpWebRequest httpRequest = WebRequest.Create(url_translator) as HttpWebRequest;
				httpRequest.ContentType = "application/x-www-form-urlencoded";
				httpRequest.Method = "POST";
				httpRequest.UserAgent = browser;

				///	prepare post data
				string str_params = string.Format(@"hl={0}&ie=utf-8&text={1}&langpair='{2}'|'{3}'", dict_lang[transl_to], HttpUtility.UrlEncode(input), dict_lang[transl_from], dict_lang[transl_to]);
				byte[] byt_params = Encoding.ASCII.GetBytes(str_params);
				using (Stream stream = httpRequest.GetRequestStream())
				{
					stream.Write(byt_params, 0, byt_params.Length);
				}
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
	}
}