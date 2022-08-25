namespace Covid19Chart.API.Models
{
    public class CovidChart
    {
        public CovidChart()
        {
            Counts = new();
        }

        public string CovidDate { get; set; }
        public List<int> Counts { get; set; }


    }
}
