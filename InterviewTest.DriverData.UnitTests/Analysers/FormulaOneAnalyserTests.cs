using System;
using InterviewTest.DriverData.Analysers;
using NUnit.Framework;

namespace InterviewTest.DriverData.UnitTests.Analysers
{
	[TestFixture]
	public class FormulaOneAnalyserTests
	{
		[Test]
		public void ShouldYieldCorrectValues()
		{
			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(10, 3, 0),
				DriverRating = 0.1231m
			};

			var formulaOneAnalyser = new FormulaOneAnalyser();

			formulaOneAnalyser.AnalyzerData = new AnalyzerData() { SpeedLimit = 200m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = formulaOneAnalyser.Analyse(CannedDrivingData.History);

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}
	}
}
