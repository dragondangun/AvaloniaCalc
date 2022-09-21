using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.ComponentModel;

namespace AvaloniaCalc {
    public partial class MainWindow:Window {
        public MainWindow() {
            InitializeComponent();
            historyLabel.Content = "";
        }

        private void numberButton_OnClick(object? sender, RoutedEventArgs args) {
            string? currentString = (currentLabel.Content as string);
            bool? isFractional = currentString?.Contains('.');
            bool? isNegative = currentString?.Contains('-');

            //explicit bool comprasion cause of nullable bool
            if(isFractional == false && isNegative== false && currentString?.Length >= 16 ||
                (isFractional ^ isNegative) == true && currentString?.Length >= 17 ||
                    isFractional == true && isNegative == true && currentString?.Length >= 18)
            {
            }
            else if(currentString?.Length == 1 && Convert.ToDouble(currentString) == 0) {
                currentLabel.Content = (sender as Button)?.Content;
            }
            else {
                string? senderString = ((sender as Button)?.Content as string);
                currentLabel.Content = $"{currentString}{senderString}";
            }

            currentLabelContentChanged();
        }

        private void eraseButton_OnClick(object? sender, RoutedEventArgs args) {
            string? currentString = (currentLabel.Content as string);

            //explicit bool comprasion cause of nullable bool
            if(currentString?.Length <= 1) {
                currentLabel.Content = "0";
            }
            else {
                currentLabel.Content = $"{currentString?[..Convert.ToInt16(currentString?.Length - 1)]}";
            }

            currentLabelContentChanged();
        }

        private void currentLabelContentChanged() {
            string? currentString = (currentLabel.Content as string);
            if(currentString?.Length > 7) {
                currentLabel.FontSize = 42;
            }
            else {
                currentLabel.FontSize = 55;
            }

            currentLabel.FontSize = currentString?.Length switch {
                < 8 => 55,
                >= 8 and < 10 => 42,
                >= 10 and < 13 => 35,
                >= 13 and < 15 => 29,
                >= 15 => 26,
                null => 55
            };
        }

    }
}
