



using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using WebApp.Models;
using WebApp.Extensions;

namespace WebApp.Repositories
{
    public class BaseCodeQuery : QueryObject<BaseCode>
    {
        public BaseCodeQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And(x => x.CodeType.Contains(search) || x.Description.Contains(search));
            return this;
        }

        public BaseCodeQuery WithPopupSearch(string search, string para = "")
        {
            if (!string.IsNullOrEmpty(search))
                And(x => x.CodeType.Contains(search) || x.Description.Contains(search));
            return this;
        }

        public BaseCodeQuery Withfilter(IEnumerable<filterRule> filters)
        {
            if (filters != null)
            {
                foreach (var rule in filters)
                {





                    if (rule.field == "Id")
                    {
                        And(x => x.Id == Convert.ToInt32(rule.value));
                    }


                    if (rule.field == "CodeType")
                    {
                        And(x => x.CodeType.Contains(rule.value));
                    }



                    if (rule.field == "Description")
                    {
                        And(x => x.Description.Contains(rule.value));
                    }




                }
            }
            return this;
        }
    }
}



