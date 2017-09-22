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

		private string InputFileName;

		private static Dictionary<string, AnalyzerData> analyserDataLookupDict
			= new Dictionary<string, AnalyzerData>
		{
			{ "friendly", new AnalyzerData() { PenaltyFactorForUndocumentedPeriod = 0.5m }  },
			{ "deliveryDriver", new AnalyzerData() { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0), SpeedLimit = 30m, PenaltyFactorForUndocumentedPeriod = 0.5m } },
			{ "formulaOne", new AnalyzerData() { SpeedLimit = 200m, PenaltyFactorForUndocumentedPeriod = 0.5m } },
			{ "getawayDriver", new AnalyzerData() { Start = new TimeSpan(13, 0, 0), End = new TimeSpan(14, 0, 0), SpeedLimit = 80m, PenaltyFactorForUndocumentedPeriod = 0.5m } }

		};

		public AnalyseHistoryCommand(IReadOnlyCollection<string> arguments)
		{
			var analysisType = arguments.ElementAt(0);

			InputFileName = arguments.ElementAt(1);

			//_analyser = AnalyserLookup.GetAnalyser(analysisType);

			_analyser = AnalyserLookup.GetAnalyserFromDict(analysisType);

			_analyser.AnalyzerData = analyserDataLookupDict[analysisType];
		}

		public void Execute()
		{
			var data = CannedDrivingData.LoadHistoryData(InputFileName);

			var analysisUsingInputData = _analyser.Analyse(data);

			var analysis = _analyser.Analyse(CannedDrivingData.History);

			Console.Out.WriteLine($"Analysed period: {analysis.AnalysedDuration:g}");
			Console.Out.WriteLine($"Driver rating: {analysis.DriverRating:P}");
		}
	}
}
