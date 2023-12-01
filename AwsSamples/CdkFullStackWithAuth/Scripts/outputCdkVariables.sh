#!/bin/bash
Prefix="cdk-fullstack-with-auth"
envName="dev"
stackName="${Prefix}-stack-${envName}"

export ApiGwUrl=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthapigwurldev`].OutputValue' \
  --output text)
echo APIGateway URL: ${ApiGwUrl}

export CognitoHostedUiUrlRoot=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthcognitohosteduiurlroot`].OutputValue' \
  --output text)
echo Cognito Hosted UI URL: ${CognitoHostedUiUrlRoot}

export CognitoAppClientIdDev=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthcognitoappclientiddev`].OutputValue' \
  --output text)
echo Cognito App Client ID: ${CognitoAppClientIdDev}

export CloudFrontDomainName=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthcloudfrontdomainnamedev`].OutputValue' \
  --output text)
echo CloudFront Domain Name: ${CloudFrontDomainName}

export CognitoUserPoolId=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthcognitouserpooliddev`].OutputValue' \
  --output text)
echo Cognito User Pool ID: ${CognitoUserPoolId}

export CognitoUserPoolDomain=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthcognitouserpooldomaindev`].OutputValue' \
  --output text)
echo Cognito User Pool Domain: ${CognitoUserPoolDomain}

echo ValidIssuer: https://cognito-idp.ap-northeast-1.amazonaws.com/${CognitoUserPoolId}
echo JWKsURI: https://cognito-idp.ap-northeast-1.amazonaws.com/${CognitoUserPoolId}/.well-known/jwks.json
