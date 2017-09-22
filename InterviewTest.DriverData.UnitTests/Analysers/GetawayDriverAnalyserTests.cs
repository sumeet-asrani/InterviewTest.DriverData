using System;
using InterviewTest.DriverData.Analysers;
using NUnit.Framework;

namespace InterviewTest.DriverData.UnitTests.Analysers
{
	[TestFixture]
	public class GetawayDriverAnalyserTests
	{
		[Test]
		public void ShouldYieldCorrectValues()
		{
			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = TimeSpan.FromHours(1),
				DriverRating = 0.1813m
			};

			var getawayDriverAnalyser = new GetawayDriverAnalyser();

			getawayDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(13, 0, 0), End = new TimeSpan(14, 0, 0), SpeedLimit = 80m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = getawayDriverAnalyser.Analyse(CannedDrivingData.History);

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}
	}
}
