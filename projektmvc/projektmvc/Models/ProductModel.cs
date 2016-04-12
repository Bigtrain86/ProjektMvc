using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projektmvc.Models
{
    public class ProductModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public string Model { get; private set; }
        public int Price { get; private set; }

        public int NumberInStock { get; private set; }

        public string ImageLink { get; private set; }

        //TEST!!
        public static void EditProduct(ProductModel mod, string name, string model, int price, int stock, string link)
        {
            mod.Name = name;
            mod.Model = model;
            mod.Price = price;
            mod.NumberInStock = stock;
            mod.ImageLink = link;
        }

        public ProductModel(int id, string name, string model, int price, int numInStock, string imgLink)
        {
            Id = id;
            Name = name;
            Model = model;
            Price = price;
            NumberInStock = numInStock;
            ImageLink = imgLink;
        }

        public ProductModel(int id, string name, string model, int price, int numInStock)
        {
            Id = id;
            Name = name;
            Model = model;
            Price = price;
            NumberInStock = numInStock;
            ImageLink = "http://png.clipart.me/graphics/thumbs/190/drift-car-symbol_190755995.jpg";
        }
    }


}