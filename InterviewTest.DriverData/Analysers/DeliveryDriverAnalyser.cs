using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTest.DriverData.Analysers
{
	// BONUS: Why internal?
	internal class DeliveryDriverAnalyser : IAnalyser
	{
		public AnalyzerData AnalyzerData { get; set; }

		public HistoryAnalysis Analyse(IReadOnlyCollection<Period> history)
		{
			List<AnalysisData> ratingsList = new List<AnalysisData>();

			AnalysisData tempAnalysisDataObj;
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

			for (int i = 0; i < history.Count; i++)
			{
				//Ignore the records that fall compeletely outside the permitted time values
				if (history.ElementAt(i).Start.TimeOfDay > AnalyzerData.End || history.ElementAt(i).End.TimeOfDay < AnalyzerData.Start)
					continue;

				//For the first record, check if it starts after the permitted start time. if yes, we have undocumented periods
				if (i == 0 && history.ElementAt(i).Start.TimeOfDay > AnalyzerData.Start)
				{
					duration = (history.ElementAt(i).Start.TimeOfDay - AnalyzerData.Start).TotalMinutes;

					unDocumentedPeriodDuration += duration;

					ratingsList.Add(new AnalysisData() { StartTime = AnalyzerData.Start, EndTime = history.ElementAt(i).Start.TimeOfDay, AnalysedDuration = (decimal)duration, DriverRating = 0 });
				}
				//For the last record, check if it ends before the permitted end time. if yes, we have undocumented periods
				else if (i == history.Count - 1 && history.ElementAt(i).End.TimeOfDay < AnalyzerData.End)
				{
					duration = (AnalyzerData.End - history.ElementAt(i).End.TimeOfDay).TotalMinutes;

					unDocumentedPeriodDuration += duration;

					ratingsList.Add(new AnalysisData() { StartTime = history.ElementAt(i).End.TimeOfDay, EndTime = AnalyzerData.End, AnalysedDuration = (decimal)duration, DriverRating = 0 });
				}
				//For the intermediate records, check if the there is difference between the start of current and end of previous
				//If yes, we have undocumented periods
				if (i > 0 && history.ElementAt(i).Start > history.ElementAt(i - 1).End)
				{
					duration = (history.ElementAt(i).Start - history.ElementAt(i - 1).End).TotalMinutes;

					unDocumentedPeriodDuration += duration;

					ratingsList.Add(new AnalysisData() { StartTime = history.ElementAt(i - 1).End.TimeOfDay, EndTime = history.ElementAt(i).Start.TimeOfDay, AnalysedDuration = (decimal)duration, DriverRating = 0 });
				}

				//Get the average speed for the record and compute the rating
				rating = (history.ElementAt(i).AverageSpeed > AnalyzerData.SpeedLimit) ? 0 : (history.ElementAt(i).AverageSpeed / AnalyzerData.SpeedLimit);

				//Assign the current record to a temp variable
				tempAnalysisDataObj = new AnalysisData() { StartTime = history.ElementAt(i).Start.TimeOfDay, EndTime = history.ElementAt(i).End.TimeOfDay, DriverRating = rating };

				//Update the start time for the record if it starts before the permitted start time
				if (history.ElementAt(i).Start.TimeOfDay < AnalyzerData.Start)
					tempAnalysisDataObj.StartTime = AnalyzerData.Start;

				//Update the end time for the record if it ends after the permitted end time
				if (history.ElementAt(i).End.TimeOfDay > AnalyzerData.End)
					tempAnalysisDataObj.EndTime = AnalyzerData.End;

				//Get the duration for the current record
				tempAnalysisDataObj.AnalysedDuration = (decimal)(tempAnalysisDataObj.EndTime - tempAnalysisDataObj.StartTime).TotalMinutes;

				ratingsList.Add(tempAnalysisDataObj);
			}

			//If no data is considered for the ratings, return empty object
			if (!ratingsList.Any())
			{
				return new HistoryAnalysis
				{
					AnalysedDuration = new TimeSpan(0, 0, 0),
					DriverRating = 0
				};
			}

			//Calculate the average rating for the entries considered
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