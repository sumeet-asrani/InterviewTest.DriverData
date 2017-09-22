using System.Collections.Generic;

namespace InterviewTest.DriverData.Analysers
{
	public interface IAnalyser
	{
		AnalyzerData AnalyzerData { get; set; }

		HistoryAnalysis Analyse(IReadOnlyCollection<Period> history);
	}
}
