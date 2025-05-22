using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Repositories;
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
        private readonly ITranskacjaRepository _transkacjaRepository;
        public IEnumerable<ISeries> SeriesLine { get; set; }
        public Axis[] XAxis { get; set; }

        public IEnumerable<ISeries> SeriesColumn { get; set; }
        public IEnumerable<ISeries> SeriesRow { get; set; }
        public IEnumerable<ISeries> SeriesPie { get; set; }

        public WykresViewModel()
        {
            var context = new DbContextDomKultury();
            _transkacjaRepository = new TransakcjaRepository(context);

            SeriesColumn = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Values = new int[] {1, 2, 8, 19, 4}
                }
            };

            SeriesPie = _transkacjaRepository.GetAllTransakcjeGroupTyp().Select(x => new PieSeries<int>
            {
                Name = x.Key,
                Values = new int[] { x.Value },
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                
            }).ToArray();
        

        }
    }
}
