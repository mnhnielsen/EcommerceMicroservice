#Creating api's
dotnet new webapi -o "sdu.bachelor.microservice.{project}"

#Creating class libraries
dotnet new classlib -o {lib}

#Adding to solution
dotnet sln add sample.microservice.{project}