import React, { useEffect, useMemo, useState } from 'react';
import apiClient from '../../services/api';
import {
  ApiResponse,
  Category,
  CategoryPurpose,
  CreateTransactionRequest,
  Person,
  Transaction,
  TransactionType,
} from '../../types';

/**
 * Seção de transações.
 * Implementa:
 * - Formulário de criação de transações
 * - Listagem de transações
 * - Filtragem de categorias no select de acordo com o tipo (despesa/receita)
 * 
 * OBS: As regras de menor de idade e compatibilidade são validadas na API.
 * Aqui no front fazemos uma UX melhor mostrando apenas categorias permitidas.
 */
const TransactionsSection: React.FC = () => {
  const [people, setPeople] = useState<Person[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [form, setForm] = useState<CreateTransactionRequest>({
    description: '',
    amount: 0,
    type: TransactionType.Expense,
    personId: '',
    categoryId: '',
  });
  const [loading, setLoading] = useState(false);
  const [loadingTransactions, setLoadingTransactions] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadPeopleAndCategories = async () => {
    setLoading(true);
    setError(null);
    try {
      const [peopleRes, catRes] = await Promise.all([
        apiClient.get<ApiResponse<Person[]>>('/people'),
        apiClient.get<ApiResponse<Category[]>>('/categories'),
      ]);
      if (peopleRes.data.success && peopleRes.data.data) {
        setPeople(peopleRes.data.data);
      }
      if (catRes.data.success && catRes.data.data) {
        setCategories(catRes.data.data);
      }
    } catch (err: any) {
      setError(err.response?.data?.message ?? 'Erro ao carregar pessoas/categorias');
    } finally {
      setLoading(false);
    }
  };

  const loadTransactions = async () => {
    setLoadingTransactions(true);
    setError(null);
    try {
      const res = await apiClient.get<ApiResponse<Transaction[]>>('/transactions');
      if (res.data.success && res.data.data) {
        setTransactions(res.data.data);
      } else {
        setError(res.data.message || 'Erro ao carregar transações');
      }
    } catch (err: any) {
      setError(err.response?.data?.message ?? 'Erro inesperado ao carregar transações');
    } finally {
      setLoadingTransactions(false);
    }
  };

  useEffect(() => {
    loadPeopleAndCategories();
    loadTransactions();
  }, []);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setForm(prev => {
      if (name === 'amount') {
        return { ...prev, amount: Number(value) };
      }
      if (name === 'type') {
        // Quando o tipo muda, limpamos a categoria para forçar usuário a escolher uma compatível
        return {
          ...prev,
          type: Number(value) as TransactionType,
          categoryId: '',
        };
      }
      return { ...prev, [name]: value };
    });
  };

  /**
   * Filtra categorias permitidas para o tipo de transação selecionado.
   * - Se tipo = Expense → categorias Expense ou Both
   * - Se tipo = Income → categorias Income ou Both
   */
  const filteredCategories = useMemo(() => {
    return categories.filter(cat => {
      if (form.type === TransactionType.Expense) {
        return cat.purpose === CategoryPurpose.Expense || cat.purpose === CategoryPurpose.Both;
      }
      return cat.purpose === CategoryPurpose.Income || cat.purpose === CategoryPurpose.Both;
    });
  }, [categories, form.type]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!form.personId) {
      setError('Selecione uma pessoa.');
      return;
    }
    if (!form.categoryId) {
      setError('Selecione uma categoria compatível.');
      return;
    }
    if (!form.description.trim()) {
      setError('Descrição é obrigatória.');
      return;
    }
    if (form.amount <= 0) {
      setError('Valor deve ser positivo.');
      return;
    }

    try {
      const res = await apiClient.post<ApiResponse<Transaction>>('/transactions', form);
      if (res.data.success && res.data.data) {
        setForm(prev => ({
          ...prev,
          description: '',
          amount: 0,
        }));
        await loadTransactions();
      } else {
        setError(res.data.message || 'Erro ao criar transação');
      }
    } catch (err: any) {
      // Aqui capturamos erros de regra de negócio, por exemplo:
      // - menor de idade tentando receita
      // - categoria incompatível
      setError(err.response?.data?.message ?? 'Erro inesperado ao criar transação');
    }
  };

  const renderTypeChip = (type: TransactionType) => {
    return type === TransactionType.Expense ? (
      <span className="chip expense">Despesa</span>
    ) : (
      <span className="chip income">Receita</span>
    );
  };

  return (
    <>
      <div className="card">
        <div className="card-title">Cadastro de Transações</div>
        <div className="card-description">
          Registre despesas e receitas vinculando uma pessoa e uma categoria.
          A API garante que menores de 18 anos só tenham despesas
          e que a categoria seja compatível com o tipo.
        </div>

        {loading ? (
          <div>Carregando pessoas/categorias...</div>
        ) : (
          <form onSubmit={handleSubmit}>
            <div className="form-row">
              <div className="form-field">
                <label htmlFor="personId">Pessoa</label>
                <select
                  id="personId"
                  name="personId"
                  value={form.personId}
                  onChange={handleChange}
                >
                  <option value="">Selecione...</option>
                  {people.map(p => (
                    <option key={p.id} value={p.id}>
                      {p.name} ({p.age} anos)
                    </option>
                  ))}
                </select>
              </div>

              <div className="form-field">
                <label htmlFor="type">Tipo</label>
                <select
                  id="type"
                  name="type"
                  value={form.type}
                  onChange={handleChange}
                >
                  <option value={TransactionType.Expense}>Despesa</option>
                  <option value={TransactionType.Income}>Receita</option>
                </select>
              </div>

              <div className="form-field">
                <label htmlFor="categoryId">Categoria</label>
                <select
                  id="categoryId"
                  name="categoryId"
                  value={form.categoryId}
                  onChange={handleChange}
                >
                  <option value="">Selecione...</option>
                  {filteredCategories.map(c => (
                    <option key={c.id} value={c.id}>
                      {c.description}
                    </option>
                  ))}
                </select>
                <div className="info-text">
                  Lista filtrada automaticamente pela finalidade da categoria.
                </div>
              </div>
            </div>

            <div className="form-row">
              <div className="form-field">
                <label htmlFor="description">Descrição</label>
                <input
                  id="description"
                  name="description"
                  value={form.description}
                  onChange={handleChange}
                  placeholder="Ex: Supermercado, Salário..."
                />
              </div>
              <div className="form-field">
                <label htmlFor="amount">Valor</label>
                <input
                  id="amount"
                  name="amount"
                  type="number"
                  min={0}
                  step="0.01"
                  value={form.amount || ''}
                  onChange={handleChange}
                />
              </div>
            </div>

            <div className="button-row">
              <button type="submit" className="btn-primary">
                Registrar transação
              </button>
            </div>

            {error && <div className="error-text">{error}</div>}
            <div className="info-text">
              Regras de negócio adicionais são validadas no backend:
              menor de idade não pode ter receita e categoria deve ser compatível.
            </div>
          </form>
        )}
      </div>

      <div className="card">
        <div className="card-title">Transações registradas</div>
        {loadingTransactions ? (
          <div>Carregando transações...</div>
        ) : transactions.length === 0 ? (
          <div className="info-text">Nenhuma transação registrada.</div>
        ) : (
          <table className="table">
            <thead>
              <tr>
                <th>Descrição</th>
                <th>Tipo</th>
                <th>Valor</th>
                <th>Pessoa</th>
                <th>Categoria</th>
                <th>Data</th>
              </tr>
            </thead>
            <tbody>
              {transactions.map(t => (
                <tr key={t.id}>
                  <td>{t.description}</td>
                  <td>{renderTypeChip(t.type)}</td>
                  <td>
                    {t.type === TransactionType.Expense ? '-' : '+'} R${' '}
                    {t.amount.toFixed(2)}
                  </td>
                  <td>{t.personName}</td>
                  <td>{t.categoryDescription}</td>
                  <td>{new Date(t.createdAt).toLocaleString()}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </>
  );
};

export default TransactionsSection;
