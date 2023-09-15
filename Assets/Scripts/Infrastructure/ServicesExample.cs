namespace Infrastructure
{
    public class ServicesExample
    {
        // 1. Define your service interface
        public interface INewService: IService{}
        
        // 2. Implement your service interface
        public class NewService: INewService{}

        // 3. Register your service in the AllService Container
        private void ServiceRegistrationExample()
        {
            AllServices.Container.RegisterSingle<INewService>(new NewService());
        }
        
        // 4. Get your service from the AllService Container
        private void GettingServiceExample()
        {
            var newService = AllServices.Container.Single<INewService>();
        }
    }
}