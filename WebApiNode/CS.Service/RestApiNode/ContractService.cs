using Microsoft.Extensions.Configuration;

namespace CS.Service.RestApiNode
{
    class ContractService : TransactionService
    {
        public ContractService(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
