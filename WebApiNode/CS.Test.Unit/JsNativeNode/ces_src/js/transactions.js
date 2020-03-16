chrome.storage.local.get(["Transactions"],(r) => {
	r = JSON.parse(r.Transactions);
	Url = r.net.Url;
	Port = r.net.Port;
	$("#website").val(r.site);
	$("#target").val(r.trans.target);
	$("#amount").val(r.trans.amount);
	if(r.trans.amount == undefined)
	{
		$("#amount").val("0.0");
	}
	else
	{
		$("#amount").val(r.trans.amount);
	}
	if(r.trans.fee == undefined)
	{
		$("#fee").val("0.9");
	}
	else
	{
		$("#fee").val(r.trans.fee);
	}
	chrome.storage.local.set({Transactions:null});
	
	$("#send").on("click",() => {
		let time = $("#time").val();
		if(time == "One")
		{
			chrome.storage.local.set({TransactionResult:{val:true,id:r.id}});
			chrome.tabs.getCurrent(r => {
				chrome.tabs.remove(r.id);
			});
		}
		else
		{
			chrome.storage.local.get(["CS_Time"],(c) => {
				if(c.CS_Time == undefined) c.CS_Time = {};
				let t = new Date();
				switch(time)
				{
					case "Two":
						c.CS_Time[r.site] = new Date(t.setMinutes(t.getMinutes()+10)).getTime();
					break;
					case "Three":
						c.CS_Time[r.site] = new Date(t.setMinutes(t.getMinutes()+30)).getTime();
					break;
				}
				chrome.storage.local.set({
					CS_Time:c.CS_Time,
					TransactionResult:{val:true,id:r.id}
				});
				chrome.tabs.getCurrent(r => {
					chrome.tabs.remove(r.id);
				});
			});
		}
		
	})
	$("#cancel").on("click",() => {
		chrome.storage.local.set({TransactionResult:{val:false,id:r.id}});
		chrome.tabs.getCurrent(r => {
			chrome.tabs.remove(r.id);
		});
	});
});