using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace UnitConverter
{
    internal class ConversionPage : ContentPage
    {
        readonly Dictionary<string, UnitOfMeasurementModel> _unitOfMeasurementDictionary;

        public Entry _firstUnitEntry = new Entry { Placeholder = "Enter your value", Keyboard = Keyboard.Numeric };
        public Picker _unitOfMeasurePicker = new Picker { Title = "Select Unit of Measure", SelectedItem = "OnUnitOfMeasurePickerSelectedItem" };
        public Picker _firstUnitPicker = new Picker { Title = "Values Unit", IsEnabled = false };
        public Picker _secondUnitPicker = new Picker { Title = "Converted Unit Type", IsEnabled = false };
        public Label _convertedUnitLabel = new Label { Text = "Output", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
        public Button _convertUnitButton = new Button { Text = "Convert" };

        public ConversionPage()
        {
			Title = "Unit Converter";

            _unitOfMeasurementDictionary = new Dictionary<string, UnitOfMeasurementModel>
            {
                { "Miles", Miles.Instance },
            	{ "Meters", Meters.Instance },
            	{ "Yards", Yards.Instance },
            	{ "Pounds", Pounds.Instance },
                { "Ounces", Ounces.Instance },
                { "Kilograms", Kilograms.Instance },
                { "Fahrenheit", Fahrenheit.Instance },
                { "Celsius", Celsius.Instance }
            };
            var unitPageGridLayout = new Grid()
            {
                RowSpacing = 2,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,

                ColumnSpacing = 20,

                Margin = new Thickness(30, 30, 30, 0),
                RowDefinitions ={
                    new RowDefinition{Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition{Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition{Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition{Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition{Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition{Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition{Height = new GridLength(1, GridUnitType.Star)}
                },
                ColumnDefinitions ={
                    new ColumnDefinition{Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition{Width = new GridLength(1, GridUnitType.Star)}
                }
            };

            unitPageGridLayout.Children.Add(_unitOfMeasurePicker, 0, 0);
            Grid.SetColumnSpan(_unitOfMeasurePicker, 2);

            unitPageGridLayout.Children.Add(_firstUnitEntry, 0, 2);
            unitPageGridLayout.Children.Add(_firstUnitPicker, 1, 2);
            unitPageGridLayout.Children.Add(_secondUnitPicker, 1, 3);
            unitPageGridLayout.Children.Add(_convertedUnitLabel, 0, 3);
            unitPageGridLayout.Children.Add(_convertUnitButton, 0, 5);
            Grid.SetColumnSpan(_convertUnitButton, 2);

            Content = unitPageGridLayout;

            foreach (UnitOfMeasurement unit in Enum.GetValues(typeof(UnitOfMeasurement)))
                _unitOfMeasurePicker.Items.Add(unit.ToString());

            _unitOfMeasurePicker.SelectedIndexChanged += OnUnitOfMeasureChanged;
            _firstUnitPicker.SelectedIndexChanged += _firstUnitPicker_SelectedIndexChanged;
            _secondUnitPicker.SelectedIndexChanged += _secondUnitPicker_SelectedIndexChanged;
            _convertUnitButton.Clicked += _convertUnitButton_Clicked;
        }

        void OnUnitOfMeasureChanged(object sender, System.EventArgs e)
        {
            var picker = sender as Picker;
            UnitOfMeasurement selectedUnitOfMeasure = (UnitOfMeasurement)picker.SelectedIndex;

            var unitsOfMeasureToAddToPicker = _unitOfMeasurementDictionary.Where(x => x.Value.MeasurementType.Equals(selectedUnitOfMeasure));

            _firstUnitPicker.Items.Clear();
            _secondUnitPicker.Items.Clear();

            foreach (var item in unitsOfMeasureToAddToPicker)
            {
                _firstUnitPicker.Items.Add(item.Key);
                _secondUnitPicker.Items.Add(item.Key);
            }

            _firstUnitPicker.IsEnabled = true;
            _secondUnitPicker.IsEnabled = false;
            _convertUnitButton.IsEnabled = false;
        }

        void _firstUnitPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            _secondUnitPicker.IsEnabled = true;
        }

        void _secondUnitPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            _convertUnitButton.IsEnabled = true;
        }

        void _convertUnitButton_Clicked(object sender, EventArgs e)
        {
			var firstUnitSelected = _unitOfMeasurementDictionary.FirstOrDefault(x => x.Key.Equals(_firstUnitPicker.SelectedItem.ToString())).Value;
			var secondUnitSelected = _unitOfMeasurementDictionary.FirstOrDefault(x => x.Key.Equals(_secondUnitPicker.SelectedItem.ToString())).Value;
			var inputAsBaseUnit = firstUnitSelected.ConvertToBaseUnits(double.Parse(_firstUnitEntry.Text));
			var convertToEndUnit = secondUnitSelected.ConvertFromBaseUnits(Math.Round(inputAsBaseUnit,2));
			_convertedUnitLabel.Text = Convert.ToString(convertToEndUnit);
        }

    }
}
