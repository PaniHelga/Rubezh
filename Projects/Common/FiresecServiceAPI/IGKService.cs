﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFiresecAPI;

namespace FiresecAPI
{
	public interface IGKService
	{
		void GKWriteConfiguration(XDevice device);
		void GKSetNewConfiguration(XDeviceConfiguration deviceConfiguration);
		void GKExecuteDeviceCommand(XDevice device, XStateBit stateType);
		void GKReset(XBase xBase);
		void GKResetFire1(XZone zone);
		void GKResetFire2(XZone zone);
		void GKSetAutomaticRegime(XBase xBase);
		void GKSetManualRegime(XBase xBase);
		void GKSetIgnoreRegime(XBase xBase);
		void GKTurnOn(XBase xBase);
		void GKTurnOnNow(XBase xBase);
		void GKTurnOff(XBase xBase);
		void GKTurnOffNow(XBase xBase);
		void GKStop(XBase xBase);
	}
}