﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common.Windows.ViewModels;
using Infrastructure;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using FiresecAPI.Automation;
using FiresecClient;

namespace AutomationModule.ViewModels
{
	public class ArgumentItemViewModel : BaseViewModel
	{
		Procedure Procedure { get; set; }
		ArithmeticParameter ArithmeticParameter { get; set; }
		List<FiresecAPI.Automation.ValueType> ValueTypes { get; set; }

		public Action UpdateDescriptionHandler { get; set; }
		public Action UpdateVariableTypeHandler { get; set; }
		public Action UpdateVariableHandler { get; set; }

		public ArgumentItemViewModel(Procedure procedure, ArithmeticParameter arithmeticParameter, List<FiresecAPI.Automation.ValueType> valueTypes, bool allowImplicitValue = true)
		{
			ArithmeticParameter = arithmeticParameter;
			Procedure = procedure;
			ValueTypes = valueTypes;

			VariableTypes = new ObservableCollection<VariableType>();
			VariableTypes.Add(VariableType.IsGlobalVariable);
			VariableTypes.Add(VariableType.IsLocalVariable);
			if (allowImplicitValue)
			{
				VariableTypes.Add(VariableType.IsValue);
			}

			SelectedVariableType = ArithmeticParameter.VariableType;
		}

		public ObservableCollection<VariableType> VariableTypes { get; private set; }

		public VariableType SelectedVariableType
		{
			get { return ArithmeticParameter.VariableType; }
			set
			{
				ArithmeticParameter.VariableType = value;
				if (UpdateVariableTypeHandler != null)
					UpdateVariableTypeHandler();
				OnPropertyChanged(() => SelectedVariableType);

				Variables = new ObservableCollection<VariableViewModel>();
				switch(value)
				{
					case VariableType.IsLocalVariable:
						foreach (var argumrnt in Procedure.Arguments)
						{
							if (ValueTypes.Contains(argumrnt.ValueType))
							{
								var variableViewModel = new VariableViewModel(argumrnt);
								Variables.Add(variableViewModel);
							}
						}
						foreach (var variable in Procedure.Variables)
						{
							if (ValueTypes.Contains(variable.ValueType))
							{
								var variableViewModel = new VariableViewModel(variable);
								Variables.Add(variableViewModel);
							}
						}
						break;

					case VariableType.IsGlobalVariable:
						foreach (var variable in FiresecManager.SystemConfiguration.AutomationConfiguration.GlobalVariables)
						{
							if (ValueTypes.Contains(variable.ValueType))
							{
								var variableViewModel = new VariableViewModel(variable);
								Variables.Add(variableViewModel);
							}
						}
						break;
				}
				ShowVariables = value != VariableType.IsValue;
				NotShowVariables = !ShowVariables;
			}
		}

		ObservableCollection<VariableViewModel> _variables;
		public ObservableCollection<VariableViewModel> Variables
		{
			get { return _variables; }
			set
			{
				_variables = value;
				OnPropertyChanged(() => Variables);
			}
		}

		bool _showVariables;
		public bool ShowVariables
		{
			get { return _showVariables; }
			set
			{
				_showVariables = value;
				OnPropertyChanged(() => ShowVariables);
			}
		}

		bool _notShowVariables;
		public bool NotShowVariables
		{
			get { return _notShowVariables; }
			set
			{
				_notShowVariables = value;
				OnPropertyChanged(() => NotShowVariables);
			}
		}

		VariableViewModel _selectedVariable;
		public VariableViewModel SelectedVariable
		{
			get { return _selectedVariable; }
			set
			{
				_selectedVariable = value;
				if (_selectedVariable != null)
				{
					ArithmeticParameter.VariableUid = value.Variable.Uid;
					if (UpdateVariableHandler != null)
						UpdateVariableHandler();
				}
				else
				{
					ArithmeticParameter.VariableUid = Guid.Empty;
				}
				OnPropertyChanged(() => SelectedVariable);
			}
		}

		public bool BoolValue
		{
			get { return ArithmeticParameter.BoolValue; }
			set
			{
				ArithmeticParameter.BoolValue = value;
				OnPropertyChanged(() => BoolValue);
			}
		}

		public DateTime DateTimeValue
		{
			get { return ArithmeticParameter.DateTimeValue; }
			set
			{
				ArithmeticParameter.DateTimeValue = value;
				OnPropertyChanged(() => DateTimeValue);
			}
		}

		public int IntValue
		{
			get { return ArithmeticParameter.IntValue; }
			set
			{
				ArithmeticParameter.IntValue = value;
				OnPropertyChanged(() => IntValue);
			}
		}

		public string StringValue
		{
			get { return ArithmeticParameter.StringValue; }
			set
			{
				ArithmeticParameter.StringValue = value;
				OnPropertyChanged(() => StringValue);
			}
		}

		public Guid UIDValue
		{
			get { return ArithmeticParameter.UidValue; }
			set
			{
				ArithmeticParameter.UidValue = value;
				OnPropertyChanged(() => UIDValue);
			}
		}

		public string TypeValue
		{
			get { return ArithmeticParameter.TypeValue; }
			set
			{
				ArithmeticParameter.TypeValue = value;
				OnPropertyChanged(() => TypeValue);
			}
		}

		public new void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			ServiceFactory.SaveService.AutomationChanged = true;
			base.OnPropertyChanged(propertyExpression);
			if (UpdateDescriptionHandler != null)
				UpdateDescriptionHandler();
		}
	}
}