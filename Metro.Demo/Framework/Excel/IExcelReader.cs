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
	using System.Collections;
	using System.Collections.Generic;

	public interface IExcelReader
	{
		string GetFilename();
		List<string> GetWorksheetNames();
		IEnumerable<IDictionary> GetWorksheet(string worksheetName);
	}
}