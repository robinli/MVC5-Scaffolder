
                    
      
    
 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;

using WebApp.Models;

namespace WebApp.Repositories
{
  public static class CompanyRepository  
    {
 
        
                public static IEnumerable<Department>   GetDepartmentsByCompanyId (this IRepositoryAsync<Company> repository,int companyid)
        {
            var query = repository.Query(n => n.Id == companyid).Include(x => x.Departments).Select().FirstOrDefault();
            return query.Departments;
            //return repository.Query(n => n.Id == companyid).Include(x => x.Departments).Select(x => x.Departments).FirstOrDefault();
        }
         
	}
}



