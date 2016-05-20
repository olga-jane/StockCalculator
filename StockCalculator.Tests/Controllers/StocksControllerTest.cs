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
        [ExpectedException(typeof(System.Web.Http.HttpResponseException))]
        public void GetStockNullTest()
        {
            var repository = new Mock<IStockRepository>();
            var calculator = new Mock<IFinanceCalculator>();
            var stocksController = new StocksController(repository.Object, calculator.Object);

            repository.Setup(r => r.GetStock(It.IsAny<Guid>())).Returns((Stock)null);

            stocksController.Get(new Guid("1d35cabb-b21e-4813-8026-ec301c118730"));
        }

        public void GetStockTest()
        {
            var repository = new Mock<IStockRepository>();
            var calculator = new Mock<IFinanceCalculator>();
            var stocksController = new StocksController(repository.Object, calculator.Object);

            repository.Setup(r => r.GetStock(new Guid("1d35cabb-b21e-4813-8026-ec301c118730"))).Returns(
                new Stock { Id = new Guid("1d35cabb-b21e-4813-8026-ec301c118730"), Name = "Q", Percentage = 3.5M, Price = 10.3M, Quantity = 2, Years = 1,
                    Values = new List<StockYearValue> {
                        new StockYearValue { Id = 0, Year = 0, Value = 10.3M },
                        new StockYearValue { Id = 1, Year = 1, Value = 20M },
                    }
                });

            var stock = stocksController.Get(new Guid("1d35cabb-b21e-4813-8026-ec301c118730"));

            Assert.IsNotNull(stock);
            Assert.IsNotNull(stock.Name);
            Assert.AreEqual("Q", stock.Name);
            Assert.AreEqual(3.5M, stock.Percentage);
            Assert.AreEqual(10.3M, stock.Price);
            Assert.AreEqual(2, stock.Quantity);
            Assert.AreEqual(1, stock.Years);
            Assert.IsNotNull(stock.Values);
            Assert.AreEqual(2, stock.Values.Count);
            Assert.AreEqual(0, stock.Values[0].Id);
            Assert.AreEqual(1, stock.Values[1].Id);
            Assert.AreEqual(0, stock.Values[0].Year);
            Assert.AreEqual(1, stock.Values[1].Year);
            Assert.AreEqual(10.3M, stock.Values[0].Value);
            Assert.AreEqual(20M, stock.Values[1].Value);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Web.Http.HttpResponseException))]
        public void GetStocksNullTest()
        {
            var repository = new Mock<IStockRepository>();
            var calculator = new Mock<IFinanceCalculator>();
            var stocksController = new StocksController(repository.Object, calculator.Object);

            repository.Setup(r => r.GetStocks(It.IsAny<bool>())).Returns((IEnumerable<Stock>)null);

            stocksController.Get();
        }

        [TestMethod]
        public void GetStocksEmptyTest()
        {
            var repository = new Mock<IStockRepository>();
            var calculator = new Mock<IFinanceCalculator>();
            var stocksController = new StocksController(repository.Object, calculator.Object);

            repository.Setup(r => r.GetStocks(It.IsAny<bool>())).Returns(new List<Stock>());

            Assert.AreEqual(0, stocksController.Get().Count());
        }

        [TestMethod]
        public void GetStocksNoValuesTest()
        {
            var repository = new Mock<IStockRepository>();
            var calculator = new Mock<IFinanceCalculator>();
            var stocksController = new StocksController(repository.Object, calculator.Object);

            repository.Setup(r => r.GetStocks(false)).Returns(
                new List<Stock>() {
                    new Stock { Id = new Guid("1d35cabb-b21e-4813-8026-ec301c118730"), Name = "N", Percentage = 0.1M, Price = 4M, Quantity = 5, Years = 4 },
                    new Stock { Id = new Guid("2d35cabb-b21e-4813-8026-ec301c118730"), Name = "D", Percentage = 0.2M, Price = 3M, Quantity = 6, Years = 1 },
                });

            var stocks = stocksController.Get().ToList();

            Assert.AreEqual(2, stocks.Count());
            Assert.IsNull(stocks[0].Values);
            Assert.IsNull(stocks[1].Values);
            Assert.AreEqual("1d35cabb-b21e-4813-8026-ec301c118730", stocks[0].Id.ToString());
            Assert.AreEqual("2d35cabb-b21e-4813-8026-ec301c118730", stocks[1].Id.ToString());
            Assert.IsNotNull(stocks[0].Name);
            Assert.IsNotNull(stocks[1].Name);
            Assert.AreEqual("N", stocks[0].Name);
            Assert.AreEqual("D", stocks[1].Name);
            Assert.AreEqual(0.1M, stocks[0].Percentage);
            Assert.AreEqual(0.2M, stocks[1].Percentage);
            Assert.AreEqual(4M, stocks[0].Price);
            Assert.AreEqual(3M, stocks[1].Price);
            Assert.AreEqual(5, stocks[0].Quantity);
            Assert.AreEqual(6, stocks[1].Quantity);
            Assert.AreEqual(4, stocks[0].Years);
            Assert.AreEqual(1, stocks[1].Years);
        }

        [TestMethod]
        public void CalculateAndSaveSuccessTest()
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
        public void CalculateAndSaveFailureTest()
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
