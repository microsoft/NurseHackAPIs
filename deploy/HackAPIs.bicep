param sqlDatabaseName string = 'hackDB'
param sqlServerName string = '${uniqueString(resourceGroup().id)}-hackDB-server'
param location string = resourceGroup().location
param sqlAdminUserName string = 'hackdb-admin'
param hackAPIAppPlanName string = 'nh4happs-plan'
param hackAPIAppName string = 'hackapi-${uniqueString(resourceGroup().id)}'
param hackKeyVaultName string = 'NH4HKV'
param hackKeyVaultResourceGroup string = 'rg-NH4H'

@secure()
param sqlAdminPassword string

@allowed([
  'nonprod'
  'prod'
])
param environmentType string

resource appConfigSecrets 'Microsoft.KeyVault/vaults@2021-06-01-preview' existing = {
  name: hackKeyVaultName
  scope: resourceGroup(hackKeyVaultResourceGroup)
}

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
    gitHubToken: appConfigSecrets.getSecret('GitHubToken')
    sqlConnection: 'Server=tcp:${sqlDatabase.outputs.sqlServerFQDN},1433;Initial Catalog=${sqlDatabase.outputs.sqlDbName};Persist Security Info=False;User ID=${sqlAdminUserName};Password=${sqlAdminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
    graphClientSecret: appConfigSecrets.getSecret('GraphClientSecret')
    mailChimpApiKey: appConfigSecrets.getSecret('MailChimpKey')
  }
}

output appServiceAppName string = appService.outputs.appServiceAppName
output sqlServerFQDN string = sqlDatabase.outputs.sqlServerFQDN
output sqlServerName string =sqlDatabase.outputs.sqlServerName
output sqlDbName string = sqlDatabase.outputs.sqlDbName
output sqlUserName string = sqlDatabase.outputs.sqlUserName
