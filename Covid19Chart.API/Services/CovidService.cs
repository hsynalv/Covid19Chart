using Covid19Chart.API.Hubs;
using Covid19Chart.API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Covid19Chart.API.Services
{
    public class CovidService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<CovidHub> _hubContext;

        public CovidService(AppDbContext context, IHubContext<CovidHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IQueryable<Covid> GetCovidList()
        {
            return _context.Covids.AsQueryable();
        }

        public async Task SaveCovid(Covid covid)
        {
            await _context.Covids.AddAsync(covid);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList", GetCovidChartList());
        }

        public List<CovidChart> GetCovidChartList()
        {
            var covidCharts = new List<CovidChart>();

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = @"select tarih, [1],[2],[3],[4],[5] from
                    (select [City],[Count],CAST([CovidDate] as date) as tarih from Covids) as CovidT
                    PIVOT
                    (SUM(Count) for City IN ([1],[2],[3],[4],[5])) as PTable
                    order by tarih asc";

                cmd.CommandType = System.Data.CommandType.Text;
                _context.Database.OpenConnection();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cc = new CovidChart();
                        cc.CovidDate = reader.GetDateTime(0).ToShortDateString();
                        Enumerable.Range(1,5).ToList().ForEach(x =>
                        {
                            if (System.DBNull.Value.Equals(reader[x]))
                            {
                                cc.Counts.Add(0);
                            }
                            else
                            {
                                cc.Counts.Add(reader.GetInt32(x));
                            }
                        });

                        covidCharts.Add(cc);
                    }
                }

                _context.Database.CloseConnection();
                return covidCharts;
            }
        }
    }
}
