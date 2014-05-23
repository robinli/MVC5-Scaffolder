using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1 {
    public partial class FieldLabelField : System.Web.DynamicData.FieldTemplateUserControl {
    
		protected void Page_Load(object sender, EventArgs e) {
			Label1.Text = Column.DisplayName;
        }

        public override Control DataControl {
            get {
                return Label1;
            }
        }
    
    }
}
