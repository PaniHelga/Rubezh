﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.296
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SKDDriver.DataAccess
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="PassJournal_0")]
	public partial class PassJournalDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Определения метода расширяемости
    partial void OnCreated();
    partial void InsertPassJournal(PassJournal instance);
    partial void UpdatePassJournal(PassJournal instance);
    partial void DeletePassJournal(PassJournal instance);
    partial void InsertEmployeeDay(EmployeeDay instance);
    partial void UpdateEmployeeDay(EmployeeDay instance);
    partial void DeleteEmployeeDay(EmployeeDay instance);
    #endregion
		
		public PassJournalDataContext() : 
				base(global::SKDDriver.Properties.Settings.Default.PassJournal_0ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public PassJournalDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PassJournalDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PassJournalDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PassJournalDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<PassJournal> PassJournals
		{
			get
			{
				return this.GetTable<PassJournal>();
			}
		}
		
		public System.Data.Linq.Table<EmployeeDay> EmployeeDays
		{
			get
			{
				return this.GetTable<EmployeeDay>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.PassJournal")]
	public partial class PassJournal : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _UID;
		
		private System.Guid _EmployeeUID;
		
		private System.Guid _ZoneUID;
		
		private System.DateTime _EnterTime;
		
		private System.Nullable<System.DateTime> _ExitTime;
		
    #region Определения метода расширяемости
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnUIDChanging(System.Guid value);
    partial void OnUIDChanged();
    partial void OnEmployeeUIDChanging(System.Guid value);
    partial void OnEmployeeUIDChanged();
    partial void OnZoneUIDChanging(System.Guid value);
    partial void OnZoneUIDChanged();
    partial void OnEnterTimeChanging(System.DateTime value);
    partial void OnEnterTimeChanged();
    partial void OnExitTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnExitTimeChanged();
    #endregion
		
		public PassJournal()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid UID
		{
			get
			{
				return this._UID;
			}
			set
			{
				if ((this._UID != value))
				{
					this.OnUIDChanging(value);
					this.SendPropertyChanging();
					this._UID = value;
					this.SendPropertyChanged("UID");
					this.OnUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EmployeeUID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid EmployeeUID
		{
			get
			{
				return this._EmployeeUID;
			}
			set
			{
				if ((this._EmployeeUID != value))
				{
					this.OnEmployeeUIDChanging(value);
					this.SendPropertyChanging();
					this._EmployeeUID = value;
					this.SendPropertyChanged("EmployeeUID");
					this.OnEmployeeUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ZoneUID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid ZoneUID
		{
			get
			{
				return this._ZoneUID;
			}
			set
			{
				if ((this._ZoneUID != value))
				{
					this.OnZoneUIDChanging(value);
					this.SendPropertyChanging();
					this._ZoneUID = value;
					this.SendPropertyChanged("ZoneUID");
					this.OnZoneUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EnterTime", DbType="DateTime NOT NULL")]
		public System.DateTime EnterTime
		{
			get
			{
				return this._EnterTime;
			}
			set
			{
				if ((this._EnterTime != value))
				{
					this.OnEnterTimeChanging(value);
					this.SendPropertyChanging();
					this._EnterTime = value;
					this.SendPropertyChanged("EnterTime");
					this.OnEnterTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ExitTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> ExitTime
		{
			get
			{
				return this._ExitTime;
			}
			set
			{
				if ((this._ExitTime != value))
				{
					this.OnExitTimeChanging(value);
					this.SendPropertyChanging();
					this._ExitTime = value;
					this.SendPropertyChanged("ExitTime");
					this.OnExitTimeChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.EmployeeDay")]
	public partial class EmployeeDay : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _UID;
		
		private System.Guid _EmployeeUID;
		
		private bool _IsIgnoreHoliday;
		
		private bool _IsOnlyFirstEnter;
		
		private int _AllowedLate;
		
		private int _AllowedEarlyLeave;
		
		private string _DayIntervalsString;
		
		private System.DateTime _Date;
		
    #region Определения метода расширяемости
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnUIDChanging(System.Guid value);
    partial void OnUIDChanged();
    partial void OnEmployeeUIDChanging(System.Guid value);
    partial void OnEmployeeUIDChanged();
    partial void OnIsIgnoreHolidayChanging(bool value);
    partial void OnIsIgnoreHolidayChanged();
    partial void OnIsOnlyFirstEnterChanging(bool value);
    partial void OnIsOnlyFirstEnterChanged();
    partial void OnAllowedLateChanging(int value);
    partial void OnAllowedLateChanged();
    partial void OnAllowedEarlyLeaveChanging(int value);
    partial void OnAllowedEarlyLeaveChanged();
    partial void OnDayIntervalsStringChanging(string value);
    partial void OnDayIntervalsStringChanged();
    partial void OnDateChanging(System.DateTime value);
    partial void OnDateChanged();
    #endregion
		
		public EmployeeDay()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid UID
		{
			get
			{
				return this._UID;
			}
			set
			{
				if ((this._UID != value))
				{
					this.OnUIDChanging(value);
					this.SendPropertyChanging();
					this._UID = value;
					this.SendPropertyChanged("UID");
					this.OnUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EmployeeUID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid EmployeeUID
		{
			get
			{
				return this._EmployeeUID;
			}
			set
			{
				if ((this._EmployeeUID != value))
				{
					this.OnEmployeeUIDChanging(value);
					this.SendPropertyChanging();
					this._EmployeeUID = value;
					this.SendPropertyChanged("EmployeeUID");
					this.OnEmployeeUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsIgnoreHoliday", DbType="Bit NOT NULL")]
		public bool IsIgnoreHoliday
		{
			get
			{
				return this._IsIgnoreHoliday;
			}
			set
			{
				if ((this._IsIgnoreHoliday != value))
				{
					this.OnIsIgnoreHolidayChanging(value);
					this.SendPropertyChanging();
					this._IsIgnoreHoliday = value;
					this.SendPropertyChanged("IsIgnoreHoliday");
					this.OnIsIgnoreHolidayChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsOnlyFirstEnter", DbType="Bit NOT NULL")]
		public bool IsOnlyFirstEnter
		{
			get
			{
				return this._IsOnlyFirstEnter;
			}
			set
			{
				if ((this._IsOnlyFirstEnter != value))
				{
					this.OnIsOnlyFirstEnterChanging(value);
					this.SendPropertyChanging();
					this._IsOnlyFirstEnter = value;
					this.SendPropertyChanged("IsOnlyFirstEnter");
					this.OnIsOnlyFirstEnterChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AllowedLate", DbType="Int NOT NULL")]
		public int AllowedLate
		{
			get
			{
				return this._AllowedLate;
			}
			set
			{
				if ((this._AllowedLate != value))
				{
					this.OnAllowedLateChanging(value);
					this.SendPropertyChanging();
					this._AllowedLate = value;
					this.SendPropertyChanged("AllowedLate");
					this.OnAllowedLateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AllowedEarlyLeave", DbType="Int NOT NULL")]
		public int AllowedEarlyLeave
		{
			get
			{
				return this._AllowedEarlyLeave;
			}
			set
			{
				if ((this._AllowedEarlyLeave != value))
				{
					this.OnAllowedEarlyLeaveChanging(value);
					this.SendPropertyChanging();
					this._AllowedEarlyLeave = value;
					this.SendPropertyChanged("AllowedEarlyLeave");
					this.OnAllowedEarlyLeaveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DayIntervalsString", DbType="NVarChar(MAX)")]
		public string DayIntervalsString
		{
			get
			{
				return this._DayIntervalsString;
			}
			set
			{
				if ((this._DayIntervalsString != value))
				{
					this.OnDayIntervalsStringChanging(value);
					this.SendPropertyChanging();
					this._DayIntervalsString = value;
					this.SendPropertyChanged("DayIntervalsString");
					this.OnDayIntervalsStringChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Date", DbType="DateTime NOT NULL")]
		public System.DateTime Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this.OnDateChanging(value);
					this.SendPropertyChanging();
					this._Date = value;
					this.SendPropertyChanged("Date");
					this.OnDateChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
