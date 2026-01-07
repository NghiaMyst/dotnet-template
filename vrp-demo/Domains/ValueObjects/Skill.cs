using dotnet_boilderplate.SharedKernel.Common;

namespace vrp_demo.Domains.ValueObjects
{
    public class Skill : ValueObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Skill(string name, string? description = null) 
        { 
            Id = Guid.NewGuid();
            Name = name;
            Description = description ?? string.Empty;
        }

        // TODO: Skill can be specified more 

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
