using System;
using Microsoft.AspNet.Scaffolding.VSExtension.UI;

namespace Microsoft.AspNet.Scaffolding.WebForms.UI
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
