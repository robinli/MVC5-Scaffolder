using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplication1 {
    public partial class Default_InsertEntityTemplate : System.Web.DynamicData.EntityTemplateUserControl {
        private MetaColumn currentColumn;
    
		// exclude [Key] or Id or ModelId column from scaffolding
	    private bool ShouldScaffold(MetaColumn column) {
			// [key] attribute
			if (column.IsPrimaryKey) {
				return false;
			}

			// convention
            var columnName = column.Name.ToLowerInvariant();
            var tableName = this.Table.Name.ToLowerInvariant();
            return columnName != "id" && columnName != tableName + "id";
        }


        protected override void OnLoad(EventArgs e) {
            foreach (MetaColumn column in Table.GetScaffoldColumns(Mode, ContainerType)) {
				if (ShouldScaffold(column)) {
					currentColumn = column;
					Control item = new DefaultEntityTemplate._NamingContainer();
					EntityTemplate1.ItemTemplate.InstantiateIn(item);
					EntityTemplate1.Controls.Add(item);
				}
            }
        }
    
        protected void Label_Init(object sender, EventArgs e) {
            Label label = (Label)sender;
            label.Text = currentColumn.DisplayName;
        }
    
        protected void Label_PreRender(object sender, EventArgs e) {
            Label label = (Label)sender;
            DynamicControl dynamicControl = (DynamicControl)label.FindControl("DynamicControl");
            FieldTemplateUserControl ftuc = dynamicControl.FieldTemplate as FieldTemplateUserControl;
            if (ftuc != null && ftuc.DataControl != null) {
                label.AssociatedControlID = ftuc.DataControl.GetUniqueIDRelativeTo(label);
            }
        }
    
        protected void DynamicControl_Init(object sender, EventArgs e) {
            DynamicControl dynamicControl = (DynamicControl)sender;
            dynamicControl.DataField = currentColumn.Name;
        }

		// show bootstrap has-error
		protected void Div1_PreRender(object sender, EventArgs e)
        {
            var Div1 = (HtmlGenericControl)sender;

            // get column
            DynamicControl dynamicControl = (DynamicControl)Div1.FindControl("DynamicControl");
            var columnName = dynamicControl.DataField;

            // if validation error then apply bootstrap has-error CSS class
            var isValid = this.Page.ModelState.IsValidField(columnName);
            Div1.Attributes["class"] = isValid ? "form-group" : "form-group has-error";
        }

    
    }
}
