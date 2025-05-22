using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.ViewModel
{
    public class WykresViewModel
    {
        //przykładowe dane żeby pokazać mniej więcej jak to będzie wyglądać

        public IEnumerable<ISeries> SeriesLine { get; set; }
        public Axis[] XAxis { get; set; }

        public IEnumerable<ISeries> SeriesColumn { get; set; }
        public IEnumerable<ISeries> SeriesRow { get; set; }
        public IEnumerable<ISeries> SeriesPie { get; set; }

        public WykresViewModel()
        {
            SeriesLine = new ISeries[]
            {
                new LineSeries<int>
                {
                    Values = new int[] {1, 2, 8, 19, 4}
                }
            };
            XAxis = new Axis[]
            {
                new Axis
                {
                    Labels = new[] {"Pn", "Wt", "Śr", "Cz", "Pt", "Sb", "Ndz"},
                    Position = LiveChartsCore.Measure.AxisPosition.Start
                }
            };

            SeriesColumn = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Values = new int[] {1, 2, 8, 19, 4}
                }
            };

            SeriesRow = new ISeries[]
            {
                new RowSeries<int>
                {
                    Values = new int[] {1, 2, 8, 19, 4}
                }
            };

            SeriesPie = new ISeries[]
            {
                new PieSeries<int>
                {
                    Values = new int[] { 1 },
                    Name = "Wart1"
                },
                new PieSeries<int>
                {
                    Values = new int[] { 5,4 },
                    Name = "Wart2"
                },
                new PieSeries<int>
                {
                    Values = new int[] { 3 },
                    Name = "Wart3",
                    
                }
            };
        }
    }
}
