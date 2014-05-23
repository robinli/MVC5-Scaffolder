using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

namespace WebApplication1.Models.SYSCKIND
{
    public partial class Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // This is the Update methd to update the selected SYSCKIND item
        // USAGE: <asp:FormView UpdateMethod="UpdateItem">
        public void UpdateItem(string  CKIND)
        {
            using (var context = new Entities())
            {
                var item = context.SYSCKIND.Find(CKIND);

                if (item == null)
                {
                    // The item wasn't found
                    ModelState.AddModelError("", String.Format("Item with id {0} was not found", CKIND));
                    return;
                }

                TryUpdateModel(item);

                if (ModelState.IsValid)
                {
                    // Save changes here
                    context.SaveChanges();
                    Response.Redirect("Default.aspx");
                }
            }
        }

        // This is the Select method to selects a single SYSCKIND item with the id
        // USAGE: <asp:FormView SelectMethod="GetItem">
        public WebApplication1.SYSCKIND GetItem([QueryString]string ? CKIND)
        {
            if (CKIND == null)
            {
                return null;
            }

            using (var context = new Entities())
            {
                return context.SYSCKIND.Find(CKIND);
            }
        }

        protected void ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
}
