# README

- `EC2`のサーバーにごく簡単な`API`を立てる
- `EventBridge`+`Lambda`で`EC2`を平日の朝に自動起動, 夕方に自動停止する機構を追加している
- `TODO`: `API Gateway`+`Lambda`で`API`キーを設定してセキュアにする.
  構成イメージは[AWS API Gateway+EC2でセキュアなREST APIを構築する](https://qiita.com/masatojames/items/ecc8ee215b502c5eb6a8)などを参照

## 参考リンク

### `EC2`で`API`サーバーを立てる

- [Bootstrapping an Amazon EC2 Instance Using User-data to Run a Python Web App](https://community.aws/tutorials/using-ec2-userdata-to-bootstrap-python-web-app)
- [AWS EC2とAPI gatewayによるAPIサーバをterraformで実装する](https://zenn.dev/kitchy/articles/8bb46ab186f9c3)
- [AWS API Gateway+EC2でセキュアなREST APIを構築する](https://qiita.com/masatojames/items/ecc8ee215b502c5eb6a8)
- [AWS CDK で Infrastructure as Code する: EC2編](https://qiita.com/masatomix/items/03dd59929ef1869ea027)

### `EC2`の自動起動・停止

- [できるだけシンプルな仕組みで簡単にEC2の自動起動・停止を実現したい！](https://dev.classmethod.jp/articles/simple-auto-start-stop-for-ec2/)

### `EC2`との`SCP`通信

- [SCPコマンドで、EC2インスタンスにあるファイルのダウンロード/アップロードを秒で対応するために](https://qiita.com/Takao_/items/902ce0b12bb8fe74069e)

### `API Gateway`で`API`キーを設定する

- `TODO`: [AWS API Gateway+EC2でセキュアなREST APIを構築する](https://qiita.com/masatojames/items/ecc8ee215b502c5eb6a8)

## `EC2`サーバーと`API`の基本設定

- `CDK`で`EC2`に`8080`を開けてそこに`Web API`を立てている
- `Web API`の最小サンプルは`Flask`製で, `FlaskSample`ディレクトリに置いてある
- `EC2`への`SSH`ログイン用の鍵は下記手順で生成して、それを使う想定

## 環境構築

### ローカル作業

- 必要に応じて`CDK`と`AWS CLI`をインストールする
- `cdk deploy`する
- 生成した秘密鍵をファイルに出力する

```shell
keyName=cdk-ec2-api-server-dev-key \
  && keyPairId=$(aws ec2 describe-key-pairs --key-names ${keyName} --query 'KeyPairs[*].[KeyPairId]' --output text) \
  && aws ssm get-parameter --name /ec2/keypair/${keyPairId} --with-decryption --query Parameter.Value --output text > ${keyName}.pem \
  && chmod 400 ${keyName}.pem
```

- EC2インスタンスにキーペアが関連づけられているか確認する

```shell
id=$(aws cloudformation describe-stacks --stack-name cdk-ec2-api-server-dev --query 'Stacks[].Outputs[?OutputKey==`cdkec2apiserverdevinstanceid`].OutputValue' --output text) \
  && aws ec2 describe-instances --instance-ids ${id} --query 'Reservations[*].Instances[*].KeyName' --output text
```

- `IP`アドレスを取得しつつ生成済みキーを指定して`SSH`ログインできるか確認する

```shell
keyName=cdk-ec2-api-server-dev-key \
  && ip=$(aws cloudformation describe-stacks --stack-name cdk-ec2-api-server-dev --query 'Stacks[].Outputs[?OutputKey==`cdkec2apiserverdevipaddress`].OutputValue' --output text) \
  && ssh -i ${keyName}.pem ec2-user@${ip}
```

- 一旦ログアウトする
- ローカルから再帰的にディレクトリをアップロードする

```shell
ip=$(aws cloudformation describe-stacks --stack-name cdk-ec2-api-server-dev --query 'Stacks[].Outputs[?OutputKey==`cdkec2apiserverdevipaddress`].OutputValue' --output text) \
  && scp -r -i cdk-ec2-api-server-dev-key.pem FlaskSample ec2-user@${ip}:/home/ec2-user/
```

### `EC2`インスタンスの整備

- `Python`の整備は次のリンク先を参考にすること：必要な内容は下記に転載している
  - [Pythonの様々なバージョンを導入](https://phasetr.com/archive/fc/pg/python/#python_1)
  - [venvを作る](https://phasetr.com/archive/fc/pg/python/#venv)
- `SSH`ログインする

```shell
keyName=cdk-ec2-api-server-dev-key \
  && ip=$(aws cloudformation describe-stacks --stack-name cdk-ec2-api-server-dev --query 'Stacks[].Outputs[?OutputKey==`cdkec2apiserverdevipaddress`].OutputValue' --output text) \
  && ssh -i ${keyName}.pem ec2-user@${ip}
```
- `pyenv`を導入する

```shell
echo 'export PATH="$HOME/.pyenv/bin:$PATH"' >> ~/.bashrc \
  && echo 'eval "$(pyenv init -)"' >> ~/.bashrc \
  && git clone https://github.com/pyenv/pyenv.git ~/.pyenv \
  && source ~/.bashrc \
  && pyenv install 3.11.7 \
  && pyenv global 3.11.7
```

- `FlaskSample`ディレクトリに移動して`venv`を作成する

```shell
cd FlaskSample \
  && python -m venv .venv \
  && source .venv/bin/activate \
  && pip install -r requirements.txt
```

- `Web API`を起動する

```shell
python app.py
```

### `API`実行確認

- ローカルから`API`を実行する

```shell
ip=$(aws cloudformation describe-stacks --stack-name cdk-ec2-api-server-dev --query 'Stacks[].Outputs[?OutputKey==`cdkec2apiserverdevipaddress`].OutputValue' --output text) \
  && curl http://${ip}:8080/
```

## メモ

### `CDK`で設定した`Lambda`の実行

#### コンソールから`Lambda`を実行するときの`JSON`の例

- 開始

```shell
id=$(aws cloudformation describe-stacks --stack-name cdk-ec2-api-server-dev --query 'Stacks[].Outputs[?OutputKey==`cdkec2apiserverdevinstanceid`].OutputValue' --output text) \
  && echo "{\"Action\": \"start\", \"Region\": \"ap-northeast-1\", \"Instances\": [\"${id}\"]}"
```

- 停止

```shell
id=$(aws cloudformation describe-stacks --stack-name cdk-ec2-api-server-dev --query 'Stacks[].Outputs[?OutputKey==`cdkec2apiserverdevinstanceid`].OutputValue' --output text) \
  && echo "{\"Action\": \"stop\", \"Region\": \"ap-northeast-1\", \"Instances\": [\"${id}\"]}"
```

### `AWS CLI`で`EC2`インスタンスを開始・停止する

#### 状態確認

```shell
aws ec2 describe-instances --query 'Reservations[*].Instances[*].{ID:InstanceId,State:State.Name}' --output table
```

#### 開始

```shell
id=$(aws cloudformation describe-stacks --stack-name cdk-ec2-api-server-dev --query 'Stacks[].Outputs[?OutputKey==`cdkec2apiserverdevinstanceid`].OutputValue' --output text) \
  && aws ec2 start-instances --instance-ids ${id}
```

#### 停止

```shell
id=$(aws cloudformation describe-stacks --stack-name cdk-ec2-api-server-dev --query 'Stacks[].Outputs[?OutputKey==`cdkec2apiserverdevinstanceid`].OutputValue' --output text) \
  && aws ec2 stop-instances --instance-ids ${id}
```

### ローカルでのサンプル`Web API`用の環境構築

```shell
cd FlaskSample \
  && python -m venv .venv \
  && source .venv/bin/activate \
  && pip install -r requirements.txt
```
