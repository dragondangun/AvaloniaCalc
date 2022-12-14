using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.ComponentModel;

namespace AvaloniaCalc {
    public partial class MainWindow:Window {
        public MainWindow() {
            InitializeComponent();
            historyLabel.Content = "";
        }

        Operations operation = Operations.none;
        bool errorHappened = false;

        private void numberButton_OnClick(object? sender, RoutedEventArgs args) {
            if(errorHappened) {
                OnErrorSkip();
                return;
            }

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
            if(errorHappened) {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            bool? isNegative = currentString?.StartsWith('-');

            //explicit bool comprasion cause of nullable bool
            if(isNegative == false && currentString?.Length <= 1 || isNegative == true && currentString?.Length <=2) {
                currentLabel.Content = "0";
            }
            else {
                currentLabel.Content = $"{currentString?[..Convert.ToInt16(currentString?.Length - 1)]}";
            }

            currentLabelContentChanged();
        }



        private void clearEntryButton_OnClick(object? sender, RoutedEventArgs args) {
            if(errorHappened) {
                OnErrorSkip();
                return;
            }

            currentLabel.Content = "0";
            currentLabelContentChanged();
        }

        private void clearButton_OnClick(object? sender, RoutedEventArgs args) {
            if(errorHappened) {
                OnErrorSkip();
                return;
            }

            clear();
        }

        private void clear() {
            operation = Operations.none;
            currentLabel.Content = "0";
            historyLabel.Content = "";
            currentLabelContentChanged();
        }

        private void comaButton_OnClick(object? sender, RoutedEventArgs args) {
            if(errorHappened) {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            bool? isFractional = currentString?.Contains(',');
            
            if(isFractional == true) {
                return;
            }

            currentLabel.Content = $"{currentLabel.Content as string},"; 

            currentLabelContentChanged();
        }

        private void negateButton_OnClick(object? sender, RoutedEventArgs args) {
            if(errorHappened) {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);

            if(currentString?.Length == 1 && Convert.ToDouble(currentString) == 0) {
                return;
            }
            
            bool? isNegative = currentString?.StartsWith('-');

            if(isNegative == true) {
                currentLabel.Content = $"{(currentLabel.Content as string)?.TrimStart('-')}";
            }
            else {
                currentLabel.Content = $"-{currentLabel.Content as string}";
            }


            currentLabelContentChanged();
        }

        private void squareButton_OnClick(object? sender, RoutedEventArgs args) {
            if(errorHappened) {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);

            try {
                double? result = double.Parse(currentString);
                result *= result;

                if(double.IsNaN(result.Value) || double.IsInfinity(result.Value)) {
                    OnError();
                    return;
                }
                
                updateLabels(@$"{currentString}²", result?.ToString());
            }
            catch(Exception) {
                OnError();
                return;
            }
        }

        private void squareRootButton_OnClick(object? sender, RoutedEventArgs args) {
            if(errorHappened) {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);

            try {
                double? result = Math.Sqrt(double.Parse(currentString));

                if(double.IsNaN(result.Value) || double.IsInfinity(result.Value)) {
                    OnError();
                    return;
                }

                updateLabels(@$"sqrt({currentString})", result?.ToString());
            }
            catch(Exception) {
                OnError();
                return;
            }
        }

        private void percentButton_OnClick(object? sender, RoutedEventArgs args) {
            if(errorHappened) {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);

            try {
                double? result = double.Parse(currentString)/100;

                if(double.IsNaN(result.Value) || double.IsInfinity(result.Value)) {
                    OnError();
                    return;
                }

                updateLabels(result?.ToString(), result?.ToString());
            }
            catch(Exception) {
                OnError();
                return;
            }
        }

        private void reciprocalButton_OnClick(object? sender, RoutedEventArgs args) {
            if(errorHappened) {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            bool? isNegative = currentString?.StartsWith('-');
            double? result = null; 

            try {
                result = 1 / double.Parse(currentString);

                if(double.IsNaN(result.Value) || double.IsInfinity(result.Value)) {
                    OnError();
                    return;
                }

                string rightOperand = isNegative == true ? $"{1}/({currentString})" : $"{1}/{currentString}";
  
                updateLabels(rightOperand, result?.ToString());
            }
            catch(Exception) {
                OnError();
                return;
            }
        }


        private void operationButton_OnClick(object? sender, RoutedEventArgs args) {
            if(errorHappened) {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            string? senderString = (sender as Button)?.Content as string;

            Operations prevOperation = operation;

            operation = StringToOperations(senderString);
            
            double result = double.MinValue;

            try {
                if(prevOperation is not Operations.none) {
                    string? left = (historyLabel.Content as string)?.Split(' ')[0];
                    string? right = currentLabel.Content as string;
                    double leftPart = Convert.ToDouble(left);
                    double rightPart = Convert.ToDouble(right);
                    switch(prevOperation) {
                        case Operations.plus: {
                                result = leftPart + rightPart;
                                break;
                            }
                        case Operations.minus: {
                                result = leftPart - rightPart;
                                break;
                            }
                        case Operations.times: {
                                result = leftPart * rightPart;
                                break;
                            }
                        case Operations.divide: {
                                result = leftPart / rightPart;
                                break;
                            }
                        default: {
                                historyLabel.Content = $"{currentString} {senderString}";
                                currentLabel.Content = "0";
                                currentLabelContentChanged();
                                return;
                            }
                    }
                }
                else {
                    historyLabel.Content = $"{currentString} {senderString}";
                    currentLabel.Content = "0";
                    currentLabelContentChanged();
                    return;
                }
            }
            catch(FormatException ex) {
                operation = prevOperation;
                historyLabel.Content = currentLabel.Content as string;
                operationButton_OnClick(sender, args);
                return;
            }
            catch(Exception ex) {
                OnError();
                return;
            }

            if(double.IsNaN(result) || double.IsInfinity(result)) {
                OnError();
                return;
            }


            if(operation != Operations.equal) {
                historyLabel.Content = $"{result} {senderString}";
                currentLabel.Content = "0";
            }
            else if(prevOperation == Operations.equal) { 
                clear();
            }
            else {
                writeRightHistoryOperand($" {currentString} =");
                currentLabel.Content = result.ToString();
            }

            currentLabelContentChanged();
        }

        private void currentLabelContentChanged() {
            currentLabel.FontSize = (currentLabel.Content as string)?.Length switch {
                < 8 => 55,
                >= 8 and < 10 => 42,
                >= 10 and < 13 => 35,
                >= 13 and < 15 => 29,
                >= 15 => 26,
                null => 55
            };
        }

        enum Operations {
            none = 0,
            plus,
            minus,
            times,
            divide,
            equal
        }

        void writeRightHistoryOperand(string rightOperand) {
            string? historyString = (historyLabel.Content as string);
            string[]? historyStringArr = historyString?.Split(' ');

            if(historyStringArr?.Length < 3) {
                historyString += $"{rightOperand}";
            }
            else {
                historyStringArr[2] = $"{rightOperand}";
                historyString = $"{historyStringArr[0]} {historyStringArr[1]} {historyStringArr[2]}";
            }

            historyLabel.Content = historyString;
        }

        void OnErrorSkip() {
            errorHappened = false;
            clear();
            currentLabel.Foreground = Brushes.Black;
        }

        void OnError() {
            clear();
            errorHappened = true;
            currentLabel.Content = "ОШИБКА";
            currentLabel.Foreground = Brushes.Red;
        }

        void updateLabels(string rightOperand, string result) {
            if(operation != Operations.none && operation != Operations.equal) {
                writeRightHistoryOperand(@$" {rightOperand}");
            }
            else {
                historyLabel.Content = @$"{rightOperand}";
            }

            currentLabel.Content = result;

            currentLabelContentChanged();
        }

        Operations StringToOperations(string s) {
            switch(s) {
                case "+":
                    return Operations.plus;
                case @"⨉":
                    return Operations.times;
                case "/":
                    return Operations.divide;
                case @"—":
                    return Operations.minus;
                case "=":
                    return Operations.equal;
                default:
                    return Operations.none;
            }
        }
    }
}
