/// <summary>
/// File: CategoryRepository.cs
/// Purpose: The repository and unit of work patterns are intended
/// to create an abstraction layer between the data access layer and
/// the business logic layer of an application.
/// Date: 2018/12/20 8:54:07
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
  public static class CategoryRepository  
    {
                        public static IEnumerable<Product>   GetProductsByCategoryId (this IRepositoryAsync<Category> repository,int categoryid)
        {
			var productRepository = repository.GetRepository<Product>(); 
            return productRepository.Queryable().Include(x => x.Category).Where(n => n.CategoryId == categoryid);
        }
         
	}
}



