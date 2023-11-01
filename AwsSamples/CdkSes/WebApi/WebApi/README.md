# README

- [AWS CDKでApp Runnerに実行時の環境変数を渡す](https://www.findxfine.com/infrastructure/aws/995562497.html)

## Initialize

```shell
dotnet tool restore
dotnet restore
```

## Dockerでの実行

```shell
docker build -t webapi .
docker run -it --rm -p 80:80 webapi
```
