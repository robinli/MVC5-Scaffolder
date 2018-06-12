using Happy.Scaffolding.MVC.UI.Base;
using System;

namespace Happy.Scaffolding.MVC.UI
{
    /// <summary>
    /// Interaction logic for WebFormsScaffolderDialog.xaml
    /// </summary>
    internal partial class ModelMetadataDialog : VSPlatformDialogWindow
    {
        public ModelMetadataDialog(ModelMetadataViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            InitializeComponent();

            viewModel.Close += result => DialogResult = result;

            DataContext = viewModel;
        }

       
    }
}
