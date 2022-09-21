using Avalonia.Controls;

namespace AvaloniaCalc {
    public partial class MainWindow:Window {
        public MainWindow() {
            InitializeComponent();
            historyLabel.Content = "";
        }
    }
}
