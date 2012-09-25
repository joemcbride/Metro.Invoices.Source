namespace Metro.ViewModels
{
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Printing;
	using Caliburn.Micro;
	using Metro.Messages;

	public class InvoiceGeneratorViewModel : Screen
	{
		public InvoiceGeneratorViewModel()
		{
			this.DisplayName = "billing";
		}

		private Screen _invoiceModel;
		public Screen InvoiceModel
		{
			get { return _invoiceModel; }
			set
			{
				_invoiceModel = value;
				NotifyOfPropertyChange(() => InvoiceModel);
			}
		}

		public void ImportExcel()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == true)
			{
				IoC.Get<IEventAggregator>().Publish(new InvoiceGeneratorMessage { File = dialog.File });
			}
		}

		public void Print()
		{
			PrintDocument document = new PrintDocument();
			document.PrintPage += Document_PrintPage;
			document.Print("Invoice");
		}

		private void Document_PrintPage(object sender, PrintPageEventArgs e)
		{
			e.PageVisual = this.InvoiceModel.GetView(null) as UIElement;

			e.HasMorePages = false;
		}
	}
}