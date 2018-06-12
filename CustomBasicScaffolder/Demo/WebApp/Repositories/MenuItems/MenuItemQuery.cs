



using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.SqlServer;
using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using System.Web.WebPages;
using WebApp.Models;
 

namespace WebApp.Repositories
{
    public class MenuItemQuery : QueryObject<MenuItem>
    {
        public MenuItemQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And(x => x.Id.ToString().Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) || x.Code.Contains(search) || x.Url.Contains(search) || x.IconCls.Contains(search) || x.ParentId.ToString().Contains(search));
            return this;
        }

        public MenuItemQuery WithPopupSearch(string search, string para = "")
        {
            if (!string.IsNullOrEmpty(search))
                And(x => x.Id.ToString().Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) || x.Code.Contains(search) || x.Url.Contains(search) || x.IconCls.Contains(search) || x.ParentId.ToString().Contains(search));
            return this;
        }

        public MenuItemQuery Withfilter(IEnumerable<filterRule> filters)
        {
            if (filters != null)
            {
                foreach (var rule in filters)
                {


                    if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
                    {
                        int val = Convert.ToInt32(rule.value);
                        And(x => x.Id == val);
                    }




                    if (rule.field == "Title" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.Title.Contains(rule.value));
                    }





                    if (rule.field == "Description" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.Description.Contains(rule.value));
                    }





                    if (rule.field == "Code" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.Code.Contains(rule.value));
                    }





                    if (rule.field == "Url" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.Url.Contains(rule.value));
                    }

                    if (rule.field == "Action" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.Action.Contains(rule.value));
                    }

                    if (rule.field == "Controller" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.Controller.Contains(rule.value));
                    }

                    if (rule.field == "IconCls" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.IconCls.Contains(rule.value));
                    }









                    if (rule.field == "IsEnabled" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
                    {
                        var boolval = Convert.ToBoolean(rule.value);
                        And(x => x.IsEnabled == boolval);
                    }


                    if (rule.field == "ParentId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
                    {
                        int val = Convert.ToInt32(rule.value);
                        And(x => x.ParentId == val);
                    }





                }
            }
            return this;
        }
    }
}



