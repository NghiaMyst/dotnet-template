using dotnet_boilderplate.SharedKernel.Common;
using dotnet_boilderplate.SharedKernel.Results;
using NetTopologySuite.Geometries;
using vrp_demo.Domains.Enums;

namespace vrp_demo.Domains.Aggregates
{
    /// <summary>
    /// This is a demo, so base location of driver will be manually set
    /// </summary>
    public class Driver : BaseEntity<Guid>
    {
        public string Name { get; private set; } = String.Empty;

        public string Address { get; private set; } = String.Empty;

        public Point Location { get; private set; }

        public List<Guid> SkillIds { get; private set; } = [];

        public string Role { get; private set; } = string.Empty;
        
        private Driver() { }
    
        public static Result<Driver> Create(string name, string address, double lat, double lng, RoleType role, List<Guid>? skillIds = null)
        {
            var driver = new Driver()
            {
                Name = name,
                Address = address,
                Location = new Point(lng, lat),
                SkillIds = skillIds ?? [],
                Role = role.ToString()
            };

            driver.SetCreated();

            return Result<Driver>.Success(driver);
        }

        public Result AddSkills(List<Guid> skillIds)
        {
            if (skillIds == null || skillIds.Count == 0)
                return Result.Failure(Error.Validation("Skill IDs cannot be empty"));

            foreach (var skillId in skillIds)
            {
                if (!SkillIds.Contains(skillId))
                {
                    SkillIds.Add(skillId);
                }
            }

            return Result.Success();
        }

        public Result Update(string name, string address, double lat, double lng, RoleType role)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(Error.Validation("Name cannot be empty"));

            Name = name;
            Address = address;
            Location = new Point(lng, lat);
            Role = role.ToString();

            return Result.Success();
        }
        
    }
}
