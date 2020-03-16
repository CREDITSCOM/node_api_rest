console.log("Credits Api");

(function () {
	let randStr = length => 
	{
		let r = '';
		let cs = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz';
		for (let i = 0; i < length; i++ ) {
			r += cs.charAt(Math.floor(Math.random() * cs.length));
		}
		return r;
	}
	
	window.CS_Extension_Id = document.getElementById("CS_Extension_Id").value;
	window.CreditsExtension = 
	{
			balanceGet: function(Obj)
			{
				console.log("balanceGet");
				
				switch(typeof Obj)
				{
					case "undefined":
						Obj = {};
					break;
					case "string":
						Obj = {Key:Obj};
					break;
				}
				
				if(typeof Obj === "object")
				{
					Obj.method = "CS_Extension_Balance";
					return SendMess(Obj);
				}
				else
				{
					throw new Error("Obj is not an object");
				}
			},
			sendTransaction: Transaction =>
			{
				console.log("sendTransaction");
				if(typeof Transaction === "object")
				{
					return SendMess({method:"CS_Extension_Transaction",Trans:Transaction});
				}
				else
				{
					throw new Error("Transaction is not valid");
				}
			},
			getHistory: Obj =>
			{
				console.log("getHistory");
				if(typeof Obj === "object")
				{
					return SendMess({method:"CS_Extension_History",Data:Obj});
				}
				else
				{
					throw new Error("Obj is not an object");
				}
			},
			authorization: () =>
			{
				console.log("authorization");
				return SendMess({method:"CS_Extension_Authorization"});
			},
			addWebSite: () =>
			{
				console.log("CS_Extension_AddWebsite");
				return SendMess({method:"CS_Extension_AddWebsite"});
			},
			compiledSmartContractCode: Code =>
			{
				console.log("compiledSmartContractCode");
				return SendMess({method:"CS_Extension_compiledSmartContractCode",code:Code});
			},
			curNet: () => 
			{
				console.log("CurNet");
				return SendMess({method:"CS_CurNet"});
			},
			user: () =>
			{
				console.log("User");
				return SendMess({method:"CS_User"});
			}
	}
	
	let SendMess = (m) => {
		let d = {
			data: m,
			requestId: randStr(6)
		};
		window.dispatchEvent(new CustomEvent("CS_extension_event_request",{
			detail: d,
			bubbles: true
		}));
		
		return new Promise((res,rej) => {
			window.addEventListener("CS_extension_event_response",event => {
				if(event.detail.CS_Res !== undefined && d.requestId === event.detail.requestId)
				{
					delete event.detail.CS_Res.requestId;
					if(event.detail.CS_Res.result == undefined)
					{
						rej(event.detail.CS_Res.message);
					}
					else
					{
						res(event.detail.CS_Res.result);
					}
					event.stopPropagation();
				}
			},true);
		});
	};
}());