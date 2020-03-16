chrome.runtime.onMessage.addListener( (m, s, c) => {
	if(m == "Api")
	{
		chrome.tabs.executeScript(null,{
		  file: `js/i.js`
		});
	}else if(m === "CS_Extension_AddWebSite_Confirm"){
		chrome.storage.local.get(["CS_ApproveWebSite"], function(d) {
			if(d.CS_ApproveWebSite === undefined)
			{
				d.CS_ApproveWebSite = [];
			}
			d.CS_ApproveWebSite.push(s.url);
			chrome.storage.local.set({CS_ApproveWebSite:d.CS_ApproveWebSite});
		});
		c(true);
	}
});

let RequestCount  = 0;

chrome.runtime.onConnect.addListener((p) => {
	p.onMessage.addListener(M => {
		let Res = {
			message: undefined,
			result: undefined,
			requestId: M.requestId
		};
		try
		{
			switch(M.data.method)
			{
				case "CS_Extension_Balance":
					chrome.storage.local.get(['CS_PublicKey',"CS_NET"], function(d) {
						if(d.CS_NET === undefined) throw new Error("Network is not set");
						Url = d.CS_NET.Url;
						Port = d.CS_NET.Port;
						let Key;
						if(M.data.Key === undefined){
							Key = d.CS_PublicKey;
						}
						else
						{
							Key = M.data.Key;
						}
						
						try	
						{
							Key = Base58.decode(Key);
						}
						catch (e)
						{
							throw new Error("Public Key invalid");
						}
						
						SignCS.Connect().WalletBalanceGet(
							Key,
							function(r){
								if(r.status.code > 0 && r.status.message != "Not found")
								{
									throw new Error(r.status.message);
								}
								else
								{
									Res.result = {Credits_CS:r.balance.integral + r.balance.fraction * Math.pow(10,-18)};
								}
								p.postMessage(Res);
							}
						);
					});
				break;
				case "CS_Extension_Transaction":
					if(typeof M.data.Trans !== "object") throw new Error("Invalid transaction data");
					
					let val = M.data.Trans;  
					chrome.storage.local.get(
						[
							'CS_PrivateKey',
							'CS_PublicKey',
							'CS_NET',
							'CS_Time'
						], 
						function(d) 
						{
							if(d.CS_PublicKey === undefined) throw new Error("User is not authorized");
							
							if(d.CS_Time == undefined) d.CS_Time = {};
							Url = d.CS_NET.Url;
							Port = d.CS_NET.Port;
							val.source = d.CS_PublicKey;
							val.privateKey = d.CS_PrivateKey;
							if((val.smartContract != undefined 
							&& val.smartContract.newState) 
							|| new Date(d.CS_Time[p.sender.url]).getTime() > new Date().getTime())
							{
								SignCS.Connect().TransactionFlow(SignCS.CreateTransaction(val),function(r)
								{
									if(r.status.code > 0)
									{
										Res.message = r.status.message;
									}
									else
									{
										Res.result = r;
									}
									p.postMessage(Res);
								});
							}
							else
							{
								if(RequestCount >= 2) throw new Error("Exceeded the number of one-time requests");
								
								++RequestCount;
								
								chrome.storage.local.set({
									Transactions:JSON.stringify({
										net:d.CS_NET,
										trans: val,
										site:p.sender.url,
										id:Res.requestId
									})
								});
								chrome.tabs.create(
									{url: chrome.extension.getURL('Transaction.html')}, 
									function(tab){
										let v = val;
										let index = 0;
										let ChecRes = s => {
											if(index == 120) 
											{
												--RequestCount;
												throw new Error("Timeout");
											}
											++index;
											chrome.storage.local.get(
												["TransactionResult"], 
												(d) => {
													if(d.TransactionResult == null || d.TransactionResult == undefined)
													{
														setTimeout(() => {ChecRes(s)},500);
													}
													else
													{
														if(d.TransactionResult.id == s)
														{
															if(d.TransactionResult.val)
															{
																try
																{
																	SignCS.Connect().TransactionFlow(SignCS.CreateTransaction(v),function(r)
																	{
																		if(r.status.code > 0)
																		{
																			Res.message = r.status.message;
																		}
																		else
																		{
																			Res.result = r;
																		}
																		chrome.storage.local.set({TransactionResult:null});
																		--RequestCount;
																		p.postMessage(Res);
																	});
																}
																catch(ex)
																{
																	chrome.storage.local.set({TransactionResult:null});
																	Res.message = ex.message;
																	--RequestCount;
																	p.postMessage(Res);
																}
															}
															else
															{
																chrome.storage.local.set({TransactionResult:null});
																Res.message = "User didn't confirm transaction";
																--RequestCount;
																p.postMessage(Res);
															}
														}
														else
														{
															setTimeout(() => {ChecRes(s)},500);
														}
													}
												}
											);
												
										}
										ChecRes(Res.requestId);
									}
								);
							}
						}
					);
				break;
				case "CS_Extension_History":
					if(typeof M.data.data.Data !== "object") throw new Error("Data is not be empty");
					
					if(isNaN(Number(M.data.Data.Page))) throw new Error("Page must be a number");
					if(isNaN(Number(M.data.Data.Size))) throw new Error("Size must be a number");
					
					let Key = ["CS_NET"];
					
					if(M.data.Data.Key === undefined) Key.push('CS_PublicKey');
					
					chrome.storage.local.get(Key, function(d) {
						if(d.CS_NET === undefined) throw new Error("Network is not set");
						
						Url = d.CS_NET.Url;
						Port = d.CS_NET.Port;
						
						let PubKey;
						if(M.data.Data.Key === undefined)
						{
							PubKey = d.CS_PublicKey;
						}else{
							PubKey = M.data.Data.Key;
						}
						
						try	
						{
							PubKey = Base58.decode(PubKey);
						}
						catch (e)
						{
							throw new Error("Public Key invalid");
						}
						
						SignCS.Connect().TransactionsGet(PubKey,M.data.Data.Page * M.data.Data.Size - M.data.Data.Size,M.data.Data.Size,r => {
							if(r.status.code > 0)
							{
								Res.message = r.status.message;
							}
							else
							{
								Res.result = [];
								for(let i in r.transactions)
								{
									let val = r.transactions[i];
									Res.result.push({
										id: `${val.id.poolSeq}.${val.id.index + 1}`,
										amount: val.trxn.amount.integral + val.trxn.amount.fraction * Math.pow(10,-18),
										fee: SignCS.FeeToNumber(val.trxn.fee.commission),
										source: Base58.encode(StrToByte(val.trxn.source)),
										target: Base58.encode(StrToByte(val.trxn.target)),
										smartContract: val.trxn.smartContract,
										smartInfo: val.trxn.smartInfo,
										datetime: val.trxn.timeCreation,
										userData: val.trxn.userFields
									});
								}
							}
							p.postMessage(Res);
						});
					});
				break;
				case "CS_Extension_Authorization":
					chrome.storage.local.get(['CS_PublicKey'], function(d) {
						if(d.CS_PublicKey === undefined)
						{
							Res.result = false;
						}else{
							Res.result = true;
						}
						p.postMessage(Res);
					});
				break;
				case "CS_Extension_compiledSmartContractCode":
					if(M.data.code == "" || M.data.code == undefined) throw new Error("The code must not be empty");
					
					chrome.storage.local.get(["CS_NET"], d => {
						if(d.CS_NET === undefined) throw new Error("Network is not set");
						
						Url = d.CS_NET.Url;
						Port = d.CS_NET.Port;
						
						SignCS.Connect().SmartContractCompile(M.data.code,r => {
							if(r.status.code > 0)
							{
								Res.message = r.status.message;
							}
							else
							{
								Res.result = r.byteCodeObjects;
							}
							p.postMessage(Res);
						});
					});
				break;
				case "CS_CurNet":
					chrome.storage.local.get(["CS_NET"], d => {
						if(d.CS_NET === undefined)
						{
							Res.message = "User is not authorized";
						}
						else
						{
							Res.result = d.CS_NET.Mon;
						}
						p.postMessage(Res);
					});
				break;
				case "CS_User":
					chrome.storage.local.get(["CS_PublicKey"], d => {
						if(d.CS_PublicKey === undefined)
						{
							Res.message = "User is not authorized";
						}
						else
						{
							Res.result = d.CS_PublicKey;
						}
						p.postMessage(Res);
					});
				break;
			}
		}
		catch(ex)
		{
			Res.message = ex.message;
			p.postMessage(Res);
		}
	});
});

function randStr(length)
{
	let r = '';
	let cs = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz';
	for (let i = 0; i < length; i++ ) {
		r += cs.charAt(Math.floor(Math.random() * cs.length));
	}
	return r;
}
	
function ByteToHex(Byte)
{
	let ArHex = "0123456789abcdef";
	let Hex = "";
	for (let j = 0; j < Byte.length; j++) {
		Hex += ArHex[Math.floor(Byte[j] / 16)];
		Hex += ArHex[Math.floor(Byte[j] % 16)];
	}
	return Hex;
}

function StrToByte(Str)
{
	let B = new Uint8Array(Str.length);
	for (let i in Str) {
		B[i] = Str[i].charCodeAt();
	}
	return B;
}