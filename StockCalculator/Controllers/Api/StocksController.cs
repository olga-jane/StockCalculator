using StockCalculator.Models;
using StockCalculator.Models.Services;
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
        private readonly IFinanceCalculator calculator;

        private readonly string errorMessage = "An error happened. Please see error log";

        public StocksController() : this(new StockFileRepository(System.IO.Path.GetTempPath()), new FinanceCalculator())
        {
        }

        public StocksController(IStockRepository repository, IFinanceCalculator calculator)
        {
            this.repository = repository;
            this.calculator = calculator;
        }

        [HttpGet]
        public IEnumerable<Stock> Get()
        {
            IEnumerable<Stock> result = null;
            try
            {
                result = repository.GetStocks();
                if (result == null)
                {
                    throw new Exception("Null result");
                }
            }
            catch (Exception e)
            {
                // TODO: log critical error
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = errorMessage });
            }
            return result;
        }

        [HttpGet]
        public Stock Get(Guid id)
        {
            Stock stock = null;
            try
            {
                stock = repository.GetStock(id);
            }
            catch (Exception e)
            {
                // TODO: log critical error
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = errorMessage });
            }
            if (stock == null)
            {
                // TODO: log critical error
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return stock;
        }

        [HttpPost]
        public Stock CalculateAndSave(Stock stock)
        {
            try
            {
                stock.Values = calculator.Calculate(stock).ToList();
                if (stock.Id == Guid.Empty)
                {
                    stock.Id = Guid.NewGuid();
                }
                repository.SaveStock(stock);
            }
            catch (CalculationException e)
            {
                // TODO: log critical error
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = errorMessage });
            }
            catch (Exception e)
            {
                // TODO: log critical error
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = errorMessage });
            }
            return stock;
        }
    }
}
