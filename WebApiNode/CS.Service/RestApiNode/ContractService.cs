using CS.NodeApi.Api;
using CS.Service.RestApiNode.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SauceControl.Blake2Fast;
using NodeApi;


namespace CS.Service.RestApiNode
{
    class ContractService : TransactionService
    {

        public ContractService(IConfiguration configuration) : base(configuration)
        {
        }

    }
}
