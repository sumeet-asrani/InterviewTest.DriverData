using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTest.DriverData
{
	public static class Extensions
	{
		public static decimal WeightedAverage<T>(this IEnumerable<T> records, Func<T, decimal> value, Func<T, decimal> weight)
		{
			var weightedValueSum = records.Sum(x => value(x) * weight(x));
			var weightSum = records.Sum(x => weight(x));

			if (weightSum != 0)
				return weightedValueSum / weightSum;
			else
				throw new DivideByZeroException();
		}
	}
}
