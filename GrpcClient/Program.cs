using Grpc.Core;
using Grpc.Net.Client;
using GrpcClient;

var channel = GrpcChannel.ForAddress("https://localhost:7211");
var customerClient = new Customer.CustomerClient(channel);

Console.WriteLine("Insert a Customer Id");
int id = int.TryParse(Console.ReadLine(), out var resultado) ? resultado : 0;

var clientRequested = new CustomerLookupModel { UserId = id };
var customer = await customerClient.GetCustomerInfoAsync(clientRequested);
Console.WriteLine($"{customer.FirstName} {customer.LastName}");
Console.WriteLine(customer.Email);

Console.WriteLine("\n-----------------------------");
Console.WriteLine("STREAMING \n");

using (var call = customerClient.GetNewCustomers(new NewCustomerRequest()))
{
    while(await call.ResponseStream.MoveNext())
    {
        var currentCustomer = call.ResponseStream.Current;
        Console.WriteLine($"{currentCustomer.FirstName} {currentCustomer.LastName} - {currentCustomer.Email}\n" +
            $"Active? {currentCustomer.Active}\n");
    }
}
