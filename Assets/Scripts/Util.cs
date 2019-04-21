using System.Collections.Generic;
using UnityEngine;

namespace Kristian
{
	/* Provides global functions to use across the application
	 * the intent was to hang all global functions here, not just random object generators
	 */
	public class Util 
	{
		private static System.Random random = new System.Random();

		public static T RandomElement<T>(List<T> list)
		{
			return RandomElement<T>(list.ToArray());
		}

		public static T RandomElement<T>(T[] array)
		{
			return array[RandInt(0, array.Length)];
		}

		public static int RandInt(int start, int end)
		{
			return random.Next(start, end);
		}

		public static bool RandBool(int percentChanceTrue = 50)
		{
			double fraction = ((double)percentChanceTrue) / 100;
			return random.NextDouble() < fraction;
		}
	}
}