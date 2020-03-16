/// Only shell thrift

class NodeApi {

    
    UrlRest;

    constructor(url, urlRest) {
        SignCS.Connect = this.GetApiClient(url);
        this.Connect = SignCS.Connect;
        this.UrlRest = urlRest;
    }

    GetApiClient = function (url) {
        var transport = new Thrift.Transport(url);
        var protocol = new Thrift.Protocol(transport);
        return new APIClient(protocol);
    };






    GetBalance(key, callback) {
        var keyByte = Base58.decode(key);
        SignCS.Connect.WalletBalanceGet(keyByte, r => {
            if (r.status.code > 0) {
                throw r.status.message;
            }
             let b = [{ balance: this.AmountToStr(r.balance) }];
            this.Connect.TokenBalancesGet(keyByte, r => {
                if (r.status.code > 0) {
                    throw r.status.message;
                }
                for (let i in r.balances) {
                    b.push(
                        {
                            balance: r.balances[i].balance,
                            code: r.balances[i].code,
                            name: r.balances[i].name,
                            key: Base58.encode(SignCS.ConvertCharToByte(r.balances[i].token))
                        }
                    )
                }
                callback(b);
            });
        });
    }


    SendTransaction(transactionData, callback) {
        var transaction = SignCS.CreateTransaction(transactionData);
        SignCS.Connect.TransactionFlow(transaction, callback);
    }


    CreateAccount() {
        var pair = nacl.sign.keyPair();
        var publicKey = Base58.encode(pair.publicKey);
        var privateKey = Base58.encode(pair.secretKey); 
        return { PublicKey: publicKey, PrivateKey: privateKey };
    }


    GetTransactionInfo(id) {
        var list = id.split('.');

     
        var transactionId = { poolSeq: list[0], index: list[1]-1 };

        //transactionId.index = '' + transactionId.index;
        //transactionId.poolSeq = '' + transactionId.poolSeq;
        //Base58.decode(
     //   transactionId.index = Base58.decode(transactionId.index);
       // transactionId.poolSeq = Base58.decode(transactionId.poolSeq);

        var transactionResult = SignCS.Connect.TransactionGet(transactionId);
        var transaction = transactionResult.transaction;
        console.log(transactionResult);
        var result = {
            Id: id,
            Amount: this.AmountToStr(transaction.trxn.amount),
            Source: Base58.encode(SignCS.ConvertCharToByte(transaction.trxn.source)),
            Receiver: Base58.encode(SignCS.ConvertCharToByte(transaction.trxn.target)),
            Fee: transaction.trxn.extraFee,
            Status: 'success',
            Block: list[0]
        };
        return result;
    }


    GetTransactionInfoRest(id) {

        // POST REQUEST ON this.UrlRest




        var result = {
            Id: id,
            Amount: this.AmountToStr(transaction.trxn.amount),
            Source: Base58.encode(SignCS.ConvertCharToByte(transaction.trxn.source)),
            Receiver: Base58.encode(SignCS.ConvertCharToByte(transaction.trxn.target)),
            Fee: transaction.trxn.extraFee,
            Status: 'success',
            Block: ''
        };
        return result;
    }




    GetLastBlockId() {

        var block = SignCS.Connect.SyncStateGet();

       
        return block.lastBlock;
    }


    GetInfoByLastBlock() {
        var blockId = this.GetLastBlockId();
        var block = this.GetInfoByBlockId(blockId);

        return block;
    }


    GetInfoByBlockId(blockId) {
        var block = SignCS.Connect.PoolListGet(blockId, 1);

        return block;
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

    AmountToStr(a) {
        return `${a.integral}.${(a.fraction / Math.pow(10, 18)).toFixed(12)}`
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


(function () {
    function Connect(Ip) {
        let transport = new Thrift.Transport(Ip);
        let protocol = new Thrift.Protocol(transport);
        return new APIClient(protocol);
    }

    function CommissionToNumb(c) {
        let sign = c >> 15;
        let m = c & 0x3FF;
        let f = (c >> 10) & 0x1F;
        let v1024 = 1.0 / 1024;
        return (sign != 0 ? -1 : 1) * m * v1024 * Math.pow(10, f - 18);
    }

    let ConvertCharToByte = s => {
        let b = new Uint8Array(s.length);
        for (let i in s) {
            b[i] = s[i].charCodeAt();
        }
        return b;
    }

    let concatTypedArrays = (a, b) => {
        let c = new (Uint8Array.prototype.constructor)(a.length + b.length);
        c.set(a, 0);
        c.set(b, a.length);
        return c;
    }

    let CombinObj = (d, o) => {
        for (let i in d) {
            if (o[i] === undefined) {
                o[i] = d[i];
            }
        }
    }

    let NumberNormalization = (n, p) => {
        switch (typeof n) {
            case "string":
                return n.replace(',', '.');
                break;
            case "number":
                return String(n).replace(',', '.');
                break;
            default:
                throw new Error(`${p} must be number or string`)
        }
    }

    let CheckStrTransaction = (f, fn) => {
        switch (typeof f) {
            case "string":
                if (f === "") {
                    throw new Error(`${fn} is not found`);
                }
                else {
                    try {
                        return Base58.decode(f);
                    } catch (e) {
                        throw new Error(`${fn} is not valid`);
                    }
                }
                break;
            case "object":
                return f;
                break;
            default:
                throw new Error(`${fn} is not valid`);
        }
    }

    let Fee = v => {
        let s = v > 0 ? 0 : 1;
        v = Math.abs(v);
        exp = v === 0 ? 0 : Math.log10(v);
        exp = Math.floor(exp >= 0 ? exp + 0.5 : exp - 0.5);
        v /= Math.pow(10, exp);
        if (v >= 1) {
            v *= 0.1;
            ++exp;
        }
        v = Number((v * 1024).toFixed(0));
        return (exp + 18 << 10) | (v === 1024 ? 1023 : v);
    }

    let SignTrans = () => {

    };

    var CreateTransaction = () => {
    };

    window.SignCS = {
        ConvertCharToByte: ConvertCharToByte,
        Connect: Connect,
        CreateTransaction: (Obj) => {
            let DefObj = {
                _connect: Connect(),
                source: "",
                target: "",
                privateKey: "",
                amount: "0.0",
                fee: "0.9",
                smartContract: undefined,
                transactionObj: undefined,
                userData: undefined,
                usedContracts: undefined
            };

            let DefSmart = {
                params: undefined,
                method: undefined,
                code: undefined,
                usedContracts: undefined,
                newState: false
            };

            CombinObj(DefObj, Obj);

            if (Obj.smartContract !== undefined) {
                CombinObj(DefSmart, Obj.smartContract);
            }

            Obj.amount = NumberNormalization(Obj.amount, "Amount");
            Obj.fee = NumberNormalization(Obj.fee, "Fee");

            let Trans;
            if (Obj.TransactionObj === undefined) {
                Trans = new Transaction();
            }
            else {
                Trans = Obj.TransactionObj;
            }

            Trans.source = CheckStrTransaction(Obj.source, "Source");

            Obj.privateKey = CheckStrTransaction(Obj.privateKey, "Private");

            let TRes = SignCS.Connect.WalletTransactionsCountGet(Trans.source);
            if (TRes.status.code === 0) {
                Trans.id = TRes.lastTransactionInnerId + 1;
            }
            else {
                throw new Error(TRes.status.message);
            }

            if (Obj.smartContract === undefined || Obj.smartContract.code === undefined) {
                Trans.target = CheckStrTransaction(Obj.target, "Target");
            }
            else {
                let Target = Trans.source;
                concatTypedArrays(Target, NumbToByte(Trans.id, 6));
                var byteCode = SignCS.Connect.SmartContractCompile(Obj.smartContract.code);
                if (byteCode.status.code === 0) {
                    for (let i in byteCode.byteCodeObjects) {
                        concatTypedArrays(Target, ConvertCharToByte(byteCode.byteCodeObjects[i].byteCode));
                    }
                }
                else {
                    throw new Error(ByteCode.status.message);
                }

                Trans.target = blake2s(Target);
            }

            Trans.amount = new Amount({
                integral: Math.trunc(Obj.amount),
                fraction: 0
            });

            if (Trans.amount.integral > 0) {
                Trans.amount.fraction = (Obj.amount - Trans.amount.integral).toFixed(12) * Math.pow(10, 18)
            }
            else {
                Trans.amount.fraction = Number(Obj.amount).toFixed(12) * Math.pow(10, 18)
            }

            Trans.fee = new AmountCommission({
                commission: Fee(Obj.fee)
            });
            Trans.currency = 1;

            let ps = NumbToByte(Trans.id, 6);
            ps = concatTypedArrays(ps, Trans.source);
            ps = concatTypedArrays(ps, Trans.target);
            ps = concatTypedArrays(ps, NumbToByte(Trans.amount.integral, 4));
            ps = concatTypedArrays(ps, NumbToByte(Trans.amount.fraction, 8));
            ps = concatTypedArrays(ps, NumbToByte(Trans.fee.commission, 2));
            ps = concatTypedArrays(ps, new Uint8Array([1]));

            if (Obj.smartContract === undefined &&
                Obj.userData === undefined &&
                Obj.usedContracts === undefined) {
                ps = concatTypedArrays(ps, new Uint8Array(1));
            }
            else if (Obj.smartContract) {
                ps = concatTypedArrays(ps, new Uint8Array([1]));

                Trans.smartContract = new SmartContractInvocation();

                let uf = new Uint8Array([11, 0, 1]);

                if (Obj.smartContract.method === undefined) {
                    uf = concatTypedArrays(uf, new Uint8Array(4));
                }
                else {
                    Trans.smartContract.method = Obj.smartContract.method;
                    uf = concatTypedArrays(uf, NumbToByte(Trans.smartContract.method.length, 4).reverse());
                    uf = concatTypedArrays(uf, ConvertCharToByte(Trans.smartContract.method));
                }

                uf = concatTypedArrays(uf, new Uint8Array([15, 0, 2, 12]));
                if (Obj.smartContract.params === undefined) {
                    uf = concatTypedArrays(uf, new Uint8Array(4));
                }
                else {
                    Trans.smartContract.params = [];
                    uf = concatTypedArrays(uf, NumbToByte(Obj.smartContract.params.length, 4).reverse());
                    for (let i in Obj.smartContract.params) {
                        let val = Obj.smartContract.params[i];

                        switch (val.k) {
                            case "STRING":
                                uf = concatTypedArrays(uf, new Uint8Array([11, 0, 17]));
                                uf = concatTypedArrays(uf, NumbToByte(val.v.length, 4).reverse());
                                uf = concatTypedArrays(uf, ConvertCharToByte(val.v));
                                Trans.smartContract.params.push(new Variant({ v_string: val.v }));
                                uf = concatTypedArrays(uf, new Uint8Array(1));
                                break;
                            case "INT":
                                uf = concatTypedArrays(uf, new Uint8Array([8, 0, 9]));
                                uf = concatTypedArrays(uf, NumbToByte(val.v, 4).reverse());
                                Trans.smartContract.params.push(new Variant({ v_int: val.v }));
                                uf = concatTypedArrays(uf, new Uint8Array(1));
                                break;
                            case "BOOL":
                                uf = concatTypedArrays(uf, new Uint8Array([2, 0, 3, val.v]));
                                Trans.smartContract.params.push(new Variant({ v_boolean: val.v }));
                                uf = concatTypedArrays(uf, new Uint8Array(1));
                                break;
                            case "DOUBLE":
                                uf = concatTypedArrays(uf, new Uint8Array([4, 0, 15]));
                                let Buf = new ArrayBuffer(8);
                                let UB = new Uint8Array(Buf);
                                let N = new Float64Array(Buf);
                                N[0] = val.v;
                                uf = concatTypedArrays(uf, UB.reverse());
                                uf = concatTypedArrays(uf, new Uint8Array(1));
                                Trans.smartContract.params.push(new Variant({ v_double: val.v }));
                                break;
                        }
                    }
                }

                uf = concatTypedArrays(uf, new Uint8Array([15, 0, 3, 11]));
                if (Obj.smartContract.usedContracts === undefined) {
                    uf = concatTypedArrays(uf, new Uint8Array(4));
                }
                else {
                    Trans.smartContract.usedContracts = [];
                    uf = concatTypedArrays(uf, NumbToByte(Obj.smartContract.usedContracts.length, 4).reverse());
                    for (let i in Obj.smartContract.usedContracts) {
                        uf = concatTypedArrays(uf, NumbToByte(Obj.smartContract.usedContracts[i].length, 4).reverse());
                        uf = concatTypedArrays(uf, ConvertCharToByte(Obj.smartContract.usedContracts[i]));
                        Trans.smartContract.usedContracts.push(ConvertCharToByte(Obj.smartContract.usedContracts[i]));
                    }
                }

                Trans.smartContract.forgetNewState = Obj.smartContract.newState;
                uf = concatTypedArrays(uf, new Uint8Array([2, 0, 4, Trans.smartContract.forgetNewState]));

                if (Obj.smartContract.code !== undefined) {
                    uf = concatTypedArrays(uf, new Uint8Array([12, 0, 5, 11, 0, 1]));

                    Trans.smartContract.smartContractDeploy = new SmartContractDeploy({
                        sourceCode: Obj.smartContract.code
                    });

                    uf = concatTypedArrays(uf, NumbToByte(Obj.smartContract.code.length, 4).reverse());
                    uf = concatTypedArrays(uf, ConvertCharToByte(Obj.smartContract.code));

                    uf = concatTypedArrays(uf, new Uint8Array([15, 0, 2, 12]));

                    Trans.smartContract.smartContractDeploy.byteCodeObjects = [];

                    uf = concatTypedArrays(uf, NumbToByte(byteCode.byteCodeObjects.length, 4).reverse());

                    for (let i in ByteCode.byteCodeObjects) {
                        let val = ByteCode.byteCodeObjects[i];

                        uf = concatTypedArrays(uf, new Uint8Array([11, 0, 1]));
                        uf = concatTypedArrays(uf, NumbToByte(val.name.length, 4).reverse());
                        uf = concatTypedArrays(uf, ConvertCharToByte(val.name));

                        uf = concatTypedArrays(uf, new Uint8Array([11, 0, 2]));
                        uf = concatTypedArrays(uf, NumbToByte(val.byteCode.length, 4).reverse());
                        uf = concatTypedArrays(uf, ConvertCharToByte(val.byteCode));

                        Trans.smartContract.smartContractDeploy.byteCodeObjects.push(new ByteCodeObject({
                            name: val.name,
                            byteCode: val.byteCode
                        }));

                        uf = concatTypedArrays(uf, new Uint8Array(1));
                    }

                    uf = concatTypedArrays(uf, new Uint8Array([11, 0, 3, 0, 0, 0, 0, 8, 0, 4, 0, 0, 0, 0, 0]));
                }

                uf = concatTypedArrays(uf, new Uint8Array([6, 0, 6, 0, 1]));

                uf = concatTypedArrays(uf, new Uint8Array(1));
                ps = concatTypedArrays(ps, NumbToByte(uf.length, 4));
                ps = concatTypedArrays(ps, uf);
            }
            else if (Obj.userData) {
                let uf = new Uint8Array([1]);
                uf = concatTypedArrays(uf, NumbToByte(Obj.userData.length, 4));
                uf = concatTypedArrays(uf, ConvertCharToByte(Obj.userData));
                ps = concatTypedArrays(ps, uf);
                Trans.userFields = ConvertCharToByte(Obj.userData);
            }
            else if (Obj.usedContracts) {
                Trans.usedContracts = [];

                uf = concatTypedArrays(uf, new Uint8Array(1));
                ps = concatTypedArrays(ps, NumbToByte(UserField.length, 4));
                ps = concatTypedArrays(ps, uf);

                for (let i in Obj.usedContracts) {
                    Trans.usedContracts.push(Base58.decode(Obj.usedContracts[i]));
                }
            }

            let ArHex = "0123456789ABCDEF";
            let Hex = "";
            for (let j = 0; j < ps.length; j++) {
                Hex += ArHex[Math.floor(ps[j] / 16)];
                Hex += ArHex[Math.floor(ps[j] % 16)];
            }
            console.log(Hex);

            Trans.signature = nacl.sign.detached(ps, Obj.privateKey);
            console.log(Trans);

            return Trans;
        }
    };

    function NumbToBits(int) {
        let Bits = "";

        let numb = String(int);
        while (true) {
            Bits = (numb % 2) + Bits;
            numb = Math.floor(numb / 2);

            if (numb <= 1) {
                Bits = numb + Bits;
                break;
            }
        }

        return Bits;
    }

    function BitsToByts(Bits) {
        let Lng = 0;
        if (Bits.length % 8 === 0) {
            Lng = Math.floor(Bits.length / 8);
        } else {
            Lng = Math.floor(Bits.length / 8) + 1;
        }

        let Byts = new Uint8Array(Lng);
        let Stage = 1;
        let i = Bits.length - 1;
        while (true) {
            if (Math.floor(((i + 1) % 8)) === 0) {
                Stage = 1;
            }
            Byts[Math.floor(i / 8)] += Stage * Bits[i];
            Stage *= 2;
            if (i === 0) {
                break;
            }
            i -= 1;
        }

        return Byts;
    }

    function BitsToNumb(Bits) {
        let numb = 0;
        let mnoj = 1;
        for (var i = Bits.length - 1; i > 0; i -= 1) {
            if (Bits[i] !== 0) {
                numb += mnoj * Bits[i];
            }
            mnoj *= 2;
        }
        return numb;
    }

    function GetBitArray(n, i) {
        var Ar = new Uint8Array(i);
        for (var index in Ar) {
            Ar[index] = index > 0 ? (n >> index * 8) & 0xFF : n & 0xFF;
        }
        return Ar;
    }

    (function () {
        var ALPHABET, ALPHABET_MAP, Base58, i;

        Base58 = (typeof module !== "undefined" && module !== null ? module.exports : void 0) || (window.Base58 = {});

        ALPHABET = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        ALPHABET_MAP = {};

        i = 0;

        while (i < ALPHABET.length) {
            ALPHABET_MAP[ALPHABET.charAt(i)] = i;
            i++;
        }

        Base58.encode = function (buffer) {
            var carry, digits, j;
            if (buffer.length === 0) {
                return "";
            }
            i = void 0;
            j = void 0;
            digits = [0];
            i = 0;
            while (i < buffer.length) {
                j = 0;
                while (j < digits.length) {
                    digits[j] <<= 8;
                    j++;
                }
                digits[0] += buffer[i];
                carry = 0;
                j = 0;
                while (j < digits.length) {
                    digits[j] += carry;
                    carry = (digits[j] / 58) | 0;
                    digits[j] %= 58;
                    ++j;
                }
                while (carry) {
                    digits.push(carry % 58);
                    carry = (carry / 58) | 0;
                }
                i++;
            }
            i = 0;
            while (buffer[i] === 0 && i < buffer.length - 1) {
                digits.push(0);
                i++;
            }
            return digits.reverse().map(function (digit) {
                return ALPHABET[digit];
            }).join("");
        };

        Base58.decode = function (string) {
            var bytes, c, carry, j;
            if (string.length === 0) {
                return new (typeof Uint8Array !== "undefined" && Uint8Array !== null ? Uint8Array : Buffer)(0);
            }
            i = void 0;
            j = void 0;
            bytes = [0];
            i = 0;
            while (i < string.length) {
                c = string[i];
                if (!(c in ALPHABET_MAP)) {
                    throw "Base58.decode received unacceptable input. Character '" + c + "' is not in the Base58 alphabet.";
                }
                j = 0;
                while (j < bytes.length) {
                    bytes[j] *= 58;
                    j++;
                }
                bytes[0] += ALPHABET_MAP[c];
                carry = 0;
                j = 0;
                while (j < bytes.length) {
                    bytes[j] += carry;
                    carry = bytes[j] >> 8;
                    bytes[j] &= 0xff;
                    ++j;
                }
                while (carry) {
                    bytes.push(carry & 0xff);
                    carry >>= 8;
                }
                i++;
            }
            i = 0;
            while (string[i] === "1" && i < string.length - 1) {
                bytes.push(0);
                i++;
            }
            return new (typeof Uint8Array !== "undefined" && Uint8Array !== null ? Uint8Array : Buffer)(bytes.reverse());
        };

    }).call(this);


    function B2S_GET32(v, i) {
        return v[i] ^ (v[i + 1] << 8) ^ (v[i + 2] << 16) ^ (v[i + 3] << 24)
    }

    function B2S_G(a, b, c, d, x, y) {
        v[a] = v[a] + v[b] + x
        v[d] = ROTR32(v[d] ^ v[a], 16)
        v[c] = v[c] + v[d]
        v[b] = ROTR32(v[b] ^ v[c], 12)
        v[a] = v[a] + v[b] + y
        v[d] = ROTR32(v[d] ^ v[a], 8)
        v[c] = v[c] + v[d]
        v[b] = ROTR32(v[b] ^ v[c], 7)
    }

    function ROTR32(x, y) {
        return (x >>> y) ^ (x << (32 - y))
    }

    var BLAKE2S_IV = new Uint32Array([
        0x6A09E667, 0xBB67AE85, 0x3C6EF372, 0xA54FF53A,
        0x510E527F, 0x9B05688C, 0x1F83D9AB, 0x5BE0CD19])

    var SIGMA = new Uint8Array([
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
        14, 10, 4, 8, 9, 15, 13, 6, 1, 12, 0, 2, 11, 7, 5, 3,
        11, 8, 12, 0, 5, 2, 15, 13, 10, 14, 3, 6, 7, 1, 9, 4,
        7, 9, 3, 1, 13, 12, 11, 14, 2, 6, 5, 10, 4, 0, 15, 8,
        9, 0, 5, 7, 2, 4, 10, 15, 14, 1, 11, 12, 6, 8, 3, 13,
        2, 12, 6, 10, 0, 11, 8, 3, 4, 13, 7, 5, 15, 14, 1, 9,
        12, 5, 1, 15, 14, 13, 4, 10, 0, 7, 6, 3, 9, 2, 8, 11,
        13, 11, 7, 14, 12, 1, 3, 9, 5, 0, 15, 4, 8, 6, 2, 10,
        6, 15, 14, 9, 11, 3, 0, 8, 12, 2, 13, 7, 1, 4, 10, 5,
        10, 2, 8, 4, 7, 6, 1, 5, 15, 11, 9, 14, 3, 12, 13, 0])

    var v = new Uint32Array(16)
    var m = new Uint32Array(16)
    function blake2sCompress(ctx, last) {
        var i = 0
        for (i = 0; i < 8; i++) {
            v[i] = ctx.h[i]
            v[i + 8] = BLAKE2S_IV[i]
        }

        v[12] ^= ctx.t
        v[13] ^= (ctx.t / 0x100000000)
        if (last) {
            v[14] = ~v[14]
        }

        for (i = 0; i < 16; i++) {
            m[i] = B2S_GET32(ctx.b, 4 * i)
        }

        for (i = 0; i < 10; i++) {

            B2S_G(0, 4, 8, 12, m[SIGMA[i * 16 + 0]], m[SIGMA[i * 16 + 1]])
            B2S_G(1, 5, 9, 13, m[SIGMA[i * 16 + 2]], m[SIGMA[i * 16 + 3]])
            B2S_G(2, 6, 10, 14, m[SIGMA[i * 16 + 4]], m[SIGMA[i * 16 + 5]])
            B2S_G(3, 7, 11, 15, m[SIGMA[i * 16 + 6]], m[SIGMA[i * 16 + 7]])
            B2S_G(0, 5, 10, 15, m[SIGMA[i * 16 + 8]], m[SIGMA[i * 16 + 9]])
            B2S_G(1, 6, 11, 12, m[SIGMA[i * 16 + 10]], m[SIGMA[i * 16 + 11]])
            B2S_G(2, 7, 8, 13, m[SIGMA[i * 16 + 12]], m[SIGMA[i * 16 + 13]])
            B2S_G(3, 4, 9, 14, m[SIGMA[i * 16 + 14]], m[SIGMA[i * 16 + 15]])
        }

        for (i = 0; i < 8; i++) {
            ctx.h[i] ^= v[i] ^ v[i + 8]
        }
    }

    function blake2sInit(outlen, key) {
        if (!(outlen > 0 && outlen <= 32)) {
            throw new Error('Incorrect output length, should be in [1, 32]')
        }
        var keylen = key ? key.length : 0
        if (key && !(keylen > 0 && keylen <= 32)) {
            throw new Error('Incorrect key length, should be in [1, 32]')
        }

        var ctx = {
            h: new Uint32Array(BLAKE2S_IV),
            b: new Uint32Array(64),
            c: 0,
            t: 0,
            outlen: outlen
        }
        ctx.h[0] ^= 0x01010000 ^ (keylen << 8) ^ outlen

        if (keylen > 0) {
            blake2sUpdate(ctx, key)
            ctx.c = 64
        }

        return ctx
    }

    function blake2sUpdate(ctx, input) {
        for (var i = 0; i < input.length; i++) {
            if (ctx.c === 64) {
                ctx.t += ctx.c
                blake2sCompress(ctx, false)
                ctx.c = 0
            }
            ctx.b[ctx.c++] = input[i]
        }
    }

    function blake2sFinal(ctx) {
        ctx.t += ctx.c
        while (ctx.c < 64) {
            ctx.b[ctx.c++] = 0
        }
        blake2sCompress(ctx, true)

        var out = new Uint8Array(ctx.outlen)
        for (var i = 0; i < ctx.outlen; i++) {
            out[i] = (ctx.h[i >> 2] >> (8 * (i & 3))) & 0xFF
        }
        return out
    }

    function blake2s(input, key, outlen) {
        outlen = outlen || 32

        var ctx = blake2sInit(outlen, key)
        blake2sUpdate(ctx, input)
        return blake2sFinal(ctx)
    }

    /* function Fee(v) {
        let s = v > 0 ? 0 : 1;
        v = Math.abs(v);
        exp = v === 0 ? 0 : Math.log10(v);
        exp = Math.floor(exp >= 0 ? exp + 0.5 : exp - 0.5);
        v /= Math.pow(10, exp);
        if (v >= 1) {
            v *= 0.1;
            ++exp;
        }
        v = Number((v * 1024).toFixed(0));
        return { exp: exp + 18, man: v === 1024 ? 1023 : v };
    } */

    function NumbToByte(numb, CountByte) {
        let InnerId = new Uint8Array(CountByte);
        numb = String(numb);
        let i = 1;
        let index = 0;
        while (true) {
            InnerId[index] += (numb % 2) * i;
            numb = Math.floor(numb / 2);
            if (numb === 0) {
                break;
            }
            if (numb === 1) {
                var b = (numb % 2) * i * 2;
                if (b === 256) {
                    ++InnerId[index + 1];
                } else {
                    InnerId[index] += (numb % 2) * i * 2;
                }
                break;
            }

            if (i === 128) {
                i = 1;
                index++;
            } else {
                i *= 2;
            }
        }
        return InnerId;
    }
}());
