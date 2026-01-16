import React, { useEffect, useState } from 'react';
import apiClient from '../../services/api';
import {
  ApiResponse,
  Category,
  CategoryPurpose,
  CreateCategoryRequest,
} from '../../types';

/**
 * Seção de gerenciamento de categorias.
 * Implementa:
 * - Formulário de criação
 * - Listagem de categorias
 * - Exibição da finalidade (Despesa / Receita / Ambas)
 */
const CategoriesSection: React.FC = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [form, setForm] = useState<CreateCategoryRequest>({
    description: '',
    purpose: CategoryPurpose.Expense,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadCategories = async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await apiClient.get<ApiResponse<Category[]>>('/categories');
      if (res.data.success && res.data.data) {
        setCategories(res.data.data);
      } else {
        setError(res.data.message || 'Erro ao carregar categorias');
      }
    } catch (err: any) {
      setError(err.response?.data?.message ?? 'Erro inesperado ao carregar categorias');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadCategories();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: name === 'purpose' ? Number(value) : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!form.description.trim()) {
      setError('Descrição é obrigatória.');
      return;
    }

    try {
      const res = await apiClient.post<ApiResponse<Category>>('/categories', form);
      if (res.data.success && res.data.data) {
        setForm({ description: '', purpose: CategoryPurpose.Expense });
        await loadCategories();
      } else {
        setError(res.data.message || 'Erro ao criar categoria');
      }
    } catch (err: any) {
      setError(err.response?.data?.message ?? 'Erro inesperado ao criar categoria');
    }
  };

  const renderPurposeChip = (purpose: CategoryPurpose) => {
    if (purpose === CategoryPurpose.Expense) {
      return <span className="chip expense">Despesa</span>;
    }
    if (purpose === CategoryPurpose.Income) {
      return <span className="chip income">Receita</span>;
    }
    return <span className="chip both">Ambas</span>;
  };

  return (
    <>
      <div className="card">
        <div className="card-title">Cadastro de Categorias</div>
        <div className="card-description">
          Cada categoria possui descrição e finalidade (despesa, receita ou ambas).
          A API valida se a categoria é compatível com o tipo da transação.
        </div>
        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <div className="form-field">
              <label htmlFor="description">Descrição</label>
              <input
                id="description"
                name="description"
                value={form.description}
                onChange={handleChange}
                placeholder="Ex: Alimentação, Salário, Transporte..."
              />
            </div>
            <div className="form-field">
              <label htmlFor="purpose">Finalidade</label>
              <select
                id="purpose"
                name="purpose"
                value={form.purpose}
                onChange={handleChange}
              >
                <option value={CategoryPurpose.Expense}>Despesa</option>
                <option value={CategoryPurpose.Income}>Receita</option>
                <option value={CategoryPurpose.Both}>Ambas</option>
              </select>
            </div>
          </div>
          <div className="button-row">
            <button type="submit" className="btn-primary">
              Cadastrar categoria
            </button>
          </div>
          {error && <div className="error-text">{error}</div>}
        </form>
      </div>

      <div className="card">
        <div className="card-title">Categorias cadastradas</div>
        {loading ? (
          <div>Carregando...</div>
        ) : categories.length === 0 ? (
          <div className="info-text">Nenhuma categoria cadastrada.</div>
        ) : (
          <table className="table">
            <thead>
              <tr>
                <th>Descrição</th>
                <th>Finalidade</th>
                <th>ID</th>
              </tr>
            </thead>
            <tbody>
              {categories.map(c => (
                <tr key={c.id}>
                  <td>{c.description}</td>
                  <td>{renderPurposeChip(c.purpose)}</td>
                  <td>{c.id}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </>
  );
};

export default CategoriesSection;
