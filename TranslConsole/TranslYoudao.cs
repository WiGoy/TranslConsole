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
	public class TranslYoudao : TranslationEngine
	{
		private const string keyFrom = "whoscored";
		private const string key = "654173872";
		private const string reg_result = @"(?<=""translation"":\["").*?(?=""\])";
		private const string url_translator = "http://fanyi.youdao.com/openapi.do?";

		public TranslYoudao(string input, string transl_from, string transl_to)
		{
			this.input = input;
			this.transl_from = transl_from;
			this.transl_to = transl_to;
		}

		/// <summary>
		///	translate input with youdao engine
		/// </summary>
		/// <param name="input"></param>
		/// <param name="transl_from"></param>
		/// <param name="transl_to"></param>
		/// <returns></returns>
		public override void Translate()
		{
			try
			{
				///	create youdao engine's url
				string url = string.Format("{0}keyfrom={1}&key={2}&type=data&doctype=json&version=1.1&only=translate&q={3}", url_translator, keyFrom, key, HttpUtility.UrlEncode(input));
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