﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.ComponentModel;

namespace Controls
{
	public partial class TimePicker : UserControl
	{
		static readonly int HoursMax = 23;
		static readonly int MinutesMax = 59;
		static readonly int HoursMin = 0;
		static readonly int MinutesMin = 0;

		public static readonly DependencyProperty TimeProperty =
			DependencyProperty.Register("Time", typeof(DateTime), typeof(TimePicker),
			new FrameworkPropertyMetadata(new DateTime(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnTimePropertyChanged)));

		private static void OnTimePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			TimePicker timePicker = dp as TimePicker;
			if (timePicker != null)
			{
				timePicker.InitializeTime();
			}
		}

		public TimePicker()
		{
			InitializeComponent();
		}

		public DateTime Time
		{
			get { return (DateTime)GetValue(TimeProperty); }
			set { SetValue(TimeProperty, value); }
		}

		public void InitializeTime()
		{
			if (TextBox.Text.Length != 5)
				TextBox.Text = "00:00";
			Hours = Time.Hour;
			Minutes = Time.Minute;
		}

		int Hours
		{
			get 
			{
				int result = 0;
				int.TryParse(TextBox.Text.Substring(0, 2), out result);
				return result;
			}
			set
			{
				if (value > HoursMax)
					value = HoursMax;
				if (value < HoursMin)
					value = HoursMin;
				var stringValue = value.ToString("D2");
				var text = TextBox.Text.ToCharArray();
				var caretIndex = TextBox.CaretIndex;
				text[0] = stringValue[0];
				text[1] = stringValue[1];
				TextBox.Text = new string(text);
				TextBox.CaretIndex = caretIndex;
				Time = new DateTime(Time.Year, Time.Month, Time.Day, value, Time.Minute, 0); 
			}
		}

		int Minutes
		{
			get 
			{
				int result = 0;
				int.TryParse(TextBox.Text.Substring(3, 2), out result);
				return result;
			}
			set
			{
				if (value > MinutesMax)
					value = MinutesMax;
				if (value < MinutesMin)
					value = MinutesMin;
				var stringValue = value.ToString("D2");
				var text = TextBox.Text.ToCharArray();
				var caretIndex = TextBox.CaretIndex;
				text[3] = stringValue[0];
				text[4] = stringValue[1];
				TextBox.Text = new string(text);
				TextBox.CaretIndex = caretIndex;
				Time = new DateTime(Time.Year, Time.Month, Time.Day, Time.Hour, value, 0); 
			}
		}

		bool IsDigitKey(Key key)
		{
			if (
				key == System.Windows.Input.Key.D0 ||
				key == System.Windows.Input.Key.D1 ||
				key == System.Windows.Input.Key.D2 ||
				key == System.Windows.Input.Key.D3 ||
				key == System.Windows.Input.Key.D4 ||
				key == System.Windows.Input.Key.D5 ||
				key == System.Windows.Input.Key.D6 ||
				key == System.Windows.Input.Key.D7 ||
				key == System.Windows.Input.Key.D8 ||
				key == System.Windows.Input.Key.D9 ||
				key == System.Windows.Input.Key.NumPad0 ||
				key == System.Windows.Input.Key.NumPad1 ||
				key == System.Windows.Input.Key.NumPad2 ||
				key == System.Windows.Input.Key.NumPad3 ||
				key == System.Windows.Input.Key.NumPad4 ||
				key == System.Windows.Input.Key.NumPad5 ||
				key == System.Windows.Input.Key.NumPad6 ||
				key == System.Windows.Input.Key.NumPad7 ||
				key == System.Windows.Input.Key.NumPad8 ||
				key == System.Windows.Input.Key.NumPad9
			)
				return true;
			return false;
		}

		void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (IsDigitKey(e.Key))
			{
				if (TextBox.CaretIndex == 2)
					e.Handled = true;
				return;
			}
			var caretIndex = TextBox.CaretIndex;
			switch (e.Key)
			{
				case (Key.Left):
					if (TextBox.CaretIndex > 0)
						TextBox.CaretIndex--;
					e.Handled = true;
					break;
				case (Key.Right):
					if (TextBox.CaretIndex < TextBox.Text.Length)
						TextBox.CaretIndex++;
					e.Handled = true;
					break;
				case (Key.Up):
					SetValueAtIndex(GetValueAtIndex(caretIndex) + 1, caretIndex);
					e.Handled = true;
					break;
				case (Key.Down):
					SetValueAtIndex(GetValueAtIndex(caretIndex) - 1, caretIndex);
					e.Handled = true;
					break;
				default:
					e.Handled = true;
					break;
			}
		}

		void SetValueAtIndex(int value, int caretIndex)
		{
			if (caretIndex == 2)
				return;
			var text = TextBox.Text.ToCharArray();
			if (value < 0)
				value = 0;
			if (caretIndex == 0 && value > 2)
				value = 2;
			if (caretIndex == 3 && value > 5)
				value = 5;
			if ((caretIndex == 4 || caretIndex == 1) && value > 9)
				value = 9;
			text[TextBox.CaretIndex] = value.ToString()[0];
			TextBox.Text = new string(text);
			TextBox.CaretIndex = caretIndex;
			Time = new DateTime(Time.Year, Time.Month, Time.Day, Hours, Minutes, 0);
		}

		int GetValueAtIndex(int caretIndex)
		{
			if (caretIndex == 2)
				return -1;
			return (int)Char.GetNumericValue(TextBox.Text.ToCharArray()[caretIndex]);
		}

		void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			if (TextBox.Text.Length == 0)
				return;
			
			if (TextBox.SelectionStart >= TextBox.Text.Length)
				TextBox.SelectionStart = TextBox.Text.Length - 1;

			if (TextBox.SelectionLength != 1)
				TextBox.SelectionLength = 1;
		}

		void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (Hours > HoursMax)
				Hours = HoursMax;
			if (Hours < HoursMin)
				Hours = HoursMin;
			if (Minutes > MinutesMax)
				Minutes = MinutesMax;
			if (Minutes < MinutesMin)
				Minutes = MinutesMin;
		}

		private void Inc_Click(object sender, RoutedEventArgs e)
		{
			if (Hours < HoursMax)
				Hours++;
		}

		private void Dec_Click(object sender, RoutedEventArgs e)
		{
			if (Hours > HoursMin)
				Hours--;
		}

		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			Time = new DateTime(Time.Year, Time.Month, Time.Day, Hours, Minutes, 0);
		}

		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (e.Text.Any(x => !Char.IsDigit(x)))
				e.Handled = true;
			return;
		}
	}
}