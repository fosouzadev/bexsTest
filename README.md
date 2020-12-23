# Rota de Viagem #

Um turista deseja viajar pelo mundo pagando o menor preço possível independentemente do número de conexões necessárias.
Vamos construir um programa que facilite ao nosso turista, escolher a melhor rota para sua viagem.

Para isso precisamos inserir as rotas através de um arquivo de entrada.

## Dados de Entrada ##
```csv
GRU,BRC,10
BRC,SCL,5
GRU,CDG,75
GRU,SCL,20
GRU,ORL,56
ORL,CDG,5
SCL,ORL,20
```

## Explicando ## 
Caso desejemos viajar de **GRU** para **CDG** existem as seguintes rotas:

1. GRU - BRC - SCL - ORL - CDG ao custo de **$40**
2. GRU - ORL - CDG ao custo de **$64**
3. GRU - CDG ao custo de **$75**
4. GRU - SCL - ORL - CDG ao custo de **$45**

O melhor preço é da rota **1** logo, o output da consulta deve ser **GRU - BRC - SCL - ORL - CDG**.

## Execução do programa ##

Foi desenvolvido duas interfaces:
  - Console
  - Api Rest

Para executar a aplicação **Console** execute os seguintes comandos:

```shell
$ cd ConsoleApp
$ dotnet clean
$ dotnet build
$ dotnet run
```

A aplicação irá solicitar a digitação das rotas de origem e destino para realizar o calculo da rota com menor custo.

```shell
please enter the route: GRU-CDG
best route: GRU - BRC - SCL - ORL - CDG > $40
please enter the route: BRC-CDG
best route: BRC - SCL - ORL - CDG > $30
```
Para executar a aplicação **Api Rest** execute os seguintes comandos:

```shell
$ cd ApiRest
$ dotnet clean
$ dotnet build
$ dotnet run
```
Em seguida abra no seu navegador de preferência com a seguinte URL **https://localhost:5001/swagger/index.html**,
e será exibida a documentação dos endpoints da API com todas as informações necessárias para realizar requisições de teste.

## Decisões de Desenvolvimento ##

Como a proposta do exercício era uma mesma aplicação com duas interfaces, decidi criar uma camada **Domain** que cuidasse das regras de negócio e apenas fornecesse os serviços e dados para as interfaces.

Na interface de **Console** é feito a instanciação do serviço necessário por ser uma aplicação primitiva, porém na **Api Rest** o serviço é injetado na controller via injeção de dependência no arquivo **Startup.cs**
