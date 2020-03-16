var UrlNode = 'http://165.22.212.106:8081'; 
var UrlRest = 'http://158.175.71.115:5000';


function TRANSACTION() {
    var api = new NodeApi(UrlNode, UrlRest);

    var transactionData = {
        source: "FeFjpcsfHErXPk5HkfVcwH6zYaRT2xNytDTeSjfuVywt",
        target: "HhhRGwgA3W5qcNFrLC3odC4GmbkQnhdEc5XPqBiRW3Wx",
        privateKey: "",
        amount: "0.8",
        fee: "1",
        smartContract: undefined,
        transactionObj: undefined,
        userData: undefined,
        usedContracts: undefined

    };

    api.SendTransaction(transactionData, function(r) {
        if (r.status.code > 0) {
            console.log("Error: " + r.status.message );
        } else {
            console.log("Success" + r);
        }
    });

}




function BALANCE() {
    var api = new NodeApi(UrlNode, UrlRest);

    api.GetBalance("HhhRGwgA3W5qcNFrLC3odC4GmbkQnhdEc5XPqBiRW3Wx", function(t) { console.log(t); });

}

function CREATE_ACCOUNT() {
    var api = new NodeApi(UrlNode, UrlRest);

    console.log(api.CreateAccount());

}



function TRANSACTION_INFO() {
    // var api = new NodeApi(Url);
    alert("GET TRANSACTION_INFO");
    var api = new NodeApi(UrlNode, UrlRest);

    var res = api.GetTransactionInfoRest("25504337.1");

    console.log(res);

    console.log(api.GetLastBlockId());

    console.log(api.GetInfoByLastBlock());

}