﻿using System;
using System.ComponentModel;
using System.Windows;
using FiresecClient;
using Infrastructure.Common;
using Infrastructure.Common.Theme;
using Infrastructure.Common.Windows;

namespace FireAdministrator
{
	public partial class App : Application
	{
		private const string SignalId = "{8599F876-2147-4694-A822-B24E36D7F92F}";
		private const string WaitId = "{07193C2C-CE04-478C-880A-49AB239C6550}";
		private Bootstrapper _bootstrapper;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			string fileName;
			PatchManager.Patch();
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			ApplicationService.Closing += new System.ComponentModel.CancelEventHandler(ApplicationService_Closing);
			ThemeHelper.LoadThemeFromRegister();
#if DEBUG
			bool trace = true;
			BindingErrorListener.Listen(m => { if (trace) MessageBox.Show(m); });
#endif
			_bootstrapper = new Bootstrapper();
			using (new DoubleLaunchLocker(SignalId, WaitId))
				_bootstrapper.Initialize();
			try
			{
				if (e.Args != null && e.Args.Length > 0)
				{
					fileName = e.Args[0];
					FileConfigurationHelper.LoadFromFile(fileName);
				}
			}
			catch { }
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			MessageBoxService.ShowException(e.ExceptionObject as Exception);
			if (MessageBoxService.ShowQuestion("В результате работы программы произошло исключение. Приложение будет закрыто. Вы хотите сохранить конфигурацию в файл") == MessageBoxResult.Yes)
			{
				FileConfigurationHelper.SaveToFile();
			}
		}
		private void ApplicationService_Closing(object sender, CancelEventArgs e)
		{
			if (e.Cancel)
				return;

			if (ApplicationService.Modules != null)
				foreach (var module in ApplicationService.Modules)
					module.Dispose();
			FiresecManager.Disconnect();
		}
	}
}