
             
           
 

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using Service.Pattern;

using WebApp.Models;
using WebApp.Repositories;
namespace WebApp.Services
{
    public class DepartmentService : Service< Department >, IDepartmentService
    {

        private readonly IRepositoryAsync<Department> _repository;
        public  DepartmentService(IRepositoryAsync< Department> repository)
            : base(repository)
        {
            _repository=repository;
        }
        
                 public  IEnumerable<Department> GetByCompanyId(int  companyid)
         {
            return _repository.GetByCompanyId(companyid);
         }
                   
        
    }
}



