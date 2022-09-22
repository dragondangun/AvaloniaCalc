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

        private void reciprocalButton_OnClick(object? sender, RoutedEventArgs args) {
            if(errorHappened) {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            string? historyString = (historyLabel.Content as string);
            string[]? historyStringArr = historyString?.Split(' ');
            bool? isNegative = currentString?.StartsWith('-');
            double? result = null; 

            try {
                result = 1 / double.Parse(currentString);

                if(operation != Operations.none && operation != Operations.equal) {
                    if(historyStringArr.Length < 3) {
                        if(isNegative == false) {
                            historyString += $" {1}/{currentString}";
                        }
                        else {
                            historyString += $" {1}/({currentString})";
                        }

                    }
                    else {
                        if(isNegative == false) {
                            historyStringArr[2] = $" {1}/{currentString}";
                        }
                        else {
                            historyStringArr[2] = $" {1}/({currentString})";
                        }

                        historyString = $"{historyStringArr[0]} {historyStringArr[1]} {historyStringArr[2]}";
                    }
                }
                else {
                    if(isNegative == false) {
                        historyString = $"{1}/{currentString}";
                    }
                    else {
                        historyString = $"{1}/({currentString})";
                    }


                }

                historyLabel.Content = historyString;
                currentLabel.Content = result.ToString();

                if(double.IsNaN(result.Value) || double.IsInfinity(result.Value)) {
                    OnError();
                    return;
                }

                currentLabelContentChanged();
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
            string? historyString = historyLabel.Content as string;

            Operations prevOperation = operation;

            switch(senderString) {
                case "+":
                    operation = Operations.plus;
                    break;
                case @"⨉":
                    operation = Operations.times;
                    break;
                case "/":
                    operation = Operations.divide;
                    break;
                case @"—":
                    operation = Operations.minus;
                    break;
                case "=":
                    operation = Operations.equal;
                    break;
            }
            
            double result = double.MinValue;

            try {
                if(prevOperation != Operations.none) {
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
            catch(Exception) {
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
                historyLabel.Content = $"{historyString} {currentString} =";
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
    }
}
