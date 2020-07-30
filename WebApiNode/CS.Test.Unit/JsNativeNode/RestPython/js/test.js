var UrlRest = 'http://localhost:60476/api/monitor';
//var UrlRest = 'http://158.175.71.115:8000';


function TRANSACTION() {
   var api = new NodeApi(UrlRest);

    var transactionData = {
        src: "Bsr45y6siaoRYZnhJ6W1TJijkw7RANBfcvmQ18tg2AAe",
        dst: "AYStrMFYmwP19M3hr58cFZ2RppPJ9bUrKvpN4B5TCqSv",
        priv: "3sH7Vepy82PsQSbLYuZuXjwu9FbMdNF4zCcMRY7z3AxF9ukmZrRrDP1cn5s9NpfgKQBYsMGiXyi3qdrVinRfiuNJ",
        amount: "0.8",
        fee: "0.1",
    };

    api.SendTransaction(transactionData, function (r) {
        console.log(r);
    });
 
}




function BALANCE() {
    var api = new NodeApi(UrlRest);

   
    console.log(api.GetBalance("5qw4jgAXHC5z63NTgk91yrnL5mtUAdwYUuxKb9MsK77s")); 

}

function CREATE_ACCOUNT() {
    var api = new NodeApi(UrlRest);

  
  
    console.log(api.CreateAccount());

}



function TRANSACTION_INFO() {
  
    var api = new NodeApi(UrlRest);

   
    console.log(api.GetInfoTransactionById("500970711111111111.1"));
    console.log(api.GetLastBlockId());
    console.log(api.GetInfoLastBlock());
}