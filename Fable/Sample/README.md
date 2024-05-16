# README

## 開発用に`watch`で実行

- `dotnet fable watch ./src --run vite --open`のように1コマンドでどうにかしたい
- 二つターミナルを開く
  - 一つは`dotnet fable watch ./src`または`npm run fw`を実行
  - もう一つは`npx vite --open`を実行

## 単純な実行

- 以下のコマンドを実行して`dist/index.html`をブラウザで開く

```shell
npm run build
```
