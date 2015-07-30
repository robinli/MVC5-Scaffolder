using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using EnvDTE;
using Microsoft.AspNet.Scaffolding.EntityFramework;
using Microsoft.AspNet.Scaffolding.NuGet;
using Happy.Scaffolding.MVC.UI;
using Microsoft.AspNet.Scaffolding.Core.Metadata;
using Happy.Scaffolding.MVC.Utils;
using System.IO;
using Microsoft.AspNet.Scaffolding;
using Happy.Scaffolding.MVC;
using Happy.Scaffolding.MVC.Models;
using System.Reflection;

namespace Happy.Scaffolding.MVC.Scaffolders
{
    // This class performs all of the work of scaffolding. The methods are executed in the
    // following order:
    // 1) ShowUIAndValidate() - displays the Visual Studio dialog for setting scaffolding options
    // 2) Validate() - validates the model collected from the dialog
    // 3) GenerateCode() - if all goes well, generates the scaffolding output from the templates
    public class MvcScaffolder : CodeGenerator
    {

        private MvcCodeGeneratorViewModel _codeGeneratorViewModel;
        private ModelMetadataViewModel _ModelMetadataVM;

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

                if(_codeGeneratorViewModel.GenerateViews)
                {
                    isOk = ShowColumnSetting();
                }
            }
            return (isOk == true);
        }


        // Setting Columns : display name, allow null
        private bool? ShowColumnSetting()
        {
            var modelType = _codeGeneratorViewModel.ModelType.CodeType;
            string savefolderPath = Path.Combine(Context.ActiveProject.GetFullPath(), "CodeGen");
            StorageMan<MetaTableInfo> sm = new StorageMan<MetaTableInfo>(modelType.Name, savefolderPath);
            MetaTableInfo data = sm.Read();
            if (data.Columns.Any())
            {
                _ModelMetadataVM = new ModelMetadataViewModel(data);
            }
            else
            {
                string dbContextTypeName = _codeGeneratorViewModel.DbContextModelType.TypeName;
                ICodeTypeService codeTypeService = GetService<ICodeTypeService>();
                CodeType dbContext = codeTypeService.GetCodeType(Context.ActiveProject, dbContextTypeName);
                IEntityFrameworkService efService = Context.ServiceProvider.GetService<IEntityFrameworkService>();
                ModelMetadata efMetadata = efService.AddRequiredEntity(Context, dbContextTypeName, modelType.FullName);
                _ModelMetadataVM = new ModelMetadataViewModel(efMetadata);
            }

            ModelMetadataDialog dialog = new ModelMetadataDialog(_ModelMetadataVM);
            bool? isOk = dialog.ShowModal();
            if (isOk == true)
            {
                sm.Save(_ModelMetadataVM.DataModel);
            }

            return isOk;
        }

        // Validates the model returned by the Visual Studio dialog.
        // We always force a Visual Studio build so we have a model
        // that we can use with the Entity Framework.
        private void Validate()
        {
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
        }
        
        // Top-level method that generates all of the scaffolding output from the templates.
        // Shows a busy wait mouse cursor while working.
        public override void GenerateCode()
        {
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
        }

        // Collects the common data needed by all of the scaffolded output and generates:
        // 1) Add Controller
        // 2) Add View
        private void GenerateCode(Project project, string selectionRelativePath, MvcCodeGeneratorViewModel codeGeneratorViewModel)
        {
            // Get Model Type
            var modelType = codeGeneratorViewModel.ModelType.CodeType;

            // Get the dbContext
            string dbContextTypeName = codeGeneratorViewModel.DbContextModelType.TypeName;
            ICodeTypeService codeTypeService = GetService<ICodeTypeService>();
            CodeType dbContext = codeTypeService.GetCodeType(project, dbContextTypeName);
            string dbContextNamespace = dbContext.Namespace != null ? dbContext.Namespace.FullName : String.Empty;
            // Get the Entity Framework Meta Data
            IEntityFrameworkService efService = Context.ServiceProvider.GetService<IEntityFrameworkService>();
            ModelMetadata efMetadata = efService.AddRequiredEntity(Context, dbContextTypeName, modelType.FullName);
           
            var fieldDisplayNames = GetAllFieldDisplayNames(modelType, efMetadata);
            var oneToManyModels = GetOneToManyModelDictionary(efMetadata, efService, dbContextTypeName);

            var oneToManyAnonymousObjTextDic = GetOneToManyAnonymousObjTextDic(oneToManyModels);
            // Create Controller
            string controllerName = codeGeneratorViewModel.ControllerName;
            string controllerRootName = controllerName.Replace("Controller","");
            string outputFolderPath = Path.Combine(selectionRelativePath, controllerName);
            string viewPrefix = codeGeneratorViewModel.ViewPrefix;
            string programTitle = codeGeneratorViewModel.ProgramTitle;
            bool generateMasterDetailRelationship = codeGeneratorViewModel.GenerateMasterDetailRelationship;
            bool checkformcols = codeGeneratorViewModel.CheckFormViewCols;
            int formcols = codeGeneratorViewModel.FormViewCols;
             
            AddEntityRepositoryExtensionTemplates(project, selectionRelativePath,
                dbContextNamespace,
                dbContextTypeName,
                modelType,
                efMetadata, 
                oneToManyModels,
                false);

            AddEntityServiceTemplates(project, selectionRelativePath,
                dbContextNamespace,
                dbContextTypeName,
                modelType, efMetadata,
                oneToManyModels,
                false);

            AddMvcController(project: project
                , controllerName: controllerName
                , controllerRootName: controllerRootName
                , outputPath: outputFolderPath
                , ContextTypeName: dbContext.Name
                , modelType: modelType
                , efMetadata: efMetadata
                , viewPrefix: viewPrefix
                , oneToManyAnonymousObjText:oneToManyAnonymousObjTextDic
                , oneToManyModels:oneToManyModels
                , generateMasterDetailRelationship: generateMasterDetailRelationship
                , overwrite: codeGeneratorViewModel.OverwriteViews);

            if (!codeGeneratorViewModel.GenerateViews)
                return;

            // add Metadata for Model
            outputFolderPath = Path.Combine(GetModelFolderPath(selectionRelativePath), modelType.Name + "Metadata");
            AddModelMetadata(project: project
                , controllerName: controllerName
                , controllerRootName: controllerRootName
                , outputPath: outputFolderPath
                , ContextTypeName: dbContext.Name
                , modelType: modelType
                , efMetadata: efMetadata
                , overwrite: codeGeneratorViewModel.OverwriteViews);
            
            //_ViewStart & Create _Layout
            string viewRootPath = GetViewsFolderPath(selectionRelativePath);
            if (codeGeneratorViewModel.LayoutPageSelected)
            {
                string areaName = GetAreaName(selectionRelativePath);
                AddDependencyFile(project, viewRootPath, areaName);
            }
            // EditorTemplates, DisplayTemplates
            AddDataFieldTemplates(project, viewRootPath);

            var modelDisplayNames = new Dictionary<string, string>();
            foreach (var property in efMetadata.Properties)
            {
                if (property.AssociationDirection == AssociationDirection.OneToMany)
                {
                   modelDisplayNames = GetDisplayNames(property.RelatedModel.TypeName);
                }
            }
        
            // Views for  C.R.U.D 
            string viewFolderPath = Path.Combine(viewRootPath, controllerRootName);
            // Shared Layout Views
            //AddSharedLayoutTemplates(project, viewRootPath, selectionRelativePath, dbContextNamespace, dbContextTypeName, modelType, efMetadata);
            foreach (string viewName in new string[5] { "Index", "Create", "Edit", "EditForm","_PopupSearch" })
            {
                //string viewName = string.Format(view, viewPrefix);
                //未完成
                /*
                 Index        CustIndex
                 Create       CustCreate
                 Edit           CustEdit
                 EditForm    CustEditForm
                 * 
                 _Edit      _CustEdit
                 */

                AddView(project
                    , viewFolderPath, viewPrefix, viewName, programTitle
                    , controllerRootName, modelType, efMetadata
                    , referenceScriptLibraries: codeGeneratorViewModel.ReferenceScriptLibraries
                    , isLayoutPageSelected: codeGeneratorViewModel.LayoutPageSelected
                    , layoutPageFile: codeGeneratorViewModel.LayoutPageFile
                    , overwrite: codeGeneratorViewModel.OverwriteViews
                    , generateMasterDetailRelationship: generateMasterDetailRelationship
                    , oneToManyModels: oneToManyModels
                    , checkedFormCols:checkformcols
                    , formClos: formcols
                    , modelDisplayNames:modelDisplayNames
                    );
            }
            
            foreach (var property in efMetadata.Properties)
            {
                if (property.AssociationDirection == AssociationDirection.OneToMany)
                {
                    string _detialViewName="_DetailEditForm";
                    var detailModelMeta = oneToManyModels[property.PropertyName];
                    var modelTypeName = property.RelatedModel.ShortTypeName;
                   
                    AddDetailsView(project
                        , viewFolderPath
                        , viewPrefix
                        , _detialViewName
                        , programTitle
                        , controllerRootName
                        , modelTypeName
                        , modelType
                        , detailModelMeta
                        , modelDisplayNames
                        
                        , codeGeneratorViewModel.OverwriteViews);
                }
            }
        }

        private bool HasRelatedMasterModel(Microsoft.AspNet.Scaffolding.Core.Metadata.ModelMetadata modelMdetadata, string propertyName)
        {
            bool result= modelMdetadata.Properties.Where(n => n.PropertyName == propertyName && n.RelatedModel.TypeName == "").Any();

            if (!result)
            {
                return modelMdetadata.RelatedEntities.Where(n => n.ForeignKeyPropertyNames[0] == propertyName && n.TypeName == "").Any();
            }
            return result;
        }
        private string GetForeignKeyName(Microsoft.AspNet.Scaffolding.Core.Metadata.ModelMetadata modelMdetadata, string relatedmodeTypename)
        {
            return modelMdetadata.RelatedEntities.Where(n => n.ShortTypeName == relatedmodeTypename).Select(n => n.ForeignKeyPropertyNames).First()[0];
        }
        private IDictionary<string,string> GetAllFieldDisplayNames(CodeType modelType, ModelMetadata efMetadata)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic = GetDisplayNames(modelType.FullName);
            foreach (var property in efMetadata.Properties)
            {
                if (property.AssociationDirection == AssociationDirection.OneToMany)
                {
                    string typename= property.RelatedModel.TypeName;
                    var dis = GetDisplayNames(typename);
                    foreach (var item in dis)
                    {
                        if (!dic.ContainsKey(item.Key))
                        {
                            dic.Add(item.Key, item.Value);
 
                        }
                    }
                }
            }
            return dic;

        }

        private void AddDetailsView(Project project
            , string viewsFolderPath
            , string viewPrefix
            , string viewName
            , string programTitle
            , string controllerRootName
            , string modelTypeName
            , CodeType modelType
            , ModelMetadata efMetadata
            , IDictionary<string, string> modelDisplayNames
           
            , bool overwrite = false
           )
        {

            string outputPath = Path.Combine(viewsFolderPath, "_"+modelTypeName+"EditForm");
            string templatePath = Path.Combine("MvcView", viewName);
            string viewDataTypeName = modelType.Namespace.FullName + "." + modelTypeName;
            string modelNameSpace = modelType.Namespace != null ? modelType.Namespace.FullName : String.Empty;
            
            string masterModelTypeName = modelNameSpace + "." + modelType.Name;
            Dictionary<string, object> templateParams = new Dictionary<string, object>(){
               {"ControllerRootName" , controllerRootName}
                , {"ModelMetadata", efMetadata}
                , {"ViewPrefix", viewPrefix}
                , {"ViewName", viewName}
                , {"ProgramTitle", programTitle}
                , {"ModelNameSpace", modelNameSpace}
                , {"ViewDataTypeName", viewDataTypeName}
                , {"ModelDisplayNames",modelDisplayNames}
                ,{"MasterModelTypeName" , masterModelTypeName}
                , {"IsPartialView" , true}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);
        }
        private Dictionary<string ,string > GetOneToManyAnonymousObjTextDic(Dictionary<string, ModelMetadata> oneToManyModels)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in oneToManyModels)
            {
                //dic.Add(item.Key, GetAnonymousObjLambdaText(item.Value));
                dic.Add(item.Key, GetSelectLambdaText(item.Value));
            }
            return dic;
        }

        private Dictionary<string, ModelMetadata> GetOneToManyModelDictionary(ModelMetadata efMetadata, IEntityFrameworkService efService, string dbContextTypeName)
        {
            var dict = new Dictionary<string, ModelMetadata>();
            foreach (var prop in efMetadata.Properties)
            {
             
                if (prop.AssociationDirection == AssociationDirection.OneToMany)
                {
                    string propname = prop.PropertyName;
                    //var relmeta = prop.RelatedModel;
                    string typename = prop.TypeName;

                    ModelMetadata modelMetadata = efService.AddRequiredEntity(Context, dbContextTypeName, typename);
                    if (!dict.ContainsKey(propname))
                    {
                        dict.Add(propname, modelMetadata);
                    }
                }
            }


            return dict;
        }


        //add MVC Controller
        private void AddMvcController(Project project
            , string controllerName
            , string controllerRootName
            , string outputPath
            , string ContextTypeName /*"Entities"*/
            , CodeType modelType
            , ModelMetadata efMetadata
            , string viewPrefix
            , Dictionary<string , string > oneToManyAnonymousObjText
            , Dictionary<string , ModelMetadata > oneToManyModels
            , bool generateMasterDetailRelationship
            , bool overwrite = false
            )
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
            string modelName = modelType.Name;
            //string defaultNamespace = project.GetDefaultNamespace();
            PropertyMetadata primaryKey = efMetadata.PrimaryKeys.FirstOrDefault();
            string pluralizedName = efMetadata.EntitySetName;
            string modelNameSpace = modelType.Namespace != null ? modelType.Namespace.FullName : String.Empty;
            string relativePath = outputPath.Replace(@"\", @"/");
            string selectLambdaText = GetSelectLambdaText(efMetadata);

            //Project project = Context.ActiveProject;
            var templatePath = Path.Combine("MvcControllerWithContext", "Controller");
            var defaultNamespace = GetDefaultNamespace();
            string modelTypeVariable = GetTypeVariable(modelType.Name);
            string bindAttributeIncludeText =GetBindAttributeIncludeText(efMetadata);

            Dictionary<string, object> templateParams=new Dictionary<string, object>(){
                {"ControllerName", controllerName}
                , {"ModelName",modelName}
                , {"ControllerRootName" , controllerRootName}
                , {"Namespace", defaultNamespace}
                , {"AreaName", string.Empty}
                , {"ContextTypeName", ContextTypeName}
                , {"ModelTypeName", modelType.Name}
                , {"ModelVariable", modelTypeVariable}
                , {"ModelMetadata", efMetadata}
                , {"OneToManyModelMetadata", oneToManyModels}
                , {"OneToManyAnonymousObjText", oneToManyAnonymousObjText}
                , {"GenerateMasterDetailRelationship", generateMasterDetailRelationship}
                , {"SelectLambdaText",selectLambdaText}
                , {"EntitySetVariable", modelTypeVariable}
                , {"UseAsync", false}
                , {"IsOverpostingProtectionRequired", true}
                , {"BindAttributeIncludeText", bindAttributeIncludeText}
                , {"OverpostingWarningMessage", "To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598."}
                , {"RequiredNamespaces", new HashSet<string>(){modelType.Namespace.FullName,project.GetDefaultNamespace() + ".Services",project.GetDefaultNamespace() + ".Repositories",project.GetDefaultNamespace() + ".Extensions"}}
                , {"ViewPrefix", viewPrefix}
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

        private string GetAnonymousObjLambdaText(ModelMetadata efMetadata)
        {
            //string result = "n => new { {0} }";
            string field = "";
            foreach (PropertyMetadata m in efMetadata.Properties)
                field += "," + m.PropertyName + " = n."  + m.PropertyName ;
            //return string.Format(result,field.Substring(1));
            return "n => new { " + field.Substring(1)  + " }";
        }
        public string GetQueryLambdaText(ModelMetadata efMetadata)
        {
           
            string linqtxt = "";


            foreach (PropertyMetadata m in efMetadata.Properties.Where(n => n.Scaffold && n.ShortTypeName.ToLower() != "int"
                && n.ShortTypeName.ToLower() != "decimal" && n.ShortTypeName.ToLower() != "float"))
            {
                
                if (m.ShortTypeName.ToLower() == "string")
                {
                      linqtxt  += "|| " + string.Format("x.{0}.Contains({1}) ", m.PropertyName, "search");
                }
                else if (m.ShortTypeName.ToLower() == "int" || m.ShortTypeName.ToLower() == "decimal" || m.ShortTypeName.ToLower() == "float")
                {
                    linqtxt += "|| " + string.Format("x.{0}.ToString().Contains({1}) ", m.PropertyName, "search");
                }
                else if (m.ShortTypeName.ToLower() == "datetime")
                {
                    linqtxt += "|| " + string.Format("x.{0}.ToString().Contains({1}) ", m.PropertyName, "search");
                }
            }
             
            return  " x => " + linqtxt.Substring(2);
        }
        public string GetSelectLambdaText(ModelMetadata efMetadata)
        {
            string linqtxt = "";
            string linqtxt1 = "";
            string linqtxt2 = "";
            linqtxt1= String.Join("",efMetadata.Properties.Where(n=>n.IsAssociation == true && n.AssociationDirection== AssociationDirection.ManyToOne && n.Scaffold)
                            .Select(n=> String.Format(",{0}{1} = (n.{2}==null?\"\": n.{2}.{3}) " ,n.PropertyName,n.RelatedModel.DisplayPropertyName ,n.PropertyName,n.RelatedModel.DisplayPropertyName )));
            linqtxt2 = String.Join("", efMetadata.Properties.Where(n => n.IsAssociation == false && n.Scaffold).Select(n => String.Format(", {0} = n.{1} ", n.PropertyName, n.PropertyName)));

            linqtxt = linqtxt1 + linqtxt2;
            return " n => new { " + linqtxt.Substring(1) + "}";
        }
        private void AddModelMetadata(Project project
            , string controllerName
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


            //Project project = Context.ActiveProject;
            var templatePath = Path.Combine("Model", "Metadata");
            string defaultNamespace = modelType.Namespace.FullName;
            string modelTypeVariable = GetTypeVariable(modelType.Name);
            string bindAttributeIncludeText = GetBindAttributeIncludeText(efMetadata);

            Dictionary<string, object> templateParams = new Dictionary<string, object>(){
                {"Namespace", defaultNamespace}
                , {"ModelTypeName", modelType.Name}
                , {"ModelMetadata", efMetadata}
                , {"MetaTable", _ModelMetadataVM.DataModel}
            };
            
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: !overwrite);
        }

        private void AddView(Project project
            , string viewsFolderPath 
            , string viewPrefix
            , string viewName
            , string programTitle
            , string controllerRootName
            , CodeType modelType
            , ModelMetadata efMetadata
            , bool referenceScriptLibraries = true
            , bool isLayoutPageSelected = true
            , string layoutPageFile = null
            , bool isBundleConfigPresent=true
            , bool overwrite = false
            , bool generateMasterDetailRelationship = false
            , bool checkedFormCols=false
            , int formClos=2
            , Dictionary<string,ModelMetadata> oneToManyModels = null
            , Dictionary<string, string> modelDisplayNames=null
            )
        {
           
            //Project project = Context.ActiveProject;
            string outputPath = Path.Combine(viewsFolderPath, viewPrefix+viewName);
            string templatePath = Path.Combine("MvcView", viewName);
            string viewDataTypeName = modelType.Namespace.FullName + "." + modelType.Name;
            string modelNameSpace = modelType.Namespace != null ? modelType.Namespace.FullName : String.Empty;
            if (layoutPageFile == null)
                layoutPageFile = string.Empty;

            if (modelDisplayNames == null)
                modelDisplayNames = GetDisplayNames(modelType.FullName);
      
            Dictionary<string, object> templateParams = new Dictionary<string, object>(){
                {"ControllerRootName" , controllerRootName}
                , {"ModelMetadata", efMetadata}
                , {"ModelTypeName", modelType.Name}
                , {"ViewPrefix", viewPrefix}
                , {"ViewName", viewName}
                , {"ProgramTitle", programTitle}
                , {"ModelNameSpace", modelNameSpace}
                , {"ViewDataTypeName", viewDataTypeName}
                , {"IsPartialView" , false}
                , {"LayoutPageFile", layoutPageFile}
                , {"IsLayoutPageSelected", isLayoutPageSelected}
                , {"ReferenceScriptLibraries", referenceScriptLibraries}
                , {"IsBundleConfigPresent", isBundleConfigPresent}
                , {"OneToManyModelMetadata", oneToManyModels}
                ,{"ModelDisplayNames",modelDisplayNames}
                ,{"CheckedFromLayoutCols",checkedFormCols}
                ,{"FromLayoutCols",formClos}
                ,{"GenerateMasterDetailRelationship" ,generateMasterDetailRelationship}
                //, {"ViewDataTypeShortName", modelType.Name} // 可刪除
                , {"MetaTable", _ModelMetadataVM.DataModel}
                , {"JQueryVersion","2.1.0"} // 如何讀取專案的 jQuery 版本
                , {"MvcVersion", new Version("5.1.2.0")}
            };

            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: !overwrite);
        }

        // Create a mapping between property names and display names in case
        // the property is decorated with a DisplayAttribute
        //protected Dictionary<string, string> GetDisplayNames(string modelmetaTypeName)
        //{
        //    var type = GetReflectionType(modelmetaTypeName);
        //    var lookup = new Dictionary<string, string>();
        //    foreach (PropertyInfo prop in type.GetProperties())
        //    {
        //        var attr = (System.ComponentModel.DataAnnotations.DisplayAttribute)prop.GetCustomAttribute(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), true);
        //        var value = attr != null && !String.IsNullOrWhiteSpace(attr.Name) ? attr.Name : prop.Name;
        //        if (!lookup.ContainsKey(prop.Name))
        //        {
        //            lookup.Add(prop.Name, value);
        //        }
        //    }
        //    return lookup;
             
        //}
        protected Dictionary<string, string> GetDisplayNames(string fullclassName)
        {
            var type = GetReflectionType(fullclassName);
            var lookup = new Dictionary<string, string>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                var attr = (System.ComponentModel.DataAnnotations.DisplayAttribute)prop.GetCustomAttribute(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), true);
                var value = attr != null && !String.IsNullOrWhiteSpace(attr.Name) ? attr.Name : prop.Name;
                if (!lookup.ContainsKey(prop.Name))
                    lookup.Add(prop.Name, value);
            }
            return lookup;
        }

        //add _Layout & _ViewStart
        private void AddDependencyFile(Project project, string viewRootPath, string areaName
            )
        {
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

            // add _LoginLayout
            viewName = "_LoginLayout";
            outputPath = Path.Combine(viewRootPath, "Shared", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);

            // add _SideNavBar
            viewName = "_SideNavBar";
            outputPath = Path.Combine(viewRootPath, "Shared", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);

            // add _TopNavBar
            viewName = "_TopNavBar";
            outputPath = Path.Combine(viewRootPath, "Shared", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);

            // add _LoginPartial
            viewName = "_LoginPartial";
            outputPath = Path.Combine(viewRootPath, "Shared", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);


            // add HtmlExtensions
            viewName = "HtmlExtensions";
            outputPath = Path.Combine("Extensions", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);


            // add PageListExtensions
            //viewName = "PageListExtensions";
            //outputPath = Path.Combine("Extensions", viewName);
            //templatePath = Path.Combine("MvcFullDependencyCodeGenerator", viewName);
            //templateParams = new Dictionary<string, object>(){
            //   {"DefaultNamespace", project.GetDefaultNamespace()}
            //};
            //AddFileFromTemplate(project: project
            //    , outputPath: outputPath
            //    , templateName: templatePath
            //    , templateParameters: templateParams
            //    , skipIfExists: true);

            //LinqOrderByColumnsNameExtensions
            viewName = "LinqOrderByColumnsNameExtensions";
            outputPath = Path.Combine("Extensions", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);

            // add sb_admin.css
            viewName = "sb-admin";
            outputPath = Path.Combine("Content", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator", viewName);
            templateParams = new Dictionary<string, object>(){
               
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);


            //RoleManager
            // add ApplicationRole.cs
            viewName = "ApplicationRole";
            outputPath = Path.Combine("Models", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\RoleManager", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);


            // add ApplicationRoleManager.cs
            viewName = "ApplicationRoleManager";
            outputPath = Path.Combine("App_Start", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\RoleManager", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);

            // add ManagementController.cs
            viewName = "ManagementController";
            outputPath = Path.Combine("Controllers", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\RoleManager", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);

            // add ButtonAttribute
            viewName = "ButtonAttribute";
            outputPath = Path.Combine("Controllers", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\RoleManager", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);

            // add ManagementViewModels.cs
            viewName = "ManagementViewModels";
            outputPath = Path.Combine("Models", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\RoleManager", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);

            // add managerviews\
            var managerviews = new string[] { "Index", "_RoleRecordPartial", "_DeleteAccountPartial", "_AttachRolePartial", "_AddRolePartial", "_AddAccountPartial", "_AccountRecordPartial" };
            foreach (string vname in managerviews)
            {
                viewName = vname;
                outputPath = Path.Combine(viewRootPath,"Management", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\RoleManager\\View", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }

            var accountmanageviews = new string[] { "Index", "Edit", "Create" };
            foreach (string vname in accountmanageviews)
            {
                viewName = vname;
                outputPath = Path.Combine(viewRootPath,"AccountManage", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\UserManage\\View\\AccountManage", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }

            var loginviews = new string[] { "Login", "Register"};
            foreach (string vname in loginviews)
            {
                viewName = vname;
                outputPath = Path.Combine(viewRootPath,"Account", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\Account", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }

            // add AccountManageController.cs
            viewName = "AccountManageController";
            outputPath = Path.Combine("Controllers", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\UserManage", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);

           //add basecode
            // add BaseCodesController.cs
            viewName = "BaseCodesController";
            outputPath = Path.Combine("Controllers", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\BaseCode", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);


            var basecodeviews = new string[] { "_CodeItemEditForm", "Create", "Edit", "EditForm", "Index" };
            foreach (string vname in basecodeviews)
            {
                viewName = vname;
                outputPath = Path.Combine(viewRootPath, "BaseCodes", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\BaseCode\\Views", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }
            var basecodemodels = new string[] { "BaseCode", "BaseCodeMetadata" };
            foreach (string vname in basecodemodels)
            {
                viewName = vname;
                outputPath = Path.Combine("Models", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\BaseCode\\Models", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }

            var basecodeservices = new string[] { "BaseCodeService", "IBaseCodeService" };
            foreach (string vname in basecodeservices)
            {
                viewName = vname;
                outputPath = Path.Combine("Services","BaseCodes", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\BaseCode\\Services", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }

            var basecoderepository = new string[] { "BaseCodeQuery", "BaseCodeRepository" };
            foreach (string vname in basecoderepository)
            {
                viewName = vname;
                outputPath = Path.Combine("Repositories", "BaseCodes", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\BaseCode\\Repositories", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }

           //end basecode


            //add menuitem
            // add MenuItemsController.cs
            viewName = "MenuItemsController";
            outputPath = Path.Combine("Controllers", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\MenuItem", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);


            var menitemviews = new string[] { "_MenuItemEditForm", "Create", "Edit", "EditForm", "Index" };
            foreach (string vname in menitemviews)
            {
                viewName = vname;
                outputPath = Path.Combine(viewRootPath, "MenuItems", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\MenuItem\\Views", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }
            var menuitemmodels = new string[] { "MenuItem", "MenuItemMetadata" };
            foreach (string vname in menuitemmodels)
            {
                viewName = vname;
                outputPath = Path.Combine("Models", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\MenuItem\\Models", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }

            var menuitemservices = new string[] { "MenuItemService", "IMenuItemService" };
            foreach (string vname in menuitemservices)
            {
                viewName = vname;
                outputPath = Path.Combine("Services", "MenuItems", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\MenuItem\\Services", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }

            var menuitemrepository = new string[] { "MenuItemQuery", "MenuItemRepository" };
            foreach (string vname in menuitemrepository)
            {
                viewName = vname;
                outputPath = Path.Combine("Repositories", "MenuItems", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\MenuItem\\Repositories", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }

            //end menuitem


            //add rolemenu
            // add RoleMenusController.cs
            viewName = "RoleMenusController";
            outputPath = Path.Combine("Controllers", viewName);
            templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\RoleMenu", viewName);
            templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
            AddFileFromTemplate(project: project
                , outputPath: outputPath
                , templateName: templatePath
                , templateParameters: templateParams
                , skipIfExists: true);


            var rolemenuviews = new string[] { "_navMenuBar", "Index" };
            foreach (string vname in rolemenuviews)
            {
                viewName = vname;
                outputPath = Path.Combine(viewRootPath, "RoleMenus", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\RoleMenu\\Views", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }
            var rolemenummodels = new string[] { "RoleMenu", "RoleMenuMetadata", "RoleMenuViewModel" };
            foreach (string vname in rolemenummodels)
            {
                viewName = vname;
                outputPath = Path.Combine("Models", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\RoleMenu\\Models", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }

            var rolemanuservices = new string[] { "RoleMenuService", "IRoleMenuService" };
            foreach (string vname in rolemanuservices)
            {
                viewName = vname;
                outputPath = Path.Combine("Services", "RoleMenus", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\RoleMenu\\Services", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }

            var rolemenurepository = new string[] { "RoleMenuQuery", "RoleMenuRepository" };
            foreach (string vname in rolemenurepository)
            {
                viewName = vname;
                outputPath = Path.Combine("Repositories", "RoleMenus", viewName);
                templatePath = Path.Combine("MvcFullDependencyCodeGenerator\\RoleMenu\\Repositories", viewName);
                templateParams = new Dictionary<string, object>(){
               {"DefaultNamespace", project.GetDefaultNamespace()}
            };
                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }

            //end menuitem

        }

        private void AddDataFieldTemplates(Project project, string viewRootPath)
        {
            Dictionary<string, object> templateParams = new Dictionary<string, object>();

            var fieldTemplates = new[] { 
                "EditorTemplates\\Date",
                "EditorTemplates\\BaseCode",
                "EditorTemplates\\Slider"
                , "DisplayTemplates\\Date"
                , "DisplayTemplates\\DateTime"
            };

            foreach (var fieldTemplate in fieldTemplates)
            {
                string outputPath = Path.Combine(viewRootPath, "Shared", fieldTemplate);
                string templatePath = Path.Combine("DataFieldTemplates", fieldTemplate);

                AddFileFromTemplate(project: project
                    , outputPath: outputPath
                    , templateName: templatePath
                    , templateParameters: templateParams
                    , skipIfExists: true);
            }
            
            //var fieldTemplatesPath = "DynamicData\\FieldTemplates";

            //// Add the folder
            //AddFolder(project, fieldTemplatesPath);

            //foreach (var fieldTemplate in fieldTemplates)
            //{
            //    var templatePath = Path.Combine(fieldTemplatesPath, fieldTemplate);
            //    var outputPath = Path.Combine(fieldTemplatesPath, fieldTemplate);



            //    AddFileFromTemplate(
            //        project: project,
            //        outputPath: outputPath,
            //        templateName: templatePath,
            //        templateParameters: new Dictionary<string, object>() 
            //        {
            //            {"DefaultNamespace", project.GetDefaultNamespace()},
            //            {"GenericRepositoryNamespace", genericRepositoryNamespace}
            //        },
            //        skipIfExists: true);
            //}
        }


        private void AddEntityRepositoryExtensionTemplates(
      Project project,
      string selectionRelativePath,
      string dbContextNamespace,
      string dbContextTypeName,
      CodeType modelType,
      ModelMetadata efMetadata,
      Dictionary<string,ModelMetadata> oneToManyModels,
      bool overwriteViews = true
      
        
  )
        {
            string modelName = "";
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }
            if (modelName == "")
            {
                modelName = modelType.Name;
            }
            string modelNameSpace = modelType.Namespace != null ? modelType.Namespace.FullName : String.Empty;
            // Get pluralized name used for web forms folder name
            string pluralizedModelName = efMetadata.EntitySetName;
            var repositoryTemplates = new[] { "EntityRepositoryExtension","EntityQuery" };
            var repositoryTemplatesPath = "Repositories";
            var queryLambdaText = GetQueryLambdaText(efMetadata);
            PropertyMetadata primaryKey = efMetadata.PrimaryKeys.FirstOrDefault();

            // Add folder for views. This is necessary to display an error when the folder already exists but 
            // the folder is excluded in Visual Studio: see https://github.com/Superexpert/WebFormsScaffolding/issues/18
            string outputFolderPath = Path.Combine("Repositories", pluralizedModelName.Replace("_", ""));
            //AddFolder(Context.ActiveProject, outputFolderPath);

        
            AddFolder(Context.ActiveProject, outputFolderPath);

            // Now add each view
            foreach (string repository in repositoryTemplates)
            {
                var templatePath = Path.Combine(repositoryTemplatesPath, repository);
                var outputFileName = "";
                if (repository == "EntityQuery")
                    outputFileName = modelName + "Query";
                else
                    outputFileName = modelName + "Repository";
                var outputPath = Path.Combine(outputFolderPath, outputFileName);

                var defaultNamespace = Context.ActiveProject.GetDefaultNamespace();
                var folderNamespace = GetDefaultNamespace() + ".Repositories";
                AddFileFromTemplate(
                    project: project,
                    outputPath: outputPath,
                    templateName: templatePath,
                    templateParameters: new Dictionary<string, object>() 
                    {
                        {"DefaultNamespace", project.GetDefaultNamespace()},
                        {"DbContextNamespace", dbContextNamespace},
                        {"DbContextTypeName", dbContextTypeName},
                        {"ModelMetadata",efMetadata},
                        {"PrimaryKeyName", primaryKey.PropertyName}, 
                        {"ModelName", modelName}, // singular model name (e.g., Movie)
                        {"FolderNamespace", folderNamespace.Replace("_","")}, // the namespace of the current folder (used by C#)
                        {"PluralizedModelName",pluralizedModelName},
                        {"QueryLambdaText",queryLambdaText},
                        {"OneToManyModelMetadata", oneToManyModels},
                        {"ModelNamespace", modelNameSpace} // the namespace of the model (e.g., Samples.Models)               
                    },
                    skipIfExists: true);

            }
        }
        private void AddEntityServiceTemplates(
           Project project,
          string selectionRelativePath,
           string dbContextNamespace,
           string dbContextTypeName,
           CodeType modelType,
           ModelMetadata efMetadata,
           Dictionary<string, ModelMetadata> oneToManyModels,
           bool overwriteViews = true
          

       )
        {
            string modelName = "";
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }
            if (modelName == "")
            {
                modelName = modelType.Name;
            }
            string modelNameSpace = modelType.Namespace != null ? modelType.Namespace.FullName : String.Empty;
            // Get pluralized name used for web forms folder name
            string pluralizedModelName = efMetadata.EntitySetName;
          
            var serviceTemplates = new[] { "IEntityService", "EntityService" };
            var repositoryTemplatesPath = "Services";


            // Add folder for views. This is necessary to display an error when the folder already exists but 
            // the folder is excluded in Visual Studio: see https://github.com/Superexpert/WebFormsScaffolding/issues/18
            string outputFolderPath = Path.Combine("Services", pluralizedModelName.Replace("_", ""));
            //AddFolder(Context.ActiveProject, outputFolderPath);


            AddFolder(Context.ActiveProject, outputFolderPath);
            PropertyMetadata primaryKey = efMetadata.PrimaryKeys.FirstOrDefault();
            // Now add each view
            foreach (string service in serviceTemplates)
            {
                var templatePath = Path.Combine(repositoryTemplatesPath, service);
                var outputFileName = "";
                if (service == "IEntityService")
                    outputFileName = "I" + modelName + "Service";
                else
                    outputFileName = modelName + "Service";
                var outputPath = Path.Combine(outputFolderPath, outputFileName);

                var defaultNamespace = Context.ActiveProject.GetDefaultNamespace();
                var folderNamespace = GetDefaultNamespace() + ".Services";
                AddFileFromTemplate(
                    project: project,
                    outputPath: outputPath,
                    templateName: templatePath,
                    templateParameters: new Dictionary<string, object>() 
                    {
                        {"DefaultNamespace", project.GetDefaultNamespace()},
                        {"DbContextNamespace", dbContextNamespace},
                        {"DbContextTypeName", dbContextTypeName},
                        {"ModelMetadata",efMetadata},
                        {"PrimaryKeyName", primaryKey.PropertyName}, 
                        {"ModelName", modelName}, // singular model name (e.g., Movie)
                        {"FolderNamespace", folderNamespace.Replace("_","")}, // the namespace of the current folder (used by C#)
                        {"PluralizedModelName",pluralizedModelName},
                        {"OneToManyModelMetadata", oneToManyModels},
                        {"ModelNamespace", modelNameSpace} // the namespace of the model (e.g., Samples.Models)               
                    },
                    skipIfExists: true);

            }
        }

        private void AddSharedLayoutTemplates( Project project,
               string viewsFolderPath,
           string selectionRelativePath,
           string dbContextNamespace,
           string dbContextTypeName,
           CodeType modelType,
           ModelMetadata efMetadata)
        {
            var layoutTemplates = new[] { "_Layout", "_SideNavBar", "_TopNavBar" };
            var layoutTemplatesPath = "Shared";


            // Add folder for views. This is necessary to display an error when the folder already exists but 
            // the folder is excluded in Visual Studio: see https://github.com/Superexpert/WebFormsScaffolding/issues/18
            string outputFolderPath = Path.Combine(viewsFolderPath,"Shared");
            //AddFolder(Context.ActiveProject, outputFolderPath);


            AddFolder(Context.ActiveProject, outputFolderPath);
            PropertyMetadata primaryKey = efMetadata.PrimaryKeys.FirstOrDefault();
            // Now add each view
            foreach (string layout in layoutTemplates)
            {
                var templatePath = Path.Combine(layoutTemplatesPath, layout);

                var outputPath = Path.Combine(outputFolderPath, layout);

                //var defaultNamespace = Context.ActiveProject.GetDefaultNamespace();
                //var folderNamespace = GetDefaultNamespace() + ".Services";
                AddFileFromTemplate(
                    project: project,
                    outputPath: outputPath,
                    templateName: templatePath,
                    templateParameters: new Dictionary<string, object>() 
                    {
                        {"DefaultNamespace", project.GetDefaultNamespace()}
                     
                       
                    },
                    skipIfExists: true);

            }
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


        // Called to ensure that the project was compiled successfully
        private Type GetReflectionType(string typeName)
        {
            return GetService<IReflectedTypesService>().GetType(Context.ActiveProject, typeName);
        }

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
        private string GetViewsFolderPath(string selectionRelativePath)
        {
            //string keyControllers = "Controllers";
            //string keyViews = "Views";

            //return (
            //    (
            //    controllerPath.IndexOf(keyControllers) >= 0)
            //    ? controllerPath.Replace(keyControllers, keyViews)
            //    : Path.Combine(controllerPath, keyViews)
            //    );
            return GetRelativeFolderPath(selectionRelativePath, "Views");
        }
        private string GetModelFolderPath(string selectionRelativePath)
        {
            return GetRelativeFolderPath(selectionRelativePath, "Models");
        }
        private string GetRelativeFolderPath(string selectionRelativePath, string folderName)
        {
            string keyControllers = "Controllers";
            string keyViews = folderName;

            return (
                (
                selectionRelativePath.IndexOf(keyControllers) >= 0)
                ? selectionRelativePath.Replace(keyControllers, keyViews)
                : Path.Combine(selectionRelativePath, keyViews)
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





        #endregion

        #region no used
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

        //private void WriteLog(string message)
        //{
        //    System.IO.StreamWriter sw = new StreamWriter("R:\\LOG.Scaffold.txt", true);
        //    sw.WriteLine(message);
        //    sw.Close();
        //}

        #endregion

    }
}
