# Readme


### Windows

Crie a migration inicial: No terminal, navegue até o diretório do projeto Infrastructure e execute o comando para criar a migration:
```
dotnet ef migrations add InitialCreate --project .\Infrastructure.csproj  --startup-project ..\Api\Api.csproj
```

Aplique a migration para criar o banco: Após criar a migration, aplique-a para gerar o banco de dados:
```
dotnet ef database update --project .\Infrastructure.csproj --startup-project ..\Api\Api.csproj
```


## Linuxs

```
 dotnet ef migrations add InitialCreate --project Infrastructure.csproj  --startup-project ../Api/Api.csproj


dotnet ef migrations list --project Infrastructure.csproj --startup-project ../Api/Api.csproj
dotnet ef migrations remove 20250327225921_ProductRating --project Infrastructure.csproj  --startup-project ../Api/Api.csproj
```

```
dotnet ef database update --project Infrastructure.csproj --startup-project ../Api/Api.csproj


dotnet ef database update 20250327225921_ProductRating --project Infrastructure.csproj --startup-project ../Api/Api.csproj
```
