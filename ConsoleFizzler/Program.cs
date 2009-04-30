﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using CommandNames = System.Collections.Generic.KeyValuePair<
          /* key   */ System.Func<ConsoleFizzler.ICommand>,
          /* value */ System.Collections.Generic.IEnumerable<string>>;

namespace ConsoleFizzler
{
    internal static class Program
	{
        internal static int Main(string[] args)
        {
            try
            {
                return Run(args);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Trace.TraceError(e.ToString());
                return 100;
            }
        }

		static int Run(string[] args)
		{
			if (args.Length == 0)
                throw new ApplicationException("Missing command.");

		    var commands = new[] 
            {
	            new CommandNames(() => new SelectCommand(), Aliases("select", "sel")),
	            new CommandNames(() => new ExplainCommand(), Aliases("explain", "describe", "desc")),
	        }
		    .SelectMany(e => e.Value.Select(v => new KeyValuePair<string, Func<ICommand>>(v, e.Key)))
		    .ToDictionary(e => e.Key, e => e.Value);

		    var name = args[0];
		    
            Func<ICommand> command;
            if (!commands.TryGetValue(name, out command))
                throw new ApplicationException("Invalid command.");

		    return command().Run(args.Skip(1).ToArray());
           
		}

        static IEnumerable<string> Aliases(params string[] values)
        {
            return values;
        }
        /*
	    private static void RunUri(string rawuri)
		{
			Console.WriteLine("Please wait...");

			Uri uri = new Uri(rawuri);

			string result = Webclient.DownloadString(uri);

			var document = new HtmlDocument();
			document.LoadHtml(result);

			var generator = new SelectorGenerator<HtmlNode>(new HtmlNodeOps());
			var helper = new HumanReadableSelectorGenerator();

			Parser.Parse(_selector, new SelectorGeneratorTee(generator, helper));
			HtmlNode[] nodes = generator.Selector(Enumerable.Repeat(document.DocumentNode, 1)).ToArray();

			Console.WriteLine(helper.Text);

			foreach (var node in nodes)
			{
				Console.WriteLine(node.OuterHtml);
			}
		}*/
	}
}