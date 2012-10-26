﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Common.GK.DB;

namespace Common.GK
{
    public static class GKDBHelper
    {
		public static void Add(JournalItem journalItem)
        {
            try
            {
                using (var dataContext = ConnectionManager.CreateGKDataContext())
                {
                    var journal = new Journal();
					journal.GKIpAddress = journalItem.GKIpAddress;
					journal.GKJournalRecordNo = journalItem.GKJournalRecordNo;
					journal.JournalItemType = (byte)journalItem.JournalItemType;
                    journal.DateTime = journalItem.DateTime;
					journal.Name = journalItem.Name;
					journal.YesNo = journalItem.YesNo;
					journal.Description = journalItem.Description;
					journal.ObjectUID = journalItem.ObjectUID;
					journal.ObjectState = journalItem.ObjectState;
                    dataContext.Journal.InsertOnSubmit(journal);
                    dataContext.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "GKDBHelper.Add");
            }
        }

		public static List<JournalItem> Select(XArchiveFilter archiveFilter)
        {
			var journalItems = new List<JournalItem>();
            try
            {
                using (var dataContext = ConnectionManager.CreateGKDataContext())
                {
					var query =
                    "SELECT * FROM Journal WHERE " +
					"\n DateTime > '" + archiveFilter.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
					"\n AND DateTime < '" + archiveFilter.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";

                    var result = dataContext.ExecuteQuery<Journal>(query);
					foreach (var journal in result)
					{
                        var journalItem = JournalItem.FromJournal(journal);
						journalItems.Add(journalItem);
					}
					var journalRecordsCount = journalItems.Count;
                    Trace.WriteLine("Journal Count = " + journalRecordsCount.ToString());
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "GKDBHelper.Select");
            }
			return journalItems;
        }

        public static int GetLastGKID(string gkIPAddress)
        {
            try
            {
                using (var dataContext = ConnectionManager.CreateGKDataContext())
                {
                    var query =
                    "SELECT MAX(GKJournalRecordNo) FROM Journal WHERE GKIpAddress = '" + gkIPAddress + "'";

                    var result = dataContext.ExecuteQuery<int?>(query);
                    var firstResult = result.FirstOrDefault();
                    if (firstResult != null)
                        return firstResult.Value;
                    return 0;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "GKDBHelper.GetLastGKID");
                return 0;
            }
        }
    }
}