namespace Metro.ViewModels
{
	using System;
	using System.Linq;
	using System.Windows.Input;
	using Caliburn.Micro;
	using Metro.Framework;

	public class ShellViewModel
		:Conductor<IScreen>.Collection.OneActive
	{
		public ShellViewModel()
		{
			this.ActivateItemCommand = new DelegateCommand<IScreen>(item =>
			{
				this.ActiveItem = item;
			});
		}

		private ICommand _activateItemCommand;
		public ICommand ActivateItemCommand
		{
			get { return _activateItemCommand; }
			set
			{
				_activateItemCommand = value;
				NotifyOfPropertyChange(() => ActivateItemCommand);
			}
		}

		protected override void OnActivate()
		{
			base.OnActivate();

			IScreen[] screens = new IScreen[]
			{
				new FirstViewModel(),
				new InvoiceGeneratorViewModel{ InvoiceModel = new ClientInvoiceViewModel() }
			};

			screens.ToObservable()
				.OnTimeline(TimeSpan.FromSeconds(0.2))
				.ObserveOnDispatcher()
				.Subscribe(AddScreen);
		}

		private void AddScreen(IScreen screen)
		{
			if (this.ActiveItem == null)
				this.ActiveItem = screen;
			else
				this.Items.Add(screen);
		}
	}
}