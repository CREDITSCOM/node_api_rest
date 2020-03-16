async function getBalanceTest() {
    let _url = "http://rest.creditsenterprise.com/api/Monitor/GetBalance";

    let model = {
        publicKey: "5B3YXqDTcWQFGAqEJQJP3Bg1ZK8FFtHtgCiFLT5VAxpe",
        networkAlias: "MainNet",
        networkIp: "68.183.230.109",
        networkPort: "9090",
        methodApi: "GetBalance",

    };
    // _url += `?publicKey=${model.publicKey}&`;
    // _url += `networkAlias=${model.networkAlias}&`;
    // _url += `networkIp=${model.networkIp}&`;
    // _url += `networkPort=${model.networkPort}&`;
    let rsp = await fetch(_url, {
        method: "POST",
        mode: "cors",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(model)
    });

    let responseJson = await rsp.json();

    return JSON.stringify(responseJson);
}


async function testGetBalance() {
    let el = document.getElementById("getBalance");
    el.innerText = await getBalanceTest();
}