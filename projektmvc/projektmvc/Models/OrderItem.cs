using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projektmvc.Models
{
    public class OrderItem
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public int Price { get; private set; }

        public int Numbers { get; set; }

        public OrderItem(int id, string name, int price, int number)
        {
            Id = id;
            Name = name;
            Price = price;
            Numbers = number;
        }
    }
}