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
using Microsoft.AspNet.Scaffolding;
using EnvDTE80;
using Happy.Scaffolding.MVC.Models;
using System.Windows;
using Happy.Scaffolding.MVC.Utils;


namespace Happy.Scaffolding.MVC.UI
{
    internal class SPCodeGeneratorViewModel : ViewModel<MvcCodeGeneratorViewModel>
    {
        private ObservableCollection<ModelType> _dbContextTypeCollection;
        private ObservableCollection<MethodType> _methodTypeCollection;
        private MetadataSettingViewModel _queryFormViewModel;
        private MetadataSettingViewModel _resultListViewModel;

        private List<String> _projectPaths;
        private List<String> _masterPagePaths;

        private readonly CodeGenerationContext _context;

        public SPCodeGeneratorViewModel(CodeGenerationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;
            //_useMasterPage = true;
            _GenerateViews = true;
            _ReferenceScriptLibraries = true;
            _LayoutPageSelected = true;
        }

        #region Switch Tab / Button event

        private int CurrentStepIndex = 0;
        //private void ShowStep(int stepIndex)
        //{
        //    Step1Visibale = (CurrentStepIndex == 1 ? Visibility.Visible : Visibility.Collapsed);
        //    Step2Visibale = (CurrentStepIndex == 2 ? Visibility.Visible : Visibility.Collapsed);
        //    Step3Visibale = (CurrentStepIndex == 3 ? Visibility.Visible : Visibility.Collapsed);
        //}
        //private Visibility _Step1Visibale = Visibility.Visible;
        //private Visibility _Step2Visibale = Visibility.Collapsed;
        //private Visibility _Step3Visibale = Visibility.Collapsed;
        //public Visibility Step1Visibale
        //{
        //    get { return this._Step1Visibale; }
        //    set 
        //    {
        //        if (value == this._Step1Visibale) return;
        //        this._Step1Visibale = value;
        //        OnPropertyChanged();
        //    }
        //}
        //public Visibility Step2Visibale
        //{
        //    get { return this._Step2Visibale; }
        //    set
        //    {
        //        if (value == this._Step2Visibale) return;
        //        this._Step2Visibale = value;
        //        OnPropertyChanged();
        //    }
        //}
        //public Visibility Step3Visibale
        //{
        //    get { return this._Step3Visibale; }
        //    set
        //    {
        //        if (value == this._Step3Visibale) return;
        //        this._Step3Visibale = value;
        //        OnPropertyChanged();
        //    }
        //}

        private ObservableCollection<Visibility> _StepVisibale
            = new ObservableCollection<Visibility>() { Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed };

        public ObservableCollection<Visibility> StepVisibale 
        {
            get 
            {
                return _StepVisibale;
            }
        }

        public void ShowStep()
        {
            for(int x=0; x<StepVisibale.Count; x++)
            {
                StepVisibale[x] = (x == CurrentStepIndex ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        private DelegateCommand _NextStepCommand;
        public ICommand NextStepCommand
        {
            get
            {
                if (_NextStepCommand == null)
                {
                    _NextStepCommand = new DelegateCommand(_ =>
                    {
                        Validate(propertyName: null);
                        if (!HasErrors)
                        {
                            CurrentStepIndex += 1;
                            if (CurrentStepIndex==1)
                                CreateViewModelFromMethodParameters();
                            
                            ShowStep();
                        }
                    });
                }
                return _NextStepCommand;
            }
        }

        private DelegateCommand _BackStepCommand;
        public ICommand BackStepCommand
        {
            get
            {
                if (_BackStepCommand == null)
                {
                    _BackStepCommand = new DelegateCommand(_ =>
                    {
                        Validate(propertyName: null);
                        if (!HasErrors)
                        {
                            CurrentStepIndex -= 1;
                            ShowStep();
                        }
                    });
                }
                return _BackStepCommand;
            }
        }



        //private DelegateCommand _Step1Command;
        //public ICommand Step1Command
        //{
        //    get
        //    {
        //        if (_Step1Command == null)
        //        {
        //            _Step1Command = new DelegateCommand(_ =>
        //            {
        //                Validate(propertyName: null);
        //                if (!HasErrors)
        //                {
        //                    CreateViewModelFromMethodParameters();
        //                    ShowStep(2);
        //                }
        //            });
        //        }
        //        return _Step1Command;
        //    }
        //}

        //private DelegateCommand _Step2Command;
        //public ICommand Step2Command
        //{
        //    get
        //    {
        //        if (_Step2Command == null)
        //        {
        //            _Step2Command = new DelegateCommand(_ =>
        //            {
        //                Validate(propertyName: null);
        //                if (!HasErrors)
        //                {
        //                    ShowStep(3);
        //                }
        //            });
        //        }
        //        return _Step2Command;
        //    }
        //}

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
                            if (DbContextModelType == null)
                            {
                                //OnPromptForNewDataContext();
                            }

                            if (DbContextModelType != null)
                            {
                                SaveDesignData();
                                OnClose(result: true);
                            }
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

        #endregion

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

                //_ControllerName = _modelType.ShortName + "Controller";
                //OnPropertyChanged(m => m.ControllerName);
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

        private string _methodTypeName;

        public string MethodTypeName
        {
            get { return _methodTypeName; }
            set
            {
                Validate();

                if (value == _methodTypeName)
                {
                    return;
                }

                _methodTypeName = value;
                if (ModelType != null)
                {
                    if (ModelType.DisplayName.StartsWith(_methodTypeName, StringComparison.Ordinal))
                    {
                        _methodTypeName = ModelType.DisplayName;
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
                GetStoreProcedureFunctions();
            }
        }

        #region 從 Entity Framework 預存方法讀取資訊
        private void GetStoreProcedureFunctions()
        {
            string dbContextTypeName = this.DbContextModelType.TypeName;
            ICodeTypeService codeTypeService = GetService<ICodeTypeService>();
            Project project = _context.ActiveProject;
            CodeType dbContext = codeTypeService.GetCodeType(project, dbContextTypeName);

            _methodTypeCollection=new ObservableCollection<MethodType>();
            foreach (CodeElement code in dbContext.Members)
            {
                if (code.Kind == vsCMElement.vsCMElementFunction)
                {
                    CodeFunction fun=(CodeFunction)code;
                    //if( fun.Type != null)
                        _methodTypeCollection.Add(new MethodType(fun));
                }
            }
            OnPropertyChanged("MethodTypeCollection");
        }

        private void CreateViewModelFromMethodParameters()
        {            
            ICodeTypeService codeTypeService = GetService<ICodeTypeService>();
            Project project = _context.ActiveProject;
            CodeType dbContext = codeTypeService.GetCodeType(project, DbContextModelType.TypeName);

            foreach(CodeElement code in dbContext.Members)
            {
                if(code.Name==this.MethodTypeName)
                {
                    //讀取參數
                    CodeFunction myMethod = (CodeFunction)code;
                    MetaTableInfo data1 = new StorageMan<MetaTableInfo>(myMethod.Name, SaveFolderPath).Read();
                    if (data1.Columns.Any())
                    {
                        this.QueryFormViewModel = new MetadataSettingViewModel(data1);
                    }
                    else
                    {
                        this.QueryFormViewModel = new MetadataSettingViewModel(myMethod);
                    }
                    

                    // 讀取回傳型別
                    CodeTypeRef returnTypeRef = myMethod.Type;
                    string enumType = returnTypeRef.AsFullName;
                    int idx1 = enumType.IndexOf("<");
                    int idx2 = enumType.LastIndexOf(">");
                    string baseType = enumType.Substring(idx1 + 1, idx2 - idx1 - 1);

                    CodeType returnContextType = codeTypeService.GetCodeType(project, baseType);
                    ModelType = new ModelType(returnContextType);

                    MetaTableInfo data2 = new StorageMan<MetaTableInfo>(ModelType.ShortName, SaveFolderPath).Read();
                    if (data2.Columns.Any())
                    {
                        this.ResultListViewModel = new MetadataSettingViewModel(data2);
                    }
                    else
                    {
                        //IEntityFrameworkService efService = _context.ServiceProvider.GetService<IEntityFrameworkService>();
                        //Microsoft.AspNet.Scaffolding.Core.Metadata.ModelMetadata efMetadata
                        //    = efService.AddRequiredEntity(_context, DbContextModelType.TypeName, ModelType.CodeType.FullName);

                        //this.ResultListViewModel = new MetadataSettingViewModel(efMetadata);

                        //TODO partal class 會找不到成員
                        this.ResultListViewModel = new MetadataSettingViewModel(returnContextType);
                    }
                }
            }
            
        }

        private void SaveDesignData()
        {
            StorageMan<MetaTableInfo> sm = new StorageMan<MetaTableInfo>(this.MethodTypeName, SaveFolderPath);
            sm.Save(this.QueryFormViewModel.DataModel);

            sm.ModelName = ModelType.ShortName;
            sm.Save(this.ResultListViewModel.DataModel);
        }

        private string SaveFolderPath
        {
            get
            {
                return Path.Combine(_context.ActiveProject.GetFullPath(), "CodeGen");
            }
        }
        
        private void GetStoreProcedureFunction_TESTCODE()
        {
            string dbContextTypeName=this.DbContextModelType.TypeName;
            ICodeTypeService codeTypeService = GetService<ICodeTypeService>();
            Project project = _context.ActiveProject;
            CodeType dbContext = codeTypeService.GetCodeType(project, dbContextTypeName);
            IEntityFrameworkService efService = _context.ServiceProvider.GetService<IEntityFrameworkService>();

            //ModelMetadata efMetadata = efService.AddRequiredEntity(_context, dbContextTypeName, "");
            
            //GetMethodInfo(string.Format("{0}, {1}", dbContextTypeName, project.Name));


            foreach(CodeElement code in dbContext.Members )
            {
                if(code.Kind == vsCMElement.vsCMElementFunction )
                {

                    //if (code.Name != "QueryBooks")
                    //    continue;

                    CodeFunction myMethod = (CodeFunction)code;
                    //讀取參數
                    foreach (CodeElement p in myMethod.Parameters)
                    {
                        CodeParameter p1 = (CodeParameter)p;

                        string pName = p1.Name;
                        string pType = p1.Type.AsString;

                    }

                    CodeTypeRef returnTypeRef= myMethod.Type;
                    string enumType = returnTypeRef.AsFullName;
                    int idx1 = enumType.IndexOf("<");
                    int idx2 = enumType.LastIndexOf(">");
                    string baseType = enumType.Substring(idx1 + 1, idx2 - idx1 - 1);
                    CodeType returnModel = codeTypeService.GetCodeType(project, baseType);
                    // 讀取回傳型別
                    foreach (CodeElement cc in returnModel.Members)
                    {
                        if (cc.Kind == vsCMElement.vsCMElementProperty) 
                        {
                            CodeProperty p2 = (CodeProperty)cc;
                            string pName = p2.Name;
                            string pType = p2.Type.AsString;
                        }
                        
                    }

                }
                   
            }
        }
        #endregion

        #region MVC
        private bool _GenerateViews;
        public bool GenerateViews
        {
            get { return _GenerateViews; }
            set
            {
                Validate();

                if (value == _GenerateViews)
                {
                    return;
                }

                _GenerateViews = value;
                OnPropertyChanged();
            }
        }

        private bool _ReferenceScriptLibraries;
        public bool ReferenceScriptLibraries
        {
            get { return _ReferenceScriptLibraries; }
            set
            {
                Validate();

                if (value == _ReferenceScriptLibraries)
                {
                    return;
                }

                _ReferenceScriptLibraries = value;
                OnPropertyChanged();
            }
        }

        private bool _LayoutPageSelected;
        public bool LayoutPageSelected
        {
            get { return _LayoutPageSelected; }
            set
            {
                Validate();

                if (value == _LayoutPageSelected)
                {
                    return;
                }

                _LayoutPageSelected = value;
                OnPropertyChanged();
            }
        }


        private string _LayoutPageFile;
        public string LayoutPageFile
        {
            get { return _LayoutPageFile; }
            set
            {
                if (value == _LayoutPageFile)
                {
                    return;
                }
                _LayoutPageFile = value;
                OnPropertyChanged();
            }
        }

        private string _ControllerName="ReportController";
        public string ControllerName
        {
            get { return _ControllerName; }
            set
            {
                if (value == _ControllerName)
                {
                    return;
                }
                _ControllerName = value;
                OnPropertyChanged();
            }
        }
        #endregion

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

        public ObservableCollection<MethodType> MethodTypeCollection
        {
            get
            {
                return _methodTypeCollection;
            }
        }

        public MetadataSettingViewModel QueryFormViewModel 
        {
            get
            {
                return _queryFormViewModel;
            }
            private set
            {
                Validate();

                if (value == this._queryFormViewModel)
                {
                    return;
                }

                this._queryFormViewModel = value;
                OnPropertyChanged();
            }
        }

        public MetadataSettingViewModel ResultListViewModel 
        {
            get
            {
                return _resultListViewModel;
            }
            set
            {
                Validate();

                if (value == this._resultListViewModel)
                {
                    return;
                }

                this._resultListViewModel = value;
                OnPropertyChanged();
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
                    //_dbContextTypeCollection.Insert(0, _addNewDataContextItem);
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
                //ClearError(currentPropertyName);
                //if (ModelType == null)
                //{
                //    AddError(currentPropertyName, MvcScaffolderDialogResources.Error_ModelTypeRequired);
                //}
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
