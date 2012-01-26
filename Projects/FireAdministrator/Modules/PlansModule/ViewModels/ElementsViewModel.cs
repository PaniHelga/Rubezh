﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecAPI.Models;
using Infrastructure;
using Infrastructure.Common;
using PlansModule.Designer;
using PlansModule.Events;

namespace PlansModule.ViewModels
{
    public class ElementsViewModel : BaseViewModel
    {
        public ElementsViewModel(DesignerCanvas designerCanvas)
        {
            ServiceFactory.Events.GetEvent<ElementAddedEvent>().Subscribe(OnElementAdded);
            ServiceFactory.Events.GetEvent<ElementRemovedEvent>().Subscribe(OnElementRemoved);
            ServiceFactory.Events.GetEvent<ElementChangedEvent>().Subscribe(OnElementChanged);
            ServiceFactory.Events.GetEvent<ElementSelectedEvent>().Subscribe(OnElementSelected);
            DesignerCanvas = designerCanvas;

            Elements = new ObservableCollection<ElementBaseViewModel>();
            Update();
        }

        DesignerCanvas DesignerCanvas;
        public ObservableCollection<ElementBaseViewModel> Elements { get; set; }
        ElementGroupViewModel ElementDevices;
        ElementGroupViewModel ElementZones;
        ElementGroupViewModel ElementSubPlans;
        ElementGroupViewModel ElementElements;

        ElementViewModel _selectedElement;
        public ElementViewModel SelectedElement
        {
            get { return _selectedElement; }
            set
            {
                _selectedElement = value;
                OnPropertyChanged("SelectedElement");
            }
        }

        void OnPlansChanged(Guid planUID)
        {
            Update();
        }

        public void Update()
        {
            AllElements = new List<ElementViewModel>();
            Elements.Clear();
            Elements.Add(ElementDevices = new ElementGroupViewModel(Elements, this, "Устройства", "Device"));
            Elements.Add(ElementZones = new ElementGroupViewModel(Elements, this, "Зоны", "Zone"));
            Elements.Add(ElementSubPlans = new ElementGroupViewModel(Elements, this, "Подпланы", "SubPlan"));
            Elements.Add(ElementElements = new ElementGroupViewModel(Elements, this, "Элементы", "Element"));

            foreach (var designerItem in DesignerCanvas.Items)
            {
                AddDesignerItem(designerItem);
            }

            CollapseChild(ElementDevices);
            ExpandChild(ElementDevices);
            CollapseChild(ElementZones);
            ExpandChild(ElementZones);
            CollapseChild(ElementSubPlans);
            ExpandChild(ElementSubPlans);
            CollapseChild(ElementElements);
            ExpandChild(ElementElements);
        }

        #region ElementSelection

        public List<ElementViewModel> AllElements;

        public void Select(Guid elementUID)
        {
            var elementViewModel = AllElements.FirstOrDefault(x => x.ElementUID == elementUID);
            if (elementViewModel != null)
                elementViewModel.ExpantToThis();
            SelectedElement = elementViewModel;
        }

        #endregion

        void AddElement(ElementGroupViewModel parentElementViewModel, ElementViewModel elementViewModel)
        {
            elementViewModel.Parent = parentElementViewModel;
            parentElementViewModel.Children.Add(elementViewModel);
            var indexOf = Elements.IndexOf(parentElementViewModel);
            Elements.Insert(indexOf + 1, elementViewModel);
            AllElements.Add(elementViewModel);
        }

        void AddDesignerItem(DesignerItem designerItem)
        {
            string name = "";
            var elementBase = designerItem.ElementBase;

            if (elementBase is ElementDevice)
            {
                ElementDevice elementDevice = elementBase as ElementDevice;
                name = elementDevice.Device.DottedAddress + " " + elementDevice.Device.Driver.ShortName;
                AddElement(ElementDevices, new ElementViewModel(Elements, designerItem, name, "Device"));
            }
            if (elementBase is IElementZone)
            {
                IElementZone elementZone = elementBase as IElementZone;
                if (elementZone.Zone != null)
                {
                    name = elementZone.Zone.PresentationName;
                }
                else
                {
                    name = "Несвязанная зона";
                }
                AddElement(ElementZones, new ElementViewModel(Elements, designerItem, name, "Zone"));
            }
            if (elementBase is ElementSubPlan)
            {
                ElementSubPlan elementSubPlan = elementBase as ElementSubPlan;
                if (elementSubPlan.Plan != null)
                {
                    name = elementSubPlan.Plan.Caption;
                }
                else
                {
                    name = "Несвязанный подплан";
                }
                AddElement(ElementElements, new ElementViewModel(Elements, designerItem, name, "SubPlan"));
            }
            if (elementBase is ElementEllipse)
            {
                AddElement(ElementElements, new ElementViewModel(Elements, designerItem, "Эллипс", "Element"));
            }
            if (elementBase is ElementPolygon)
            {
                AddElement(ElementElements, new ElementViewModel(Elements, designerItem, name, "SubPlan"));
            }
            if (elementBase is ElementRectangle)
            {
                AddElement(ElementElements, new ElementViewModel(Elements, designerItem, "Прямоугольник", "Element"));
            }
            if (elementBase is ElementTextBlock)
            {
                AddElement(ElementElements, new ElementViewModel(Elements, designerItem, "Надпись", "Element"));
            }
        }

        void OnElementAdded(List<ElementBase> elements)
        {
            foreach (var elementBase in elements)
            {
                var designerItem = DesignerCanvas.Items.FirstOrDefault(x => x.ElementBase.UID == elementBase.UID);
                if (designerItem != null)
                {
                    AddDesignerItem(designerItem);
                }
            }
        }

        void OnElementRemoved(List<ElementBase> elements)
        {
            foreach (var elementBase in elements)
            {
                var element = AllElements.FirstOrDefault(x => x.ElementUID == elementBase.UID);
                if (element != null)
                {
                    Elements.Remove(element);
                }
            }
        }

        void OnElementChanged(List<ElementBase> elements)
        {
        }

        void OnElementSelected(Guid elementUID)
        {
            Select(elementUID);
        }

        void CollapseChild(ElementBaseViewModel parentElementViewModel)
        {
            parentElementViewModel.IsExpanded = false;

            foreach (var elementViewModel in parentElementViewModel.Children)
            {
                CollapseChild(elementViewModel);
            }
        }

        void ExpandChild(ElementBaseViewModel parentElementViewModel)
        {
            parentElementViewModel.IsExpanded = true;
            foreach (var elementViewModel in parentElementViewModel.Children)
            {
                ExpandChild(elementViewModel);
            }
        }
    }
}