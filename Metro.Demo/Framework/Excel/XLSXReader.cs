/// Author:					Walter Ferrari
/// Created:				2010-01-18
/// ref:                    http://www.snello.it
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

namespace Metro.Framework.Excel
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Xml;
	using System.Xml.Linq;

	public class XLSXReader : IExcelReader
	{
		FileInfo theFile;
		internal static XNamespace excelNamespace = XNamespace.Get("http://schemas.openxmlformats.org/spreadsheetml/2006/main");
		internal static XNamespace excelRelationshipsNamepace = XNamespace.Get("http://schemas.openxmlformats.org/officeDocument/2006/relationships");

		const string sharedStringsMarker = @"xl/sharedStrings.xml";
		const string worksheetsMarker = @"/xl/worksheets/";
		const string worksheetMarker = "/xl/worksheets/sheet{0}.xml";
		const string workbooksMarker = @"xl/workbook.xml";

		private int worksheetsCount = 0;
		private XElement sharedStringsElement;

		private Dictionary<int, string> sharedStrings;

		public XLSXReader(FileInfo _file)
		{
			theFile = _file;
		}

		public string GetFilename()
		{
			return theFile.Name;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="worksheetID"></param>
		/// <returns></returns>
		public XElement GetWorksheet(int worksheetID)
		{
			string worksheetMarker = String.Format(CultureInfo.CurrentCulture, "xl/worksheets/sheet{0}.xml", worksheetID);

			return GetXLSXPart(worksheetMarker);
		}

		public List<string> GetWorksheetNames()
		{
			XElement worksheetElement = GetXLSXPart(workbooksMarker);

			IEnumerable<XElement> workSheetItems = from s in worksheetElement.Descendants(XLSXReader.excelNamespace + "sheet")
												   select s;

			IEnumerable<XAttribute> workSheetAttrs = workSheetItems.Attributes("name");

			List<string> worksheets = new List<string>();

			foreach (XAttribute xattr in workSheetAttrs)
			{
				worksheets.Add(xattr.Value);
			}

			// a good place to get the sharedStrings included in the xlsx file
			ParseSharedStrings(GetSharedStringPart());

			return worksheets;

		}

		private XElement GetSharedStringPart()
		{
			return GetXLSXPart(sharedStringsMarker);
		}

		/// <summary>
		/// Extracts from the xslx file the part specified with partMarker
		/// </summary>
		/// <param name="partMarker"></param>
		/// <returns></returns>
		private XElement GetXLSXPart(string partMarker)
		{
			UnZipper unzip;
			Stream s = theFile.OpenRead();
			unzip = new UnZipper(s);
			XElement partElement = null;

			foreach (string filename in unzip.GetFileNamesInZip())
			{
				Stream partStream = unzip.GetFileStream(filename);
				if (filename == partMarker)
				{
					partElement = XElement.Load(XmlReader.Create(partStream));
					partStream.Close();
					return partElement;
				}
			}
			return null;
		}


		public int GetWorksheetIndex(string itemSelected)
		{
			if (string.IsNullOrEmpty(itemSelected) == true)
				return 0;

			List<string> worksheets = GetWorksheetNames();

			if (worksheets == null)
				return 0;

			int count = 0;
			foreach (string worksheetName in worksheets)
			{
				count += 1;
				if (worksheetName == itemSelected)
				{
					return count;
				}
			}
			return 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="WorksheetIdex"></param>
		/// <returns></returns>
		public IEnumerable<IDictionary> GetWorksheet(string itemSelected)
		{
			int worksheetIdex = GetWorksheetIndex(itemSelected);

			if (worksheetIdex <= 0)
				yield break;

			XElement wsSelectedElement = GetWorksheet(worksheetIdex);
			if (wsSelectedElement == null)
				yield break;

			IEnumerable<XElement> rowsExcel = from row in wsSelectedElement.Descendants(XLSXReader.excelNamespace + "row")
											  select row;

			if (rowsExcel == null)
				yield break;

			foreach (XElement row in rowsExcel)
			{
				var dict = new Dictionary<string, object>();
				IEnumerable<XElement> cellsRow = row.Elements(XLSXReader.excelNamespace + "c");
				foreach (XElement cell in cellsRow)
				{
					if (cell.HasElements == true)
					{
						string cellValue = cell.Element(XLSXReader.excelNamespace + "v").Value;
						if (cell.Attribute("t") != null)
						{
							if (cell.Attribute("t").Value == "s")
							{
								cellValue = sharedStrings[Convert.ToInt32(cellValue)];
							}
						}

						dict[cell.Attribute("r").Value.Substring(0, 1)] = cellValue as Object;
					}
				}
				yield return dict;
			}
		}

		private void ParseSharedStrings(XElement SharedStringsElement)
		{
			sharedStrings = new Dictionary<int, string>();
			IEnumerable<XElement> sharedStringsElements = from s in SharedStringsElement.Descendants(excelNamespace + "t")
														  select s;
			int Counter = 0;
			foreach (XElement sharedString in sharedStringsElements)
			{
				sharedStrings.Add(Counter, sharedString.Value);
				Counter++;
			}
			return;
		}
	}
}