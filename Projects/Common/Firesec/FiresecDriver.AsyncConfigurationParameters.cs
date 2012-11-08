﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiresecAPI;
using FiresecAPI.Models;
using Common;
using System.Threading;
using System.Diagnostics;

namespace Firesec
{
    public partial class FiresecDriver
    {
        public OperationResult<bool> BeginGetConfigurationParameters(Device device)
        {
            var devicePropertyRequest = new DevicePropertyRequest(device);
            foreach (var propertyNo in devicePropertyRequest.PropertyNos)
            {
                int requestId = 0;
                var result = FiresecSerializedClient.ExecuteRuntimeDeviceMethod(device.PlaceInTree, "Device$ReadSimpleParam", propertyNo.ToString(), ref requestId);
                if (result.HasError)
                {
                    return new OperationResult<bool>(result.Error);
                }
				Trace.WriteLine(requestId.ToString() + " " + device.PlaceInTree);
                devicePropertyRequest.RequestIds.Add(requestId);
            }
            DevicePropertyRequests.Add(devicePropertyRequest);

            if (AUParametersThread == null)
            {
                AUParametersThread = new Thread(AUParametersThreadRun);
                AUParametersThread.Start();
            }
            return new OperationResult<bool>() { Result = true };
        }

        List<DevicePropertyRequest> DevicePropertyRequests = new List<DevicePropertyRequest>();
        Thread AUParametersThread;

        void AUParametersThreadRun()
        {
            while (DevicePropertyRequests.Count > 0)
            {
                DevicePropertyRequests.RemoveAll(x=>x.IsDeleting);
				var devicePropertyRequests = DevicePropertyRequests.ToList();
				Trace.WriteLine("devicePropertyRequests.Count = " + devicePropertyRequests.Count().ToString());

                    int stateConfigQueriesRequestId = 0;
					var result = FiresecSerializedClient.ExecuteRuntimeDeviceMethod("", "StateConfigQueries", null, ref stateConfigQueriesRequestId);
					if (result == null || result.HasError || result.Result == null)
                    {
						Thread.Sleep(TimeSpan.FromSeconds(1));
                        continue;
                    }

                    Firesec.Models.DeviceCustomFunctions.requests requests = SerializerHelper.Deserialize<Firesec.Models.DeviceCustomFunctions.requests>(result.Result);
					if (requests != null && requests.request.Count() > 0)
					{
						foreach (var request in requests.request)
						{
							foreach (var devicePropertyRequest in devicePropertyRequests)
							{
								if (devicePropertyRequest.RequestIds.Contains(request.id))
								{
									devicePropertyRequest.RequestIds.Remove(request.id);
									int propertyNo = request.param.FirstOrDefault(x => x.name == "ParamNo").value;
									int propertyValue = request.param.FirstOrDefault(x => x.name == "ParamValue").value;

									foreach (var driverProperty in devicePropertyRequest.Device.Driver.Properties.FindAll(x => x.No == propertyNo))
									{
										if (devicePropertyRequest.Properties.FirstOrDefault(x => x.Name == driverProperty.Name) == null)
										{
											devicePropertyRequest.Properties.Add(CreateProperty(propertyValue, driverProperty));
										}
									}
								}
							}

							foreach (var devicePropertyRequest in devicePropertyRequests)
							{
								if (devicePropertyRequest.RequestIds.Count == 0)
								{
									foreach (var resultProperty in devicePropertyRequest.Properties)
									{
										var property = devicePropertyRequest.Device.Properties.FirstOrDefault(x => x.Name == resultProperty.Name);
										if (property == null)
										{
											property = new Property()
											{
												Name = resultProperty.Name
											};
											devicePropertyRequest.Device.Properties.Add(property);
										}
										property.Value = resultProperty.Value;
									}
									devicePropertyRequest.Device.OnAUParametersChanged();
								}
							}
						}
					}
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            AUParametersThread = null;
        }

        private static Property CreateProperty(int paramValue, DriverProperty driverProperty)
        {
            var offsetParamValue = paramValue;

            var highByteValue = paramValue / 256;
            var lowByteValue = paramValue - highByteValue * 256;

            if (driverProperty.HighByte)
                offsetParamValue = highByteValue;
            else if (driverProperty.LargeValue)
                offsetParamValue = paramValue;
            else
                offsetParamValue = lowByteValue;

            if (driverProperty.MinBit > 0)
            {
                byte byteOffsetParamValue = (byte)offsetParamValue;
                byteOffsetParamValue = (byte)(byteOffsetParamValue >> driverProperty.MinBit);
                byteOffsetParamValue = (byte)(byteOffsetParamValue << driverProperty.MinBit);
                offsetParamValue = byteOffsetParamValue;
            }

            if (driverProperty.MaxBit > 0)
            {
                byte byteOffsetParamValue = (byte)offsetParamValue;
                byteOffsetParamValue = (byte)(byteOffsetParamValue << 8 - driverProperty.MaxBit);
                byteOffsetParamValue = (byte)(byteOffsetParamValue >> 8 - driverProperty.MaxBit);
                offsetParamValue = byteOffsetParamValue;
            }

            if (driverProperty.BitOffset > 0)
            {
                offsetParamValue = offsetParamValue >> driverProperty.BitOffset;
            }

            if (driverProperty.Name == "Задержка включения МРО, сек")
            {
                offsetParamValue = offsetParamValue * 5;
            }

            var property = new Property()
            {
                Name = driverProperty.Name,
                Value = offsetParamValue.ToString()
            };

            return property;
        }
    }

    class DevicePropertyRequest
    {
        public DevicePropertyRequest(Device device)
        {
            Device = device;
            Properties = new List<Property>();
            PropertyNos = new HashSet<int>();
            RequestIds = new List<int>();

            foreach (var property in device.Driver.Properties)
            {
                if (property.IsAUParameter)
                {
                    PropertyNos.Add(property.No);
                }
            }
        }

        public Device Device { get; set; }
        public List<Property> Properties { get; set; }
        public HashSet<int> PropertyNos { get; set; }
        public List<int> RequestIds { get; set; }

        DateTime StartDateTime = DateTime.Now;
        public bool IsDeleting
        {
            get
            {
				var timeoutExpired = DateTime.Now - StartDateTime > TimeSpan.FromMinutes(5) || RequestIds.Count == 0;
				if (timeoutExpired)
				{
					Device.OnAUParametersChanged();
				}
				return timeoutExpired;
            }
        }
    }
}