namespace Metro.Converters
{
	using System;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Media;
	
	public class AlternatingRowBackgroundConverter : IValueConverter
	{
		public Brush NormalBrush { get; set; }
		public Brush AlternateBrush { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Grid element = (Grid)value;
			element.Loaded += Element_Loaded;

			return NormalBrush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private void Element_Loaded(object sender, RoutedEventArgs e)
		{
			Grid element = sender as Grid;

			var parent = element.FindParent(x => x is ItemsControl) as ItemsControl;

			if (parent != null)
			{
				var container = parent.ItemContainerGenerator.ContainerFromItem(element.DataContext);

				if (container != null)
				{
					int index = parent.ItemContainerGenerator.IndexFromContainer(container);

					if (index % 2 != 0)
						element.Background = AlternateBrush;
				}
			}
		}
	}
}