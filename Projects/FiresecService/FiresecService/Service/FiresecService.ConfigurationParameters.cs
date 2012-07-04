﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiresecAPI;
using System.Threading;
using FiresecService.Processor;
using FiresecAPI.Models;

namespace FiresecService.Service
{
	public partial class FiresecService
	{
		public OperationResult<List<Property>> GetConfigurationParameters(Guid deviceUID)
		{
			var properties = new List<Property>();
			var requestIds = new List<int>();
			int requestId = 0;
			int waitCount = 0;
			var device = ConfigurationCash.DeviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);

			foreach (var property in device.Driver.Properties)
			{
				if (property.IsInternalDeviceParameter)
				{
					FiresecSerializedClient.ExecuteRuntimeDeviceMethod(device.InitialPlaceInTree, "Device$ReadSimpleParam", property.No.ToString(), ref requestId);
					requestIds.Add(requestId);
				}
			}

			while (true)
			{
				var result = FiresecSerializedClient.ExecuteRuntimeDeviceMethod(device.InitialPlaceInTree, "StateConfigQueries", null, ref requestId);
				if (result.HasError)
				{
					return new OperationResult<List<Property>>()
					{
						Result = null,
						HasError = true,
						Error = result.ErrorString
					};
				}
				Firesec.DeviceCustomFunctions.requests requests = SerializerHelper.Deserialize<Firesec.DeviceCustomFunctions.requests>(result.Result);
				if (requests != null)
				{
					foreach (var request in requests.request)
					{
						if (requestIds.Contains(request.id))
						{
							requestIds.Remove(request.id);
							var paramNo = request.param.FirstOrDefault(x => x.name == "ParamNo").value;
							var paramValue = request.param.FirstOrDefault(x => x.name == "ParamValue").value;

							var driverProperty = device.Driver.Properties.FirstOrDefault(x => x.No == paramNo);
							var property = new Property()
							{
								Name = driverProperty.Name,
								Value = paramValue.ToString()
							};
							properties.Add(property);
						}
					}
				}
				if (requestIds.Count == 0)
				{
					break;
				}
				Thread.Sleep(TimeSpan.FromSeconds(1));
				waitCount++;
				if (waitCount > 60)
				{
					return new OperationResult<List<Property>>()
					{
						Result = null,
						HasError = true,
						Error = "Превышено время выполнения запроса"
					};
					break;
				}
			}

			return new OperationResult<List<Property>>()
			{
				Result = properties
			};
		}

		public void SetConfigurationParameters(Guid deviceUID, List<Property> properties)
		{
			var device = ConfigurationCash.DeviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);

			foreach (var property in device.Driver.Properties)
			{
				if (property.IsInternalDeviceParameter)
				{
					int requestId = 0;
					FiresecSerializedClient.ExecuteRuntimeDeviceMethod(device.InitialPlaceInTree, "Device$ReadSimpleParam", property.No.ToString(), ref requestId);
				}
			}
		}
	}
}