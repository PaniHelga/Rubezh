﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Common;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Common.GK;

namespace FireMonitor
{
	public partial class App : Application
	{
		const string SignalId = "{B8150ECC-9433-4535-89AA-5BF6EF631575}";
		const string WaitId = "{358D5240-9A07-4134-9EAF-8D7A54BCA81F}";
		Bootstrapper _bootstrapper;
		bool bootstrapperLoaded = false;
		public static bool IsClosingOnException = false;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			ServerLoadHelper.Load();

#if DEBUG
			//BindingErrorListener.Listen(m => MessageBox.Show(m));
#endif
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			ApplicationService.Closing += new System.ComponentModel.CancelEventHandler(ApplicationService_Closing);

			_bootstrapper = new Bootstrapper();
			using (new DoubleLaunchLocker(SignalId, WaitId, true))
			{
				_bootstrapper.Initialize();
			}
			bootstrapperLoaded = true;
		}

		void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			IsClosingOnException = true;
			Logger.Error(e.ExceptionObject as Exception, "App.CurrentDomain_UnhandledException");

			if (bootstrapperLoaded)
			{
#if RELEASE
				System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
#endif
				Application.Current.MainWindow.Close();
				Application.Current.Shutdown();
			}
			else
			{
				MessageBoxService.ShowError("Во время загрузки программы произошло исключение. Приложение будет закрыто");
			}
		}
		private void ApplicationService_Closing(object sender, CancelEventArgs e)
		{
			GKDBHelper.AddMessage("Выход пользователя из системы");
			foreach (var module in ApplicationService.Modules)
				module.Dispose();
			AlarmPlayerHelper.Dispose();
			ClientSettings.SaveSettings();
			FiresecManager.Disconnect();
			if (RegistryHelper.IsIntegrated)
			{
				if (!IsClosingOnException)
					RegistryHelper.ShutDown();
			}
		}
	}
}