using Covid19Chart.API.Models;
using Covid19Chart.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Chart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController : ControllerBase
    {
        private readonly CovidService _covidService;

        public CovidsController(CovidService covidService)
        {
            _covidService = covidService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await _covidService.SaveCovid(covid);
            //var covidList =  _covidService.GetCovidList();
            return Ok(_covidService.GetCovidChartList());
        }

        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random rnd = new();
            
            Enumerable.Range(1,10).ToList().ForEach(x =>
            {
                foreach (ECity item in Enum.GetValues(typeof(ECity)))
                {
                    var newCovid = new Covid()
                        { City = item, Count = rnd.Next(100, 1000), CovidDate = DateTime.Now.AddDays(x) };
                    _covidService.SaveCovid(newCovid).Wait();
                    System.Threading.Thread.Sleep(1000);
                }
            });

            return Ok("Covid19 dataları veritabanına kaydedildi");
        }
    }
}
