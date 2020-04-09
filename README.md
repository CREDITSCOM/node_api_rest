# Credits public REST API

## How to get source code.
- clone repository and enter repository root folder
- update submodule(s) by executing in repo root directory: <code>git submodule update --init --recursive</code>
## How to build and run in Windows
- enter the repo subdirectory WebApiNode\third-party\node_api\thrift-utils and execute batch file <code>thrift_gen_node.bat</code>
- return to WebApiNode directory and open WebApiNode.sln in visual Studio
- build in Visual Studio
- publish in Visual Studio
- run from command prompt or intergate into IIS
## How to build and run in Ubuntu 18.04
- to prepare dotnet core 3.1: runtime + SDK + asp runtime, run
	<code>
	<p/>sudo add-apt-repository universe
	<p/>sudo apt-get update
	<p/>sudo apt-get install apt-transport-https
	<p/>sudo apt-get update
	<p/>sudo apt-get install dotnet-runtime-3.1
	<p/>sudo apt-get install aspnetcore-runtime-3.1
	<p/>sudo apt-get install dotnet-sdk-3.1
	</code>
	<p/>if you receive an error message similar to Unable to locate package aspnetcore-runtime-3.1, see the [Troubleshoot](https://docs.microsoft.com/en-us/dotnet/core/install/linux-package-manager-ubuntu-1804#troubleshoot-the-package-manager) the package manager section.
- enter the subdirectory WebApiNode/thirt-party/node_api/thrift-utils/
- generate node native API support files running <p/><code>sudo ./thrift_gen_node.sh</code>
- return to subdirectory WebApiNode/
- publish application by <p/><code>dotnet publish -c Release -r ubuntu.18.04-x64</code>
- copy application files from WebApiNode/CS.WebApi/bin/Release/netcoreapp3.1 wherever you want
- enter to application direcory and run (you may set desired port instead of 8080) <p/><code>./CS.WebApi --urls="http://0.0.0.0:8080"</code>