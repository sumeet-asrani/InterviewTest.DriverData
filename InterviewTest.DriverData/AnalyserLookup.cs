using System;
using InterviewTest.DriverData.Analysers;
using System.Collections.Generic;

namespace InterviewTest.DriverData
{
	public static class AnalyserLookup
	{
		private static readonly Dictionary<string, IAnalyser> analyserLookupDict
			= new Dictionary<string, IAnalyser>
		{
			{ "friendly", new FriendlyAnalyser() },
			{ "deliveryDriver", new DeliveryDriverAnalyser() },
			{ "formulaOne", new FormulaOneAnalyser() },
            { "getawayDriver", new GetawayDriverAnalyser() }
		};

		public static IAnalyser GetAnalyserFromDict(string type)
		{
			try
			{
				return analyserLookupDict[type];
			}
			catch (KeyNotFoundException)
			{
				throw new ArgumentOutOfRangeException(nameof(type), type, "Unrecognised analyser type");
			}
		}

		public static IAnalyser GetAnalyser(string type)
		{
			switch (type)
			{
				case "friendly":
					return new FriendlyAnalyser();
				case "deliveryDriver":
					return new DeliveryDriverAnalyser();
				case "formulaOne":
					return new FormulaOneAnalyser();
				case "getawayDriver":
					return new GetawayDriverAnalyser();

				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, "Unrecognised analyser type");
			}
		}
	}
}
