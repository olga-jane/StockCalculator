using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCalculator.Models
{
    public interface IStockRepository
    {
        /// <summary>
        /// Get list of all available stocks
        /// </summary>
        /// <param name="loadValues">whether to load list of values inside each stock</param>
        /// <returns>list of available stocks</returns>
        IEnumerable<Stock> GetStocks(bool loadValues = false);

        /// <summary>
        /// Get stock by id
        /// </summary>
        /// <param name="id">stock internal id</param>
        /// <returns>It is assumed that single stock will always be returned with values</returns>
        Stock GetStock(Guid id);

        /// <summary>
        /// Save stock data with list of values
        /// </summary>
        /// <param name="stock">stock data with list of values</param>
        void SaveStock(Stock stock);
    }
}
