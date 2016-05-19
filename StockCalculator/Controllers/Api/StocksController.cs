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
        private readonly IStockRepository repository;

        public StocksController() : this(new StockFileRepository("C:/TmpData"))
        {
        }

        public StocksController(IStockRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Stock> Get()
        {
            return repository.GetStocks();
        }

        public Stock Get(int id)
        {
            var stock = repository.GetStock(id);
            if (stock == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return stock;
        }

        [HttpPost]
        public bool Save(Stock stock)
        {
            return repository.SaveStock(stock);
        }
    }
}
