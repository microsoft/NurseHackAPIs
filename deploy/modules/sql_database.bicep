param sqlDatabaseName string = 'hackDB'
param sqlServerName string = '${uniqueString(resourceGroup().id)}-hackDB-server'
param location string = resourceGroup().location
param sqlAdminUserName string = 'hackdb-admin'
@secure()
param sqlAdminPassword string

@allowed([
  'nonprod'
  'prod'
])
param environmentType string

var databaseSkuName = (environmentType == 'prod') ? 'S1' : 'Basic'
var databaseTierName = (environmentType == 'prod') ? 'Standard' : 'Basic'

resource hackDBServer 'Microsoft.Sql/servers@2021-02-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdminUserName
    administratorLoginPassword:sqlAdminPassword
    version: '12.0'
    publicNetworkAccess: 'Enabled'
    restrictOutboundNetworkAccess: 'Disabled'    
  }
  resource allowAzure 'firewallRules' = {
    name: 'AllowAllWindowsAzureIps'
    properties: {
      startIpAddress: '0.0.0.0'
      endIpAddress: '0.0.0.0'
    }
  }
}

resource hackSQLDatabase 'Microsoft.Sql/servers/databases@2021-02-01-preview' = {
  parent: hackDBServer
  name: sqlDatabaseName
  location: location
  sku: {
    name: databaseSkuName
    tier: databaseTierName
    capacity: 5
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 2147483648
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    zoneRedundant: false
    readScale: 'Disabled'
    requestedBackupStorageRedundancy: 'Local'
    isLedgerOn: false
  }
}
