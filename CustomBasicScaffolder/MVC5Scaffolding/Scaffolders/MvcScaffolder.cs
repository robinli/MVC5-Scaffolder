using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using EnvDTE;
using Microsoft.AspNet.Scaffolding.EntityFramework;
using Microsoft.AspNet.Scaffolding.NuGet;
using Microsoft.AspNet.Scaffolding.WebForms.UI;
using Microsoft.AspNet.Scaffolding.Core.Metadata;
using Microsoft.AspNet.Scaffolding.WebForms.Utils;
using System.IO;
using Microsoft.AspNet.Scaffolding;
using Microsoft.AspNet.Scaffolding.WebForms;

namespace CustomMVC5.Scaffolders
{
    // This class performs all of the work of scaffolding. The methods are executed in the
    // following order:
    // 1) ShowUIAndValidate() - displays the Visual Studio dialog for setting scaffolding options
    // 2) Validate() - validates the model collected from the dialog
    // 3) GenerateCode() - if all goes well, generates the scaffolding output from the templates
    public class MvcScaffolder : CodeGenerator
    {

        private MvcCodeGeneratorViewModel _codeGeneratorViewModel;

        internal MvcScaffolder(CodeGenerationContext context, CodeGeneratorInformation information)
            : base(context, information)
        {

        }


        // Shows the Visual Studio dialog that collects scaffolding options
        // from the user.
        // Passing the dialog to this method so that all scaffolder UIs
        // are modal is still an open question and tracked by bug 578173.
        public override bool ShowUIAndValidate()
        {
            _codeGeneratorViewModel = new MvcCodeGeneratorViewModel(Context);

            MvcScaffolderDialog window = new MvcScaffolderDialog(_codeGeneratorViewModel);
            bool? isOk = window.ShowModal();

            if (isOk == true)
            {
                Validate();
            }

            return (isOk == true);
        }

        // Validates the model returned by the Visual Studio dialog.
        // We always force a Visual Studio build so we have a model
        // that we can use with the Entity Framework.
        private void Validate()
        {
            //WriteLog("Validate start.");

            CodeType modelType = _codeGeneratorViewModel.ModelType.CodeType;
            ModelType dbContextType = _codeGeneratorViewModel.DbContextModelType;
            string dbContextTypeName = (dbContextType != null)
                ? dbContextType.TypeName
                : null;

            if (modelType == null)
            {
                throw new InvalidOperationException(Resources.WebFormsScaffolder_SelectModelType);
            }

            if (dbContextType == null || String.IsNullOrEmpty(dbContextTypeName))
            {
                throw new InvalidOperationException(Resources.WebFormsScaffolder_SelectDbContextType);
            }

            // always force the project to build so we have a compiled
            // model that we can use with the Entity Framework
            var visualStudioUtils = new VisualStudioUtils();
            visualStudioUtils.BuildProject(Context.ActiveProject);


            Type reflectedModelType = GetReflectionType(modelType.FullName);
            if (reflectedModelType == null)
            {
                throw new InvalidOperationException(Resources.WebFormsScaffolder_ProjectNotBuilt);
            }

            //WriteLog("Validate finish.");
        }
        private void WriteLog(string message)
        {
            System.IO.StreamWriter sw = new StreamWriter("R:\\LOG.Scaffold.txt", true);
            sw.WriteLine(message);
            sw.Close();
        }

        // Top-level method that generates all of the scaffolding output from the templates.
        // Shows a busy wait mouse cursor while working.
        public override void GenerateCode()
        {
            //WriteLog("GenerateCode start.");

            var project = Context.ActiveProject;
            var selectionRelativePath = GetSelectionRelativePath();

            if (_codeGeneratorViewModel == null)
            {
                throw new InvalidOperationException(Resources.WebFormsScaffolder_ShowUIAndValidateNotCalled);
            }

            Cursor currentCursor = Mouse.OverrideCursor;
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                GenerateCode(project, selectionRelativePath, this._codeGeneratorViewModel);
            }
            finally
            {
                Mouse.OverrideCursor = currentCursor;
            }
            //WriteLog("GenerateCode finish.");
        }

        // Collects the common data needed by all of the scaffolded output and generates:
        // 1) Add Controller
        // 2) Add View
        private void GenerateCode(Project project, string selectionRelativePath, MvcCodeGeneratorViewModel codeGeneratorViewModel)
        {
            //TODO get jQuery Version
            //GetJqueryVersion(project);

            // Get Model Type
            var modelType = codeGeneratorViewModel.ModelType.CodeType;

            // Get the dbContext
            string dbContextTypeName = codeGeneratorViewModel.DbContextModelType.TypeName;
            ICodeTypeService codeTypeService = GetService<ICodeTypeService>();
            CodeType dbContext = codeTypeService.GetCodeType(project, dbContextTypeName);

            // Get the Entity Framework Meta Data
            IEntityFrameworkService efService = Context.ServiceProvider.GetService<IEntityFrameworkService>();
            ModelMetadata efMetadata = efService.AddRequiredEntity(Context, dbContextTypeName, modelType.FullName);

            // Generate dictionary for related entities
            //var relatedModels = GetRelatedModelDictionary(efMetadata);

            string controllerName = codeGeneratorViewModel.ControllerName;
            string controllerRootName = controllerName.Replace("Controller","");
            string outputFolderPath = Path.Combine(selectionRelativePath, controllerName);

            AddMvcController(
                controllerName: controllerName
                , controllerRootName: controllerRootName
                , outputPath: outputFolderPath
                , ContextTypeName: dbContext.Name
                , modelType: modelType
                , efMetadata: efMetadata
                , overwrite: codeGeneratorViewModel.OverwriteViews);

            if (!codeGeneratorViewModel.GenerateViews)
                return;

            // Create Views Folder
            string viewRootPath = GetViewsFolderPath(selectionRelativePath);
            string viewPath = Path.Combine(viewRootPath, controllerRootName);
            AddFolder(Context.ActiveProject, viewPath);

            //_ViewStart & Create _Layout
            if (codeGeneratorViewModel.LayoutPageSelected)
            {
                string areaName = GetAreaName(selectionRelativePath);
                AddDependencyFile(viewRootPath, areaName);
            }
            

            // Views for  C.R.U.D 
            foreach (string viewName in new string[4] { "Index", "Create", "Edit", "_Edit" })
            {
                AddView(viewPath, viewName, controllerRootName, modelType, efMetadata
                    , referenceScriptLibraries: codeGeneratorViewModel.ReferenceScriptLibraries
                    , isLayoutPageSelected: codeGeneratorViewModel.LayoutPageSelected
                    , layoutPageFile: codeGeneratorViewModel.LayoutPageFile
                    , overwrite: codeGeneratorViewModel.OverwriteViews
                    );
            }
        }

        //add MVC Controller
        private void AddMvcController(string controllerName
            , string controllerRootName
            , string outputPath
            , string ContextTypeName /*"Entities"*/
            , CodeType modelType
            , ModelMetadata efMetadata
            , bool overwrite = false)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }
            if (String.IsNullOrEmpty(controllerName))
            {
                //TODO
                throw new ArgumentException(Resources.WebFormsViewScaffolder_EmptyActionName, "webFormsName");
            }

            PropertyMetadata primaryKey = efMetadata.PrimaryKeys.FirstOrDefault();
            string pluralizedName = efMetadata.EntitySetName;
            string modelNameSpace = modelType.Namespace != null ? modelType.Namespace.FullName : String.Empty;
            string relativePath = outputPath.Replace(@"\", @"/");


            Project project = Context.ActiveProject;
            var templatePath = Path.Combine("MvcControllerWithContext", "Controller");
            var defaultNamespace = GetDefaultNamespace();
            string modelTypeVariable = GetTypeVariable(modelType.Name);
            string bindAttributeIncludeText =GetBindAttributeIncludeText(efMetadata);

            Dictionary<string, object> templateParams=new Dictionary<string, object>(){
                {"ControllerName", controllerName}
                , {"ControllerRootName" , controllerName.Replace("Controller", "")}
                , {"Namespace", defaultNamespace}
                , {"AreaName", string.Empty}
                , {"ContextTypeName", ContextTypeName}
                , {"ModelTypeName", modelType.Name}
                , {"ModelVariable", modelTypeVariable}
                , {"ModelMetadata", efMetadata}
                , {"EntitySetVariable", modelTypeVariable}
                , {"UseAsync", false}
                , {"IsOverpostingProtectionRequired", true}
                , {"BindAttributeIncludeText", bindAttributeIncludeText}
                , {"OverpostingWarningMessage", "To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598."}
                , {"RequiredNamespaces", new HashSet<string>(){modelType.Namespace.FullName}}
            };

            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: !overwrite);
        }
        private string GetTypeVariable(string typeName)
        {
            return typeName.Substring(0,1).ToLower() + typeName.Substring(1, typeName.Length - 1);
        }
        private string GetBindAttributeIncludeText(ModelMetadata efMetadata)
        {
            string result="";
            foreach (PropertyMetadata m in efMetadata.Properties)
                result += "," + m.PropertyName;
            return result.Substring(1);
        }


        private void AddView(string selectionRelativePath 
            , string viewName
            , string controllerRootName
            , CodeType modelType
            , ModelMetadata efMetadata
            , bool referenceScriptLibraries = true
            , bool isLayoutPageSelected = true
            , string layoutPageFile = null
            , bool isBundleConfigPresent=true
            , bool overwrite = false)
        {
            Project project = Context.ActiveProject;
            string outputPath = Path.Combine(selectionRelativePath, viewName);
            string templatePath = Path.Combine("MvcView", viewName);
            string viewDataTypeName = modelType.Namespace.FullName + "." + modelType.Name;

            if (layoutPageFile == null)
                layoutPageFile = string.Empty;

            Dictionary<string, object> templateParams = new Dictionary<string, object>(){
                {"ModelMetadata", efMetadata}
                , {"ViewName", viewName}
                , {"ViewDataTypeName", viewDataTypeName}
                , {"IsPartialView" , false}
                , {"LayoutPageFile", layoutPageFile}
                , {"IsLayoutPageSelected", isLayoutPageSelected}
                , {"ReferenceScriptLibraries", referenceScriptLibraries}
                , {"IsBundleConfigPresent", isBundleConfigPresent}
                , {"ViewDataTypeShortName", modelType.Name}
                , {"JQueryVersion","2.1.0"} // 如何讀取專案的 jQuery 版本
                , {"MvcVersion", new Version("5.1.2.0")}
            };

            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: !overwrite);
        }

        //add _Layout & _ViewStart
        private void AddDependencyFile(string viewRootPath
            , string areaName
            )
        {
            Project project = Context.ActiveProject;

            // add _Layout
            string viewName = "_ViewStart";
            string outputPath = Path.Combine(viewRootPath, viewName);
            string templatePath = Path.Combine("MvcFullDependencyCodeGenerator", viewName);
            Dictionary<string, object> templateParams = new Dictionary<string, object>(){
                {"AreaName", areaName}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);

            // add _ViewStart
            viewName = "_Layout";
            outputPath = Path.Combine(viewRootPath, "Shared", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator", viewName);
            templateParams = new Dictionary<string, object>(){
                {"IsBundleConfigPresent", true}
                , {"JQueryVersion",""}
                , {"ModernizrVersion", ""}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);
        }



        #region function library



        public string GetJqueryVersion(Project project)
        {
            //NuGetPackage p=new NuGetPackage("jquery",
            //                                 "1.6.4",
            //                                 new NuGetSourceRepository());
            // context.Packages.Add(p);

            for(int x=0; x<project.Properties.Count; x++)
            {
                object xx = project.Properties.Item(x);
            }

            NuGetPackage currPage = Context.Packages.FirstOrDefault(p => p.PackageId == "jquery");
            return (currPage != null? currPage.Version : string.Empty);
        }



        //// A single generic repository is created no matter how many models are scaffolded 
        //// with the Web Forms scaffolder. This generic repository is added to the Models folder. 
        //private void EnsureGenericRepository(Project project, CodeType dbContext, string genericRepositoryNamespace)
        //{
        //    string dbContextNameSpace = dbContext.Namespace != null ? dbContext.Namespace.FullName : String.Empty;

        //    // Add the folder
        //    AddFolder(project, "Models");

        //    AddFileFromTemplate(
        //        project: project,
        //        outputPath: "Models\\GenericRepository",
        //        templateName: "Models\\GenericRepository",
        //        templateParameters: new Dictionary<string, object>() 
        //            {
        //                {"Namespace", genericRepositoryNamespace},
        //                {"DBContextType", dbContext.Name},
        //                {"DBContextNamespace", dbContextNameSpace}
        //            },
        //        skipIfExists: true);
        //}


        //// A set of Dynamic Data field templates is created that support Bootstrap
        //private void EnsureDynamicDataFieldTemplates(Project project, string genericRepositoryNamespace)
        //{
        //    var fieldTemplates = new[] { 
        //        "Boolean", "Boolean.ascx.designer", "Boolean.ascx",
        //        "Boolean_Edit", "Boolean_Edit.ascx.designer", "Boolean_Edit.ascx",
        //        "Children", "Children.ascx.designer", "Children.ascx",
        //        "Children_Insert", "Children_Insert.ascx.designer", "Children_Insert.ascx",
        //        "DateTime", "DateTime.ascx.designer", "DateTime.ascx",
        //        "DateTime_Edit", "DateTime_Edit.ascx.designer", "DateTime_Edit.ascx",
        //        "Decimal_Edit", "Decimal_Edit.ascx.designer", "Decimal_Edit.ascx",
        //        "EmailAddress", "EmailAddress.ascx.designer", "EmailAddress.ascx",
        //        "Enumeration", "Enumeration.ascx.designer", "Enumeration.ascx",
        //        "Enumeration_Edit", "Enumeration_Edit.ascx.designer", "Enumeration_Edit.ascx",
        //        "ForeignKey", "ForeignKey.ascx.designer", "ForeignKey.ascx",
        //        "ForeignKey_Edit", "ForeignKey_Edit.ascx.designer", "ForeignKey_Edit.ascx",
        //        "Integer_Edit", "Integer_Edit.ascx.designer", "Integer_Edit.ascx",
        //        "FieldLabel", "FieldLabel.ascx.designer", "FieldLabel.ascx",
        //        "MultilineText_Edit", "MultilineText_Edit.ascx.designer", "MultilineText_Edit.ascx",
        //        "Text", "Text.ascx.designer", "Text.ascx",
        //        "Text_Edit", "Text_Edit.ascx.designer", "Text_Edit.ascx",
        //        "Url", "Url.ascx.designer", "Url.ascx",
        //        "Url_Edit", "Url_Edit.ascx.designer", "Url_Edit.ascx"
        //    };
        //    var fieldTemplatesPath = "DynamicData\\FieldTemplates";

        //    // Add the folder
        //    AddFolder(project, fieldTemplatesPath);

        //    foreach (var fieldTemplate in fieldTemplates)
        //    {
        //        var templatePath = Path.Combine(fieldTemplatesPath, fieldTemplate);
        //        var outputPath = Path.Combine(fieldTemplatesPath, fieldTemplate);



        //        AddFileFromTemplate(
        //            project: project,
        //            outputPath: outputPath,
        //            templateName: templatePath,
        //            templateParameters: new Dictionary<string, object>() 
        //            {
        //                {"DefaultNamespace", project.GetDefaultNamespace()},
        //                {"GenericRepositoryNamespace", genericRepositoryNamespace}
        //            },
        //            skipIfExists: true);
        //    }
        //}


        //// Generates all of the Web Forms Pages (Default Insert, Edit, Delete), 
        //private void AddWebFormsPages(
        //    Project project,
        //    string selectionRelativePath,
        //    string genericRepositoryNamespace,
        //    CodeType modelType,
        //    ModelMetadata efMetadata,
        //    bool useMasterPage,
        //    string masterPage = null,
        //    string desktopPlaceholderId = null,
        //    bool overwriteViews = true
        //)
        //{

        //    if (modelType == null)
        //    {
        //        throw new ArgumentNullException("modelType");
        //    }

        //    // Generate dictionary for related entities
        //    var relatedModels = GetRelatedModelDictionary(efMetadata);


        //    var webForms = new[] { "Default", "Insert", "Edit", "Delete" };

        //    // Extract these from the selected master page : Tracked by 721707
        //    var sectionNames = new[] { "HeadContent", "MainContent" };

        //    // Add folder for views. This is necessary to display an error when the folder already exists but 
        //    // the folder is excluded in Visual Studio: see https://github.com/Superexpert/WebFormsScaffolding/issues/18
        //    string outputFolderPath = Path.Combine(selectionRelativePath, modelType.Name);
        //    AddFolder(Context.ActiveProject, outputFolderPath);


        //    // Now add each view
        //    foreach (string webForm in webForms)
        //    {
        //        AddWebFormsViewTemplates(
        //            outputFolderPath: outputFolderPath,
        //            modelType: modelType,
        //            efMetadata: efMetadata,
        //            relatedModels: relatedModels,
        //            genericRepositoryNamespace: genericRepositoryNamespace,
        //            webFormsName: webForm,
        //            useMasterPage: useMasterPage,
        //            masterPage: masterPage,
        //            sectionNames: sectionNames,
        //            primarySectionName: desktopPlaceholderId,
        //            overwrite: overwriteViews);
        //    }
        //}




        //private void AddWebFormsViewTemplates(
        //                        string outputFolderPath,
        //                        CodeType modelType,
        //                        ModelMetadata efMetadata,
        //                        IDictionary<string, RelatedModelMetadata> relatedModels,
        //                        string genericRepositoryNamespace,
        //                        string webFormsName,
        //                        bool useMasterPage,
        //                        string masterPage = "",
        //                        string[] sectionNames = null,
        //                        string primarySectionName = "",
        //                        bool overwrite = false
        //)
        //{
        //    if (modelType == null)
        //    {
        //        throw new ArgumentNullException("modelType");
        //    }
        //    if (String.IsNullOrEmpty(webFormsName))
        //    {
        //        throw new ArgumentException(Resources.WebFormsViewScaffolder_EmptyActionName, "webFormsName");
        //    }

        //    PropertyMetadata primaryKey = efMetadata.PrimaryKeys.FirstOrDefault();
        //    string pluralizedName = efMetadata.EntitySetName;

        //    string modelNameSpace = modelType.Namespace != null ? modelType.Namespace.FullName : String.Empty;
        //    string relativePath = outputFolderPath.Replace(@"\", @"/");

        //    List<string> webFormsTemplates = new List<string>();
        //    webFormsTemplates.AddRange(new string[] { webFormsName, webFormsName + ".aspx", webFormsName + ".aspx.designer" });

        //    // Scaffold aspx page and code behind
        //    foreach (string webForm in webFormsTemplates)
        //    {
        //        Project project = Context.ActiveProject;
        //        var templatePath = Path.Combine("WebForms", webForm);
        //        string outputPath = Path.Combine(outputFolderPath, webForm);

        //        var defaultNamespace = GetDefaultNamespace() + "." + modelType.Name;
        //        AddFileFromTemplate(project,
        //            outputPath,
        //            templateName: templatePath,
        //            templateParameters: new Dictionary<string, object>() 
        //            {
        //                {"RelativePath", relativePath},
        //                {"DefaultNamespace", defaultNamespace},
        //                {"Namespace", modelNameSpace},
        //                {"IsContentPage", useMasterPage},
        //                {"MasterPageFile", masterPage},
        //                {"SectionNames", sectionNames},
        //                {"PrimarySectionName", primarySectionName},
        //                {"PrimaryKeyMetadata", primaryKey},
        //                {"PrimaryKeyName", primaryKey.PropertyName},
        //                {"PrimaryKeyType", primaryKey.ShortTypeName},
        //                {"ViewDataType", modelType},
        //                {"ViewDataTypeName", modelType.Name},
        //                {"GenericRepositoryNamespace", genericRepositoryNamespace},
        //                {"PluralizedName", pluralizedName},
        //                {"ModelMetadata", efMetadata},
        //                {"RelatedModels", relatedModels}
        //            },
        //            skipIfExists: !overwrite);
        //    }

        //}


        // Called to ensure that the project was compiled successfully
        private Type GetReflectionType(string typeName)
        {
            return GetService<IReflectedTypesService>().GetType(Context.ActiveProject, typeName);
        }

        // We are just pulling in some dependent nuget packages
        // to meet "Web Application Project" experience in this change.
        // There are some open questions regarding the experience for
        // webforms scaffolder in the case of an empty project.
        // Those details need to be worked out and
        // depending on that, we would modify the list of packages below
        // or conditions which determine when they are installed etc.
        //public override IEnumerable<NuGetPackage> Dependencies
        //{
        //    get
        //    {
        //        return GetService<IEntityFrameworkService>().Dependencies;
        //    }
        //}

        private TService GetService<TService>() where TService : class
        {
            return (TService)ServiceProvider.GetService(typeof(TService));
        }
        

        // Returns the relative path of the folder selected in Visual Studio or an empty 
        // string if no folder is selected.
        protected string GetSelectionRelativePath()
        {
            return Context.ActiveProjectItem == null ? String.Empty : ProjectItemUtils.GetProjectRelativePath(Context.ActiveProjectItem);
        }

        private string GetAreaName(string selectionRelativePath)
        {
            string[] dirs = selectionRelativePath.Split(new char[1] { '\\' });

            if (dirs[0].Equals("Areas"))
                return dirs[1];
            else
                return string.Empty;

        }

        /// <summary>
        /// Get Views folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        private string GetViewsFolderPath(string controllerPath)
        {
            string keyControllers = "Controllers";
            string keyViews = "Views";

            return(
                (
                controllerPath.IndexOf(keyControllers) > 0)
                ? controllerPath.Replace(keyControllers, keyViews) 
                : Path.Combine(controllerPath, keyViews)
                );
        }

        // If a Visual Studio folder is selected then returns the folder's namespace, otherwise
        // returns the project namespace.
        protected string GetDefaultNamespace()
        {
            return Context.ActiveProjectItem == null
                ? Context.ActiveProject.GetDefaultNamespace()
                : Context.ActiveProjectItem.GetDefaultNamespace();
        }


        // Create a dictionary that maps foreign keys to related models. We only care about associations
        // with a single key (so we can display in a DropDownList)
        //protected IDictionary<string, RelatedModelMetadata> GetRelatedModelDictionary(ModelMetadata efMetadata)
        //{
        //    var dict = new Dictionary<string, RelatedModelMetadata>();

        //    foreach (var relatedEntity in efMetadata.RelatedEntities)
        //    {
        //        if (relatedEntity.ForeignKeyPropertyNames.Count() == 1)
        //        {
        //            dict[relatedEntity.ForeignKeyPropertyNames[0]] = relatedEntity;
        //        }
        //    }
        //    return dict;
        //}

        #endregion
    }
}
