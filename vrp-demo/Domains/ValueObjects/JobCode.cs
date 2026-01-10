using dotnet_boilderplate.SharedKernel.Common;

namespace vrp_demo.Domains.ValueObjects
{
    public class JobCode : ValueObject
    {
        public string Code { get; set; } = string.Empty;

        private JobCode(string value)
        {
            Code = value;
        }

        public static JobCode Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Job code cannot be empty");

            return new JobCode(value);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Code;
        }

        public override string ToString()
        {
            return Code;
        }

        public static implicit operator string(JobCode code) => code.Code;
    }
}
