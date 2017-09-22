using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTest.DriverData.Analysers
{
	// BONUS: Why internal?
	internal class FormulaOneAnalyser : IAnalyser
	{
		public AnalyzerData AnalyzerData { get; set; }

		public HistoryAnalysis Analyse(IReadOnlyCollection<Period> history)
		{
			List<AnalysisData> ratingsList = new List<AnalysisData>();

			double unDocumentedPeriodDuration = 0;
			double duration = 0;
			decimal rating = 0;

			//If the passed list is empty or null, return
			if (history == null || history.Count == 0)
			{
				return new HistoryAnalysis
				{
					AnalysedDuration = new TimeSpan(0, 0, 0),
					DriverRating = 0
				};
			}

			//Get the history list having non-zero speeds
			var histortyList = history.ToList();
			var startIndex = histortyList.IndexOf(history.First(h => h.AverageSpeed > 0));
			var endIndex = histortyList.IndexOf(history.Last(h => h.AverageSpeed > 0));

			for (int i = startIndex; i <= endIndex; i++)
			{
				//For the intermediate records, check if the there is difference between the start of current and end of previous
				//If yes, we have undocumented periods
				if (i > startIndex && histortyList[i].Start > histortyList[i - 1].End)
				{
					duration = (history.ElementAt(i).Start - history.ElementAt(i - 1).End).TotalMinutes;

					unDocumentedPeriodDuration += duration;

					ratingsList.Add(new AnalysisData() { StartTime = histortyList[i - 1].End.TimeOfDay, EndTime = histortyList[i].Start.TimeOfDay, AnalysedDuration = (decimal)duration, DriverRating = 0 });
				}

				rating = (histortyList[i].AverageSpeed > AnalyzerData.SpeedLimit) ? 1 : (histortyList[i].AverageSpeed / AnalyzerData.SpeedLimit);

				duration = (histortyList[i].End - histortyList[i].Start).TotalMinutes;

				ratingsList.Add(new AnalysisData() { StartTime = histortyList[i].Start.TimeOfDay, EndTime = histortyList[i].End.TimeOfDay, AnalysedDuration = (decimal)duration, DriverRating = rating });
			}

			if (!ratingsList.Any())
			{
				return new HistoryAnalysis
				{
					AnalysedDuration = new TimeSpan(0, 0, 0),
					DriverRating = 0
				};
			}

			var weightedAvgRating = ratingsList.WeightedAverage(r => r.DriverRating, r => r.AnalysedDuration);

			//Impose Penalty for undocumented periods
			if (unDocumentedPeriodDuration > 0)
				weightedAvgRating *= AnalyzerData.PenaltyFactorForUndocumentedPeriod;

			return new HistoryAnalysis
			{
				AnalysedDuration = ratingsList.Last().EndTime - ratingsList.First().StartTime - new TimeSpan(0, (int)unDocumentedPeriodDuration, 0),
				DriverRating = weightedAvgRating
			};
		}
	}
}