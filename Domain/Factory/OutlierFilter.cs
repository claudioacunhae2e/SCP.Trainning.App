using Domain.Abstraction.Factory;
using Domain.Abstraction.Model;
using Old._42.Util.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Factory
{
    public class OutlierFilter : IInputFilter
    {
        public OutlierFilter(double inferiorLimit, double superiorLimit)
        {
            _InferiorLimit = inferiorLimit;
            _SuperiorLimit = superiorLimit;
        }

        private readonly double _InferiorLimit;
        private readonly double _SuperiorLimit;

        public IList<IInput> Filter(IList<IInput> inputs)
        {
            var observations = inputs.Where(i => i.Observation.Y >= 0);

            if (observations.Empty())
                return inputs;

            var avg = observations.Average(m => m.Observation.Y);
            var min = _InferiorLimit * avg;
            var max = _SuperiorLimit * avg;

            RemoveInputs(inputs, min, max);

            return inputs;
        }

        private void RemoveInputs(IList<IInput> inputs, double min, double max)
        {
            var inputsToRemove = inputs.Where(i => i.Observation.Y <= min || i.Observation.Y > max).ToArray();

            if (inputsToRemove != null && inputsToRemove.Length > 0)
            {
                var count = inputsToRemove.Length;

                for (int i = 0; i < count; i++)
                {
                    inputs.Remove(inputsToRemove[i]);
                }
            }
        }
    }
}
