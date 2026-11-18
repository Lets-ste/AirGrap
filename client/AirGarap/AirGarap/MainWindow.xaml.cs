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

namespace AirGarap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TextBlock t0 = new TextBlock {Text = "str 1"};
            TextBlock t1 = new TextBlock { Text = "str 22" };
            TextBlock t2 = new TextBlock { Text = "str 3" };
            var panel = new StackPanel();
            panel.Children.Add(t0);
            panel.Children.Add(t1);
            panel.Children.Add(t2);
            Button B1 = new Button();
            B1.Content = panel;
            B1.Width = 100;
            B1.Height = 200;

            var root = new Grid();
            root.Children.Add(B1);
            Content = root;
        }
    }
}