using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1 {
    public partial class Boolean_EditField : System.Web.DynamicData.FieldTemplateUserControl {

		protected void Page_Load(object sender, EventArgs e)
        {
            CheckBox1.ToolTip = Column.Description;
			Label1.Text = Column.DisplayName;
        }


        protected override void OnDataBinding(EventArgs e) {
            base.OnDataBinding(e);
    
            object val = FieldValue;
            if (val != null)
                CheckBox1.Checked = (bool) val;
        }
    
        protected override void ExtractValues(IOrderedDictionary dictionary) {
            dictionary[Column.Name] = CheckBox1.Checked;
        }
    
        public override Control DataControl {
            get {
                return CheckBox1;
            }
        }
    
    }
}
