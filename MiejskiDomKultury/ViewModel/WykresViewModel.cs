using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Repositories;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.ViewModel
{
    public class WykresViewModel
    {
        private readonly ITranskacjaRepository _transkacjaRepository;
        private readonly IRezerwacjaRepository _rezerwacjaRepository;
        public Axis[] YAxis { get; set; }
        public Axis[] XAxis { get; set; }

        public IEnumerable<ISeries> SeriesColumn { get; set; }
        public IEnumerable<ISeries> SeriesPie { get; set; }

        public WykresViewModel()
        {
            var context = new DbContextDomKultury();
            _transkacjaRepository = new TransakcjaRepository(context);
            _rezerwacjaRepository = new RezerwacjeRepository(context);

            var listaSalZZajetoscia = _rezerwacjaRepository.GetHowManyTimesRoomBooked();

            SeriesColumn = new[]
            {
                new ColumnSeries<int>
                {
                    Values = listaSalZZajetoscia.Select(s => s.Value).ToArray(),
                }
            };

            YAxis = new[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MinStep = 1,
                }
            };
            XAxis = new[]
            {
                new Axis
                {
                    Labels = listaSalZZajetoscia.Select(x => x.Key).ToArray()
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
