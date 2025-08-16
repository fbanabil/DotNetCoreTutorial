using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.Models;
using StockApp.ServiceContracts;
using StockApp.Services;

namespace StockApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly IFinnhubService _finhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;

        public HomeController(IFinnhubService finnhubService,IOptions<TradingOptions> tradingOptions)
        {
            _finhubService = finnhubService;
            _tradingOptions = tradingOptions;
        }




        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if(_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }
            Dictionary<string, object>? stockQuote = 
                    await _finhubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);


            Stock stock = new Stock()
            {
                StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
                CurrentPrice = stockQuote.ContainsKey("c") ? Convert.ToDouble(stockQuote["c"].ToString()) : null,
                LowestPrice = stockQuote.ContainsKey("l") ? Convert.ToDouble(stockQuote["l"].ToString()) : null,
                HighestPrice = stockQuote.ContainsKey("h") ? Convert.ToDouble(stockQuote["h"].ToString()) : null,
                OpenPrice = stockQuote.ContainsKey("o") ? Convert.ToDouble(stockQuote["o"].ToString()) : null
            };

            return View(stock);
        }
    }
}
