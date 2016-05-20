using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;

namespace StockCalculator.Models
{
    public class StockFileRepository : IStockRepository
    {
        private readonly string dataDir;
        private readonly string fileNameTemplate;
        private readonly string fullFileNameTemplate;
        private readonly DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Stock));

        public StockFileRepository(string dataDirectory)
        {
            dataDir = dataDirectory + "/StockData";
            fileNameTemplate = "stock{0}.json";
            fullFileNameTemplate = dataDir + "/" + fileNameTemplate;
        }

        public Stock GetStock(Guid id)
        {
            Stock stock = null;

            // TODO: check directory/file name availability
            using (var reader = new StreamReader(string.Format(fullFileNameTemplate, id)))
            {
                stock = (Stock)serializer.ReadObject(reader.BaseStream);
            }

            return stock;
        }

        public IEnumerable<Stock> GetStocks(bool loadValues = false)
        {
            List<Stock> stocks = new List<Stock>();

            // TODO: check directory/file name availability
            foreach (var file in System.IO.Directory.GetFiles(dataDir, string.Format(fileNameTemplate, "*")))
            {
                using (var reader = new StreamReader(file))
                {
                    Stock stock = (Stock)serializer.ReadObject(reader.BaseStream);
                    if (!loadValues)
                    {
                        stock.Values = null;
                    }
                    stocks.Add(stock);
                }
            }
            return stocks;
        }

        public void SaveStock(Stock stock)
        {
            // TODO: check directory/file name availability
            using (var writer = new StreamWriter(string.Format(fullFileNameTemplate, stock.Id), false))
            {
                serializer.WriteObject(writer.BaseStream, stock);
            }
        }
    }
}