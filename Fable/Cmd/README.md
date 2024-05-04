# Fable Getting Started

- [Original Repository](https://github.com/Zaid-Ajaj/fable-getting-started)
- [The Elmish book](https://zaid-ajaj.github.io/the-elmish-book/#/)
- [Elmish Commands](https://zaid-ajaj.github.io/the-elmish-book/#/chapters/commands/commands)から

## 注意

- `Cmd.ofSub`は単に`Cmd.ofEffect`に置き換わった：[参考](https://elmish.github.io/elmish/docs/subscription.html#subscription-reusability)
- `The Elmish book`での`webpack`を`vite`に置き換えた。
  - デフォルトの設定のままでよいため`vite.config.js`は不要
  - 自動でブラウザを開くため`vite`コマンドのオプションで`--open`を指定している
  - `index.html`も`vite`に合わせて書き換えてある

## 開発用に`watch`で実行

```shell
npm start
```

## 単純な実行

- 以下のコマンドを実行して`dist/index.html`をブラウザで開く

```shell
npm run build
```
