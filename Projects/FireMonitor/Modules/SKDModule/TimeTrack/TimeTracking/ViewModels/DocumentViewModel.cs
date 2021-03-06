﻿using FiresecAPI.SKD;
using Infrastructure.Common.Windows.ViewModels;

namespace SKDModule.ViewModels
{
	public class DocumentViewModel : BaseViewModel
	{
		public TimeTrackDocument Document { get; private set; }

		public DocumentViewModel(TimeTrackDocument timeTrackDocument)
		{
			Document = timeTrackDocument;
			Update();
		}

		public void Update()
		{
			Name = Document.TimeTrackDocumentType.Name;
			ShortName = Document.TimeTrackDocumentType.ShortName;
			StartDateTime = Document.StartDateTime.ToString("yyyy-MM-dd HH:mm");
			EndDateTime = Document.EndDateTime.ToString("yyyy-MM-dd HH:mm");

			OnPropertyChanged(() => Name);
			OnPropertyChanged(() => ShortName);
			OnPropertyChanged(() => StartDateTime);
			OnPropertyChanged(() => EndDateTime);
		}

		public void Update(TimeTrackDocument timeTrackDocument)
		{
			Document = timeTrackDocument;
			Update();
		}

		public string Name { get; private set; }
		public string ShortName { get; private set; }
		public string StartDateTime { get; private set; }
		public string EndDateTime { get; private set; }
	}
}