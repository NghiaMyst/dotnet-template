using dotnet_boilderplate.SharedKernel.Common;
using dotnet_boilderplate.SharedKernel.Results;

namespace vrp_demo.Domains.Aggregates
{
    /// <summary>
    /// This is a demo, so base location of driver will be manually set
    /// </summary>
    public class Driver : BaseEntity<Guid>
    {
        public string Name { get; private set; } = String.Empty;

        public string Address { get; private set; } = String.Empty;

        public double Lat { get; private set; }

        public double Lng { get; private set; }

        public List<Guid> SkillIds { get; private set; } = [];

        public string Role { get; private set; } = string.Empty;
        
        private Driver() { }
    
        public static Result<Driver> Create(string name, string address, double lat, double lng, RoleType role, List<Guid>? skillIds = null)
        {
            var driver = new Driver()
            {
                Name = name,
                Address = address,
                Lat = lat,
                Lng = lng,
                SkillIds = skillIds ?? [],
                Role = role.ToString()
            };

            driver.SetCreated();

            return Result<Driver>.Success(driver);
        }
        
    }

    public enum RoleType
    {
        Administrator,
        Driver
    }
}
