// <copyright file="OrderDetailService.cs" company="neozhu/SmartCode-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>9/29/2018 11:07:49 AM </date>
// <summary>
//  根据需求定义实现具体的业务逻辑,通过依赖注入降低模块之间的耦合度
//   
//  
//  
// </summary>
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;
namespace WebApp.Services
{
    public class OrderDetailService : Service<OrderDetail>, IOrderDetailService
    {
        private readonly IRepositoryAsync<OrderDetail> repository;
        private readonly IDataTableImportMappingService mappingservice;
        public OrderDetailService(IRepositoryAsync<OrderDetail> repository, IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            this.repository = repository;
            this.mappingservice = mappingservice;
        }
        public IEnumerable<OrderDetail> GetByProductId(int productid) => this.repository.GetByProductId(productid);
        public IEnumerable<OrderDetail> GetByOrderId(int orderid) => this.repository.GetByOrderId(orderid);



        private int getOrderIdByCustomer(string customer)
        {
            var orderRepository = this.repository.GetRepository<Order>();
            var order = orderRepository.Queryable().Where(x => x.Customer == customer).FirstOrDefault();
            if (order == null)
            {
                throw new Exception("not found ForeignKey:OrderId with " + customer);
            }
            else
            {
                return order.Id;
            }
        }
        private int getProductIdByName(string name)
        {
            var productRepository = this.repository.GetRepository<Product>();
            var product = productRepository.Queryable().Where(x => x.Name == name).FirstOrDefault();
            if (product == null)
            {
                throw new Exception("not found ForeignKey:ProductId with " + name);
            }
            else
            {
                return product.Id;
            }
        }

        public void ImportDataTable(System.Data.DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                var item = new OrderDetail();
                var mapping = this.mappingservice.Queryable().Where(x => x.EntitySetName == "OrderDetail" && ( ( x.IsEnabled == true ) || ( x.IsEnabled == false && !( x.DefaultValue == null || x.DefaultValue.Equals(string.Empty) ) ) )).ToList();
                var requiredfield = mapping.Where(x => x.IsRequired == true).FirstOrDefault()?.SourceFieldName;
                if (requiredfield != null && !row.IsNull(requiredfield) && row[requiredfield] != DBNull.Value && Convert.ToString(row[requiredfield]).Trim() != string.Empty)
                {
                    foreach (var field in mapping)
                    {
                        var defval = field.DefaultValue;
                        var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
                        if (contain && !row.IsNull(field.SourceFieldName) && row[field.SourceFieldName] != DBNull.Value && row[field.SourceFieldName].ToString() != string.Empty)
                        {
                            var orderdetailtype = item.GetType();
                            var propertyInfo = orderdetailtype.GetProperty(field.FieldName);
                            //关联外键查询获取Id
                            switch (field.FieldName)
                            {
                                case "OrderId":
                                    var customer = row[field.SourceFieldName].ToString();
                                    var orderid = this.getOrderIdByCustomer(customer);
                                    propertyInfo.SetValue(item, Convert.ChangeType(orderid, propertyInfo.PropertyType), null);
                                    break;
                                case "ProductId":
                                    var name = row[field.SourceFieldName].ToString();
                                    var productid = this.getProductIdByName(name);
                                    propertyInfo.SetValue(item, Convert.ChangeType(productid, propertyInfo.PropertyType), null);
                                    break;
                                default:
                                    var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                                    var safeValue = ( row[field.SourceFieldName] == null ) ? null : Convert.ChangeType(row[field.SourceFieldName], safetype);
                                    propertyInfo.SetValue(item, safeValue, null);
                                    break;
                            }
                        }
                        else if (!string.IsNullOrEmpty(defval))
                        {
                            var orderdetailtype = item.GetType();
                            var propertyInfo = orderdetailtype.GetProperty(field.FieldName);
                            if (defval.ToLower() == "now" && propertyInfo.PropertyType == typeof(DateTime))
                            {
                                propertyInfo.SetValue(item, Convert.ChangeType(DateTime.Now, propertyInfo.PropertyType), null);
                            }
                            else
                            {
                                var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                                var safeValue = Convert.ChangeType(defval, safetype);
                                propertyInfo.SetValue(item, safeValue, null);
                            }
                        }
                    }
                    this.Insert(item);
                }

            }
        }

        public Stream ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);

            var orderdetails = this.Query(new OrderDetailQuery().Withfilter(filters)).Include(p => p.Order).Include(p => p.Product).OrderBy(n => n.OrderBy(sort, order)).Select().ToList();

            var datarows = orderdetails.Select(n => new
            {

                OrderCustomer = ( n.Order == null ? "" : n.Order.Customer ),
                ProductName = ( n.Product == null ? "" : n.Product.Name ),
                Id = n.Id,
                ProductId = n.ProductId,
                Qty = n.Qty,
                Price = n.Price,
                Amount = n.Amount,
                OrderId = n.OrderId
            }).ToList();

            return ExcelHelper.ExportExcel(typeof(OrderDetail), datarows);

        }
    }
}



