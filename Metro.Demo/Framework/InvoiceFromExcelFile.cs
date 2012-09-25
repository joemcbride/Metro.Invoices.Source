namespace Metro.Framework
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Metro.Common.Model;
	using Metro.Framework.Excel;

	public class InvoiceFromExcelFile
	{
		public static InvoiceItemCollection GetInvoiceItems(FileInfo file)
		{
			InvoiceItemCollection items = new InvoiceItemCollection();
			List<string> worksheets = null;

			IExcelReader _excelReader = new XLSXReader(file);

			try
			{
				worksheets = _excelReader.GetWorksheetNames();
			}
			catch (Exception exc)
			{
				return items;
			}

			foreach (var worksheet in worksheets)
			{
				var datasource = _excelReader.GetWorksheet(worksheet);

				if (datasource != null)
				{
					var rows = datasource.Skip(2).Take(7);

					foreach (IDictionary row in rows)
					{
						// uncomment to loop over each cell in the row and print out it's data
						//foreach (DictionaryEntry cell in row)
						//{
						//    System.Diagnostics.Debug.WriteLine(" {0} | {1}", cell.Key, cell.Value);
						//}

						//System.Diagnostics.Debug.WriteLine("");

						var item = CreateInvoiceItem(row);
						if (item.Hours > 0)
						{
							var foundItem = items.Where(x => x.Description == item.Description).FirstOrDefault();

							if (foundItem != null)
								foundItem.Hours += item.Hours;
							else
								items.Add(item);
						}
					}
				}
			}

			return items;
		}

		private static InvoiceItem CreateInvoiceItem(IDictionary row)
		{
			InvoiceItem item = new InvoiceItem();

			if (row.Contains("A"))
				item.Description = ExcelExtensions.ParseDate(row["A"].ToString()).ToShortDateString();

			if (row.Contains("D"))
				item.Hours = double.Parse(row["D"].ToString());

			return item;
		}
	}
}