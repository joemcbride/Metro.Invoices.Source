namespace Metro.ViewModels
{
	using System.IO;
	using System.Windows.Controls;
	using System.Windows.Input;
	using Caliburn.Micro;
	using Metro.Common.Model;
	using Metro.Framework;
	using Metro.Messages;

	public class ClientInvoiceViewModel : Screen, IHandle<InvoiceGeneratorMessage>
	{
		public ClientInvoiceViewModel()
		{
			this.DisplayName = "client invoice";
			this.DisplayMcBrideSoftwareLogo = true;

			this.Invoice = new Invoice();

			this.Invoice.Subject = "Consulting work for [client].";
			this.Invoice.Notes = "Please make all checks payable to [Company Name].  Thank you for your business!";

			Address company = new Address();
			company.Name = "McBride Software Solutions, Inc.";
			company.Line1 = "One McBride Way";
			company.Line2 = "Salt Lake City, UT 84102";
			company.Phone = "(801) 555-5555";

			Address client = new Address();
			client.Name = "Microsoft Corporation";
			client.Line1 = "One Microsoft Way";
			client.Line2 = "Redmond, WA 98073";
			client.Phone = "";

			this.Invoice.Company = company;
			this.Invoice.Client = client;

			IoC.Get<IEventAggregator>().Subscribe(this);

			this.RemoveLineItemCommand = new DelegateCommand<InvoiceItem>(
				item =>
				{
					this.RemoveLineItem(item);
				});

			this.AddLineItem();
		}

		private Invoice _invoice;
		public Invoice Invoice
		{
			get { return _invoice; }
			set
			{
				_invoice = value;
				NotifyOfPropertyChange(() => Invoice);
			}
		}

		private byte[] _imageSource;
		public byte[] ImageSource
		{
			get { return _imageSource; }
			set
			{
				_imageSource = value;
				NotifyOfPropertyChange(() => ImageSource);
			}
		}

		private bool _displayMcBrideSoftwareLogo;
		public bool DisplayMcBrideSoftwareLogo
		{
			get { return _displayMcBrideSoftwareLogo; }
			set
			{
				_displayMcBrideSoftwareLogo = value;
				NotifyOfPropertyChange(() => DisplayMcBrideSoftwareLogo);
			}
		}

		public ICommand RemoveLineItemCommand
		{
			get;
			set;
		}

		public void AddLineItem()
		{
			this.Invoice.Items.Add(new InvoiceItem { Description = "Consulting", Hours = 8.0 });
		}

		public void RemoveLineItem(InvoiceItem item)
		{
			if (item != null && this.Invoice.Items.Contains(item))
			{
				this.Invoice.Items.Remove(item);
			}
		}

		public void SelectLogo()
		{
			OpenFileDialog dialog = new OpenFileDialog();

			if (dialog.ShowDialog() == true)
			{
				this.DisplayMcBrideSoftwareLogo = false;

				using (FileStream fileStream = dialog.File.OpenRead())
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						CopyStream(fileStream, memoryStream);
						this.ImageSource = memoryStream.ToArray();
					}
				}
			}
		}

		public void Handle(InvoiceGeneratorMessage message)
		{
			this.Invoice.Items.Clear();

			var items = InvoiceFromExcelFile.GetInvoiceItems(message.File);

			this.Invoice.Items.AddRange(items);
		}

		private static void CopyStream(Stream input, Stream output)
		{
			byte[] b = new byte[32768];
			int r;
			while ((r = input.Read(b, 0, b.Length)) > 0)
				output.Write(b, 0, r);
		}
	}
}