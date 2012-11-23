﻿using System.Resources;
using FiresecAPI;

namespace Firesec
{
	public static class EmptyConfigHelper
	{
		public static OperationResult<Firesec.Models.CoreConfiguration.config> GetCoreConfig()
		{
			string defaultConfigString = Properties.Settings.Default.DefaultConfig;
            if (string.IsNullOrEmpty(defaultConfigString))
                return new OperationResult<Models.CoreConfiguration.config>("Нулевая конфтгурация по умолчанию");
			var result = SerializerHelper.Deserialize<Firesec.Models.CoreConfiguration.config>(defaultConfigString);
			return new OperationResult<Firesec.Models.CoreConfiguration.config>() { Result = result};
		}
	}
}