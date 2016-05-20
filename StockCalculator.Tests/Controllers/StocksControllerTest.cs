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
                      valuesData = new List<StockYearValue>()
                        {
                            new StockYearValue(),
                            new StockYearValue(),
                            new StockYearValue(),
                        }
                },
                // 0 years
                      valuesData = new List<StockYearValue>()
                        {
                            new StockYearValue(),
                        }
                },
                // 105 percentage
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
                },
                // -1 price
                },
                // -1 quantity
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
