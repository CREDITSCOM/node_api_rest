using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Web3;

namespace CS.Test.Unit.Api
{
    
    [TestClass]
    public class EthreumApiTest
    {
        [TestMethod]
        public void GetAccountBalanceTest()
        {
            var web3 = new Web3("https://mainnet.infura.io/v3/7238211010344719ad14a89db874158c");
            var balance = web3.Eth.GetBalance.SendRequestAsync("0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae").Result;
            Console.WriteLine($"Balance in Wei: {balance.Value}");

            var etherAmount = Web3.Convert.FromWei(balance.Value);
            Debug.WriteLine($"Balance in Ether: {etherAmount}");
        }
    }
}
