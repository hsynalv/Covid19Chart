using Covid19Chart.API.Services;
using Microsoft.AspNetCore.SignalR;

namespace Covid19Chart.API.Hubs
{
    public class CovidHub : Hub
    {
        private readonly CovidService _service;

        public CovidHub(CovidService service)
        {
            _service = service;
        }

        public async Task GetCovidList()
        {
            await Clients
                .All
                .SendAsync("ReceiveCovidList", _service.GetCovidChartList());
        }
    }
}
