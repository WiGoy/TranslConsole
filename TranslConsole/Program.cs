using System;

namespace TranslConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			string input = "这是一个测试用例。我们会测试一条略微复杂的语句，看看哪个引擎的翻译质量比较好，并记录其翻译耗时。";
			string transl_from = "chinese";
			string transl_to = "english";
			string translator = "BAIDU";
			Console.WriteLine("Input:  " + input);

			Translator transl = new Translator();
			transl.AutoTranslate(input, transl_from, transl_to);

			Console.ReadKey();
		}
	}
}
