param sqlDatabaseName string = 'hackDB'
param sqlServerName string = '${uniqueString(resourceGroup().id)}-hackDB-server'
param location string = resourceGroup().location
param sqlAdminUserName string = 'hackdb-admin'
param hackAPIAppPlanName string = 'nh4happs-plan'
param hackAPIAppName string = 'hackapi-${uniqueString(resourceGroup().id)}'

@secure()
param sqlAdminPassword string

@allowed([
  'nonprod'
  'prod'
])
param environmentType string

module sqlDatabase 'modules/sql_database.bicep' = {
  name: 'hackAPI-database'
  params: {
    environmentType: environmentType
    location: location
    sqlAdminUserName: sqlAdminUserName
    sqlAdminPassword: sqlAdminPassword
    sqlDatabaseName: sqlDatabaseName
    sqlServerName: sqlServerName
  }
}

module appService 'modules/appservice.bicep' = {
  name: 'hackAPI-appservice'
  params: {
    environmentType: environmentType
    location: location
    hackAPIAppName: hackAPIAppName
    hackAPIAppPlanName: hackAPIAppPlanName
  }
}

output appServiceAppName string = appService.outputs.appServiceAppName
output sqlServerFQDN string = sqlDatabase.outputs.sqlServerFQDN
output sqlServerName string =sqlDatabase.outputs.sqlServerName
output sqlDbName string = sqlDatabase.outputs.sqlDbName
