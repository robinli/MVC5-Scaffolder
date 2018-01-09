
                    
      
     
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using WebApp.Models;
 

namespace WebApp.Repositories
{
    public class CompanyQuery : QueryObject<Company>
    {
        public CompanyQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And(x => x.Name.Contains(search) || x.Address.Contains(search) || x.City.Contains(search) || x.Province.Contains(search) || x.RegisterDate.ToString().Contains(search));
            return this;
        }

        public CompanyQuery WithPopupSearch(string search, string para = "")
        {
            if (!string.IsNullOrEmpty(search))
                And(x => x.Name.Contains(search) || x.Address.Contains(search) || x.City.Contains(search) || x.Province.Contains(search) || x.RegisterDate.ToString().Contains(search));
            return this;
        }

        public CompanyQuery Withfilter(IEnumerable<filterRule> filters)
        {
            if (filters != null)
            {
                foreach (var rule in filters)
                {








                    if (rule.field == "Id")
                    {
                        And(x => x.Id == Convert.ToInt32(rule.value));
                    }


                    if (rule.field == "Name")
                    {
                        And(x => x.Name.Contains(rule.value));
                    }



                    if (rule.field == "Address")
                    {
                        And(x => x.Address.Contains(rule.value));
                    }



                    if (rule.field == "City")
                    {
                        And(x => x.City.Contains(rule.value));
                    }



                    if (rule.field == "Province")
                    {
                        And(x => x.Province.Contains(rule.value));
                    }





                    if (rule.field == "RegisterDate")
                    {
                        And(x => x.RegisterDate.Date == Convert.ToDateTime(rule.value).Date);
                    }


                    if (rule.field == "Employees")
                    {
                        And(x => x.Employees == Convert.ToInt32(rule.value));
                    }



                }
            }
            return this;
        }
    }
}



