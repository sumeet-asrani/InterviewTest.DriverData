using System;
using InterviewTest.DriverData.Analysers;
using NUnit.Framework;
using System.Collections.Generic;

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

		[Test]
		public void ShouldYieldCorrectValuesWithPenalty()
		{
			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(7, 45, 0),
				DriverRating = 0.3819m
			};

			var deliveryDriverAnalyser = new DeliveryDriverAnalyser();

			deliveryDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0), SpeedLimit = 30m, PenaltyFactorForUndocumentedPeriod = 0.5m };

			var actualResult = deliveryDriverAnalyser.Analyse(CannedDrivingData.History);

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

			var deliveryDriverAnalyser = new DeliveryDriverAnalyser();

			deliveryDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0), SpeedLimit = 30m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = deliveryDriverAnalyser.Analyse(new List<Period>());

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

			var deliveryDriverAnalyser = new DeliveryDriverAnalyser();

			deliveryDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0), SpeedLimit = 30m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = deliveryDriverAnalyser.Analyse(null);

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
					Start = new DateTimeOffset(2016, 10, 13, 9, 0, 0, 0, TimeSpan.Zero),
					End = new DateTimeOffset(2016, 10, 13, 17, 0, 0, 0, TimeSpan.Zero),
					AverageSpeed = 30m
				}
			};

			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(8, 0, 0),
				DriverRating = 1m
			};

			var deliveryDriverAnalyser = new DeliveryDriverAnalyser();

			deliveryDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0), SpeedLimit = 30m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = deliveryDriverAnalyser.Analyse(data);

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}

		[Test]
		public void ShouldAnalyseWholePeriodWithGreaterThanMaxSpeedAndReturn0ForDriverRating()
		{
			var data = new[]
			{
				new Period
				{
					Start = new DateTimeOffset(2016, 10, 13, 9, 0, 0, 0, TimeSpan.Zero),
					End = new DateTimeOffset(2016, 10, 13, 17, 0, 0, 0, TimeSpan.Zero),
					AverageSpeed = 60m
				}
			};

			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(8, 0, 0),
				DriverRating = 0m
			};

			var deliveryDriverAnalyser = new DeliveryDriverAnalyser();

			deliveryDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0), SpeedLimit = 30m, PenaltyFactorForUndocumentedPeriod = 1m };

			var actualResult = deliveryDriverAnalyser.Analyse(data);

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}

		[Test]
		public void ShouldAnalyseFileDataAndReturnDriverRating()
		{
			var expectedResult = new HistoryAnalysis
			{
				AnalysedDuration = new TimeSpan(7, 45, 0),
				DriverRating = 0.7638m
			};

			var deliveryDriverAnalyser = new DeliveryDriverAnalyser();

			deliveryDriverAnalyser.AnalyzerData = new AnalyzerData() { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0), SpeedLimit = 30m, PenaltyFactorForUndocumentedPeriod = 1m };

			var data = CannedDrivingData.LoadHistoryData(@"D:\FundsLibrary\InterviewTest.DriverData\InputFileFormat\CannedDataInput.csv");

			var actualResult = deliveryDriverAnalyser.Analyse(data);

			Assert.That(actualResult.AnalysedDuration, Is.EqualTo(expectedResult.AnalysedDuration));
			Assert.That(actualResult.DriverRating, Is.EqualTo(expectedResult.DriverRating).Within(0.001m));
		}
	}
}
