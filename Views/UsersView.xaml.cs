using System.Windows.Controls;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    /// <summary>
    /// Code-behind pour UsersView
    /// </summary>
    public partial class UsersView : UserControl
    {
        public UsersView()
        {
            InitializeComponent();
        }

        public UsersView(UsersViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}