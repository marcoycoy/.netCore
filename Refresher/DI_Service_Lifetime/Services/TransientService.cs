namespace DI_Service_Lifetime.Services
{
    public class TransientService:ITransientService
    {
        private readonly Guid Id;
        public TransientService()
        {
            Id = Guid.NewGuid();
        }

        public string GetGuid()
        {
            return Id.ToString();
        }
    }
}
