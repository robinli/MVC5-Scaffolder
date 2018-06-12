using Happy.Scaffolding.MVC.UI.Base;
using System;


namespace Happy.Scaffolding.MVC.UI
{
    /// <summary>
    /// Interaction logic for SPScaffolderDialog.xaml
    /// </summary>
    internal partial class SPScaffolderDialog : VSPlatformDialogWindow
    {
        public SPScaffolderDialog(SPCodeGeneratorViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }
            
            InitializeComponent();
            
            //viewModel.PromptForNewDataContextTypeName += model =>
            //{
            //    var dialog = new NewDataContextDialog(model);
            //    var result = dialog.ShowModal();
            //    model.Canceled = !result.HasValue || !result.Value;
            //};

            viewModel.Close += result => DialogResult = result;

            DataContext = viewModel;
        }
    }
}
