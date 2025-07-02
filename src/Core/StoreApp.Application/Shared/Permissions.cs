using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            public static List<string> All = new()
           {
              Create,
              Update,
              Delete
           };
        }
        public static class Order
        {
            public const string Create = "Order.Create";
            public const string Update = "Order.Update";
            public const string Delete = "Order.Delete";

            public static List<string> All = new()
           {
              Create,
              Update,
              Delete
           };
        }
        public static class Favorite
        {
            public const string Add = "Favorite.Add";
            
            public const string Delete = "Favorite.Delete";

            public static List<string> All = new()
           {
                Add,
                Delete

           };
        }

    }
}
