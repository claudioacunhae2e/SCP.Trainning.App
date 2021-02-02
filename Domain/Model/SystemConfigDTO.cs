using Domain.Abstraction.Model;

namespace Domain.Model
{
    public class SystemConfigDTO : ISystemConfigDTO
    {
        public int MinBeta { get; set; }
        public double Percentil { get; set; }
        public double Lambda { get; set; }
    }
}
