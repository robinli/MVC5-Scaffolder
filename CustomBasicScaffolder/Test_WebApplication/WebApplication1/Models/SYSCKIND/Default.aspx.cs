using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

namespace WebApplication1.Models.SYSCKIND
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // Model binding method to get List of SYSCKIND entries
        // USAGE: <asp:ListView SelectMethod="GetData">
        public IQueryable<WebApplication1.SYSCKIND> GetData()
        {
            var context = new Entities();

            return context.SYSCKIND;
        }
    }
}
