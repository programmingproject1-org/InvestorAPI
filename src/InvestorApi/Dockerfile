FROM microsoft/aspnetcore:2.0
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet InvestorApi.dll