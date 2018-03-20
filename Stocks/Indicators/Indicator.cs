using Stocks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Indicators
{
    public class Indicator : IIndicator
    {
        Dictionary<EParameter, double> parameters = new Dictionary<EParameter, double>();
        Func<IEnumerable<Sample>, IEnumerable<Sample>, IEnumerable<Sample>, Dictionary<EParameter, double>, IEnumerable<Sample>> behaviour = null;
        string name = String.Empty;

        public Indicator(string paramName, Func<IEnumerable<Sample>, IEnumerable<Sample>, IEnumerable<Sample>, Dictionary<EParameter, double>, IEnumerable<Sample>> paramBehaviour, IEnumerable<Tuple<EParameter, double>> paramParameters)
        {
            foreach (var tuple in paramParameters)
                parameters[tuple.Item1] = tuple.Item2;

            behaviour = paramBehaviour;
            name = paramName;
        }

        public string GetName()
        {
            return name;
        }

        public IEnumerable<Sample> GetValue(IEnumerable<Sample> paramMidPoints, IEnumerable<Sample> paramHighPoints, IEnumerable<Sample> paramLowPoints)
        {
            return behaviour(paramMidPoints, paramHighPoints, paramLowPoints, parameters);
        }

        public int GetExtraDimensionCount()
        {
            return parameters.Count();
        }

        public void SetExtraDimension(int paramDimension, double paramDimensionValue)
        {
            parameters[parameters.Keys.ElementAt(paramDimension)] = paramDimensionValue;
        }

        public string GetDimensionName(int paramDimension)
        {
            return parameters.Keys.ElementAt(paramDimension).ToString();
        }
    }
}
