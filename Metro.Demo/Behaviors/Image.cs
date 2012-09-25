namespace Metro.Behaviors
{
	using System.IO;
	using System.Windows;
	using System.Windows.Media.Imaging;
	using SWC = System.Windows.Controls;

	public class Image
	{
		public static readonly DependencyProperty SetSourceProperty =
			DependencyProperty.RegisterAttached(
				"SetSource",
				typeof(byte[]),
				typeof(Image),
				new PropertyMetadata(null, OnSetSourceChanged));

		public static byte[] GetSetSource(SWC.Image target)
		{
			return (byte[])target.GetValue(SetSourceProperty);
		}

		public static void SetSetSource(SWC.Image target, byte[] value)
		{
			target.SetValue(SetSourceProperty, value);
		}

		private static void OnSetSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			OnSetSourceChanged((SWC.Image)o, (byte[])e.OldValue, (byte[])e.NewValue);
		}

		private static void OnSetSourceChanged(SWC.Image image, byte[] oldValue, byte[] newValue)
		{
			if (newValue != null && newValue.Length > 0)
			{
				Stream stream = new MemoryStream(newValue);

				BitmapImage bitmap = new BitmapImage();
				bitmap.SetSource(stream);

				image.Source = bitmap;
			}
			else
			{
				image.Source = null;
			}
		}
	}
}