﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:2.0.50727.3053
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

//
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
//
namespace Firesec.ReadEvents
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class document
    {
        private journalType[] journalField;

        private parametersType parametersField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ЖурналСобытий", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public journalType[] Journal
        {
            get
            {
                return this.journalField;
            }
            set
            {
                this.journalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Параметры", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public parametersType Parameters
        {
            get
            {
                return this.parametersField;
            }
            set
            {
                this.parametersField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class journalType
    {
        private string iDEventsField;

        private string iDSubSystemField;

        private string dtField;

        private string sysDtField;

        private string zoneNameField;

        private string iDTypeEventsField;

        private string eventDescField;

        private string userInfoField;

        private string iDDevicesField;

        private string iDDevicesSourceField;

        private string cLC_DeviceSourceField;

        private string cLC_DeviceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string IDEvents
        {
            get
            {
                return this.iDEventsField;
            }
            set
            {
                this.iDEventsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string IDSubSystem
        {
            get
            {
                return this.iDSubSystemField;
            }
            set
            {
                this.iDSubSystemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Dt
        {
            get
            {
                return this.dtField;
            }
            set
            {
                this.dtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SysDt
        {
            get
            {
                return this.sysDtField;
            }
            set
            {
                this.sysDtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ZoneName
        {
            get
            {
                return this.zoneNameField;
            }
            set
            {
                this.zoneNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string IDTypeEvents
        {
            get
            {
                return this.iDTypeEventsField;
            }
            set
            {
                this.iDTypeEventsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string EventDesc
        {
            get
            {
                return this.eventDescField;
            }
            set
            {
                this.eventDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UserInfo
        {
            get
            {
                return this.userInfoField;
            }
            set
            {
                this.userInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string IDDevices
        {
            get
            {
                return this.iDDevicesField;
            }
            set
            {
                this.iDDevicesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string IDDevicesSource
        {
            get
            {
                return this.iDDevicesSourceField;
            }
            set
            {
                this.iDDevicesSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CLC_DeviceSource
        {
            get
            {
                return this.cLC_DeviceSourceField;
            }
            set
            {
                this.cLC_DeviceSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CLC_Device
        {
            get
            {
                return this.cLC_DeviceField;
            }
            set
            {
                this.cLC_DeviceField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class parametersType
    {
        private string date_fromField;

        private string date_toField;

        private string useSysDateField;

        private string eventFilterSetField;

        private string totalRecordsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string date_from
        {
            get
            {
                return this.date_fromField;
            }
            set
            {
                this.date_fromField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string date_to
        {
            get
            {
                return this.date_toField;
            }
            set
            {
                this.date_toField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string useSysDate
        {
            get
            {
                return this.useSysDateField;
            }
            set
            {
                this.useSysDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string eventFilterSet
        {
            get
            {
                return this.eventFilterSetField;
            }
            set
            {
                this.eventFilterSetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string totalRecords
        {
            get
            {
                return this.totalRecordsField;
            }
            set
            {
                this.totalRecordsField = value;
            }
        }
    }
}