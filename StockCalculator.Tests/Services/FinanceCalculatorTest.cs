using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCalculator.Models;
using StockCalculator.Models.Services;

namespace StockCalculator.Tests.Services
{
    [TestClass]
    public class FinanceCalculatorTest
    {
        IFinanceCalculator calculator;

        [TestInitialize]
        public void Initialize()
        {
            calculator = new FinanceCalculator();
        }

        [TestCleanup]
        public void Cleanup()
        {
            calculator = null;
        }

        /// <summary>
        /// check stock data for abcense of changes
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        private void EqualStockTest(Stock s1, Stock s2)
        {
            Assert.AreEqual(s1.Id, s2.Id);
            Assert.IsNotNull(s1.Name);
            Assert.IsNotNull(s2.Name);
            Assert.AreEqual(s1.Name, s2.Name, false);
            Assert.AreEqual(s1.Percentage, s2.Percentage);
            Assert.AreEqual(s1.Price, s2.Price);
            Assert.AreEqual(s1.Quantity, s2.Quantity);
            Assert.AreEqual(s1.Years, s2.Years);
            Assert.AreEqual(s1.Values, s2.Values);
        }

        [TestMethod]
        public void CalculateSuccessTest()
        {
            // this approach is instead of TestCase which is not available in MS Test 
            // modelStockData and stockData should contain equivalent values
            var values = new[] {
                // 2 years
                new {
                      modelStockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118740"), Name = "Apple", Percentage = 3, Price = 2, Quantity = 200, Years = 2 },
                      stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118740"), Name = "Apple", Percentage = 3, Price = 2, Quantity = 200, Years = 2 },
                      valuesData = new List<StockYearValue>()
                        {
                            new StockYearValue { Id = 0, Year = 0, Value = 400.00M },
                            new StockYearValue { Id = 1, Year = 1, Value = 412.00M },
                            new StockYearValue { Id = 2, Year = 2, Value = 424.36M },
                        }
                },
                // 0 years
                new {
                      modelStockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118741"), Name = "Apple", Percentage = 3, Price = 2, Quantity = 200, Years = 0 },
                      stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118741"), Name = "Apple", Percentage = 3, Price = 2, Quantity = 200, Years = 0 },
                      valuesData = new List<StockYearValue>()
                        {
                            new StockYearValue { Id = 0, Year = 0, Value = 400.00M },
                        }
                },
                // 105 percentage
                new {
                      modelStockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118742"), Name = "Apple", Percentage = 105, Price = 2, Quantity = 200, Years = 5 },
                      stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118742"), Name = "Apple", Percentage = 105, Price = 2, Quantity = 200, Years = 5 },
                      valuesData = new List<StockYearValue>()
                        {
                            new StockYearValue { Id = 0, Year = 0, Value = 400.00M },
                            new StockYearValue { Id = 1, Year = 1, Value = 820.00M },
                            new StockYearValue { Id = 2, Year = 2, Value = 1681.00M },
                            new StockYearValue { Id = 3, Year = 3, Value = 3446.05M },
                            new StockYearValue { Id = 4, Year = 4, Value = 7064.40M },
                            new StockYearValue { Id = 5, Year = 5, Value = 14482.02M },
                        }
                },
            };

            values.ToList().ForEach(data =>
            {
                var result = calculator.Calculate(data.stockData);
                Assert.IsNotNull(result);
                Assert.AreEqual(data.valuesData.Count, result.Count());

                EqualStockTest(data.modelStockData, data.stockData);

                // check result data
                var listResult = result.ToList();
                for (int index = 0; index < listResult.Count; index++)
                {
                    Assert.AreEqual(data.valuesData[index].Id, listResult[index].Id);
                    Assert.AreEqual(data.valuesData[index].Year, listResult[index].Year);
                    Assert.AreEqual(data.valuesData[index].Value, listResult[index].Value);
                }

            });
        }

        [TestMethod]
        public void CalculateNullResultTest()
        {
            // this approach is instead of TestCase which is not available in MS Test 
            // modelStockData and stockData should contain equivalent values
            var values = new[] {
                // -1 years
                new {
                      modelStockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118743"), Name = "Apple", Percentage = 3, Price = 2, Quantity = 200, Years = -1 },
                      stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118743"), Name = "Apple", Percentage = 3, Price = 2, Quantity = 200, Years = -1 },
                },
                // -1 price
                new {
                      modelStockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118744"), Name = "Apple", Percentage = 3, Price = -1, Quantity = 200, Years = 0 },
                      stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118744"), Name = "Apple", Percentage = 3, Price = -1, Quantity = 200, Years = 0 },
                },
                // -1 quantity
                new {
                      modelStockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118745"), Name = "Apple", Percentage = 105, Price = 2, Quantity = -1, Years = 5 },
                      stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118745"), Name = "Apple", Percentage = 105, Price = 2, Quantity = -1, Years = 5 },
                },
                // -1 percentage
                new {
                      modelStockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118746"), Name = "Apple", Percentage = -1, Price = 2, Quantity = 200, Years = 5 },
                      stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118746"), Name = "Apple", Percentage = -1, Price = 2, Quantity = 200, Years = 5 },
                },
            };

            values.ToList().ForEach(data =>
            {
                var result = calculator.Calculate(data.stockData);
                Assert.IsNull(result);

                EqualStockTest(data.modelStockData, data.stockData);

            });
        }

        [TestMethod]
        [ExpectedException(typeof(CalculationException))]
        public void CalculateExceptionPriceTest()
        {
            // max value price
            var stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118747"), Name = "Apple", Percentage = 3, Price = decimal.MaxValue, Quantity = 200, Years = 5 };

            calculator.Calculate(stockData);
        }

        [TestMethod]
        [ExpectedException(typeof(CalculationException))]
        public void CalculateExceptionYearsTest()
        {
            // max value price
            var stockData = new Stock { Id = new Guid("ad35cabb-b21e-4813-8026-ec301c118748"), Name = "Apple", Percentage = 3, Price = 3, Quantity = 200, Years = int.MaxValue };

            calculator.Calculate(stockData);
        }
    }
}
