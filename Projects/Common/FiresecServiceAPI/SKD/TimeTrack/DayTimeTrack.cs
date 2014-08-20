﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FiresecAPI.SKD
{
	[DataContract]
	public class DayTimeTrack
	{
		public DayTimeTrack()
		{
			PlannedTimeTrackParts = new List<TimeTrackPart>();
			RealTimeTrackParts = new List<TimeTrackPart>();
			DocumentTrackParts = new List<TimeTrackPart>();
			CombinedTimeTrackParts = new List<TimeTrackPart>();
			Documents = new List<TimeTrackDocument>();
		}

		public DayTimeTrack(string error)
			: this()
		{
			Error = error;
		}

		[DataMember]
		public DateTime Date { get; set; }

		[DataMember]
		public ShortEmployee ShortEmployee { get; set; }

		[DataMember]
		public Guid EmployeeUID { get; set; }

		[DataMember]
		public List<TimeTrackPart> PlannedTimeTrackParts { get; set; }

		[DataMember]
		public List<TimeTrackPart> RealTimeTrackParts { get; set; }

		[DataMember]
		public List<TimeTrackPart> DocumentTrackParts { get; set; }

		[DataMember]
		public List<TimeTrackPart> CombinedTimeTrackParts { get; set; }

		[DataMember]
		public bool IsIgnoreHoliday { get; set; }

		[DataMember]
		public bool IsOnlyFirstEnter { get; set; }

		[DataMember]
		public TimeSpan AllowedLate { get; set; }

		[DataMember]
		public TimeSpan AllowedEarlyLeave { get; set; }

		[DataMember]
		public TimeSpan SlideTime { get; set; }

		[DataMember]
		public bool IsHoliday { get; set; }

		[DataMember]
		public int HolidayReduction { get; set; }

		[DataMember]
		public TimeTrackType TimeTrackType { get; set; }

		[DataMember]
		public List<TimeTrackDocument> Documents { get; set; }

		[DataMember]
		public HolidaySettings HolidaySettings { get; set; }

		[DataMember]
		public string Error { get; set; }


		public TimeSpan FirstReal { get; set; }
		public TimeSpan LastReal { get; set; }
		public TimeSpan FirstPlanned { get; set; }
		public TimeSpan LastPlanned { get; set; }

		public TimeSpan Total { get; set; }
		public TimeSpan TotalMissed { get; set; }
		public TimeSpan TotalInSchedule { get; set; }
		public TimeSpan TotalOvertime { get; set; }
		public TimeSpan TotalLate { get; set; }
		public TimeSpan TotalEarlyLeave { get; set; }
		public TimeSpan TotalPlanned { get; set; }

		public TimeSpan TotalEavening { get; set; }
		public TimeSpan TotalNight { get; set; }
		public TimeSpan Total_DocumentOvertime { get; set; }
		public TimeSpan Total_DocumentPresence { get; set; }
		public TimeSpan Total_DocumentAbsence { get; set; }

		void CalculateDocuments()
		{
			DocumentTrackParts = new List<TimeTrackPart>();
			foreach (var document in Documents)
			{
				TimeTrackPart timeTrackPart = null;
				if (document.StartDateTime.Date < Date && document.EndDateTime.Date > Date)
				{
					timeTrackPart = new TimeTrackPart()
					{
						StartTime = TimeSpan.Zero,
						EndTime = new TimeSpan(23, 59, 59)
					};
				}
				if (document.StartDateTime.Date == Date && document.EndDateTime.Date > Date)
				{
					timeTrackPart = new TimeTrackPart()
					{
						StartTime = document.StartDateTime.TimeOfDay,
						EndTime = new TimeSpan(23, 59, 59)
					};
				}
				if (document.StartDateTime.Date == Date && document.EndDateTime.Date == Date)
				{
					timeTrackPart = new TimeTrackPart()
					{
						StartTime = document.StartDateTime.TimeOfDay,
						EndTime = document.EndDateTime.TimeOfDay
					};
				}
				if (timeTrackPart != null)
				{
					timeTrackPart.DocumentCode = document.DocumentCode;
					DocumentTrackParts.Add(timeTrackPart);
				}
			}
			DocumentTrackParts = DocumentTrackParts.OrderBy(x => x.StartTime.Ticks).ToList();

			var timeSpans = new List<TimeSpan>();
			foreach (var trackPart in DocumentTrackParts)
			{
				timeSpans.Add(trackPart.StartTime);
				timeSpans.Add(trackPart.EndTime);
			}
			timeSpans = timeSpans.OrderBy(x => x.TotalSeconds).ToList();

			var result = new List<TimeTrackPart>();
			for (int i = 0; i < timeSpans.Count - 1; i++)
			{
				var startTimeSpan = timeSpans[i];
				var endTimeSpan = timeSpans[i + 1];
				var timeTrackParts = DocumentTrackParts.Where(x => x.StartTime <= startTimeSpan && x.EndTime > startTimeSpan);

				if (timeTrackParts.Count() > 0)
				{
					var newTimeTrackPart = new TimeTrackPart()
					{
						StartTime = startTimeSpan,
						EndTime = endTimeSpan,
					};
					foreach (var timeTrackPart in timeTrackParts)
					{
						newTimeTrackPart.DocumentCode = timeTrackPart.DocumentCode;
						newTimeTrackPart.DocumentCodes.Add(timeTrackPart.DocumentCode);
					}
					result.Add(newTimeTrackPart);
				}
			}
			DocumentTrackParts = result;
		}

		public void Calculate()
		{
			CalculateDocuments();

			PlannedTimeTrackParts = NormalizeTimeTrackParts(PlannedTimeTrackParts);
			RealTimeTrackParts = NormalizeTimeTrackParts(RealTimeTrackParts);

			CalculateCombinedTimeTrackParts();
			CalculateTotal();

			TotalEavening = new TimeSpan();
			TotalNight = new TimeSpan();
			if (HolidaySettings != null)
			{
				TotalEavening = CalculateEveningTime(HolidaySettings.EveningStartTime, HolidaySettings.EveningEndTime);
				TotalNight = CalculateEveningTime(HolidaySettings.NightStartTime, HolidaySettings.NightEndTime);
			}
		}

		void CalculateCombinedTimeTrackParts()
		{
			if (RealTimeTrackParts.Count > 0)
			{
				FirstReal = RealTimeTrackParts.FirstOrDefault().StartTime;
				LastReal = RealTimeTrackParts.LastOrDefault().EndTime;
			}
			if (PlannedTimeTrackParts.Count > 0)
			{
				FirstPlanned = PlannedTimeTrackParts.FirstOrDefault().StartTime;
				LastPlanned = PlannedTimeTrackParts.LastOrDefault().EndTime;
			}

			var combinedTimeSpans = new List<TimeSpan>();
			foreach (var trackPart in RealTimeTrackParts)
			{
				combinedTimeSpans.Add(trackPart.StartTime);
				combinedTimeSpans.Add(trackPart.EndTime);
			}
			foreach (var trackPart in PlannedTimeTrackParts)
			{
				combinedTimeSpans.Add(trackPart.StartTime);
				combinedTimeSpans.Add(trackPart.EndTime);
			}
			foreach (var trackPart in DocumentTrackParts)
			{
				combinedTimeSpans.Add(trackPart.StartTime);
				combinedTimeSpans.Add(trackPart.EndTime);
			}
			combinedTimeSpans.Sort();

			TotalPlanned = new TimeSpan();
			foreach (var timeTrackPart in RealTimeTrackParts)
			{
				TotalPlanned += timeTrackPart.Delta;
			}

			CombinedTimeTrackParts = new List<TimeTrackPart>();
			for (int i = 0; i < combinedTimeSpans.Count - 1; i++)
			{
				var startTime = combinedTimeSpans[i];
				var endTime = combinedTimeSpans[i + 1];

				var timeTrackPart = new TimeTrackPart();
				timeTrackPart.StartTime = startTime;
				timeTrackPart.EndTime = endTime;
				CombinedTimeTrackParts.Add(timeTrackPart);

				var hasRealTimeTrack = RealTimeTrackParts.Any(x => x.StartTime <= startTime && x.EndTime >= endTime);
				var hasPlannedTimeTrack = PlannedTimeTrackParts.Any(x => x.StartTime <= startTime && x.EndTime >= endTime);
				var documentTimeTrack = DocumentTrackParts.FirstOrDefault(x => x.StartTime <= startTime && x.EndTime >= endTime);

				if (hasRealTimeTrack && hasPlannedTimeTrack)
				{
					timeTrackPart.TimeTrackPartType = TimeTrackPartType.AsPlanned;
				}
				if (!hasRealTimeTrack && !hasPlannedTimeTrack)
				{
					timeTrackPart.TimeTrackPartType = TimeTrackPartType.None;
				}
				if (hasRealTimeTrack && !hasPlannedTimeTrack)
				{
					timeTrackPart.TimeTrackPartType = TimeTrackPartType.Overtime;
					if (timeTrackPart.StartTime > FirstPlanned && timeTrackPart.EndTime < LastPlanned)
						timeTrackPart.TimeTrackPartType = TimeTrackPartType.InBrerak;
				}
				if (!hasRealTimeTrack && hasPlannedTimeTrack)
				{
					timeTrackPart.TimeTrackPartType = TimeTrackPartType.PlanedOnly;

					if (RealTimeTrackParts.Any(x => x.StartTime < startTime) && IsOnlyFirstEnter)
					{
						timeTrackPart.TimeTrackPartType = TimeTrackPartType.MissedButInsidePlan;
					}

					if (timeTrackPart.StartTime == FirstPlanned && timeTrackPart.EndTime < LastPlanned && !PlannedTimeTrackParts.Any(x => x.EndTime == timeTrackPart.EndTime))
					{
						if (timeTrackPart.Delta > AllowedLate)
						{
							timeTrackPart.TimeTrackPartType = TimeTrackPartType.Late;
						}
					}

					if (timeTrackPart.EndTime == LastPlanned && timeTrackPart.StartTime > FirstPlanned && !PlannedTimeTrackParts.Any(x => x.StartTime == timeTrackPart.StartTime))
					{
						if (timeTrackPart.Delta > AllowedEarlyLeave)
						{
							timeTrackPart.TimeTrackPartType = TimeTrackPartType.EarlyLeave;
						}
					}
				}
				if (documentTimeTrack != null)
				{
					var documentType = DocumentType.Absence;
					foreach (var documentCode in documentTimeTrack.DocumentCodes)
					{
						var timeTrackDocumentType = TimeTrackDocumentTypesCollection.TimeTrackDocumentTypes.FirstOrDefault(x => x.Code == documentCode);
						if (timeTrackDocumentType != null)
						{
							if (timeTrackDocumentType.DocumentType < documentType)
								documentType = timeTrackDocumentType.DocumentType;
						}
					}

					if (documentType == DocumentType.Overtime)
					{
						timeTrackPart.TimeTrackPartType = TimeTrackPartType.DocumentOvertime;
					}
					if (documentType == DocumentType.Presence)
					{
						if (hasPlannedTimeTrack)
							timeTrackPart.TimeTrackPartType = TimeTrackPartType.DocumentPresence;
						else
							timeTrackPart.TimeTrackPartType = TimeTrackPartType.None;
					}
					if (documentType == DocumentType.Absence)
					{
						if (hasRealTimeTrack || hasPlannedTimeTrack)
							timeTrackPart.TimeTrackPartType = TimeTrackPartType.DocumentAbsence;
						else
							timeTrackPart.TimeTrackPartType = TimeTrackPartType.None;
					}
				}
			}
		}

		void CalculateTotal()
		{
			Total = new TimeSpan();
			TotalInSchedule = new TimeSpan();
			TotalMissed = new TimeSpan();
			TotalLate = new TimeSpan();
			TotalEarlyLeave = new TimeSpan();
			TotalOvertime = new TimeSpan();
			TotalEavening = new TimeSpan();
			TotalNight = new TimeSpan();
			Total_DocumentOvertime = new TimeSpan();
			Total_DocumentPresence = new TimeSpan();
			Total_DocumentAbsence = new TimeSpan();

			foreach (var timeTrack in CombinedTimeTrackParts)
			{
				if (timeTrack.TimeTrackPartType == TimeTrackPartType.AsPlanned || timeTrack.TimeTrackPartType == TimeTrackPartType.Overtime || timeTrack.TimeTrackPartType == TimeTrackPartType.MissedButInsidePlan)
				{
					Total += timeTrack.Delta;
				}
				if (timeTrack.TimeTrackPartType == TimeTrackPartType.AsPlanned || timeTrack.TimeTrackPartType == TimeTrackPartType.MissedButInsidePlan)
				{
					TotalInSchedule += timeTrack.Delta;
				}
				if (timeTrack.TimeTrackPartType == TimeTrackPartType.PlanedOnly)
				{
					TotalMissed = timeTrack.Delta;
				}
				if (timeTrack.TimeTrackPartType == TimeTrackPartType.Late)
				{
					TotalLate = timeTrack.Delta;
				}
				if (timeTrack.TimeTrackPartType == TimeTrackPartType.EarlyLeave)
				{
					TotalEarlyLeave = timeTrack.Delta;
				}
				if (timeTrack.TimeTrackPartType == TimeTrackPartType.Overtime)
				{
					TotalOvertime += timeTrack.Delta;
				}

				if (timeTrack.TimeTrackPartType == TimeTrackPartType.DocumentOvertime)
				{
					Total_DocumentOvertime += timeTrack.Delta;
				}
				if (timeTrack.TimeTrackPartType == TimeTrackPartType.DocumentPresence)
				{
					Total_DocumentPresence += timeTrack.Delta;
				}
				if (timeTrack.TimeTrackPartType == TimeTrackPartType.DocumentAbsence)
				{
					Total_DocumentAbsence += timeTrack.Delta;
				}
			}

			if (SlideTime.TotalSeconds > 0)
			{
				if (Total > SlideTime)
				{
					TotalInSchedule = SlideTime;
					TotalOvertime = Total - SlideTime;
				}
				else
				{
					TotalInSchedule = Total;
					TotalOvertime = new TimeSpan();
				}	
			}

			if (IsHoliday)
			{
				TotalInSchedule = new TimeSpan();
				TotalMissed = new TimeSpan();
				TotalLate = new TimeSpan();
				TotalEarlyLeave = new TimeSpan();
				TotalOvertime = new TimeSpan();
			}

			//if (Documents != null && Documents.Count > 0)
			//{
			//    Total = TotalPlanned;
			//    TotalInSchedule = TotalPlanned;
			//    TotalMissed = new TimeSpan();
			//    TotalLate = new TimeSpan();
			//    TotalEarlyLeave = new TimeSpan();
			//    TotalOvertime = new TimeSpan();
			//}
			TimeTrackType = CalculateTimeTrackType();
		}

		TimeTrackType CalculateTimeTrackType()
		{
			if (!string.IsNullOrEmpty(Error))
			{
				return TimeTrackType.None;
			}
			if (Documents != null && Documents.Count > 0)
			{
				return TimeTrackType.Document;
			}
			if (IsHoliday)
			{
				return TimeTrackType.Holiday;
			}
			if (PlannedTimeTrackParts.Count == 0)
			{
				return TimeTrackType.DayOff;
			}
			if (Total.TotalSeconds == 0)
			{
				return TimeTrackType.Missed;
			}
			if (TotalLate.TotalSeconds > 0)
			{
				return TimeTrackType.Late;
			}
			if (TotalEarlyLeave.TotalSeconds > 0)
			{
				return TimeTrackType.EarlyLeave;
			}
			if (TotalOvertime.TotalSeconds > 0)
			{
				return TimeTrackType.Overtime;
			}
			return TimeTrackType.AsPlanned;
		}

		TimeSpan CalculateEveningTime(TimeSpan start, TimeSpan end)
		{
			var result = new TimeSpan();
			if (end > TimeSpan.Zero)
			{
				foreach (var trackPart in RealTimeTrackParts)
				{
					if (trackPart.StartTime <= start && trackPart.EndTime >= end)
					{
						result += end - start;
					}
					else
					{
						if ((trackPart.StartTime >= start && trackPart.StartTime <= end) ||
							(trackPart.EndTime >= start && trackPart.EndTime <= end))
						{
							var minStartTime = trackPart.StartTime < start ? start : trackPart.StartTime;
							var minEndTime = trackPart.EndTime > end ? end : trackPart.EndTime;
							result += minEndTime - minStartTime;
						}
					}
				}
			}
			return result;
		}

		List<TimeTrackPart> NormalizeTimeTrackParts(List<TimeTrackPart> timeTrackParts)
		{
			if (timeTrackParts.Count == 0)
				return new List<TimeTrackPart>();

			var result = new List<TimeTrackPart>();

			var timeSpans = new List<TimeSpan>();
			foreach (var timeTrackPart in timeTrackParts)
			{
				timeSpans.Add(timeTrackPart.StartTime);
				timeSpans.Add(timeTrackPart.EndTime);
			}
			timeSpans = timeSpans.OrderBy(x => x.TotalSeconds).ToList();

			for (int i = 0; i < timeSpans.Count - 1; i ++)
			{
				var startTimeSpan = timeSpans[i];
				var endTimeSpan = timeSpans[i + 1];
				var timeTrackPart = timeTrackParts.FirstOrDefault(x => x.StartTime <= startTimeSpan && x.EndTime > startTimeSpan);

				if (timeTrackPart != null)
				{
					var newTimeTrackPart = new TimeTrackPart()
					{
						StartTime = startTimeSpan,
						EndTime = endTimeSpan,
						ZoneUID = timeTrackPart.ZoneUID,
						TimeTrackPartType = timeTrackPart.TimeTrackPartType
					};
					result.Add(newTimeTrackPart);
				}
			}

			for (int i = result.Count - 1; i > 0; i--)
			{
				if (result[i].StartTime == result[i - 1].EndTime)
				{
					result[i].StartTime = result[i - 1].StartTime;
					result.RemoveAt(i - 1);
				}
			}
			return result;
		}
	}
}