class NodeApi {


    UrlRest;

    constructor(urlRest) {
        this.UrlRest = urlRest;
    }

  





    GetBalance(key, callback) {


        let result = {};
        let _url = new URL(`${this.UrlRest}/balance`);
        let params = { wallet: key };
        Object
            .keys(params)
            .forEach(k => _url.searchParams.append(k, params[k]));
        let success = true;
        var response = fetch(_url, {
            mode: "cors"
        })
            .then(r => r.json())
            .catch(ex => {
                success = false;
            });

        if (success) {
            response.then(x => {
                if (callback) {
                    callback(x);
                }
                result.Data = x;
                result.Status = true;
                return result;
            });
        } else {
            result.Status = false;
        }
        return result;
    }


   SendTransaction(transactionData, callback) {
        let result = {};
        let _url = new URL(`${this.UrlRest}/send`);
        let params = transactionData;
        Object
            .keys(params)
            .forEach(k => _url.searchParams.append(k, params[k]));
        let success = true;
        var response = fetch(_url, {
            mode: "cors"
        })
            .then(r => r.json())
            .catch(ex => {
                success = false;
            });

        if (success) {
            response.then(x => {
                if (callback) {
                    callback(x);
                }
              
                result.Status = true;
                result.Data = x;
            
            });

        } else {
            result.Status = false;
           
        }

       
     
    }


    CreateAccount() {
      
        let result = {};
        let _url = new URL(`${this.UrlRest}/newwallet`);
        let success = true;
        var response = fetch(_url, {
            mode: "cors"
        })
            .then(r => r.json())
            .catch(ex => {
                success = false;
            });

        if (success) {
            response.then(x => {
                
                result.Status = true;
                result.Data = x;
                return result;
            });
        } else {
            result.Status = false;
        }
        return result;

    }


  

    GetInfoTransactionById(id, callback) {

     
        let result = {};
        let _url = new URL(`${this.UrlRest}/transactions/${id}`);

        let success = true;
        var response = fetch(_url, {
            mode: "cors"
        })
            .then(r => r.json())
            .catch(ex => {
                success = false;
            });

        if (success) {
            response.then(x => {
                if (callback) {
                    callback
                }
                var data = {
                    Id: x.id,
                    Amount: this.AmountToStrNew({ amount_int: x.amount_int, amount_fraq: x.amount_fraq, }),
                    Source: x.source,
                    Receiver: x.target,
                    Fee: x.fee,
                    Block: x.block,
                }

                result.Status = true;
                result.Data = data;
            });

        } else {
            result.Status = "failure";
        }

      
        return result;
    }




   GetLastBlockId(callback) {

        let result = {};
        let _url = new URL(`${this.UrlRest}/heightblock`);

        let success = true;
        var response = fetch(_url, {
            mode: "cors"
        })
            .then(r => r.json())
            .catch(ex => {
                success = false;
            });

        if (success) {
            response.then(x => {
                if (callback) {
                    callback(x);
                }
                result.Data = x;
                result.Status = true;
                return result;
         });

        } else {
            result.Status = false;
        }


        return result;
    }


    GetInfoLastBlock(callback) {
     
            let result = {};

        let _url = new URL(`${this.UrlRest}/infoheightblock`);

            let success = true;
            var response = fetch(_url, {
                mode: "cors"
            })
                .then(r => r.json())
                .catch(ex => {
                    success = false;
                });

            if (success) {
                response.then(x => {
                    if (callback) {
                        callback(x);
                    }
                    result.Data = x;
                    result.Status = true;
                   
                });

            } else {
                result.Status = false;
                
            }
       
        return result;
    }


    GetInfoByBlockId(blockId, callback) {


        let result = {};
        let _url = new URL(`${this.UrlRest}/block/${blockId}`);

        let success = true;
        var response = fetch(_url, {
            mode: "cors"
        })
            .then(r => r.json())
            .catch(ex => {
                success = false;
            });

        if (success) {
            response.then(x => {
                if (callback) {
                    callback(x);
                }
                result.Data = x;
                result.Status = true;
            });

        } else {
            result.Status = false;
        }


        return result;
    }



    ///
    ///
    ///
    ///
    ///
    History(key, page, size, callback) {
        key = this.CheckPublicKey(key);
        this.Connect.TransactionsGet(key, page * size - size, size, r => {
            if (r.status.code > 0) {
                throw r.status.message;
            }
            let h = [];
            for (let i in r.transactions) {
                h.push({
                    id: `${r.transactions[i].id.poolSeq}.${r.transactions[i].id.index + 1}`,
                    amount: this.AmountToStr(r.transactions[i].trxn.amount),
                    source: Base58.encode(SignCS.ConvertCharToByte(r.transactions[i].trxn.source)),
                    target: Base58.encode(SignCS.ConvertCharToByte(r.transactions[i].trxn.target)),
                    smartContract: r.transactions[i].trxn.smartContract,
                    smartInfo: r.transactions[i].trxn.smartInfo,
                    timeCreation: new Date(r.transactions[i].trxn.timeCreation),
                    extraFee: r.transactions[i].trxn.extraFee
                });

            }
            callback(h);
        });
    }

    AmountToStrNew(a) {
      
        var d = (a.amount_fraq / Math.pow(10, 18))
        return (a.amount_int + d).toFixed(12);
    }

  

    CheckPublicKey(k) {
        switch (typeof k) {
            case "string":
                try {
                    k = Base58.decode(k);
                }
                catch (ex) {
                    throw "Key is not Base58";
                }
                break;
            case "object":
                if (k.length != 32) throw "The Key must be a 32-bit Uint8Array array";
                break;
            default:
                throw "The Key must be a string or object";
        }
        return k;
    }
}
