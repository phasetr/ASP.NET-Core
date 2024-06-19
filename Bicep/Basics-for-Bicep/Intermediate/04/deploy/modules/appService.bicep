@description('The Azure region into which the resources should be deployed.')
param location string

@description('The name of the App Service app to deploy. This name must be globally unique.')
param appServiceAppName string

@description('The name of the storage account to deploy. This name must be globally unique.')
param storageAccountName string

@description('The name of the queue to deploy for processing orders.')
param processOrderQueueName string

@description('The type of the environment. This must be nonprod or prod.')
@allowed([
  'nonprod'
  'prod'
])
param environmentType string
