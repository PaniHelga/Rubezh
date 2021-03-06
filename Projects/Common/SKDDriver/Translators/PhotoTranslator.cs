﻿using FiresecAPI.SKD;

namespace SKDDriver
{
	public class PhotoTranslator : WithFilterTranslator<DataAccess.Photo, Photo, PhotoFilter>
	{
		public PhotoTranslator(SKDDatabaseService databaseService)
			: base(databaseService) { }

		protected override Photo Translate(DataAccess.Photo tableItem)
		{
			var result = base.Translate(tableItem);
			result.Data = tableItem.Data.ToArray();
			return result;
		}

		protected override void TranslateBack(DataAccess.Photo tableItem, Photo apiItem)
		{
			tableItem.Data = apiItem.Data;
		}
	}
}