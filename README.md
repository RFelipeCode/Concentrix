# Concentrix

ConcentrixAPI

############# Descrição do Projeto ##################

ConcentrixAPI é uma API desenvolvida em .NET para gerenciar pedidos e seus respectivos itens. A API permite criar, atualizar e consultar pedidos, garantindo validações de dados e regras de negócio.


################ Instruções para Rodar a Aplicação Localmente ##########################

Pré-requisitos

Certifique-se de ter os seguintes requisitos instalados em seu ambiente:

.NET 6 ou superior

Ferramenta de gerenciamento de API (Postman, Insomnia, etc.)


################ Passos para Execução ################

Clone o repositório:

git clone https://github.com/RFelipeCode/concentrix.git
cd concentrix

########## Configuração do banco de dados InMemory:

O projeto utiliza o banco de dados em memória do .NET Core, portanto, nenhuma configuração adicional de banco de dados é necessária.

########## Compilar e executar o projeto:

** dotnet build
** dotnet run

Acesse a API:

A aplicação estará disponível em http://localhost:5000 ou https://localhost:5001.

Exemplos de Chamadas aos Endpoints

Criar um Pedido

Requisição:

POST /api/pedidos
Content-Type: application/json

Body:

{
  "nomeCliente": "João da Silva",
  "dataPedido": "2025-01-21T12:00:00",
  "itens": [
    {
      "nomeItem": "Produto A",
      "quantidade": 2,
      "valorUnitario": 100.00
    }
  ]
}

Resposta esperada:

{
  "id": 1,
  "nomeCliente": "João da Silva",
  "dataPedido": "2025-01-21T12:00:00",
  "valorTotal": 200.00
}

Consultar um Pedido por ID

Requisição:

GET /api/pedidos/1

Resposta esperada:

{
  "id": 1,
  "nomeCliente": "João da Silva",
  "dataPedido": "2025-01-21T12:00:00",
  "itens": [
    {
      "id": 10,
      "nomeItem": "Produto A",
      "quantidade": 2,
      "valorUnitario": 100.00
    }
  ],
  "valorTotal": 200.00
}

Atualizar um Pedido

Requisição:

PUT /api/pedidos/1
Content-Type: application/json

Body:

{
  "nomeCliente": "Maria Oliveira",
  "dataPedido": "2025-01-22T15:30:00",
  "itens": [
    {
      "id": 10,
      "nomeItem": "Produto A",
      "quantidade": 3,
      "valorUnitario": 100.00
    }
  ]
}

Resposta esperada:

{
  "id": 1,
  "nomeCliente": "Maria Oliveira",
  "dataPedido": "2025-01-22T15:30:00",
  "valorTotal": 300.00
}

Deletar um Pedido

Requisição:

DELETE /api/pedidos/1

Resposta esperada:

{
  "message": "Pedido removido com sucesso."
}



