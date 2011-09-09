﻿using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common;
using FiresecAPI.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using PlansModule.Views;
using System.Windows.Input;

namespace PlansModule.ViewModels
{
    public class RectangleAdornerResize : Adorner
    {
        

        private UIElement currentElement = null;
        private Rectangle rectangle = null;
        public Canvas canvas;
        private double _leftOffset = 0;
        private double _rightOffset = 0;
        private double _topOffset = 0;
        private double _heightOffset = 0;
        private double _widthOffset = 0;
        Rect adornedElementRect;
        int number;
        private double MinWidth;
        private double MinHeight;
        private Rectangle _childRect = null;
        public RectangleAdornerResize(UIElement adornedElement)
            : base(adornedElement)
        {
            VisualBrush _brush = new VisualBrush(adornedElement);
            currentElement = adornedElement;
            Rectangle rect = (Rectangle)currentElement;
            _childRect = new Rectangle();
            _childRect.Width = adornedElement.RenderSize.Width;
            _childRect.Height = adornedElement.RenderSize.Height;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
            renderBrush.Opacity = 0.2;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);

            adornedElementRect = new Rect(this.AdornedElement.DesiredSize);
            //adornedElementRect.Y = adornedElementRect.Y + PlanCanvasView.dTop;
            drawingContext.DrawRectangle(null, renderPen, adornedElementRect);
        }

        public void SetCanvas(Canvas canvas, double minwidth, double minheight)
        {
            this.canvas = canvas;
            this.MinHeight = minheight;
            this.MinWidth = minwidth;

        }
        public void SetRect(Rect rect, int number)
        {
            this.adornedElementRect = rect;
            this.number = number;
        }
        protected override Size MeasureOverride(Size constraint)
        {
            _childRect.Measure(constraint);
            return _childRect.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _childRect.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _childRect;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }
        double tmpX = 0;
        double tmpY = 0;
        public double HeightOffset
        {
            get
            {
                return _heightOffset;
            }
            set
            {
                _heightOffset = value;
                UpdatePosition();
            }
        }
        public double WidthOffset
        {
            get
            {
                return _widthOffset;
            }
            set
            {
                _widthOffset = value;
                UpdatePosition();
            }
        }
        
        public double LeftOffset
        {
            get
            {
                return _leftOffset;
            }
            set
            {
                _leftOffset = value;
                UpdatePosition();
            }
        }
        public double RightOffset
        {
            get
            {
                return _rightOffset;
            }
            set
            {
                _rightOffset = value;
                UpdatePosition();
            }
        }
        public double TopOffset
        {
            get
            {
                return _topOffset;
            }
            set
            {
                _topOffset = value;
                UpdatePosition();
            }

        }

        private void UpdatePosition()
        {
            AdornerLayer adornerLayer = this.Parent as AdornerLayer;
            if (adornerLayer != null)
            {
                adornerLayer.Update(AdornedElement);
            }
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(_leftOffset, _topOffset));

            return result;
        }
    }
}
