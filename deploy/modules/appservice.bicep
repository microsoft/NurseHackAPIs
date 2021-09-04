param hackAPIAppPlanName string = 'nh4happs-plan'
param hackAPIAppName string = 'hackapi-${uniqueString(resourceGroup().id)}'
param location string = resourceGroup().location
@allowed([
  'nonprod'
  'prod'
])
param environmentType string

var appServicePlanSkuName = (environmentType == 'prod') ? 'P2_v2' : 'B2'
var appServicePlanTierName = (environmentType == 'prod') ? 'PremiumV2' : 'Basic'

resource appServicePlan 'Microsoft.Web/serverfarms@2021-01-15' = {
  name: hackAPIAppPlanName
  location: location
  sku: {
    name: appServicePlanSkuName
    tier: appServicePlanTierName
  }
  kind: 'Linux'
  properties: {
    reserved: true
  }
}

resource appServiceApp 'Microsoft.Web/sites@2021-01-15' = {
  name: hackAPIAppName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      cors: {
        supportCredentials: true
        allowedOrigins: [
          'https://nursehack4health.org'
          'https://apps.nursehack4health.org'
          'http://localhost'
          'http://localhost:3000'
          'http://localhost:3001'
          'https://nursehack4health.github.io'
          'https://teams.microsoft.com'
          'https://localhost:3000'
          'https://hackathonteambuife037d8e.z13.web.core.windows.net'
        ]
      }
    }
  }
}

output appServiceAppHostName string = appServiceApp.properties.defaultHostName
output appServiceAppName string = appServiceApp.name
