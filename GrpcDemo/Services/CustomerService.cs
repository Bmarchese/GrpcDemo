using Grpc.Core;

namespace GrpcServer.Services
{
    public class CustomerService : Customer.CustomerBase
    {
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            CustomerModel customer = new CustomerModel();

            if (request.UserId == 1)
            {
                customer.FirstName = "James";
                customer.LastName = "Smith";
            }
            else if (request.UserId == 2)
            {
                customer.FirstName = "Jane";
                customer.LastName = "Morgan";
            }
            else
            {
                customer.FirstName = "John";
                customer.LastName = "Doe";
            }

            customer.Email = (customer.FirstName + customer.LastName).ToLower() + "@mail.com";

            return Task.FromResult(customer);

        }


        public override async Task GetNewCustomers(NewCustomerRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            List<CustomerModel> customers = new List<CustomerModel>
            {
                new CustomerModel
                {
                    FirstName = "Tim",
                    LastName = "Corey",
                    Email = "tim@iamtimcorey.com",
                    Active = true,
                },
                new CustomerModel
                {
                    FirstName = "John",
                    LastName = "Doe Junior",
                    Email = "johndoejr@doeland.com",
                    Active = false,
                },
                new CustomerModel
                {
                    FirstName = "William",
                    LastName = "Williams",
                    Email = "willwill@contoso.com",
                    Active = true,
                }, 
                new CustomerModel
                {
                    FirstName = "Mary",
                    LastName = "Hertel",
                    Email = "maryh@email.com",
                    Active = false,
                },
                new CustomerModel
                {
                    FirstName = "Alice",
                    LastName = "Evans",
                    Email = "evansalice@contoso.com",
                    Active = true,
                }
            };

            foreach(var c in customers)
            {
                await responseStream.WriteAsync(c);
            }
        }
    }
}
