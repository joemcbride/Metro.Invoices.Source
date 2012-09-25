namespace Metro
{
	using System;
	using System.Collections.Generic;
	using Caliburn.Micro;
	using Metro.Framework;
	using Metro.ViewModels;
	using Metro.Views;

	public class AppBootstrapper : Bootstrapper<ShellViewModel>
	{
		private SimpleContainer container;

		protected override void Configure()
		{
			container = new SimpleContainer();

			container.RegisterSingleton(typeof(IEventAggregator), null, typeof(EventAggregator));

			container.RegisterPerRequest(typeof(ShellViewModel), null, typeof(ShellViewModel));
			container.RegisterPerRequest(typeof(ShellView), null, typeof(ShellView));
		}

		protected override object GetInstance(Type service, string key)
		{
			return container.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			container.BuildUp(instance);
		}
	}
}