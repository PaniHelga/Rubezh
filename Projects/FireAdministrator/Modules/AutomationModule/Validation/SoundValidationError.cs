﻿using System;
using AutomationModule.Events;
using FiresecAPI.Automation;
using Infrastructure.Common.Validation;

namespace AutomationModule.Validation
{
	class SoundValidationError : ObjectValidationError<AutomationSound, ShowAutomationSoundsEvent, Guid>
	{
		public SoundValidationError(AutomationSound sound, string error, ValidationErrorLevel level)
			: base(sound, error, level)
		{
		}

		public override string Module
		{
			get { return "AutomationModule"; }
		}
		protected override Guid Key
		{
			get { return Object.Uid; }
		}
		public override string Address
		{
			get { return ""; }
		}
		public override string Source
		{
			get { return Object.Name; }
		}
		public override string ImageSource
		{
			get { return "/Controls;component/Images/Music.png"; }
		}
	}
}