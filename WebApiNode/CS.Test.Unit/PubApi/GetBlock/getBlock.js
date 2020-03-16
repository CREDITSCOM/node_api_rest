async function testGetLastBlockId() {
    let _url = "http://rest.creditsenterprise.com/api/Monitor/GetLastBlockId";

    let model = {
     
        networkAlias: "MainNet",
        networkIp: "68.183.230.109",
        networkPort: "9090",
       

    };
    let rsp = await fetch(_url, {
        method: "POST",
        mode: "cors",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(model)
    });

    let responseJson = await rsp.json();
    let result = responseJson.blockId;//JSON.stringify(responseJson);
    console.log(result);
    return result;
}



async function testGetLastBlockInfo() {
    let _url = "http://rest.creditsenterprise.com/api/Monitor/GetListTransactionByLastBlock";

    let model = {

        networkAlias: "MainNet",
        networkIp: "68.183.230.109",
        networkPort: "9090",
        Offset: 0,
        Limit: 10000


    };
    let rsp = await fetch(_url, {
        method: "POST",
        mode: "cors",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(model)
    });

    let responseJson = await rsp.json();
    let result = responseJson;
    console.log(result);
    return result;
}




async function testGetBlockInfoById() {
    let _url =
        "http://rest.creditsenterprise.com/api/Monitor/GetListTransactionByBlockId";

    let model = {

        networkAlias: "MainNet",
        networkIp: "68.183.230.109",
        networkPort: "9090",
        BlockId: 26388412,
        Offset: 0,
        Limit: 100

    };
    let rsp = await fetch(_url, {
        method: "POST",
        mode: "cors",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(model)
    });

    let responseJson = await rsp.json();
    let result = responseJson;
    console.log(result);
    return result;
}

//async function testGetBalance() {
//    let el = document.getElementById("getBalance");
//    el.innerText = await getBalanceTest();
//}