# Sistema de Controle de Gastos Residenciais

Sistema completo para gerenciamento de despesas e receitas residenciais, desenvolvido com tecnologias modernas e boas práticas de engenharia de software.

## 🎯 Objetivo

Implementar um sistema robusto de controle de gastos residenciais que permite:
- Gerenciar pessoas (cadastro, consulta, deleção)
- Categorizar transações (receita/despesa)
- Registrar transações com validações de negócio
- Consultar totalizações por pessoa e categoria

## 🏗️ Arquitetura

```
residential-expense-control/
├── backend/                    # API .NET
│   ├── ExpenseControlAPI/     # Projeto principal
│   ├── ExpenseControlAPI.Tests/
│   └── ExpenseControl.sln
└── frontend/                   # React + TypeScript
    ├── src/
    ├── public/
    └── package.json
```

## 🛠️ Tecnologias

### Back-end
- **Framework**: .NET 8
- **Linguagem**: C#
- **ORM**: Entity Framework Core
- **Banco de Dados**: SQLite
- **Arquitetura**: Clean Architecture com Dependency Injection

### Front-end
- **Framework**: React 18+
- **Linguagem**: TypeScript
- **Styling**: CSS3 + Styled Components
- **HTTP Client**: Axios
- **State Management**: React Hooks (Context API)

## ✨ Funcionalidades

### 1. Cadastro de Pessoas
- ✅ Criar pessoa (ID gerado automaticamente, Nome, Idade)
- ✅ Listar todas as pessoas
- ✅ Deletar pessoa (remove também todas as transações)

### 2. Cadastro de Categorias
- ✅ Criar categoria (ID gerado, Descrição, Finalidade)
- ✅ Listar categorias
- ✅ Validação de categorias por tipo de transação

### 3. Cadastro de Transações
- ✅ Criar transação (ID, Descrição, Valor, Tipo, Categoria, Pessoa)
- ✅ Listar transações
- ✅ Restrição: menores de idade só podem ter despesas
- ✅ Validação: categoria deve corresponder ao tipo de transação

### 4. Consultas e Relatórios
- ✅ Totalizações por pessoa (receitas, despesas, saldo)
- ✅ Totalizações gerais
- ✅ Totalizações por categoria (opcional)

## 🚀 Como Executar

### Back-end

```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run --project ExpenseControlAPI
```

A API estará disponível em: `http://localhost:5000`

### Front-end

```bash
cd frontend
npm install
npm start
```

A aplicação estará disponível em: `http://localhost:3000`

## 📝 Documentação da API

### Endpoints Base: `/api`

#### Pessoas
- `GET /api/pessoas` - Listar todas as pessoas
- `POST /api/pessoas` - Criar nova pessoa
- `DELETE /api/pessoas/{id}` - Deletar pessoa

#### Categorias
- `GET /api/categorias` - Listar todas as categorias
- `POST /api/categorias` - Criar nova categoria

#### Transações
- `GET /api/transacoes` - Listar todas as transações
- `POST /api/transacoes` - Criar nova transação

#### Relatórios
- `GET /api/relatorios/por-pessoa` - Totalizações por pessoa
- `GET /api/relatorios/por-categoria` - Totalizações por categoria

## 💾 Persistência de Dados

O sistema utiliza **SQLite** como banco de dados. Os dados são automaticamente persistidos no arquivo `expense_control.db` na raiz do projeto backend. O banco é criado automaticamente na primeira execução através do Entity Framework Core.

## 🧪 Testes

```bash
cd backend
dotnet test
```

## 📋 Regras de Negócio

1. **Identificadores**: Todos os IDs são gerados automaticamente (GUID/UUID)
2. **Menores de Idade**: Pessoas com idade < 18 anos só podem ter transações do tipo "Despesa"
3. **Validação de Categoria**: A categoria de uma transação deve ter a finalidade correspondente ao tipo
4. **Cascata de Deleção**: Ao deletar uma pessoa, todas as suas transações são removidas
5. **Valores Positivos**: Todos os valores de transação devem ser positivos

## 👨‍💻 Qualidade de Código

- ✅ Comentários explicativos em todas as funções críticas
- ✅ Nomenclatura clara e em inglês (padrão internacional)
- ✅ Separação de responsabilidades (Controllers, Services, Models)
- ✅ Validações robustas em todos os endpoints
- ✅ Tratamento de erros com mensagens claras
- ✅ TypeScript com tipos estritos no front-end

## 📖 Estrutura de Projetos

### Backend Structure
```
Controllers/      - Endpoints da API
Services/         - Lógica de negócio
Models/          - Entidades do domínio
DTOs/            - Data Transfer Objects
Data/            - Contexto do EF Core
```

### Frontend Structure
```
components/      - Componentes React reutilizáveis
pages/          - Páginas principais
services/       - Chamadas à API
hooks/          - Custom hooks
types/          - Tipos TypeScript
```

## 🔒 Validações Implementadas

- Idade deve ser positiva
- Valores de transação devem ser positivos
- Menores de idade sem receitas
- Categorias validadas por tipo de transação
- Pessoa deve existir para ter transações

## 🎁 Funcionalidades Adicionais

- Tratamento de erros robusto
- Resposta padrão de API
- Formulários validados no front
- Interface responsiva
- Logs estruturados

## 👤 Autor

Desenvolvido como teste técnico.

---

**Última atualização**: Janeiro 2026
