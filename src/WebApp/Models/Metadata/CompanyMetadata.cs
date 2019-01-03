// <copyright file="CompanyMetadata.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2018/11/15 10:23:31 </date>
// <summary>Class representing a Metadata entity </summary>

using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    //[MetadataType(typeof(CompanyMetadata))]
    public partial class Company
    {
    }

    public partial class CompanyMetadata
    {
        [Display(Name = "Departments")]
        public Department Departments { get; set; }

        [Display(Name = "Employees")]
        public Employee Employees { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "RegisterDate")]
        public DateTime RegisterDate { get; set; }

        [Display(Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy")]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy")]
        public string LastModifiedBy { get; set; }

    }




	public class CompanyChangeViewModel
    {
        public IEnumerable<Company> inserted { get; set; }
        public IEnumerable<Company> deleted { get; set; }
        public IEnumerable<Company> updated { get; set; }
    }

}
