// <copyright file="ProductMetadata.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2018/12/20 10:19:50 </date>
// <summary>Class representing a Metadata entity </summary>

using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    //[MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
    }

    public partial class ProductMetadata
    {
        [Display(Name = "Category")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Unit")]
        public string Unit { get; set; }

        [Display(Name = "UnitPrice")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "StockQty")]
        public int StockQty { get; set; }

        [Display(Name = "IsRequiredQc")]
        public bool IsRequiredQc { get; set; }

        [Display(Name = "ConfirmDateTime")]
        public DateTime ConfirmDateTime { get; set; }

        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }

        [Display(Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy")]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy")]
        public string LastModifiedBy { get; set; }

    }




	public class ProductChangeViewModel
    {
        public IEnumerable<Product> inserted { get; set; }
        public IEnumerable<Product> deleted { get; set; }
        public IEnumerable<Product> updated { get; set; }
    }

}
