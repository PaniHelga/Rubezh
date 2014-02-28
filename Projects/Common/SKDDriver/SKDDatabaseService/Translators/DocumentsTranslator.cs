﻿using System;
using System.Linq;
using FiresecAPI;
using System.Data.Linq;
using LinqKit;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace SKDDriver
{
	public class DocumentsTranslator : OrganizationTranslatorBase<DataAccess.Document, Document, DocumentFilter>
	{
		public DocumentsTranslator(Table<DataAccess.Document> table, DataAccess.SKUDDataContext context)
			: base(table, context)
		{
			
		}

		protected override OperationResult CanSave(Document item)
		{
			bool sameName = Table.Any(x => x.Name == item.Name && 
				x.OrganizationUid == item.OrganizationUid && 
				x.Uid != item.UID && 
				x.IsDeleted == false);
			if (sameName)
				return new OperationResult("Документ с таким же именем уже содержится в базе данных");
			if (item.No <= 0)
				return new OperationResult("Номер добавляемого документа должен быть положительным числом");
			bool sameNo = Table.Any(x => x.No == item.No &&
				x.OrganizationUid == item.OrganizationUid && 
				x.Uid != item.UID && 
				x.IsDeleted == false);
			if (sameNo)
				return new OperationResult("Документ с таким же номером уже содержится в базе данных");
			return base.CanSave(item);
		}

		protected override Document Translate(DataAccess.Document tableItem)
		{
			var result = base.Translate(tableItem);
			result.Name = tableItem.Name;
			result.Description = tableItem.Description;
			result.IssueDate = tableItem.IssueDate;
			result.LaunchDate = tableItem.LaunchDate;
			result.No = tableItem.No;
			return result;
		}

		protected override void Update(DataAccess.Document tableItem, Document apiItem)
		{
			base.Update(tableItem, apiItem);
			tableItem.Name = apiItem.Name;
			tableItem.Description = apiItem.Description;
			tableItem.IssueDate = apiItem.IssueDate;
			tableItem.LaunchDate = apiItem.LaunchDate;
			tableItem.No = apiItem.No;
		}

		protected override Expression<Func<DataAccess.Document, bool>> IsInFilter(DocumentFilter filter)
		{
			var result = PredicateBuilder.True<DataAccess.Document>();
			result = result.And(base.IsInFilter(filter));
			var nos = filter.Nos;
			if (nos != null && nos.Count != 0)
				result = result.And(e => nos.Contains(e.No));
			var names = filter.Names;
			if (names != null && names.Count != 0)
				result = result.And(e => names.Contains(e.Name));
			var issueDates = filter.IssueDate;
			if (issueDates != null)
				result = result.And(e => e.RemovalDate >= issueDates.StartDate && e.RemovalDate <= issueDates.EndDate);
			var launchDates = filter.LaunchDate;
			if (launchDates != null)
				result = result.And(e => e.RemovalDate >= launchDates.StartDate && e.RemovalDate <= launchDates.EndDate);
			return result;
		}

	}
}