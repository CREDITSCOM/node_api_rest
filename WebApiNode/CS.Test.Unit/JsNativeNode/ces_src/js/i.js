let s = document.createElement('script');
s.src = chrome.runtime.getURL('js/Api.js');
s.onerror = function(){console.log("Onerror")}
document.head.appendChild(s,document.head.firstChild);

/*s = document.createElement('script');
s.src = chrome.runtime.getURL('js/CreditsWork.js');
s.onerror = function(){console.log("Onerror")}
document.head.appendChild(s,document.head.firstChild);

s = document.createElement('script');
s.src = chrome.runtime.getURL('js/Extension.js');
s.onerror = function(){console.log("Onerror")}
document.head.appendChild(s,document.head.firstChild);
*/
let i = document.createElement("input");
i.type = "hidden";
i.value = chrome.runtime.id;
i.id = "CS_Extension_Id";

document.body.insertBefore(i,document.getElementsByTagName("body")[0].firstChild);

let RandStr = l => {
    let r = "";
    let p = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
    for (let i = 0; i < l; i++)
      r += p.charAt(Math.floor(Math.random() * p.length));
    return r;
}

let port = chrome.runtime.connect({name:RandStr(8)});

window.addEventListener('CS_extension_event_request', e => {
	port.postMessage(e.detail);
	port.onMessage.addListener(m => {
		window.dispatchEvent(new CustomEvent("CS_extension_event_response",{
			detail: {CS_Res:m,requestId:m.requestId},
			bubbles: true
		}));
	});
});

