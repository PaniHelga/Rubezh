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
namespace Firesec.CoreConfig
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class config
    {
        private drvType[] drvField;

        private secObjTypeType[] secObjTypeField;

        private userGroupType[] userGroupField;

        private userType[] userField;

        private secGUIType[] secGUIField;

        private zoneType[] zoneField;

        private partType[] partField;

        private devType[] devField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("drv", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public drvType[] drv
        {
            get
            {
                return this.drvField;
            }
            set
            {
                this.drvField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("secObjType", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public secObjTypeType[] secObjType
        {
            get
            {
                return this.secObjTypeField;
            }
            set
            {
                this.secObjTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("userGroup", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public userGroupType[] userGroup
        {
            get
            {
                return this.userGroupField;
            }
            set
            {
                this.userGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("user", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public userType[] user
        {
            get
            {
                return this.userField;
            }
            set
            {
                this.userField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("secGUI", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public secGUIType[] secGUI
        {
            get
            {
                return this.secGUIField;
            }
            set
            {
                this.secGUIField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("zone", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public zoneType[] zone
        {
            get
            {
                return this.zoneField;
            }
            set
            {
                this.zoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("part", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public partType[] part
        {
            get
            {
                return this.partField;
            }
            set
            {
                this.partField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("dev", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public devType[] dev
        {
            get
            {
                return this.devField;
            }
            set
            {
                this.devField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class drvType
    {
        private string idxField;

        private string nameField;

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idx
        {
            get
            {
                return this.idxField;
            }
            set
            {
                this.idxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class inZType
    {
        private string idzField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idz
        {
            get
            {
                return this.idzField;
            }
            set
            {
                this.idzField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class dev_paramType
    {
        private string nameField;

        private string typeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class propType
    {
        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class devType
    {
        private propType[] propField;

        private paramType[] paramField;

        private devType[] devField;

        private dev_paramType[] dev_paramField;

        private inZType[] inZField;

        private string drvField;

        private string addrField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("prop", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public propType[] prop
        {
            get
            {
                return this.propField;
            }
            set
            {
                this.propField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("param", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public paramType[] param
        {
            get
            {
                return this.paramField;
            }
            set
            {
                this.paramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("dev", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public devType[] dev
        {
            get
            {
                return this.devField;
            }
            set
            {
                this.devField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("dev_param", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public dev_paramType[] dev_param
        {
            get
            {
                return this.dev_paramField;
            }
            set
            {
                this.dev_paramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("inZ", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public inZType[] inZ
        {
            get
            {
                return this.inZField;
            }
            set
            {
                this.inZField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string drv
        {
            get
            {
                return this.drvField;
            }
            set
            {
                this.drvField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class paramType
    {
        private string nameField;

        private string typeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class partType
    {
        private partTypePinZ[] pinZField;

        private paramType[] paramField;

        private string noField;

        private string idField;

        private string gidField;

        private string nameField;

        private string descField;

        private string typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PinZ", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public partTypePinZ[] PinZ
        {
            get
            {
                return this.pinZField;
            }
            set
            {
                this.pinZField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("param", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public paramType[] param
        {
            get
            {
                return this.paramField;
            }
            set
            {
                this.paramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string no
        {
            get
            {
                return this.noField;
            }
            set
            {
                this.noField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string gid
        {
            get
            {
                return this.gidField;
            }
            set
            {
                this.gidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string desc
        {
            get
            {
                return this.descField;
            }
            set
            {
                this.descField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class partTypePinZ
    {
        private string pidzField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string pidz
        {
            get
            {
                return this.pidzField;
            }
            set
            {
                this.pidzField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class zoneType
    {
        private paramType[] paramField;

        private string idxField;

        private string noField;

        private string nameField;

        private string descField;

        private string idZonesField;

        private string remidZonesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("param", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public paramType[] param
        {
            get
            {
                return this.paramField;
            }
            set
            {
                this.paramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idx
        {
            get
            {
                return this.idxField;
            }
            set
            {
                this.idxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string no
        {
            get
            {
                return this.noField;
            }
            set
            {
                this.noField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string desc
        {
            get
            {
                return this.descField;
            }
            set
            {
                this.descField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idZones
        {
            get
            {
                return this.idZonesField;
            }
            set
            {
                this.idZonesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string RemidZones
        {
            get
            {
                return this.remidZonesField;
            }
            set
            {
                this.remidZonesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class secRightType
    {
        private string actField;

        private string subjField;

        private string deleteflagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string act
        {
            get
            {
                return this.actField;
            }
            set
            {
                this.actField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string subj
        {
            get
            {
                return this.subjField;
            }
            set
            {
                this.subjField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string deleteflag
        {
            get
            {
                return this.deleteflagField;
            }
            set
            {
                this.deleteflagField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class secGUIType
    {
        private paramType paramField;

        private secRightType[] secRightField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public paramType param
        {
            get
            {
                return this.paramField;
            }
            set
            {
                this.paramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("secRight", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public secRightType[] secRight
        {
            get
            {
                return this.secRightField;
            }
            set
            {
                this.secRightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class grpType
    {
        private string idxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idx
        {
            get
            {
                return this.idxField;
            }
            set
            {
                this.idxField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class userType
    {
        private paramType paramField;

        private extSecurityType extSecurityField;

        private grpType[] grpField;

        private string idxField;

        private string nameField;

        private string fullNameField;

        private string passwordField;

        private string builtinField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public paramType param
        {
            get
            {
                return this.paramField;
            }
            set
            {
                this.paramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public extSecurityType extSecurity
        {
            get
            {
                return this.extSecurityField;
            }
            set
            {
                this.extSecurityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("grp", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public grpType[] grp
        {
            get
            {
                return this.grpField;
            }
            set
            {
                this.grpField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idx
        {
            get
            {
                return this.idxField;
            }
            set
            {
                this.idxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string fullName
        {
            get
            {
                return this.fullNameField;
            }
            set
            {
                this.fullNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string builtin
        {
            get
            {
                return this.builtinField;
            }
            set
            {
                this.builtinField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class extSecurityType
    {
        private string remoteAccessField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string remoteAccess
        {
            get
            {
                return this.remoteAccessField;
            }
            set
            {
                this.remoteAccessField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class userGroupType
    {
        private paramType paramField;

        private extSecurityType extSecurityField;

        private string idxField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public paramType param
        {
            get
            {
                return this.paramField;
            }
            set
            {
                this.paramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public extSecurityType extSecurity
        {
            get
            {
                return this.extSecurityField;
            }
            set
            {
                this.extSecurityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idx
        {
            get
            {
                return this.idxField;
            }
            set
            {
                this.idxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class secActionType
    {
        private paramType paramField;

        private string idxField;

        private string numField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public paramType param
        {
            get
            {
                return this.paramField;
            }
            set
            {
                this.paramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idx
        {
            get
            {
                return this.idxField;
            }
            set
            {
                this.idxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string num
        {
            get
            {
                return this.numField;
            }
            set
            {
                this.numField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class secObjTypeType
    {
        private secActionType[] secActionField;

        private string idxField;

        private string nameField;

        private string numField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("secAction", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public secActionType[] secAction
        {
            get
            {
                return this.secActionField;
            }
            set
            {
                this.secActionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idx
        {
            get
            {
                return this.idxField;
            }
            set
            {
                this.idxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string num
        {
            get
            {
                return this.numField;
            }
            set
            {
                this.numField = value;
            }
        }
    }
}