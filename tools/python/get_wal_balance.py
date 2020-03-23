import requests
import json

#url = 'http://apinode.credits.com/api/Monitor/GetWalletData/Get'
#url =  'http://localhost:60476/api/Monitor/GetBalance'
url =  'http://169.63.5.243:5006/api/Monitor/GetBalance'

headers = {
    'Content-type': 'application/json'
    , 'Accept': 'application/json'
    , 'Content-Encoding': 'utf-8'
    }

data = {
    "authKey": "87cbdd85-b2e0-4cb9-aebf-1fe87bf3afdd"
    , "NetworkAlias":"MainNet"
    #, "networkIp":"167.71.234.248"	# do19 mainnet
    #, "networkIp":"165.22.242.197"	# do-lon4 testnet
    #, "networkPort":"9070"
	, "PublicKey": "FeFjpcsfHErXPk5HkfVcwH6zYaRT2xNytDTeSjfuVywt"
    }

answer = requests.post(url, data=json.dumps(data), headers=headers)

print(answer)
if answer.status_code == 200:
    response = answer.json()
    print(response)
    text_file = open("wal_bal.json", "w")
    print(response, file=text_file, flush=True)
    #n = text_file.write(response)
    text_file.close()