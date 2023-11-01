# README

## 基本的なフロー

- `academic-event.com`に設定する
- `copilot`での`Load Balanced`へのドメイン設定確認用：特にDNSプロバイダーはお名前.comなど外部サービスの場合
- 参考：[DNS での検証](https://docs.aws.amazon.com/ja_jp/acm/latest/userguide/dns-validation.html)
  - `AWS Certificate Manager`でパブリック証明書をリクエスト
  - 完全修飾ドメイン名で`*.academic-event.com`を設定
  - AWSで要求された値を`CNAME`でお名前.comに設定
    - `ACM`の`CNAME名`を`お名前.com`の`ホスト名`に、
      `ACM`の`CNAME値`を `お名前.com`の`VALUE`に入力する
  - AWSで対応する証明書のステータスが`成功`になるか確認：最大三日待つ
  - 証明書の`ARN`を控える
- `Copilot`への設定
  - 下記コマンドで一通り設定・デプロイ
  - `お名前.com`で`CNAME`を追加する
    - 追加する`CNAME`レコードのホスト名に対応する値は`ALB`の`DNS`名を指定する.
    - `ALB`の`DNS`名は以下の参考資料を参照すること.
    - [Creating records by using the Amazon Route 53 console](https://docs.aws.amazon.com/Route53/latest/DeveloperGuide/resource-record-sets-creating.html)
    - 下記項目[メモ](#メモ)を参考に`DNS Name`を`CNAME`として設定すればよい.

## 構築手順

- 上記の手順で`ACM`のステータスを「発行済み・成功」にしておく
- 作業アカウントの確認

```shell
export AWS_PROFILE=dev
aws whoami
```

- 事前確認：`お名前.com`のDNSが表示されるか確認

```shell
nslookup -type=NS academic-event.com | grep "academic-event.com"``
```

- 事前確認：`copilot/pt-domain/manifest.yml`から`environments`をコメントアウト
  - もしくは`copilot`ディレクトリを削除
- 次の変数を設定する：`ACM`はすでに設定されているとする

```shell
export AWS_PROFILE=dev
export ACM="arn:aws:acm:ap-northeast-1:573143736992:certificate/4acc71e3-2ef8-4c3d-bed8-22fc6c3bb3a1"
```

- 再確認：必要に応じて`copilot`ディレクトリを削除

```shell
export AWS_PROFILE=dev
copilot app init pt-domain
```

```shell
copilot env init --name stg \
  --profile dev \
  --app pt-domain \
  --default-config \
  --import-cert-arns ${ACM}
```

```shell
copilot svc init --name pt-domain \
  --svc-type "Load Balanced Web Service" \
  --dockerfile ./Dockerfile \
  --port 80
```

- `Git`の差分を見て`copilot/pt-domain/manifest.yml`を修正する.
  特にヘルスチェック時間短縮設定を追加する.

```shell
copilot env deploy --name stg
```

```shell
copilot svc deploy --env stg && copilot svc show
```

- `copilot svc show`でエラーが出た場合,
  `AWS`コンソールから直接ロードバランサーのDNSを調べる
- `お名前.com`で`CNAME`を設定
- (`TODO`: 次の`deploy`はなくても動く?)

```shell
export AWS_PROFILE=dev
copilot svc deploy --env stg && copilot svc show
```

- <https://stg.academic-event.com>にアクセスしてサービスが起動しているか確認

```shell
export AWS_PROFILE=dev
copilot env init --name prod \
  --profile dev \
  --app pt-domain \
  --default-config \
  --import-cert-arns ${ACM}
copilot env deploy --name prod && copilot svc deploy --env prod
```

- `copilot/pt-domain/manifest.yml`の`environments`のコメントアウトを解除：特に`prod`の`alias`を設定
- `AWS`コンソールからロードバランサーのDNSを調べ、`お名前.com`で`CNAME`を設定

```shell
export AWS_PROFILE=dev
copilot svc deploy --env prod
```

- <https://www.academic-event.com>にアクセスしてサービスが起動しているか確認
- 必要に応じて`index.html`を変更してから`copilot svc deploy --env prod`して、
  変更が反映されるか確認

## 環境削除

```shell
export AWS_PROFILE=dev
aws whoami
copilot app delete --yes
```

## メモ

`Appliation Load Balancer（ALB）`へのルーティングで`ALB`の`DNS`レコードは,
エイリアスドメインがホストされている`DNS`プロバイダーで登録する.
お名前.comでDNSプロバイダーとして使っているならお名前.comで`ALB`の`DNS`レコードを登録する.

`DNS`プロバイダーに`Route 53`ではなく`お名前.com`など外部の`DNS`プロバイダーを採用する場合,
`ALB`へルーティングするための`DNS`レコードは`A`レコードではなく`CNAME`レコードで登録する.

`CNAME`レコードは`Zone Apex`を指定した状態では登録できない.
例えばドメイン`academic-event.com`の場合,
ホスト名（Name）として`xxxxxxx.academic-event.com`ならば指定できる.

-

参考資料: [サポートされるDNSレコードタイプ - CNAMEレコードタイプ](https://docs.aws.amazon.com/ja_jp/Route53/latest/DeveloperGuide/ResourceRecordTypes.html#CNAMEFormat)

```
重要

DNSプロトコルでは、Zone Apex とも呼ばれる、DNS 名前空間の最上位ノードに対して CNAME レコードを作成することができません。
例えば、example.com という DNS 名を登録する場合、Zone Apex は example.com になります。
example.com に対して CNAME レコードを作成することはできませんが、
www.example.com、newproduct.example.com などに対しては CNAME レコードを作成できます。

さらに、サブドメインに対して CNAME レコードを作成する場合、そのサブドメインの他のレコードを作成できません。
例えば、www.example.com の CNAME を作成する場合、Name フィールドの値が www.example.com の他のレコードは作成できません。
```

追加する`CNAME`レコードのホスト名に対応する値は`ALB`の`DNS`名を指定する.
`ALB`の`DNS`名は以下の参考資料を参照すること.

- [Creating records by using the Amazon Route 53 console](https://docs.aws.amazon.com/Route53/latest/DeveloperGuide/resource-record-sets-creating.html)

```
Getting the DNS name for an ELB load balancer
1. Sign in to the AWS Management Console using the AWS account that was used to create the Classic, Application, or Network Load Balancer that you want to create an alias record for.
2. Open the Amazon EC2 console at https://console.aws.amazon.com/ec2/.
3. In the navigation pane, choose Load Balancers.
4. In the list of load balancers, select the load balancer for which you want to create an alias record.
5. On the Description tab, get the value of DNS name.
```
