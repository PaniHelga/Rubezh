﻿using System;
using FiresecAPI.Models;
using Infrastructure.Common.Validation;
using Infrastructure.Events;

namespace VideoModule.Validation
{
	class VideoValidationError : ObjectValidationError<Camera, ShowVideoEvent, Guid>
	{
		public VideoValidationError(Camera camera, string error, ValidationErrorLevel level)
			: base(camera, error, level)
		{
		}

		public override string Module
		{
			get { return "Video"; }
		}
		protected override Guid Key
		{
			get { return Object.UID; }
		}
		public override string Source
		{
			get { return Object.Name; }
		}
		public override string Address
		{
			get { return Object.Address; }
		}
		public override string ImageSource
		{
			get { return "/Controls;component/Images/Video1.png"; }
		}
	}
}
