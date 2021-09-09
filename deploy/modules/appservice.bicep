param hackAPIAppPlanName string = 'nh4happs-plan'
param hackAPIAppName string = 'hackapi-${uniqueString(resourceGroup().id)}'
param location string = resourceGroup().location
param teamsCors string

@allowed([
  'nonprod'
  'prod'
])
param environmentType string

// App Settings
param azureAdClientId string = '05acec15-d6fb-4dae-a9b3-5886a7709df9'
param azureAdDomain string = 'nh4h.onmicrosoft.com'
param azureAdTenant string = 'common'
@secure()
param graphClientSecret string
@secure()
param gitHubToken string
@secure()
param sqlConnection string
@secure()
param mailChimpApiKey string

var appServicePlanSkuName = (environmentType == 'prod') ? 'P2_v2' : 'B1'
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
      linuxFxVersion: 'DOTNETCORE|3.1'
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
          teamsCors
        ]
      }
      appSettings: [
        {
          name: 'GitHubToken'
          value: gitHubToken
        }
        {
          name: 'ConnStr'
          value: sqlConnection
        }  
        {
          name: 'MailChimp__Key'
          value: mailChimpApiKey
        }              
        {
          name: 'AzureAd__ClientId'
          value: azureAdClientId
        }
        {
          name: 'AzureAd__Domain'
          value: azureAdDomain
        }        
        {
          name: 'AzureAd__TenantId'
          value: azureAdTenant
        }
        {
          name: 'GraphAPI__ClientSecret'
          value: graphClientSecret
        }
        
      ]
      
    }
  }
}

output appServiceAppName string = appServiceApp.name
