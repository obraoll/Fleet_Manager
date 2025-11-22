using System.Windows.Controls;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    public partial class UsersView : UserControl
    {
        public UsersView(UsersViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
