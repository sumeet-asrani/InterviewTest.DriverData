using System;
using System.Collections.Generic;

namespace InterviewTest.DriverData.Analysers
{
	// BONUS: Why internal?
	internal class FormulaOneAnalyser : IAnalyser
	{
		public AnalyzerData AnalyzerData { get; set; }

		public HistoryAnalysis Analyse(IReadOnlyCollection<Period> history)
		{
			throw new NotImplementedException();
		}
	}
}