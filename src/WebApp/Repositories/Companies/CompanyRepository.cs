/// <summary>
/// File: CompanyRepository.cs
/// Purpose: The repository and unit of work patterns are intended
/// to create an abstraction layer between the data access layer and
/// the business logic layer of an application.
/// Date: 2018/11/9 15:31:56
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 neo.zhu and Contributors
/// License: GNU General Public License v3.See license.txt
/// </summary>
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
			var departmentRepository = repository.GetRepository<Department>(); 
            return departmentRepository.Queryable().Include(x => x.Company).Where(n => n.CompanyId == companyid);
        }
                public static IEnumerable<Employee>   GetEmployeesByCompanyId (this IRepositoryAsync<Company> repository,int companyid)
        {
			var employeeRepository = repository.GetRepository<Employee>(); 
            return employeeRepository.Queryable().Include(x => x.Company).Where(n => n.CompanyId == companyid);
        }
         
	}
}



