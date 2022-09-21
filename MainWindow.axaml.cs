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

            //explicit bool comprasion cause of nullable bool
            if(isFractional == false && currentString?.Length >= 16 || isFractional == true && currentString?.Length >= 17) {
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

        private void currentLabelContentChanged() {
            string? currentString = (currentLabel.Content as string);
            if(currentString?.Length > 7) {
                currentLabel.FontSize = 42;
            }
            else {
                currentLabel.FontSize = 55;
            }

            currentLabel.FontSize = currentString?.Length switch {
                <= 7 => 55,
                > 7 and < 10 => 42,
                >= 10 => 29,
                null => 55
            };
        }

    }
}
