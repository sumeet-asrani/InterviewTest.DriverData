using System.Collections.Generic;
using System.Linq;

namespace InterviewTest.DriverData.Analysers
{
	// BONUS: Why internal?
	internal class FriendlyAnalyser : IAnalyser
	{
		public AnalyzerData AnalyzerData { get; set; }

		public HistoryAnalysis Analyse(IReadOnlyCollection<Period> history)
		{
			return new HistoryAnalysis
			{
				AnalysedDuration = history.Last().End - history.First().Start,
				DriverRating = 1m
			};
		}
	}
}