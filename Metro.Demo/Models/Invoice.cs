namespace Metro.Common.Model
{
	using System;
	using Caliburn.Micro;

	public class Invoice : PropertyChangedBase
	{
		public Invoice()
		{
			this.Company = new Address();
			this.Client = new Address();
			this.IssuedDate = DateTime.Today;
			this.DueDate = DateTime.Today.AddDays(30.0);
			this.Items = new InvoiceItemCollection();
		}

		private Address _company;
		public Address Company
		{
			get { return _company; }
			set
			{
				_company = value;
				NotifyOfPropertyChange(() => Company);
			}
		}

		private Address _client;
		public Address Client
		{
			get { return _client; }
			set
			{
				_client = value;
				NotifyOfPropertyChange(() => Client);
			}
		}

		private long _invoiceID;
		public long InvoiceID
		{
			get { return _invoiceID; }
			set
			{
				_invoiceID = value;
				NotifyOfPropertyChange(() => InvoiceID);
			}
		}

		private DateTime _issuedDate;
		public DateTime IssuedDate
		{
			get { return _issuedDate; }
			set
			{
				_issuedDate = value;
				NotifyOfPropertyChange(() => IssuedDate);
				NotifyOfPropertyChange(() => Net);
			}
		}

		private DateTime _dueDate;
		public DateTime DueDate
		{
			get { return _dueDate; }
			set
			{
				_dueDate = value;
				NotifyOfPropertyChange(() => DueDate);
				NotifyOfPropertyChange(() => Net);
			}
		}

		public string Net
		{
			get
			{
				var result = this.DueDate - this.IssuedDate;

				return String.Format("Net {0}", result.TotalDays);
			}
		}

		private string _subject;
		public string Subject
		{
			get { return _subject; }
			set
			{
				_subject = value;
				NotifyOfPropertyChange(() => Subject);
			}
		}

		private string _notes;
		public string Notes
		{
			get { return _notes; }
			set
			{
				_notes = value;
				NotifyOfPropertyChange(() => Notes);
			}
		}

		private InvoiceItemCollection _items;
		public InvoiceItemCollection Items
		{
			get { return _items; }
			set
			{
				_items = value;
				NotifyOfPropertyChange(() => Items);
			}
		}
	}
}