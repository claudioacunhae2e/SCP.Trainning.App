namespace Domain.Abstraction.Model
{
    public interface ISystemConfigDTO
    {
        int MinBeta { get; set; }
        double Percentil { get; set; }
        double Lambda { get; set; }
    }
}
