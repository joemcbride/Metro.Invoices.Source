namespace Metro.Framework.Excel
{
	using System;
	using System.Windows.Resources;
	using System.IO;
	using System.Collections.Generic;

	public class UnZipper
	{
		private Stream stream;

		public UnZipper(Stream zipFileStream)
		{
			this.stream = zipFileStream;
		}

		public Stream GetFileStream(string filename)
		{
			Uri fileUri = new Uri(filename, UriKind.Relative);
			StreamResourceInfo info = new StreamResourceInfo(this.stream, null);
			if (this.stream is System.IO.FileStream)
				this.stream.Seek(0, SeekOrigin.Begin);
			StreamResourceInfo stream = System.Windows.Application.GetResourceStream(info, fileUri);
			if (stream != null)
				return stream.Stream;
			return null;
		}

		public IEnumerable<string> GetFileNamesInZip()
		{
			BinaryReader reader = new BinaryReader(stream);
			stream.Seek(0, SeekOrigin.Begin);
			string name = null;
			List<string> names = new List<string>();
			while (ParseFileHeader(reader, out name))
			{
				names.Add(name);
			}
			return names;
		}

		private static bool ParseFileHeader(BinaryReader reader, out string filename)
		{
			filename = null;
			if (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				int headerSignature = reader.ReadInt32();
				if (headerSignature == 67324752) //ggggggrrrrrrrrrrrrrrrrr
				{
					reader.BaseStream.Seek(2, SeekOrigin.Current);

					short genPurposeFlag = reader.ReadInt16();
					if (((((int)genPurposeFlag) & 0x08) != 0))
						return false;
					reader.BaseStream.Seek(10, SeekOrigin.Current);
					//short method = reader.ReadInt16(); //Unused
					//short lastModTime = reader.ReadInt16(); //Unused
					//short lastModDate = reader.ReadInt16(); //Unused
					//int crc32 = reader.ReadInt32(); //Unused
					int compressedSize = reader.ReadInt32();
					int unCompressedSize = reader.ReadInt32();
					short fileNameLenght = reader.ReadInt16();
					short extraFieldLenght = reader.ReadInt16();
					filename = new string(reader.ReadChars(fileNameLenght));
					if (string.IsNullOrEmpty(filename))
						return false;

					reader.BaseStream.Seek(extraFieldLenght + compressedSize, SeekOrigin.Current);
					if (unCompressedSize == 0)
						return ParseFileHeader(reader, out filename);
					else
						return true;
				}
			}
			return false;
		}
	}
}