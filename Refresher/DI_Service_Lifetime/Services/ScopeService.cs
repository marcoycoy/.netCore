namespace DI_Service_Lifetime.Services
{
    public class ScopeService:IScopedService
    {
        private readonly Guid Id;
        public ScopeService()
        {
            Id = Guid.NewGuid();
        }

        public string GetGuid()
        {
            return Id.ToString();
        }
    }
}
