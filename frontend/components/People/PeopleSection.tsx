import React, { useEffect, useState } from 'react';
import apiClient from '../../services/api';
import { ApiResponse, CreatePersonRequest, Person } from '../../types';

/**
 * Seção de gerenciamento de pessoas.
 * Implementa:
 * - Formulário de criação
 * - Listagem
 * - Botão de deleção (que dispara delete em cascata das transações na API)
 */
const PeopleSection: React.FC = () => {
  const [people, setPeople] = useState<Person[]>([]);
  const [form, setForm] = useState<CreatePersonRequest>({ name: '', age: 0 });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadPeople = async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await apiClient.get<ApiResponse<Person[]>>('/people');
      if (res.data.success && res.data.data) {
        setPeople(res.data.data);
      } else {
        setError(res.data.message || 'Erro ao carregar pessoas');
      }
    } catch (err: any) {
      setError(err.response?.data?.message ?? 'Erro inesperado ao carregar pessoas');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadPeople();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: name === 'age' ? Number(value) : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!form.name.trim()) {
      setError('Nome é obrigatório.');
      return;
    }
    if (form.age <= 0) {
      setError('Idade deve ser um inteiro positivo.');
      return;
    }

    try {
      const res = await apiClient.post<ApiResponse<Person>>('/people', form);
      if (res.data.success && res.data.data) {
        setForm({ name: '', age: 0 });
        await loadPeople();
      } else {
        setError(res.data.message || 'Erro ao criar pessoa');
      }
    } catch (err: any) {
      setError(err.response?.data?.message ?? 'Erro inesperado ao criar pessoa');
    }
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm('Tem certeza que deseja deletar esta pessoa e TODAS as suas transações?')) {
      return;
    }
    setError(null);
    try {
      const res = await apiClient.delete<ApiResponse<string>>(`/people/${id}`);
      if (res.data.success) {
        await loadPeople();
      } else {
        setError(res.data.message || 'Erro ao deletar pessoa');
      }
    } catch (err: any) {
      setError(err.response?.data?.message ?? 'Erro inesperado ao deletar pessoa');
    }
  };

  return (
    <>
      <div className="card">
        <div className="card-title">Cadastro de Pessoas</div>
        <div className="card-description">
          Cada pessoa possui um identificador único, nome e idade. Se uma pessoa for deletada,
          todas as suas transações são apagadas em cascata pela API.
        </div>
        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <div className="form-field">
              <label htmlFor="name">Nome</label>
              <input
                id="name"
                name="name"
                value={form.name}
                onChange={handleChange}
                placeholder="Ex: João Silva"
              />
            </div>
            <div className="form-field">
              <label htmlFor="age">Idade</label>
              <input
                id="age"
                name="age"
                type="number"
                value={form.age || ''}
                onChange={handleChange}
                min={1}
              />
            </div>
          </div>
          <div className="button-row">
            <button type="submit" className="btn-primary">
              Cadastrar pessoa
            </button>
          </div>
          {error && <div className="error-text">{error}</div>}
          <div className="info-text">
            Regra: menores de 18 anos só podem ter transações do tipo despesa (validado na API).
          </div>
        </form>
      </div>

      <div className="card">
        <div className="card-title">Pessoas cadastradas</div>
        {loading ? (
          <div>Carregando...</div>
        ) : people.length === 0 ? (
          <div className="info-text">Nenhuma pessoa cadastrada.</div>
        ) : (
          <table className="table">
            <thead>
              <tr>
                <th>Nome</th>
                <th>Idade</th>
                <th>ID</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {people.map(p => (
                <tr key={p.id}>
                  <td>{p.name}</td>
                  <td>{p.age}</td>
                  <td>{p.id}</td>
                  <td>
                    <button className="btn-danger" onClick={() => handleDelete(p.id)}>
                      Deletar
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </>
  );
};

export default PeopleSection;
