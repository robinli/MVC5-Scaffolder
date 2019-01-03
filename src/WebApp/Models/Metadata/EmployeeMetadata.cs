// <copyright file="EmployeeMetadata.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2018/11/15 10:28:48 </date>
// <summary>Class representing a Metadata entity </summary>

using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    //[MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
    }

    public partial class EmployeeMetadata
    {
        [Display(Name = "Company")]
        public Company Company { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Sex")]
        public string Sex { get; set; }

        [Display(Name = "Age")]
        public int Age { get; set; }

        [Display(Name = "Brithday")]
        public DateTime Brithday { get; set; }

        [Display(Name = "IsDeleted")]
        public int IsDeleted { get; set; }

        [Display(Name = "CompanyId")]
        public int CompanyId { get; set; }

        [Display(Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy")]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy")]
        public string LastModifiedBy { get; set; }

    }




	public class EmployeeChangeViewModel
    {
        public IEnumerable<Employee> inserted { get; set; }
        public IEnumerable<Employee> deleted { get; set; }
        public IEnumerable<Employee> updated { get; set; }
    }

}
