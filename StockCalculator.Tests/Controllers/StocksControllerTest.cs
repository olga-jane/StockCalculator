using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StockCalculator.Models;
using StockCalculator.Controllers;
using StockCalculator.Models.Services;

namespace StockCalculator.Tests.Controllers
{
    [TestClass]
    public class StocksControllerTest
    {
        [TestMethod]
        public void SaveTest()
        {
            var repository = new Mock<IStockRepository>();
            var calculator = new Mock<IFinanceCalculator>();

            var stocks = new List<Stock>()
            {
                new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118737"), Name = "Name1", Percentage = 2 }
            };

            var stocksController = new StocksController(repository.Object, calculator.Object);
        }

        [TestMethod]
        public void GetStockTest()
        {

        }

        [TestMethod]
        public void GetStocksTest()
        {

        }

        [TestMethod]
        public void CalculateSuccessTest()
        {
            // this approach is instead of TestCase which is not available in MS Test 
            var values = new[] {
                // 2 years
                new { stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118730"), Name = "Apple", Percentage = 3, Price = 2, Quantity = 200, Years = 2 },
                      valuesData = new List<StockYearValue>()
                        {
                            new StockYearValue(),
                            new StockYearValue(),
                            new StockYearValue(),
                        }
                },
                // 0 years
                new { stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118731"), Name = "Apple", Percentage = 3, Price = 2, Quantity = 200, Years = 0 },
                      valuesData = new List<StockYearValue>()
                        {
                            new StockYearValue(),
                        }
                },
                // 105 percentage
                new { stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118732"), Name = "Apple", Percentage = 105, Price = 2, Quantity = 200, Years = 5 },
                      valuesData = new List<StockYearValue>()
                        {
                            new StockYearValue(),
                            new StockYearValue(),
                            new StockYearValue(),
                            new StockYearValue(),
                            new StockYearValue(),
                            new StockYearValue(),
                        }
                },
            };

            values.ToList().ForEach(data => {
                var repository = new Mock<IStockRepository>();
                var calculator = new Mock<IFinanceCalculator>();
                var stocksController = new StocksController(repository.Object, calculator.Object);

                calculator.Setup(s => s.Calculate(data.stockData)).Returns(data.valuesData);

                Assert.IsNotNull(stocksController.CalculateAndSave(data.stockData));
                Assert.IsNotNull(data.stockData.Values);
                Assert.AreEqual(data.valuesData.Count, data.stockData.Values.Count);
            });
        }

        [TestMethod]
        public void CalculateFailureTest()
        {
            // this approach is instead of TestCase which is not available in MS Test 
            var values = new[] {
                // -1 years
                new { stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118733"), Name = "Apple", Percentage = 3, Price = 2, Quantity = 200, Years = -1, Values = new List<StockYearValue>() }
                },
                // -1 price
                new { stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118734"), Name = "Apple", Percentage = 3, Price = -1, Quantity = 200, Years = 2, Values = new List<StockYearValue>() }
                },
                // -1 quantity
                new { stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118735"), Name = "Apple", Percentage = 3, Price = 3, Quantity = -1, Years = 2, Values = new List<StockYearValue>() }
                },
            };

            values.ToList().ForEach(data => {
                var repository = new Mock<IStockRepository>();
                var calculator = new Mock<IFinanceCalculator>();
                var stocksController = new StocksController(repository.Object, calculator.Object);

                calculator.Setup(s => s.Calculate(data.stockData)).Returns((IEnumerable<StockYearValue>)null);

                var oldValuesData = data.stockData.Values;

                try
                {
                    stocksController.CalculateAndSave(data.stockData);
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    // OK
                }
                Assert.IsNotNull(data.stockData.Values);
                Assert.AreEqual(oldValuesData, data.stockData.Values);
            });

        }

    }
}
