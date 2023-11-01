# AWS samples

## Database access from copilot job

- See [copilot-django-app-runner](copilot-django-app-runner) and [copilot-django-load-balanced](copilot-django-load-balanced).

### References

- [How to migrate a database by the copilot job for Load Balanced Service](https://github.com/aws/copilot-cli/issues/4579)

#### Django sample

- [How to deploy a containerized Django app with AWS Copilot](https://www.endpointdev.com/blog/2022/06/how-to-deploy-containerized-django-app-with-aws-copilot/)
  - [Original GitHub](https://github.com/aburayyanjeffry/django-copilot.git)
- [How to deploy a Django App with Aurora Serverless and AWS Copilot](https://www.endpointdev.com/blog/2022/06/how-to-deploy-django-app-with-aurora-serverless-and-copilot/)

### Setting for database

- cf. [Comment in GitHub](https://github.com/aws/copilot-cli/issues/4579#issuecomment-1459149195)
- Create an app, service, storage, and jobs by `copilot`
- Add the following description to `svc/addons/db.yml`
  - You should specify the appropriate `DBClusterSecurityGroup` name!

```
  mydjangodbDBClusterSecurityGroup: # Select the appropriate item
    Metadata:
      'aws:copilot:description': 'A security group for your Aurora Serverless v2 cluster mydjangodb'
    Type: AWS::EC2::SecurityGroup
    Properties:
      GroupDescription: The Security Group for the Aurora Serverless v2 cluster.
      SecurityGroupIngress:
        - ToPort: 5432
          FromPort: 5432
          IpProtocol: tcp
          Description: !Sub 'From the Aurora Security Group of the workload ${Name}.'
          SourceSecurityGroupId: !Ref mydjangodbSecurityGroup
        - ToPort: 5432                                                                                 # ADD
          FromPort: 5432                                                                               # ADD
          IpProtocol: tcp                                                                              # ADD
          Description: 'Access to environment security group'                                          # ADD
          SourceSecurityGroupId: { 'Fn::ImportValue': !Sub '${App}-${Env}-EnvironmentSecurityGroup' }  # ADD
```

### copilot command samples

```
copilot init \
  -a mydjango \
  -t "Load Balanced Web Service" -n django-web \
  -d ./Dockerfile
```

```
copilot env init \
  --name test \
  --profile default \
  --app mydjango \
  --default-config
```

```
copilot svc init
```

```
copilot storage init \
  -n mydjango-db \
  -t Aurora -w \
  django-web \
  --engine PostgreSQL \
  --initial-db mydb
```

```
copilot deploy --name django-web
```

```
copilot job init
```

### Django memo for myself

#### データベース切り替え

`settings.py`でデータベースの部分を書き換える.

#### 最初の`Django`インストール

```
docker-compose run web django-admin startproject mydjango .
```

#### マイグレーション関係


- [参考：`--noinput`](https://kamatimaru.hatenablog.com/entry/2021/02/28/030646)

```
copilot svc exec

# Dockerfileでスーパーユーザーの情報は環境変数に設定済み
python manage.py migrate
python manage.py createsuperuser --noinput
```

- [Django console](https://hodalog.com/how-to-use-django-shell/)

```
python manage.py shell

from django.contrib.auth.models import User
User.objects.get(username='admin')
```
