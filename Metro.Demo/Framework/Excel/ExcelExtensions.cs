namespace Metro.Framework.Excel
{
	using System;

	public static class ExcelExtensions
	{
		public static DateTime EPOCH = new DateTime(1900, 1, 1);

		public static DateTime ParseDate(string stringDate)
		{
			return EPOCH.AddDays(Double.Parse(stringDate) - 2);
		}
	}
}