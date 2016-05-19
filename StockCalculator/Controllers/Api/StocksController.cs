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

        public StocksController() : this(new StockFileRepository("C:/TmpData"), new FinanceCalculator())
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
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = e.Message });
            }
            return result;
        }

        [HttpGet]
        public Stock Get(int id)
        {
            Stock stock = null;
            try
            {
                stock = repository.GetStock(id);
            }
            catch (Exception e)
            {
                // TODO: log critical error
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = e.Message });
            }
            if (stock == null)
            {
                // TODO: log critical error
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return stock;
        }

        [HttpPost]
        public bool Save(Stock stock)
        {
            bool success = false;
            try
            {
                success = repository.SaveStock(stock);
            }
            catch (Exception e)
            {
                // TODO: log critical error
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = e.Message });
            }
            return success;
        }

        [HttpPost]
        public Stock Calculate(Stock stock)
        {
            try
            {
                stock.Values = calculator.Calculate(stock).ToList();
            }
            catch(CalculationException e)
            {
                // TODO: log critical error
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = e.Message });
            }
            catch (Exception e)
            {
                // TODO: log critical error
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = e.Message });
            }
            return stock;
        }
    }
}
