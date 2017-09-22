using System;
using System.Collections.Generic;
using System.Linq;
using InterviewTest.DriverData;
using InterviewTest.DriverData.Analysers;

namespace InterviewTest.Commands
{
	public class AnalyseHistoryCommand
	{
		// BONUS: What's great about readonly?
		private readonly IAnalyser _analyser;

		private static Dictionary<string, AnalyzerData> analyserDataLookupDict
			= new Dictionary<string, AnalyzerData>
		{
			{ "friendly", new AnalyzerData() { }  },
			{ "deliveryDriver", new AnalyzerData() { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0), SpeedLimit = 30m } },
			{ "formulaOne", new AnalyzerData() { SpeedLimit = 200m } },
			{ "getawayDriver", new AnalyzerData() { Start = new TimeSpan(13, 0, 0), End = new TimeSpan(14, 0, 0), SpeedLimit = 80m } }

		};

		public AnalyseHistoryCommand(IReadOnlyCollection<string> arguments)
		{
			var analysisType = arguments.Single();

			_analyser = AnalyserLookup.GetAnalyser(analysisType);

			_analyser.AnalyzerData = analyserDataLookupDict[analysisType];
		}

		public void Execute()
		{
			var analysis = _analyser.Analyse(CannedDrivingData.History);

			Console.Out.WriteLine($"Analysed period: {analysis.AnalysedDuration:g}");
			Console.Out.WriteLine($"Driver rating: {analysis.DriverRating:P}");
		}
	}
}
