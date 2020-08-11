#Project Variables
PROJECT_NAME ?=SolarCoffe
ORG_NAME ?=SolarCoffe
REPO_NAME ?=SolarCoffe

.PHONY: migrations db

migrations:
	cd ./SolarCoffe.Data && dotnet ef --startup-project ../SolarCoffe.Web/ migrations add $(mname) && cd..
    
db:
	cd ./SolarCoffe.Data && dotnet ef --startup-project ../SolarCoffe.Web/ database update && cd..
