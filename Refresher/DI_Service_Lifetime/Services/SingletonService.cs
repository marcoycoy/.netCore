namespace DI_Service_Lifetime.Services
{
    public class SingletonService:ISingletonService
    {
        private readonly Guid Id;
        public SingletonService() { 
            Id = Guid.NewGuid();
        }

        public string GetGuid()
        {
            return Id.ToString();
        }
    }
}
