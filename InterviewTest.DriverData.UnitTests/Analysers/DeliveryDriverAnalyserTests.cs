using System;
using InterviewTest.DriverData.Analysers;
using NUnit.Framework;

namespace InterviewTest.DriverData.UnitTests.Analysers
{
	[TestFixture]
	public class DeliveryDriverAnalyserTests
	{
		[Test]
		public void ShouldYieldCorrectValues()
		{
			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(7, 45, 0),
				DriverRating = 0.7638m
			};

			var deliveryDriverAnalyser = new DeliveryDriverAnalyser();

			deliveryDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0), SpeedLimit = 30m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = deliveryDriverAnalyser.Analyse(CannedDrivingData.History);

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}
	}
}
