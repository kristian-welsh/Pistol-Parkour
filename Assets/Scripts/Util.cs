namespace Kristian
{
	public class Util 
	{
		public static T RandomElement<T>(T[] array)
		{
			return array[RandInt(0, array.Length)];
		}

		public static int RandInt(int start, int end)
		{
			System.Random r = new System.Random();
			return r.Next(start, end);
		}
	}
}