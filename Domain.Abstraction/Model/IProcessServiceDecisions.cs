namespace Domain.Abstraction.Model
{
    public interface IProcessServiceDecisions
    {
        bool GroupHistory { get; set; }
        bool Normalization { get; set; }
        bool Rectification { get; set; }
        bool SimilarLocations { get; set; }
        bool MergeDatabaseAndUpdateSystemInfo { get; set; }
        int MaxDegreesOfParallelism { get; set; }
    }
}
