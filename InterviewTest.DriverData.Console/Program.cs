using System;
using System.Linq;
using InterviewTest.Commands;

namespace InterviewTest
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var commandName = args[0];
			var commandArguments = args.Skip(1).ToArray();

			switch (commandName)
			{
				case "analyse":
					new AnalyseHistoryCommand(commandArguments).Execute();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			Console.ReadKey();
		}
	}
}

// failing test
// non-failing 
// refactor two similar classes together
