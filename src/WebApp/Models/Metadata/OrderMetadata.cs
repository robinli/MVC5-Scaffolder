// <copyright file="OrderMetadata.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2018/11/16 8:20:01 </date>
// <summary>Class representing a Metadata entity </summary>

using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    //[MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }

    public partial class OrderMetadata
    {
        [Display(Name = "OrderDetails")]
        public OrderDetail OrderDetails { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "OrderNo")]
        public string OrderNo { get; set; }

        [Display(Name = "Customer")]
        public string Customer { get; set; }

        [Display(Name = "ShippingAddress")]
        public string ShippingAddress { get; set; }

        [Display(Name = "Remark")]
        public string Remark { get; set; }

        [Display(Name = "OrderDate")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy")]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy")]
        public string LastModifiedBy { get; set; }

    }




	public class OrderChangeViewModel
    {
        public IEnumerable<Order> inserted { get; set; }
        public IEnumerable<Order> deleted { get; set; }
        public IEnumerable<Order> updated { get; set; }
    }

}
