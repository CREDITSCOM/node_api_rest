import requests
import json

#url = 'http://192.168.0.27:8888/api/Monitor/GetWalletData/GetWalletData'
#url = 'http://apinode.credits.com/api/Monitor/GetWalletData/GetWalletData'
#url =  'http://localhost:60476/api/Monitor/GetWalletData'
url =  'http://localhost:60476/api/Transaction/Execute'

headers = {
    'Content-type': 'application/json'
    , 'Accept': 'application/json'
    , 'Content-Encoding': 'utf-8'
    }

data = {
    "authKey": "87cbdd85-b2e0-4cb9-aebf-1fe87bf3afdd"
    #, "MethodApi" : "GetWalletData222"
    , "PublicKey":"5B3YXqDTcWQFGAqEJQJP3Bg1ZK8FFtHtgCiFLT5VAxpe"
    #, "NetworkAlias":"TestNet"
    , "networkIp":"165.22.242.197"	# do-lon4 testnet
    , "networkPort":"9090"
    }

answer = requests.post(url, data=json.dumps(data), headers=headers)

print(answer)
if answer.status_code == 200:
    response = answer.json()
    print(response)
