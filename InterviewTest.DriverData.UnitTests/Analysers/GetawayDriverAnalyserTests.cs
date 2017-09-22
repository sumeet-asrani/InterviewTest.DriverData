using System;
using InterviewTest.DriverData.Analysers;
using NUnit.Framework;
using System.Collections.Generic;

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

		[Test]
		public void ShouldYieldCorrectValuesWithPenalty()
		{
			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = TimeSpan.FromHours(1),
				DriverRating = 0.1813m
			};

			var getawayDriverAnalyser = new GetawayDriverAnalyser();

			getawayDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(13, 0, 0), End = new TimeSpan(14, 0, 0), SpeedLimit = 80m, PenaltyFactorForUndocumentedPeriod = 0.5m };

			var actualResult = getawayDriverAnalyser.Analyse(CannedDrivingData.History);

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

			var getawayDriverAnalyser = new GetawayDriverAnalyser();

			getawayDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(13, 0, 0), End = new TimeSpan(14, 0, 0), SpeedLimit = 80m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = getawayDriverAnalyser.Analyse(new List<Period>());

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

			var getawayDriverAnalyser = new GetawayDriverAnalyser();

			getawayDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(13, 0, 0), End = new TimeSpan(14, 0, 0), SpeedLimit = 80m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = getawayDriverAnalyser.Analyse(null);

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
					Start = new DateTimeOffset(2016, 10, 13, 13, 0, 0, 0, TimeSpan.Zero),
					End = new DateTimeOffset(2016, 10, 13, 14, 0, 0, 0, TimeSpan.Zero),
					AverageSpeed = 80m
				}
			};

			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(1, 0, 0),
				DriverRating = 1m
			};

			var getawayDriverAnalyser = new GetawayDriverAnalyser();

			getawayDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(13, 0, 0), End = new TimeSpan(14, 0, 0), SpeedLimit = 80m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = getawayDriverAnalyser.Analyse(data);

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
					Start = new DateTimeOffset(2016, 10, 13, 13, 0, 0, 0, TimeSpan.Zero),
					End = new DateTimeOffset(2016, 10, 13, 14, 0, 0, 0, TimeSpan.Zero),
					AverageSpeed = 100m
				}
			};

			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(1, 0, 0),
				DriverRating = 1m
			};

			var getawayDriverAnalyser = new GetawayDriverAnalyser();

			getawayDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(13, 0, 0), End = new TimeSpan(14, 0, 0), SpeedLimit = 80m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = getawayDriverAnalyser.Analyse(data);

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}

		[Test]
		public void ShouldAnalyseFileDataAndReturnDriverRating()
		{
			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = TimeSpan.FromHours(1),
				DriverRating = 0.1813m
			};

			var getawayDriverAnalyser = new GetawayDriverAnalyser();

			getawayDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(13, 0, 0), End = new TimeSpan(14, 0, 0), SpeedLimit = 80m, PenaltyFactorForUndocumentedPeriod = 1m };

			var data = CannedDrivingData.LoadHistoryData(@"D:\FundsLibrary\InterviewTest.DriverData\InputFileFormat\CannedDataInput.csv");

			var actualResult = getawayDriverAnalyser.Analyse(data);

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}
	}
}
