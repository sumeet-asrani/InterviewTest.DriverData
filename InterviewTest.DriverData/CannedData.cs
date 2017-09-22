using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace InterviewTest.DriverData
{
	public static class CannedDrivingData
	{
		private static readonly DateTimeOffset _day = new DateTimeOffset(2016, 10, 13, 0, 0, 0, 0, TimeSpan.Zero);

		// BONUS: What's so great about IReadOnlyCollections?
		public static readonly IReadOnlyCollection<Period> History = new[]
		{
			new Period
			{
				Start = _day + new TimeSpan(0, 0, 0),
				End = _day + new TimeSpan(8, 54, 0),
				AverageSpeed = 0m
			},
			new Period
			{
				Start = _day + new TimeSpan(8, 54, 0),
				End = _day + new TimeSpan(9, 28, 0),
				AverageSpeed = 28m
			},
			new Period
			{
				Start = _day + new TimeSpan(9, 28, 0),
				End = _day + new TimeSpan(9, 35, 0),
				AverageSpeed = 33m
			},
			new Period
			{
				Start = _day + new TimeSpan(9, 50, 0),
				End = _day + new TimeSpan(12, 35, 0),
				AverageSpeed = 25m
			},
			new Period
			{
				Start = _day + new TimeSpan(12, 35, 0),
				End = _day + new TimeSpan(13, 30, 0),
				AverageSpeed = 0m
			},
			new Period
			{
				Start = _day + new TimeSpan(13, 30, 0),
				End = _day + new TimeSpan(19, 12, 0),
				AverageSpeed = 29m
			},
			new Period
			{
				Start = _day + new TimeSpan(19, 12, 0),
				End = _day + new TimeSpan(24, 0, 0),
				AverageSpeed = 0m
			}
		};

		public static List<Period> LoadHistoryData(string fileName)
		{
			//Pass a csv file name to this method
			//File Format is as follows:
			//{Start Time},{End Time},{Average Speed};
			//Refer CannedDataInput.csv in InputFileFormat folder
			//For executing using AnalyseHistoryCommand, pass an argument with the file name after the analyzer type

			List<Period> history = new List<Period>();

			Period period;

			try
			{
				using (var reader = new StreamReader(fileName))
				{
					while (!reader.EndOfStream)
					{
						var line = reader.ReadLine().TrimEnd(';');
						var values = line.Split(',');

						period = new Period();

						period.Start = DateTimeOffset.Parse(values[0], CultureInfo.InvariantCulture);

						period.End = DateTimeOffset.Parse(values[1], CultureInfo.InvariantCulture);

						period.AverageSpeed = Convert.ToDecimal(values[2]);

						history.Add(period);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(string.Format("An error occured while importing data from file - {0}\nException message - {1}", fileName, ex.Message));
			}

			return history;
		}
	}
}
