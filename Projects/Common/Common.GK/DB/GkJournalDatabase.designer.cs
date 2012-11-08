﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common.GK.DB
{
    using System;
    using System.ComponentModel;
    using System.Data.Linq.Mapping;
	
	
	public partial class GkJournalDatabase : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertJournal(Journal instance);
    partial void UpdateJournal(Journal instance);
    partial void DeleteJournal(Journal instance);
    #endregion
		
		public GkJournalDatabase(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public GkJournalDatabase(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public GkJournalDatabase(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public GkJournalDatabase(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Journal> Journal
		{
			get
			{
				return this.GetTable<Journal>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute()]
	public partial class Journal : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Description;
		
		private System.Nullable<System.DateTime> _DateTime;
		
		private System.Nullable<System.Guid> _ObjectUID;
		
		private string _Name;
		
		private string _YesNo;
		
		private System.Nullable<int> _ObjectState;
		
		private System.Nullable<byte> _ItemType;
		
		private System.Nullable<byte> _JournalItemType;
		
		private string _GKIpAddress;
		
		private System.Nullable<int> _GKJournalRecordNo;
		
		private byte _StateClass;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    partial void OnDateTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnDateTimeChanged();
    partial void OnObjectUIDChanging(System.Nullable<System.Guid> value);
    partial void OnObjectUIDChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnYesNoChanging(string value);
    partial void OnYesNoChanged();
    partial void OnObjectStateChanging(System.Nullable<int> value);
    partial void OnObjectStateChanged();
    partial void OnItemTypeChanging(System.Nullable<byte> value);
    partial void OnItemTypeChanged();
    partial void OnJournalItemTypeChanging(System.Nullable<byte> value);
    partial void OnJournalItemTypeChanged();
    partial void OnGKIpAddressChanging(string value);
    partial void OnGKIpAddressChanged();
    partial void OnGKJournalRecordNoChanging(System.Nullable<int> value);
    partial void OnGKJournalRecordNoChanged();
    partial void OnStateClassChanging(byte value);
    partial void OnStateClassChanged();
    #endregion
		
		public Journal()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(100)")]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> DateTime
		{
			get
			{
				return this._DateTime;
			}
			set
			{
				if ((this._DateTime != value))
				{
					this.OnDateTimeChanging(value);
					this.SendPropertyChanging();
					this._DateTime = value;
					this.SendPropertyChanged("DateTime");
					this.OnDateTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ObjectUID", DbType="UniqueIdentifier")]
		public System.Nullable<System.Guid> ObjectUID
		{
			get
			{
				return this._ObjectUID;
			}
			set
			{
				if ((this._ObjectUID != value))
				{
					this.OnObjectUIDChanging(value);
					this.SendPropertyChanging();
					this._ObjectUID = value;
					this.SendPropertyChanged("ObjectUID");
					this.OnObjectUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(50)")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_YesNo", DbType="NVarChar(10)")]
		public string YesNo
		{
			get
			{
				return this._YesNo;
			}
			set
			{
				if ((this._YesNo != value))
				{
					this.OnYesNoChanging(value);
					this.SendPropertyChanging();
					this._YesNo = value;
					this.SendPropertyChanged("YesNo");
					this.OnYesNoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ObjectState", DbType="Int")]
		public System.Nullable<int> ObjectState
		{
			get
			{
				return this._ObjectState;
			}
			set
			{
				if ((this._ObjectState != value))
				{
					this.OnObjectStateChanging(value);
					this.SendPropertyChanging();
					this._ObjectState = value;
					this.SendPropertyChanged("ObjectState");
					this.OnObjectStateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ItemType", DbType="TinyInt")]
		public System.Nullable<byte> ItemType
		{
			get
			{
				return this._ItemType;
			}
			set
			{
				if ((this._ItemType != value))
				{
					this.OnItemTypeChanging(value);
					this.SendPropertyChanging();
					this._ItemType = value;
					this.SendPropertyChanged("ItemType");
					this.OnItemTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_JournalItemType", DbType="TinyInt")]
		public System.Nullable<byte> JournalItemType
		{
			get
			{
				return this._JournalItemType;
			}
			set
			{
				if ((this._JournalItemType != value))
				{
					this.OnJournalItemTypeChanging(value);
					this.SendPropertyChanging();
					this._JournalItemType = value;
					this.SendPropertyChanged("JournalItemType");
					this.OnJournalItemTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GKIpAddress", DbType="NVarChar(20)")]
		public string GKIpAddress
		{
			get
			{
				return this._GKIpAddress;
			}
			set
			{
				if ((this._GKIpAddress != value))
				{
					this.OnGKIpAddressChanging(value);
					this.SendPropertyChanging();
					this._GKIpAddress = value;
					this.SendPropertyChanged("GKIpAddress");
					this.OnGKIpAddressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GKJournalRecordNo", DbType="Int")]
		public System.Nullable<int> GKJournalRecordNo
		{
			get
			{
				return this._GKJournalRecordNo;
			}
			set
			{
				if ((this._GKJournalRecordNo != value))
				{
					this.OnGKJournalRecordNoChanging(value);
					this.SendPropertyChanging();
					this._GKJournalRecordNo = value;
					this.SendPropertyChanged("GKJournalRecordNo");
					this.OnGKJournalRecordNoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StateClass", DbType="TinyInt NOT NULL")]
		public byte StateClass
		{
			get
			{
				return this._StateClass;
			}
			set
			{
				if ((this._StateClass != value))
				{
					this.OnStateClassChanging(value);
					this.SendPropertyChanging();
					this._StateClass = value;
					this.SendPropertyChanged("StateClass");
					this.OnStateClassChanged();
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