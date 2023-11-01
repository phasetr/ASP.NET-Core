# README

メモ：[AWS App Runner のサービスへのトラフィックのルーティング](https://docs.aws.amazon.com/ja_jp/Route53/latest/DeveloperGuide/routing-to-app-runner.html)に沿って手動でRoute 53にAレコードを追加すれば良い？

- `copilot`での`S3 storage`と`CloudFront`の連携
- 参考: [S3 バケットで CloudFront を使うには？](https://aws.github.io/copilot-cli/ja/docs/developing/content-delivery/#s3)
- 参考：[お名前.comで取得したドメインをRoute53のネームサーバで管理設定してみた](https://dev.classmethod.jp/articles/route53-domain-onamae/)
- [参考](https://aws.amazon.com/jp/blogs/news/introducing-aws-copilot/)
- [AWS Copilotでの公式のドメイン設定：App Runner](https://aws.github.io/copilot-cli/ja/docs/developing/domain/#request-driven-web-service)
  - `copilot/web/manifest.yml`に`http`を設定すれば良い

```shell
export AWS_DEFAULT_PROFILE=dev
```

- `init`の実行

```shell
export AWS_DEFAULT_PROFILE=dev
copilot app init copilot-s3-cloudfront
copilot env init --name test \
  --profile dev \
  --app copilot-s3-cloudfront \
  --default-config
copilot svc init --name copilot-s3-cloudfront \
  --svc-type "Load Balanced Web Service" \
  --dockerfile Dockerfile \
  --port 80
copilot storage init -n copilot-s3-cloudfront-bucket -t S3 -w copilot-s3-cloudfront
```

- `deploy`の実行

```shell
copilot env deploy
copilot svc deploy
copilot svc show
```

## 削除

```shell
copilot svc delete
copilot env delete
copilot app delete
```

## DNSの切り替え確認

```shell
nslookup -type=NS [対象ドメイン]
nslookup -type=NS academic-event.com
```
