using Microsoft.AspNet.Scaffolding.Core.Metadata;
using System.Collections.ObjectModel;
using System;
using System.Windows.Input;
using System.Linq;
using Happy.Scaffolding.MVC.Models;


namespace Happy.Scaffolding.MVC.UI
{
    internal class ModelMetadataViewModel : ViewModel<ModelMetadataViewModel>
    {
        public ModelMetadataViewModel(ModelMetadata efMetadata)
        {
            MetaTableInfo dataModel = new MetaTableInfo();
            foreach (PropertyMetadata p1 in efMetadata.Properties)
            {
                dataModel.Columns.Add(new MetaColumnInfo(p1));
            }
            Init(dataModel);
            //this.Columns = new ObservableCollection<MetadataFieldViewModel>();
            //foreach (PropertyMetadata p1 in efMetadata.Properties)
            //{
            //    MetadataFieldinfo info = new MetadataFieldinfo(p1);

            //    this.DataModel.Columns.Add(info);

            //    this.Columns.Add(new MetadataFieldViewModel(info));
            //}
        }
        public ModelMetadataViewModel(MetaTableInfo dataModel)
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

        public MetaTableInfo DataModel { get; private set; }

        private ObservableCollection<MetadataFieldViewModel> m_Columns = null;
        public ObservableCollection<MetadataFieldViewModel> Columns
        {
            get { return m_Columns; }
            private set { this.m_Columns = value; }
        }


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
        //public MetadataFieldViewModel(PropertyMetadata p1)
        //{
        //    //this.Name = p1.PropertyName;
        //    //DisplayName = p1.PropertyName;
        //    //Nullable = (p1.IsPrimaryKey ? false : true);
        //    this.DataModel = new MetadataFieldinfo(p1);
        //}
        public MetadataFieldViewModel(MetaColumnInfo data)
        {
            this.DataModel = data;
        }

        public MetaColumnInfo DataModel { get; private set; }

        //public string m_Name;
        public string Name
        {
            get { return DataModel.Name; }
            private set
            {
                DataModel.Name = value;
            }
        }

        //private string m_DisplayName;
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


        //public bool m_Nullable;
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
    }
}
