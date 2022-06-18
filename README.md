# WatchTime

1) Start K8S development cluster from Docker

2) Run the SQLExpress server provided in src/infrastructure/compose

3) In a terminal run
```bash
kubectl proxy
```

4) Run the following commands in src/infrastructure/K8S
```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.2.0/deploy/static/provider/aws/deploy.yaml
```
```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.5.0/aio/deploy/recommended.yaml
```
```bash
kubectl apply -f https://sunstone.dev/keel?namespace=default&username=admin&password=admin&tag=latest
```
```bash
kubectl apply -f swsp-depl.yaml
```
```bash
kubectl apply -f ingress-srv.yaml
```
```bash
kubectl apply -f frontend-ingress.yaml
```
5) Get a token run the following command

Docker >V4.8
```bash
kubectl -n kubernetes-dashboard create token admin-user
```
Docker <V4.8
```bash
kubectl -n kubernetes-dashboard get secret $(kubectl -n kubernetes-dashboard get sa/admin-user -o jsonpath="{.secrets[0].name}") -o go-template="{{.data.token | base64decode}}"
```
6) Use the token given and log into the dashboard
```bash
http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/login
```

