# Introdução 
Este é um teste técnico feito para a ELAW por João Victor Cerqueira, utilizando .NET 8.

Os testes unitários podem ser rodados com o comando `dotnet test`.

Para rodar o projeto, é necessário clonar e abrir o projeto no Visual Studio Code e criar um arquivo `launch.json` com a configuração abaixo. Em seguida, vá na aba 'Run and Debug' e rode a aplicação 'Web API Launch'. Será aberta uma aba com o endereço localhost, e a aplicação poderá ser testada pelo Swagger (ex: https://localhost:7187/swagger/index.html):

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Web API Launch",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/WebApi/bin/Debug/net8.0/WebApi.dll",
      "args": [],
      "cwd": "${workspaceFolder}/WebApi",
      "stopAtEntry": false,
      "serverReadyAction": {
        "action": "openExternally",
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)",
        "uriFormat": "%s/swagger"
      },
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "sourceFileMap": {
        "/Views": "${workspaceFolder}/Views"
      }
    },
    {
      "name": ".NET Core Attach",
      "type": "coreclr",
      "request": "attach"
    }
  ]
}
