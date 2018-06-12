using Microsoft.AspNet.Scaffolding.Core.Metadata;
using System.Collections.ObjectModel;
using System;
using System.Windows.Input;
using System.Linq;
using Happy.Scaffolding.MVC.Models;
using System.Windows;
using EnvDTE;


namespace Happy.Scaffolding.MVC.UI
{
    internal class MetadataSettingViewModel : ViewModel<ModelMetadataViewModel>
    {
        public MetadataSettingViewModel(ModelMetadata efMetadata)
        {
            MetaTableInfo dataModel = new MetaTableInfo();
            foreach (Microsoft.AspNet.Scaffolding.Core.Metadata.PropertyMetadata p1 in efMetadata.Properties)
            {
                dataModel.Columns.Add(new MetaColumnInfo(p1));
            }
            Init(dataModel);
        }

        public MetadataSettingViewModel(CodeFunction codefunction)
        {
            MetaTableInfo dataModel = new MetaTableInfo();
            foreach (CodeElement ce in codefunction.Parameters)
            {
                CodeParameter p1 = (CodeParameter)ce;
                dataModel.Columns.Add(new MetaColumnInfo(p1));
            }
            Init(dataModel);
        }

        public MetadataSettingViewModel(CodeType codeType)
        {
            MetaTableInfo dataModel = new MetaTableInfo();
            foreach (CodeElement ce in codeType.Members)
            {
                dataModel.Columns.Add(new MetaColumnInfo((CodeProperty)ce));
            }
            Init(dataModel);
        }

        public MetadataSettingViewModel(MetaTableInfo dataModel)
        {
            Init(dataModel);
        }

        private void Init(MetaTableInfo dataModel)
        {
            this.DataModel = dataModel;
            this.Columns = new ObservableCollection<MetadataFieldViewModel>();
            foreach (MetaColumnInfo f1 in dataModel.Columns)
            {
                this.Columns.Add(new MetadataFieldViewModel(f1));
            }
        }

        public MetadataFieldViewModel this[string name]
        {
            get { return this.Columns.FirstOrDefault(x => x.Name == name); }
        }

        public MetadataFieldViewModel this[int index]
        {
            get { return this.Columns[index]; }
        }

        public MetaTableInfo DataModel { get; private set; }

        private ObservableCollection<MetadataFieldViewModel> m_Columns = null;

        public ObservableCollection<MetadataFieldViewModel> Columns
        {
            get { return m_Columns; }
            private set { this.m_Columns = value; }
        }

    }

    internal class ModelMetadataViewModel : MetadataSettingViewModel
    {
        public ModelMetadataViewModel(ModelMetadata efMetadata):base(efMetadata){}
        public ModelMetadataViewModel(CodeFunction codeElements) : base(codeElements) { }
        public ModelMetadataViewModel(CodeType codeType) : base(codeType) { }
        public ModelMetadataViewModel(MetaTableInfo dataModel) : base(dataModel) { }


        private bool m_IsConfirm = false;
        
        public bool IsConfirm
        {
            get { return m_IsConfirm; }
            set
            {
                if (value == m_IsConfirm)
                {
                    return;
                }
                m_IsConfirm = value;
                OnPropertyChanged();
            }
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
                        if (IsConfirm)
                            OnClose(result: true);
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
    }

    internal class MetadataFieldViewModel : ViewModel<MetadataFieldViewModel>
    {
        public MetadataFieldViewModel(MetaColumnInfo data)
        {
            this.DataModel = data;
        }

        public MetaColumnInfo DataModel { get; private set; }

        public string Name
        {
            get { return DataModel.Name; }
            private set
            {
                DataModel.Name = value;
            }
        }

        public string DisplayName
        {
            get { return DataModel.DisplayName; }
            set
            {
                if (value == DataModel.DisplayName)
                {
                    return;
                }
                DataModel.DisplayName = value;
                OnPropertyChanged();
            }
        }

        public bool Nullable
        {
            get { return DataModel.Nullable; }
            set
            {
                if (value == DataModel.Nullable)
                {
                    return;
                }
                DataModel.Nullable = value;
                OnPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get { return DataModel.IsVisible; }
            set
            {
                if (value == DataModel.IsVisible)
                {
                    return;
                }
                DataModel.IsVisible = value;
                OnPropertyChanged();
            }
        }

        public string strDateType
        {
            get { return DataModel.strDateType; }
        }

        public int MaxLength
        {
            get { return DataModel.MaxLength; }
            set
            {
                if (value == DataModel.MaxLength)
                {
                    return;
                }
                DataModel.MaxLength = value;
                OnPropertyChanged();
            }
        }

        public int RangeMin
        {
            get { return DataModel.RangeMin; }
            set
            {
                if (value == DataModel.RangeMin)
                {
                    return;
                }
                DataModel.RangeMin = value;
                OnPropertyChanged();
            }
        }

        public int RangeMax
        {
            get { return DataModel.RangeMax; }
            set
            {
                if (value == DataModel.RangeMax)
                {
                    return;
                }
                DataModel.RangeMax = value;
                OnPropertyChanged();
            }
        }

        public Visibility ShowEditorMaxLength
        {
            get
            {
                if (DataModel.DataType == euColumnType.stringCT)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public Visibility ShowEditorRange
        {
            get
            {
                if (DataModel.DataType == euColumnType.intCT 
                    || DataModel.DataType == euColumnType.decimalCT)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

    }
}
