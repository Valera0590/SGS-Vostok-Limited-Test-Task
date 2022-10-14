using System.Windows;
using System.Windows.Controls;
using ManageStaff.ViewModel;

namespace ManageStaff.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Automatically resize height and width relative to content
            this.SizeToContent = SizeToContent.WidthAndHeight;
            InitializeComponent();

        }
    }
}
