using System;
using System.Linq.Expressions;
using System.Windows.Input;

namespace Microsoft.AspNet.Scaffolding.WebForms.UI
{
    internal class WebFormsNewDataContextViewModel : ViewModel<WebFormsNewDataContextViewModel>
    {
        public WebFormsNewDataContextViewModel(string defaultDbContextTypeName)
        {
            DbContextTypeName = defaultDbContextTypeName;
        }

        private DelegateCommand _okCommand;

        public ICommand OkCommand
        {
            get
            {
                if (_okCommand == null)
                {
                    _okCommand = new DelegateCommand(_ =>
                    {
                        Validate(propertyName: null);

                        if (!HasErrors)
                        {
                            OnClose(result: true);
                        }
                    });
                }

                return _okCommand;
            }
        }

        public event Action<bool> Close;

        private void OnClose(bool result)
        {
            if (Close != null)
            {
                Close(result);
            }
        }

        private string _dbContextTypeName;

        public string DbContextTypeName
        {
            get { return _dbContextTypeName; }
            private set
            {
                if (value == _dbContextTypeName)
                {
                    return;
                }

                _dbContextTypeName = value;
                OnPropertyChanged(m => m.DbContextTypeName);
            }
        }

        protected override void Validate(string propertyName)
        {
            var validateAll = String.IsNullOrEmpty(propertyName);
            string currentPropertyName;

            currentPropertyName = PropertyName(m => m.DbContextTypeName);
            if (validateAll || ShouldValidate(propertyName, currentPropertyName))
            {
                ClearError(currentPropertyName);
                if (String.IsNullOrWhiteSpace(DbContextTypeName))
                {
                    AddError(currentPropertyName, NewDataContextDialogResources.Error_NewDataContextRequired);
                }
                // Ensure it contains at least one period and it doesn't begin with a period
                else if (DbContextTypeName.IndexOf('.') < 1)
                {
                    AddError(currentPropertyName, NewDataContextDialogResources.Error_DataContextNameRequiresPeriod);
                }
            }
        }

        public bool Canceled { get; set; }
    }
}
