using CS.Service.RestApiNode;
using CS.Service.RestApiNode.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Test.Unit.Api
{
    [TestClass]
    public class ApiMethodGetterTest : ApiMethodTest
    {

        public ApiMethodGetterTest() : base()
        {

        }

        [TestMethod]
        public void GetTransactionTest()
        {
            var service = new MonitorService(Configuration);

            var model = new RequestGetterApiModel();
          
            model.NetworkAlias = "MainNet";//"TestNet";
            model.AuthKey = "87cbdd85-b2e0-4cb9-aebf-1fe87bf3afdd";
            model.TransactionId = "26364287.1";

            var response = service.GetTransaction(model);
            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void GetLastBlockIdTest()
        {
            var service = new MonitorService(Configuration);

            var model = new RequestGetterApiModel();

            model.NetworkAlias = "MainNet";//"TestNet";
            model.AuthKey = "87cbdd85-b2e0-4cb9-aebf-1fe87bf3afdd";
           // model.TransactionId = "26364287.1";

            var response = service.GetLastBlockId(model);
            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void GetListTransactionByBlockIdTest()
        {
            var service = new MonitorService(Configuration);
            var model = new RequestGetterApiModel();

            //model.NetworkAlias = "MainNet";//"TestNet";
            //model.AuthKey = "87cbdd85-b2e0-4cb9-aebf-1fe87bf3afdd";
            //model.Limit = 100;
            //model.Offset = 0;
            //model.BlockId = 26388412;

            model.NetworkAlias = "TestNet";
            model.NetworkIp = "157.245.26.48";
            model.NetworkPort = "9091";
            model.Limit = 100;
            model.Offset = 277;
            model.BlockId = 8987133;

            var response = service.GetListTransactionByBlockId(model);
            Assert.IsNotNull(response);
        }
    }
}
