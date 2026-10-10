using Catalyst.ViewModels;
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

namespace Catalyst
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WPF.Window.CatalystWindow
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            DataContext = viewModel = new MainViewModel();
            InitializeComponent();
        }

        private void PatternCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                viewModel.PatternCanvas_MouseLeftButtonDown(e.GetPosition(canvas));
            }
        }

        private void PatternCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                viewModel.PatternCanvas_MouseLeftButtonUp(e.GetPosition(canvas));
            }
        }

        private void PatternCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                viewModel.PatternCanvas_MouseMove(e.GetPosition(canvas), e);
            }
        }
    }
}