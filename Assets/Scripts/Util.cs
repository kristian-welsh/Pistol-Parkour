namespace Kristian
{
	public class Util 
	{
		private static System.Random random = new System.Random();

		public static T RandomElement<T>(T[] array)
		{
			return array[RandInt(0, array.Length)];
		}

		public static int RandInt(int start, int end)
		{
			return random.Next(start, end);
		}
	}
}