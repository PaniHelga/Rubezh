﻿using System.Collections.Generic;
using FiresecAPI.Models;
using Microsoft.Practices.Prism.Events;
using XFiresecAPI;

namespace Infrastructure.Events
{
	public class GetFilteredGKArchiveCompletedEvent : CompositePresentationEvent<IEnumerable<JournalItem>>
	{
	}
}