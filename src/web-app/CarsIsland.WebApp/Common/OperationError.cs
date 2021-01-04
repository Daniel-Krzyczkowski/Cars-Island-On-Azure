

namespace CarsIsland.WebApp.Common
{
    public record OperationError
    {
        public string Details { get; }

        public OperationError(string details) => (Details) = (details);
    }
}
