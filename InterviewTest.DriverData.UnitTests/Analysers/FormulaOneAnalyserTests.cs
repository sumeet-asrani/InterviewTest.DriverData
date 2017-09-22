using System;
using InterviewTest.DriverData.Analysers;
using NUnit.Framework;
using System.Collections.Generic;

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

		[Test]
		public void ShouldYieldCorrectValuesWithPenalty()
		{
			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(10, 3, 0),
				DriverRating = 0.0615m
			};

			var formulaOneAnalyser = new FormulaOneAnalyser();

			formulaOneAnalyser.AnalyzerData = new AnalyzerData() { SpeedLimit = 200m, PenaltyFactorForUndocumentedPeriod = 0.5m };

			var actualResult = formulaOneAnalyser.Analyse(CannedDrivingData.History);

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}

		[Test]
		public void ShouldYieldCorrectValuesWithEmptyRecords()
		{
			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(0, 0, 0),
				DriverRating = 0m
			};

			var formulaOneAnalyser = new FormulaOneAnalyser();

			formulaOneAnalyser.AnalyzerData = new AnalyzerData() { SpeedLimit = 200m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = formulaOneAnalyser.Analyse(new List<Period>());

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}

		[Test]
		public void ShouldYieldCorrectValuesWithNullList()
		{
			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(0, 0, 0),
				DriverRating = 0m
			};

			var formulaOneAnalyser = new FormulaOneAnalyser();

			formulaOneAnalyser.AnalyzerData = new AnalyzerData() { SpeedLimit = 200m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = formulaOneAnalyser.Analyse(null);

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}

		[Test]
		public void ShouldAnalyseWholePeriodWithMaxSpeedAndReturn1ForDriverRating()
		{
			var data = new[]
			{
				new Period
				{
					Start = new DateTimeOffset(2016, 10, 13, 10, 0, 0, 0, TimeSpan.Zero),
					End = new DateTimeOffset(2016, 10, 13, 12, 0, 0, 0, TimeSpan.Zero),
					AverageSpeed = 200m
				}
			};

			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(2, 0, 0),
				DriverRating = 1m
			};

			var formulaOneAnalyser = new FormulaOneAnalyser();

			formulaOneAnalyser.AnalyzerData = new AnalyzerData() { SpeedLimit = 200m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = formulaOneAnalyser.Analyse(data);

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}

		[Test]
		public void ShouldAnalyseWholePeriodWithGreaterThanMaxSpeedAndReturn1ForDriverRating()
		{
			var data = new[]
			{
				new Period
				{
					Start = new DateTimeOffset(2016, 10, 13, 10, 0, 0, 0, TimeSpan.Zero),
					End = new DateTimeOffset(2016, 10, 13, 12, 0, 0, 0, TimeSpan.Zero),
					AverageSpeed = 400m
				}
			};

			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(2, 0, 0),
				DriverRating = 1m
			};

			var formulaOneAnalyser = new FormulaOneAnalyser();

			formulaOneAnalyser.AnalyzerData = new AnalyzerData() { SpeedLimit = 200m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = formulaOneAnalyser.Analyse(data);

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}

		[Test]
		public void ShouldAnalyseFileDataAndReturnDriverRating()
		{
			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(10, 3, 0),
				DriverRating = 0.1231m
			};

			var formulaOneAnalyser = new FormulaOneAnalyser();

			formulaOneAnalyser.AnalyzerData = new AnalyzerData() { SpeedLimit = 200m, PenaltyFactorForUndocumentedPeriod = 1m };

			var data = CannedDrivingData.LoadHistoryData(@"D:\FundsLibrary\InterviewTest.DriverData\InputFileFormat\CannedDataInput.csv");

			var actualResult = formulaOneAnalyser.Analyse(data);

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}
	}
}
