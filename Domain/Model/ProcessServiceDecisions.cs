using Domain.Abstraction.Model;

namespace Domain.Model
{
    public class ProcessServiceDecisions : IProcessServiceDecisions
    {
        public ProcessServiceDecisions()
        {
            GroupHistory = true;
            Normalization = true;
            Rectification = true;
            SimilarLocations = true;
            MergeDatabaseAndUpdateSystemInfo = true;
            MaxDegreesOfParallelism = 120;
        }

        public ProcessServiceDecisions(bool groupHistory, bool normalization, bool rectification, bool similarLocations, bool mergeDatabaseAndUpdateSystemInfo, int maxDegreesOfParallelism)
        {
            GroupHistory = groupHistory;
            Normalization = normalization;
            Rectification = rectification;
            SimilarLocations = similarLocations;
            MergeDatabaseAndUpdateSystemInfo = mergeDatabaseAndUpdateSystemInfo;
            MaxDegreesOfParallelism = maxDegreesOfParallelism;
        }

        public bool GroupHistory { get; set; }
        public bool Normalization { get; set; }
        public bool Rectification { get; set; }
        public bool SimilarLocations { get; set; }
        public bool MergeDatabaseAndUpdateSystemInfo { get; set; }
        public int MaxDegreesOfParallelism { get; set; }
    }
}
