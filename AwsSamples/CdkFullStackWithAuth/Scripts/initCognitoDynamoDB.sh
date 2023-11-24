#!/bin/bash
cat <<EOS
CDKのデプロイをしてから実行すること
必要に応じて--endpoint-url=http://localhost:8000を追加すること.
EOS

aws dynamodb batch-write-item \
  --request-items file://Cdk/DynamoDBData.json

cat <<EOS
CognitoUserPoolでユーザーを作る。
必要に応じて--endpoint-url=http://localhost:8000を追加すること。
EOS

# aws cognito-idp list-user-pools --max-results 1 | jq ".UserPools[] | {Id, Name}"
# aws cognito-idp list-user-pools --max-results 1 --query 'UserPools[]' --output text
# shellcheck disable=SC2155
export CognitoId=$(aws cognito-idp list-user-pools --max-results 1 --query 'UserPools[].Id' --output text)
# shellcheck disable=SC2155
export CognitoUserPoolName=$(aws cognito-idp list-user-pools --max-results 1 --query 'UserPools[].Name' --output text)

cat <<EOS
ユーザー作成
EOS

export UserName="phasetr@gmail.com"
aws cognito-idp admin-create-user \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --user-attributes Name=email,Value="${UserName}" Name=email_verified,Value=true \
  --message-action SUPPRESS

cat <<EOS
パスワード設定
EOS

aws cognito-idp admin-set-user-password \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --password 'P@ssw0rd' \
  --permanent

cat <<EOS
ユーザの情報確認
EOS

aws cognito-idp admin-get-user \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --query 'UserAttributes[?Name==`email`].Value' --output text

cat <<EOS
グループの追加
EOS

aws cognito-idp create-group \
  --user-pool-id ${CognitoId} \
  --group-name "USERPOOLGROUP#read-only"
aws cognito-idp create-group \
  --user-pool-id ${CognitoId} \
  --group-name "USERPOOLGROUP#read-update-add"

cat <<EOS
USERPOOLGROUP#read-onlyのユーザーグループに新規ユーザーを追加する。
EOS

aws cognito-idp admin-add-user-to-group \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --group-name "USERPOOLGROUP#read-only"

cat <<EOS
グループ確認
EOS

aws cognito-idp list-users-in-group \
  --user-pool-id ${CognitoId} \
  --group-name "USERPOOLGROUP#read-only" \
  --query 'Users[].UserStatus' --output text

cat <<EOS
ユーザーを追加し、ユーザーグループUSERPOOLGROUP#read-update-addを追加する。
EOS

export UserName="phasetr+admin@gmail.com"
aws cognito-idp admin-create-user \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --user-attributes Name=email,Value="${UserName}" Name=email_verified,Value=true \
  --message-action SUPPRESS

cat <<EOS
ユーザーへのパスワード設定
EOS

aws cognito-idp admin-set-user-password \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --password 'P@ssw0rd' \
  --permanent

cat <<EOS
ユーザの情報確認
EOS

aws cognito-idp admin-get-user \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --query 'UserAttributes[?Name==`email`].Value' --output text

aws cognito-idp admin-add-user-to-group \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --group-name "USERPOOLGROUP#read-update-add"

cat <<EOS
グループ確認
EOS

aws cognito-idp list-users-in-group \
  --user-pool-id ${CognitoId} \
  --group-name "USERPOOLGROUP#read-update-add" \
  --query 'Users[].UserStatus' --output text

cat <<EOS
次のコマンドでCognitoHostedUIUrlを確認して、
CDKで作ったCognitoHostedUIUrlのCognitoアプリクライアントにアクセスする。
EOS

aws cloudformation describe-stacks \
  --stack-name cdk-fullstack-with-auth-stack-dev \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthcognitohosteduiurldev`].OutputValue' --output text
