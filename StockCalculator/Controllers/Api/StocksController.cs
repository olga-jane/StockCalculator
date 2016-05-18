using StockCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockCalculator.Controllers
{
    public class StocksController : ApiController
    {
        private Stock[] stocks = new Stock[]
            {
                new Stock { Id = 1, Name = "Apple", Percentage = 3.0M, Price = 2.0M, Quantity = 200, Years = 10 },
                new Stock { Id = 2, Name = "Apple2", Percentage = 4.0M, Price = 3.0M, Quantity = 300, Years = 12 },
            };

        public IEnumerable<Stock> Get()
        {
            return stocks;
        }

        public Stock Get(int id)
        {
            var stock = stocks.FirstOrDefault((p) => p.Id == id);
            if (stock == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return stock;
        }
    }
}
