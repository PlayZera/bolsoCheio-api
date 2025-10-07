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
- JWT Authentication (Bearer Token)
- BCrypt (Hash de senhas)
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

### 🔐 Auth Controller (Autenticação)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/auth/register` | Registrar novo usuário |
| POST | `/api/auth/login` | Fazer login e obter token JWT |
| GET | `/api/auth/profile` | Obter perfil do usuário autenticado |
| PUT | `/api/auth/profile` | Atualizar perfil do usuário |
| POST | `/api/auth/change-password` | Alterar senha |
| POST | `/api/auth/validate-token` | Validar token JWT |
| POST | `/api/auth/logout` | Fazer logout (invalidar token no cliente) |

### 📦 Products Controller

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

### 🔐 Autenticação

#### Registrar Usuário
```json
POST /api/auth/register
{
  "name": "João Silva",
  "email": "joao@email.com",
  "password": "123456",
  "confirmPassword": "123456",
  "phone": "11999999999",
  "monthlyIncome": 5000.00
}
```

#### Login
```json
POST /api/auth/login
{
  "email": "joao@email.com",
  "password": "123456"
}

// Resposta:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "name": "João Silva",
    "email": "joao@email.com",
    "monthlyIncome": 5000.00,
    "currency": "BRL"
  },
  "expiresAt": "2025-10-08T10:30:00Z",
  "tokenType": "Bearer"
}
```

#### Requisições Autenticadas
```bash
# Adicione o header Authorization em todas as rotas protegidas
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 📦 Produtos

#### Criar Produto
```json
POST /api/products
{
  "name": "Notebook Dell",
  "description": "Notebook para desenvolvimento",
  "price": 2500.00,
  "stock": 10
}
```

#### Atualizar Produto
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

### 🔐 Autenticação
- **Nome**: Obrigatório, máximo 100 caracteres
- **Email**: Obrigatório, formato válido, máximo 150 caracteres
- **Senha**: Obrigatório, mínimo 6 caracteres
- **Telefone**: Formato válido, máximo 15 caracteres
- **Renda Mensal**: Não pode ser negativa

### 📦 Produtos
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
