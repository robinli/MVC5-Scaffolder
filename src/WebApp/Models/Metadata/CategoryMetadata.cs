// <copyright file="CategoryMetadata.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2019 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2019/1/2 15:53:10 </date>
// <summary>Class representing a Metadata entity </summary>

using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    //[MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
    }

    public partial class CategoryMetadata
    {
        [Display(Name = "Products")]
        public Product Products { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy")]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy")]
        public string LastModifiedBy { get; set; }

    }




	public class CategoryChangeViewModel
    {
        public IEnumerable<Category> inserted { get; set; }
        public IEnumerable<Category> deleted { get; set; }
        public IEnumerable<Category> updated { get; set; }
    }

}
