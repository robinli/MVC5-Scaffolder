

     
 
 

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;
using System.Data;
using System.IO;

namespace WebApp.Services
{
    public interface IProductService:IService<Product>
    {

                  IEnumerable<Product> GetByCategoryId(int  categoryid);
        
         
 
		void ImportDataTable(DataTable datatable);
		FileInfo ExportExcel(string fileName,  string filterRules = "",string sort = "Id", string order = "asc");
	}
}