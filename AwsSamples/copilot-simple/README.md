# README

メモ：[AWS App Runner のサービスへのトラフィックのルーティング](https://docs.aws.amazon.com/ja_jp/Route53/latest/DeveloperGuide/routing-to-app-runner.html)に沿って手動でRoute 53にAレコードを追加すれば良い？

- 特に（`copilot`での）`App Runner`へのDNS設定確認用
- 参考：[お名前.comで取得したドメインをRoute53のネームサーバで管理設定してみた](https://dev.classmethod.jp/articles/route53-domain-onamae/)
- [参考](https://aws.amazon.com/jp/blogs/news/introducing-aws-copilot/)
- [AWS Copilotでの公式のドメイン設定：App Runner](https://aws.github.io/copilot-cli/ja/docs/developing/domain/#request-driven-web-service)
  - `copilot/web/manifest.yml`に`http`を設定すれば良い

```shell
export AWS_PROFILE=dev
```

```shell
copilot app init --domain academic-event.com
copilot app init
```

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
