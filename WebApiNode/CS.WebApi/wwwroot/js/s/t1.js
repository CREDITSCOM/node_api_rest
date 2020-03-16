var netWork = new CreditsWork("http://165.22.220.8", "8081");
// БАЛАНС
// netWork.Balance("HhhRGwgA3W5qcNFrLC3odC4GmbkQnhdEc5XPqBiRW3Wx", function(t){ console.log(t); });


Url = "192.168.0.24"//"157.245.103.69";



Port = "8080";//"8081";
function Connect(Ip) {

    Ip = `http://${Url}:${Port}/thrift/service/Api/`;
    //  }

    var transport = new Thrift.Transport(`http://${Url}:${Port}`);
    var protocol = new Thrift.Protocol(transport);
    return new APIClient(protocol);
}


function CLICK() {
    var obj = {
        _connect: Connect(),
        source: "FeFjpcsfHErXPk5HkfVcwH6zYaRT2xNytDTeSjfuVywt",
        target: "HhhRGwgA3W5qcNFrLC3odC4GmbkQnhdEc5XPqBiRW3Wx",
        privateKey: "ohPH5zghdzmRDxd978r7y6r8YFoTcKm1MgW2gzik3omCuZLysjwNjTd9hnGREFyQHqhShoU4ri7q748UgdwZpzA",
        amount: "1",
        fee: "1",
        smartContract: undefined,
        transactionObj: undefined,
        userData: undefined,
        usedContracts: undefined

    };

    var transaction = SignCS.CreateTransaction(obj);



    SignCS.Connect().TransactionFlow(transaction, function (r) {
        if (r.status.code > 0) {
            console.log("?" + r.status.message);
        }
        else {
            console.log("!" + r);
        }
        //	p.postMessage(Res);
    });

}
