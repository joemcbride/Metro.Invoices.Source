namespace Metro.Common.Model
{
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Linq;
	using Caliburn.Micro;

	public class InvoiceItem : PropertyChangedBase
	{
		private string _description;
		public string Description
		{
			get { return _description; }
			set
			{
				_description = value;
				NotifyOfPropertyChange(() => Description);
			}
		}

		private double _hours;
		public double Hours
		{
			get { return _hours; }
			set
			{
				_hours = value;
				NotifyOfPropertyChange(() => Hours);
				NotifyOfPropertyChange(() => Amount);
			}
		}

		private decimal _rate;
		public decimal Rate
		{
			get { return _rate; }
			set
			{
				_rate = value;
				NotifyOfPropertyChange(() => Rate);
				NotifyOfPropertyChange(() => Amount);
			}
		}

		public decimal Amount
		{
			get
			{
				return (decimal)this.Hours * Rate;
			}
		}
	}

	public class InvoiceItemCollection : BindableCollection<InvoiceItem>
	{
		public decimal AmountDue
		{
			get
			{
				return this.Sum(x => x.Amount);
			}
		}

		protected override void InsertItem(int index, InvoiceItem item)
		{
			base.InsertItem(index, item);
			item.PropertyChanged += Item_PropertyChanged;
		}

		protected override void RemoveItem(int index)
		{
			InvoiceItem item = this[index];
			item.PropertyChanged -= Item_PropertyChanged;

			base.RemoveItem(index);
		}

		private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			NotifyOfPropertyChange("AmountDue");
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnCollectionChanged(e);

			NotifyOfPropertyChange("AmountDue");
		}
	}
}