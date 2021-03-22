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
        Func<IEnumerable<SingleValueSample<double>>, IEnumerable<SingleValueSample<double>>, IEnumerable<SingleValueSample<double>>, Dictionary<EParameter, double>, IEnumerable<SingleValueSample<double>>> behaviour = null;
        string name = String.Empty;

        public Indicator(string paramName, Func<IEnumerable<SingleValueSample<double>>, IEnumerable<SingleValueSample<double>>, IEnumerable<SingleValueSample<double>>, Dictionary<EParameter, double>, IEnumerable<SingleValueSample<double>>> paramBehaviour, IEnumerable<Tuple<EParameter, double>> paramParameters)
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

        public IEnumerable<SingleValueSample<double>> GetValue(IEnumerable<SingleValueSample<double>> paramMidPoints, IEnumerable<SingleValueSample<double>> paramHighPoints, IEnumerable<SingleValueSample<double>> paramLowPoints)
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
