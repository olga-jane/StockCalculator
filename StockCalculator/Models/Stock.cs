using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockCalculator.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Percentage { get; set; }
        public int Years { get; set; }
    }
}