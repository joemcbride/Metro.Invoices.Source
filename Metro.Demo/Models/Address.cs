namespace Metro.Common.Model
{
	using Caliburn.Micro;

	public class Address : PropertyChangedBase
	{
		private string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				NotifyOfPropertyChange(() => Name);
			}
		}

		private string _line1;
		public string Line1
		{
			get { return _line1; }
			set
			{
				_line1 = value;
				NotifyOfPropertyChange(() => Line1);
			}
		}

		private string _line2;
		public string Line2
		{
			get { return _line2; }
			set
			{
				_line2 = value;
				NotifyOfPropertyChange(() => Line2);
			}
		}

		private string _phone;
		public string Phone
		{
			get { return _phone; }
			set
			{
				_phone = value;
				NotifyOfPropertyChange(() => Phone);
			}
		}
	}
}