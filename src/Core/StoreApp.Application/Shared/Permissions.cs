using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StoreApp.Application.Shared
{
   public static class Permissions
    {
        public static class Category
        {
            public const string Create = "Category.Create";
            public const string Update = "Category.Update";
            public const string Delete = "Category.Delete";

            public static List<string> All = new()
           {
              Create,
              Update,
              Delete
           };
        }
        public static class Product
        {
            public const string Create = "Product.Create";
            public const string Update = "Product.Update";
            public const string Delete = "Product.Delete";
            public const string GetMyProducts = "Product.GetMyProducts";
            public const string DeleteImage = "Product.DeleteImage";
            public const string AddImage = "Product.AddImage";

            public static List<string> All = new()
           {
              Create,
              Update,
              Delete,
              GetMyProducts,
              AddImage,
              DeleteImage

           };
        }
        public static class Order
        {
            public const string Create = "Order.Create";
            public const string Update = "Order.Update";
            public const string Delete = "Order.Delete";
            public const string GetAll = "Order.GetAll";
            public const string GetMyOrders = "Order.GetMyOrders";
            public const string GetMySales = "Order.GetMySales";
            public const string GetDetail = "Order.GetDetail";
            public static List<string> All = new()
           {
              Create,
              Update,
              Delete,
              GetAll,
              GetMyOrders,
              GetMySales,
              GetDetail
           };
        }
        public static class Account
        {
            public const string AddRole = "Account.AddRole";
            public const string Create = "Account.Create";

            public static List<string> All = new()
           {
              Create,
              AddRole
           };
        } 
        public static class Role
        {
            public const string Create = "Role.Create";
            public const string Update = "Role.Update";
            public const string Delete = "Role.Delete";
            public const string GetAllPermissions = "Role.GetAllPermissions";
            public const string GetAllRoles = "Role.GetAllRoles";
            public static List<string> All = new()
            {
                Create,
                Update,
                Delete,
                GetAllPermissions,
                GetAllRoles
            };
        }
        public static class User
        {
            public const string Create = "User.Create";
            public const string ResetPassword = "User.ResetPassword";
            public const string GetAll = "User.GetAll";
            public const string GetById = "User.GetById";
            public static List<string> All = new()
            {
                Create,
                ResetPassword,
                GetAll,
                GetById
            };
        }
        public static class Review
        {
            public const string Create = "Review.Create";
            
            public const string Delete = "Review.Delete";
            public static List<string> All = new()
            {
                Create,             
                Delete
            };
        }


    }
}
