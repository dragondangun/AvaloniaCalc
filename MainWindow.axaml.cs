using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace AvaloniaCalc {
    public partial class MainWindow:Window {
        public MainWindow() {
            InitializeComponent();
            historyLabel.Content = "";
        }

        private void numberButton_OnClick(object? sender, RoutedEventArgs args) {
            string? currentString = (currentLabel.Content as string);
            bool? isFractional = currentString?.Contains(',');

            //explicit bool comprasion cause of nullable bool
            if(isFractional==false && currentString?.Length >= 16 || isFractional==true && currentString?.Length >= 17) {
                return;
            }

            if(currentString?.Length == 1 && Convert.ToDouble(currentString) == 0) {
                currentLabel.Content = (sender as Button)?.Content;
                return;
            }

            string? senderString = ((sender as Button)?.Content as string);

            currentLabel.Content = $"{currentString}{senderString}";
        }

    }
}
