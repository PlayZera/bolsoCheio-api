# BolsoCheio API

API para gerenciamento de produtos desenvolvida em .NET 9 com arquitetura em 3 camadas.

## 🏗️ Arquitetura

A aplicação segue o padrão de arquitetura em camadas:

- **🎯 BolsoCheio.API** - Camada de Apresentação (Controllers, Swagger)
- **⚙️ BolsoCheio.Business** - Camada de Negócio (Services, DTOs, Validações)
- **💾 BolsoCheio.Data** - Camada de Dados (Repositories, Models)

## 🚀 Tecnologias Utilizadas

- .NET 9
- ASP.NET Core Web API
- Swagger/OpenAPI (Swashbuckle)
- Injeção de Dependência
- Data Annotations (Validações)
- Repositório em Memória (para demonstração)

## 📦 Estrutura do Projeto

```
Backend.BolsoCheioAPI.sln
├── src/
│   ├── BolsoCheio.API/
│   │   ├── Controllers/
│   │   │   └── ProductsController.cs
│   │   ├── Program.cs
│   │   └── BolsoCheio.API.http
│   ├── BolsoCheio.Business/
│   │   ├── DTOs/
│   │   │   └── ProductDto.cs
│   │   ├── Interfaces/
│   │   │   └── IProductService.cs
│   │   └── Services/
│   │       └── ProductService.cs
│   └── BolsoCheio.Data/
│       ├── Interfaces/
│       │   ├── IRepository.cs
│       │   └── IProductRepository.cs
│       ├── Models/
│       │   └── Product.cs
│       └── Repositories/
│           └── ProductRepository.cs
```

## 🔧 Como Executar

1. **Clone o repositório**
   ```bash
   git clone [url-do-repositorio]
   cd bolsoCheio-api
   ```

2. **Restaurar dependências**
   ```bash
   dotnet restore
   ```

3. **Executar a aplicação**
   ```bash
   cd src/BolsoCheio.API
   dotnet run
   ```

4. **Acessar o Swagger**
   - HTTPS: `https://localhost:7094`
   - HTTP: `http://localhost:5297`

## 📋 Endpoints Disponíveis

### Products Controller

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/products` | Obtém todos os produtos |
| GET | `/api/products/{id}` | Obtém um produto por ID |
| GET | `/api/products/active` | Obtém apenas produtos ativos |
| GET | `/api/products/search?name={name}` | Pesquisa produtos por nome |
| POST | `/api/products` | Cria um novo produto |
| PUT | `/api/products/{id}` | Atualiza um produto |
| DELETE | `/api/products/{id}` | Remove um produto |

## 📝 Exemplos de Uso

### Criar Produto
```json
POST /api/products
{
  "name": "Notebook Dell",
  "description": "Notebook para desenvolvimento",
  "price": 2500.00,
  "stock": 10
}
```

### Atualizar Produto
```json
PUT /api/products/1
{
  "name": "Notebook Dell - Atualizado",
  "description": "Notebook para desenvolvimento e jogos",
  "price": 2800.00,
  "stock": 8,
  "isActive": true
}
```

## ✅ Validações

- **Nome**: Obrigatório, máximo 100 caracteres
- **Descrição**: Máximo 500 caracteres
- **Preço**: Obrigatório, deve ser maior que zero
- **Estoque**: Obrigatório, não pode ser negativo

## 🔍 Testando a API

1. **Via Swagger**: Acesse `https://localhost:7094` e use a interface interativa
2. **Via HTTP Client**: Use o arquivo `BolsoCheio.API.http` no VS Code
3. **Via Postman/Insomnia**: Importe os endpoints disponíveis

## 🏛️ Padrões Implementados

- **Repository Pattern**: Abstração da camada de dados
- **Service Pattern**: Lógica de negócio centralizada
- **DTO Pattern**: Transferência de dados entre camadas
- **Dependency Injection**: Inversão de controle
- **Separation of Concerns**: Responsabilidades bem definidas

## 📚 Próximos Passos

- [ ] Integração com Entity Framework
- [ ] Implementação de autenticação JWT
- [ ] Testes unitários e de integração
- [ ] Logging estruturado
- [ ] Versionamento da API
- [ ] Rate Limiting
- [ ] Caching

## 🤝 Contribuição

1. Faça um fork do projeto
2. Crie sua feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request
