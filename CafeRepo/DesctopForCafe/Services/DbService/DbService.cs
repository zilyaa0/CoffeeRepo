﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace DesctopForCafe.Services.DbService
{
    public class DbService
    {
        public DbService() { }

        public bool CanLogin(string login, string password) 
        {
            var db = cafeEntities.GetContext();
            foreach (var person in db.Employee)
            {
                if (person.Email == login)
                    return true;
            }
            return false;
        }

        public List<OrdersData> GetOrders()
        {
            var list = new List<OrdersData>();
            var db = cafeEntities.GetContext();
            foreach (var order in db.Orders)
            {
                if ((bool)order.Status)
                {
                    list.Add(new OrdersData()
                    {
                        Id = order.Id,
                        CreatedAt = (DateTime)order.CreatedAt,
                        Customer = GetCustomerInfo((int)order.CustomerId),
                        TotalPrice = GetTotalPrice(order.Id),
                        Items = GetItems(order.Id)
                    });
                }
            }
            return list;
        }

        public List<CustomersData> GetCustomers()
        {
            var list = new List<CustomersData>();
            var db = cafeEntities.GetContext();
            foreach (var customer in db.Customers)
            {
                list.Add(new CustomersData()
                {
                    Id = customer.Id,
                    Firstname = customer.FirstName,
                    Lastname = customer.LastName,
                    Email = customer.Email,
                    Phone = customer.Phone
                });
            }
            return list;
        }

        public List<ProductsData> GetProducts()
        {
            var list = new List<ProductsData>();
            var db = cafeEntities.GetContext();
            foreach (var product in db.Menu)
            {
                list.Add(new ProductsData()
                {
                    Id = product.Id,
                    Name = product.ProductName,
                    Price = product.Price
                });
            }
            return list;
        }

        public void SaveProduct(ProductsData newproduct)
        {
            var db = cafeEntities.GetContext();
            foreach (var product in db.Menu)
            {
                if (product.Id == newproduct.Id)
                {
                    product.ProductName = newproduct.Name;
                    product.Price = newproduct.Price;
                }
            }
            db.SaveChanges();
        }

        public void CompleteOrder(int id)
        {
            var db = cafeEntities.GetContext();
            db.Orders.Where(c => c.Id == id).FirstOrDefault().Status = false;
            db.SaveChanges();
        }

        private string GetCustomerInfo(int customerId)
        {
            var db = cafeEntities.GetContext();
            var person = db.Customers.Where(c => c.Id == customerId).FirstOrDefault();
            return person.FirstName + " " + person.LastName;
        }

        private int GetTotalPrice(int orderId)
        {
            int price = 0;
            var db = cafeEntities.GetContext();
            var products = db.Product_Order.Where(c => c.OrderId == orderId);
            foreach (var product in products)
            {
                price += product.Menu.Price;
            }
            return price;
        }
        private List<ProductsData> GetItems(int orderId)
        {
            var list = new List<ProductsData>();
            var db = cafeEntities.GetContext();
            var products = db.Product_Order.Where(c => c.OrderId == orderId);
            foreach (var product in products)
            {
                var p = db.Menu.Where(c => c.Id == (int)product.ProductId).FirstOrDefault();
                list.Add(new ProductsData()
                {
                    Id = p.Id,
                    Name = p.ProductName,
                    Price = p.Price
                });
            }
            return list;
        }
    }
}
