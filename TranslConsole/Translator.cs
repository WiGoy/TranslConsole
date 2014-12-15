using System;
using System.Diagnostics;
using System.Threading;

namespace TranslConsole
{
	public abstract class TranslationEngine
	{
		public string input;
		public string output = "";
		public string transl_from;
		public string transl_to;

		public abstract void Translate();
	}

	public class Translator
	{
		private const string translBaidu = "BAIDU";
		private const string translBing = "BING";
		private const string translGoogle = "GOOGLE";
		private const string translYoudao = "YOUDAO";
		private Stopwatch watch_transl = new Stopwatch();

		/// <summary>
		///	using all four translation engines
		/// </summary>
		/// <param name="input"></param>
		/// <param name="transl_from"></param>
		/// <param name="transl_to"></param>
		public void Translate(string input, string transl_from, string transl_to)
		{
			//	using baidu engine
			TranslationEngine translEng = new TranslBaidu(input, transl_from, transl_to);
			watch_transl.Start();
			translEng.Translate();
			watch_transl.Stop();
			Console.WriteLine("Baidu:  " + translEng.output + "(" + +watch_transl.Elapsed.TotalSeconds + "s)");

			//	using bing engine
			translEng = new TranslBing(input, transl_from, transl_to);
			watch_transl.Restart();
			translEng.Translate();
			watch_transl.Stop();
			Console.WriteLine("Bing:   " + translEng.output + "(" + +watch_transl.Elapsed.TotalSeconds + "s)");

			//	using google engine
			translEng = new TranslGoogle(input, transl_from, transl_to);
			watch_transl.Restart();
			translEng.Translate();
			watch_transl.Stop();
			Console.WriteLine("Google: " + translEng.output + "(" + +watch_transl.Elapsed.TotalSeconds + "s)");

			//	using youdao engine
			translEng = new TranslYoudao(input, transl_from, transl_to);
			watch_transl.Restart();
			translEng.Translate();
			watch_transl.Stop();
			Console.WriteLine("Youdao: " + translEng.output + "(" + +watch_transl.Elapsed.TotalSeconds + "s)");
		}

		/// <summary>
		///	using the chosen translation engine
		/// </summary>
		/// <param name="input"></param>
		/// <param name="transl_from"></param>
		/// <param name="transl_to"></param>
		/// <param name="translator"></param>
		public void Translate(string input, string transl_from, string transl_to, string translator)
		{
			switch (translator)
			{
				case translBaidu:
					//	using baidu engine
					TranslationEngine translEng_baidu = new TranslBaidu(input, transl_from, transl_to);
					watch_transl.Start();
					translEng_baidu.Translate();
					watch_transl.Stop();
					Console.WriteLine("Baidu:  " + translEng_baidu.output + "(" + +watch_transl.Elapsed.TotalSeconds + "s)");
					break;

				case translBing:
					//	using bing engine
					TranslationEngine translEng_bing = new TranslBing(input, transl_from, transl_to);
					watch_transl.Start();
					translEng_bing.Translate();
					watch_transl.Stop();
					Console.WriteLine("Bing:   " + translEng_bing.output + "(" + +watch_transl.Elapsed.TotalSeconds + "s)");
					break;

				case translGoogle:
					//	using google engine
					TranslationEngine translEng_google = new TranslGoogle(input, transl_from, transl_to);
					watch_transl.Start();
					translEng_google.Translate();
					watch_transl.Stop();
					Console.WriteLine("Google: " + translEng_google.output + "(" + +watch_transl.Elapsed.TotalSeconds + "s)");
					break;

				case translYoudao:
					//	using youdao engine
					TranslationEngine translEng_youdao = new TranslYoudao(input, transl_from, transl_to);
					watch_transl.Start();
					translEng_youdao.Translate();
					watch_transl.Stop();
					Console.WriteLine("Youdao: " + translEng_youdao.output + "(" + +watch_transl.Elapsed.TotalSeconds + "s)");
					break;
			}
		}

		/// <summary>
		///	auto choosing the fastest translator
		/// </summary>
		/// <param name="input"></param>
		/// <param name="transl_from"></param>
		/// <param name="transl_to"></param>
		public void AutoTranslate(string input, string transl_from, string transl_to)
		{
			string output = "";
			bool b_translEnd = false;
			TranslationEngine translEng_baidu = new TranslBaidu(input, transl_from, transl_to);
			TranslationEngine translEng_bing = new TranslBing(input, transl_from, transl_to);
			TranslationEngine translEng_google = new TranslGoogle(input, transl_from, transl_to);
			TranslationEngine translEng_youdao = new TranslYoudao(input, transl_from, transl_to);
			Thread thread_baidu = new Thread(new ThreadStart(translEng_baidu.Translate));
			Thread thread_bing = new Thread(new ThreadStart(translEng_bing.Translate));
			Thread thread_google = new Thread(new ThreadStart(translEng_google.Translate));
			Thread thread_youdao = new Thread(new ThreadStart(translEng_youdao.Translate));

			thread_baidu.Start();
			thread_bing.Start();
			thread_google.Start();
			thread_youdao.Start();
			watch_transl.Start();

			while (true)
			{
				if (!thread_baidu.IsAlive)
				{
					watch_transl.Stop();
					output = "Baidu:  " + translEng_baidu.output + "(" + +watch_transl.Elapsed.TotalSeconds + "s)";
					b_translEnd = true;
				}
				else if (!thread_bing.IsAlive)
				{
					watch_transl.Stop();
					output = "Bing:   " + translEng_bing.output + "(" + +watch_transl.Elapsed.TotalSeconds + "s)";
					b_translEnd = true;
				}
				else if (!thread_google.IsAlive)
				{
					watch_transl.Stop();
					output = "Google: " + translEng_google.output + "(" + +watch_transl.Elapsed.TotalSeconds + "s)";
					b_translEnd = true;
				}
				else if (!thread_youdao.IsAlive)
				{
					watch_transl.Stop();
					output = "Youdao: " + translEng_youdao.output + "(" + +watch_transl.Elapsed.TotalSeconds + "s)";
					b_translEnd = true;
				}

				if (b_translEnd)
				{
					thread_baidu.Abort();
					thread_bing.Abort();
					thread_google.Abort();
					thread_youdao.Abort();
					break;
				}
			}

			Console.WriteLine(output);
		}
	}
}