param hackAPIAppPlanName string = 'nh4happs-plan'
param hackAPIAppName string = 'hackapi-${uniqueString(resourceGroup().id)}'
param appInsightName string = 'hackapi-${uniqueString(resourceGroup().id)}'
param logAnalyticsName string = 'hackapi-${uniqueString(resourceGroup().id)}'
param location string = resourceGroup().location

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
var teamsCors = 'https://hackathonteambuifee948ef.z13.web.${environment().suffixes.storage}'

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
    }
  }
}

resource appServiceLogging 'Microsoft.Web/sites/config@2020-06-01' = {
  parent: appServiceApp
  name: 'appsettings'
  properties: {
    APPINSIGHTS_INSTRUMENTATIONKEY: appInsights.properties.InstrumentationKey
    GitHubToken: gitHubToken
    ConnStr: sqlConnection
    MailChimp__Key: mailChimpApiKey
    AzureAd__ClientId: azureAdClientId
    AzureAd__Domain: azureAdDomain
    AzureAd__TenantId: azureAdTenant
    GraphAPI__ClientSecret: graphClientSecret
  }  
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightName
  location: location
  kind: 'string'  
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2020-08-01' = {
  name: logAnalyticsName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 120
    features: {
      searchVersion: 1
      legacy: 0
      enableLogAccessUsingOnlyResourcePermissions: true
    }
  }
}

output appServiceAppName string = appServiceApp.name
