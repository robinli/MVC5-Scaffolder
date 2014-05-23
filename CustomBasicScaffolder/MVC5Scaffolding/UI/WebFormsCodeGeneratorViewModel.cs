using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EnvDTE;
using Microsoft.AspNet.Scaffolding.EntityFramework;
using EnvDTE80;

namespace Microsoft.AspNet.Scaffolding.WebForms.UI
{
    internal class WebFormsCodeGeneratorViewModel : ViewModel<WebFormsCodeGeneratorViewModel>
    {
        private ObservableCollection<ModelType> _dbContextTypeCollection;
        private ObservableCollection<ModelType> _modelTypeCollection;
        private List<String> _projectPaths;
        private List<String> _masterPagePaths;

        private readonly CodeGenerationContext _context;
        private readonly ModelType _addNewDataContextItem =
            new ModelType(typeName: null) { DisplayName = Resources.WebFormsCodeGeneratorViewModel_AddNewDbContext };

        public WebFormsCodeGeneratorViewModel(CodeGenerationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;
            _useMasterPage = true;
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
                            if (DbContextModelType == null || DbContextModelType == _addNewDataContextItem)
                            {
                                OnPromptForNewDataContext();
                            }

                            if (DbContextModelType != null && DbContextModelType != _addNewDataContextItem)
                            {
                                OnClose(result: true);
                            }
                        }
                    });
                }

                return _okCommand;
            }
        }

        public event Action<WebFormsNewDataContextViewModel> PromptForNewDataContextTypeName;

        private void OnPromptForNewDataContext()
        {
            if (PromptForNewDataContextTypeName != null)
            {
                var viewModel = new WebFormsNewDataContextViewModel(DefaultDataContextTypeName);

                PromptForNewDataContextTypeName(viewModel);

                if (!viewModel.Canceled)
                {
                    var newDbContextModelType = new ModelType(viewModel.DbContextTypeName);
                    DataContextTypeCollection.Add(newDbContextModelType);
                    DbContextModelType = newDbContextModelType;
                }
                else
                {
                    DbContextModelType = null;
                    DbContextModelTypeName = null;
                }
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

        private ModelType _modelType;

        public ModelType ModelType
        {
            get { return _modelType; }
            set
            {
                Validate();

                if (value == _modelType)
                {
                    return;
                }

                _modelType = value;
                OnPropertyChanged();
            }
        }

        private string _modelTypeName;

        public string ModelTypeName
        {
            get { return _modelTypeName; }
            set
            {
                Validate();

                if (value == _modelTypeName)
                {
                    return;
                }

                _modelTypeName = value;
                if (ModelType != null)
                {
                    if (ModelType.DisplayName.StartsWith(_modelTypeName, StringComparison.Ordinal))
                    {
                        _modelTypeName = ModelType.DisplayName;
                    }
                    else
                    {
                        ModelType = null;
                    }
                }
                OnPropertyChanged();
            }
        }

        private ModelType _dbContextModelType;

        public ModelType DbContextModelType
        {
            get { return _dbContextModelType; }
            set
            {
                Validate();

                if (value == _dbContextModelType)
                {
                    return;
                }

                _dbContextModelType = value;

                if (_dbContextModelType == _addNewDataContextItem)
                {
                    // OnPromptForNewDataContext();
                    return;
                }

                OnPropertyChanged();
            }
        }

        private string _dbContextModelTypeName;

        public string DbContextModelTypeName
        {
            get { return _dbContextModelTypeName; }
            set
            {
                Validate();

                if (value == _dbContextModelTypeName)
                {
                    return;
                }

                _dbContextModelTypeName = value;
                if (DbContextModelType != null)
                {
                    if (DbContextModelType.DisplayName.StartsWith(_dbContextModelTypeName, StringComparison.Ordinal))
                    {
                        _dbContextModelTypeName = DbContextModelType.DisplayName;
                    }
                    else
                    {
                        DbContextModelType = null;
                    }
                }
                OnPropertyChanged();
            }
        }


        private bool _useMasterPage;

        public bool UseMasterPage
        {
            get { return _useMasterPage; }
            set
            {
                Validate();

                if (value == _useMasterPage)
                {
                    return;
                }

                _useMasterPage = value;
                OnPropertyChanged();
                OnPropertyChanged(m => m.UseMasterPage);
                OnPropertyChanged(m => m.DesktopPlaceholderId);
            }
        }



        private string _desktopMasterPage;

        public string DesktopMasterPage
        {
            get { return _desktopMasterPage; }
            set
            {
                Validate();

                if (value == _desktopMasterPage)
                {
                    return;
                }

                _desktopMasterPage = value;

                OnPropertyChanged();
            }
        }

        private string _desktopPlaceholderId;

        public string DesktopPlaceholderId
        {
            get { return _desktopPlaceholderId; }
            set
            {
                Validate();

                if (value == _desktopPlaceholderId)
                {
                    return;
                }

                _desktopPlaceholderId = value;

                OnPropertyChanged();
            }
        }


        private bool _overwriteViews;

        public bool OverwriteViews
        {
            get { return _overwriteViews; }
            set
            {
                Validate();

                if (value == _overwriteViews)
                {
                    return;
                }

                _overwriteViews = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ModelType> ModelTypeCollection
        {
            get
            {
                if (_modelTypeCollection == null)
                {
                    ICodeTypeService codeTypeService = GetService<ICodeTypeService>();
                    Project project = _context.ActiveProject;


                    var modelTypes = codeTypeService
                                        .GetAllCodeTypes(project)
                                        .Where(codeType => codeType.IsValidWebProjectEntityType() && IsReallyValidWebProjectEntityType(codeType))
                                        .Select(codeType => new ModelType(codeType));
                    _modelTypeCollection = new ObservableCollection<ModelType>(modelTypes);
                }
                return _modelTypeCollection;
            }
        }


        private bool IsReallyValidWebProjectEntityType(CodeType codeType)
        {
            return !IsAbstract(codeType);
        }

        private bool IsAbstract(CodeType codeType) {
            CodeClass2 codeClass2 = codeType as CodeClass2;
            if (codeClass2 != null) {
                return codeClass2.IsAbstract;
            } else {
                return false;
            }
        }


        public ObservableCollection<ModelType> DataContextTypeCollection
        {
            get
            {
                if (_dbContextTypeCollection == null)
                {
                    ICodeTypeService codeTypeService = GetService<ICodeTypeService>();
                    Project project = _context.ActiveProject;

                    var modelTypes = codeTypeService
                                        .GetAllCodeTypes(project)
                                        .Where(codeType => codeType.IsValidDbContextType())
                                        .Select(codeType => new ModelType(codeType));
                    _dbContextTypeCollection = new ObservableCollection<ModelType>(modelTypes);
                    _dbContextTypeCollection.Insert(0, _addNewDataContextItem);
                }
                return _dbContextTypeCollection;
            }
        }

        public IEnumerable<String> DesktopMasterPagePaths
        {
            get
            {
                return MasterPagePaths;
            }
        }


        private IEnumerable<string> MasterPagePaths
        {
            get
            {
                if (_masterPagePaths == null)
                {
                    LoadMasterPagePaths();
                }
                return _masterPagePaths;
            }
        }

        public string DefaultDataContextTypeName
        {
            get
            {
                var project = _context.ActiveProject;

                return String.Format(CultureInfo.InvariantCulture, "{0}.Models.{1}Context", project.GetDefaultNamespace(), project.Name);
            }
        }

        protected override void Validate([CallerMemberName]string propertyName = "")
        {
            string currentPropertyName;

            // ModelType
            currentPropertyName = PropertyName(m => m.ModelType);
            if (ShouldValidate(propertyName, currentPropertyName))
            {
                ClearError(currentPropertyName);
                if (ModelType == null)
                {
                    AddError(currentPropertyName, WebFormsScaffolderDialogResources.Error_ModelTypeRequired);
                }
            }

            // DesktopMasterPage
            currentPropertyName = PropertyName(m => m.DesktopMasterPage);
            if (ShouldValidate(propertyName, currentPropertyName))
            {
                ClearError(currentPropertyName);
                if (UseMasterPage &&
                    (String.IsNullOrWhiteSpace(DesktopMasterPage) ||
                    !DesktopMasterPagePaths.Contains(DesktopMasterPage)))
                {
                    AddError(currentPropertyName, WebFormsScaffolderDialogResources.Error_DesktopMasterPageRequired);
                }
            }

            // DesktopPlaceholderId
            currentPropertyName = PropertyName(m => m.DesktopPlaceholderId);
            if (ShouldValidate(propertyName, currentPropertyName))
            {
                ClearError(currentPropertyName);
                if (UseMasterPage &&
                    String.IsNullOrWhiteSpace(DesktopPlaceholderId))
                {
                    AddError(currentPropertyName, WebFormsScaffolderDialogResources.Error_PlaceholderIdRequired);
                }
            }

        }

        private void LoadMasterPagePaths()
        {
            _masterPagePaths = ProjectPaths.Where(path => String.Equals(Path.GetExtension(path), ".master", StringComparison.OrdinalIgnoreCase))
                .ToList();
            _desktopMasterPage = _masterPagePaths.FirstOrDefault(path => !path.EndsWith(".mobile.master", StringComparison.OrdinalIgnoreCase));

            // Extract these from the content of the master pages themselves when selected by the user: Tracked by 721707.
            _desktopPlaceholderId = "MainContent";
        }

        // Do a breadth first search for project paths.
        // Does it make sense to move this logic to architecture? As part of UI consistency feature, there will be
        // a file picker dialog for picking the layouts and this implementation should go away.
        private List<string> ProjectPaths
        {
            get
            {
                if (_projectPaths == null)
                {
                    var project = _context.ActiveProject;
                    var projectRoot = project.GetFullPath();
                    _projectPaths = new List<String>();

                    var projectItems = new Stack<ProjectItems>();
                    projectItems.Push(project.ProjectItems);

                    while (projectItems.Any())
                    {
                        var currentItems = projectItems.Pop();
                        foreach (ProjectItem item in currentItems)
                        {
                            if (item.Kind == VsConstants.VSProjectItemKindPhysicalFile)
                            {
                                var fullPath = item.GetFullPath();
                                _projectPaths.Add(fullPath.Substring(projectRoot.Length));
                            }
                            if (item.Kind == VsConstants.VSProjectItemKindPhysicalFolder)
                            {
                                projectItems.Push(item.ProjectItems);
                            }
                        }
                    }
                }
                return _projectPaths;
            }
        }

        private TService GetService<TService>() where TService : class
        {
            return (TService)_context.ServiceProvider.GetService(typeof(TService));
        }
    }
}
