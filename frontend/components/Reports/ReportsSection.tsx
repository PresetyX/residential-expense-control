import React, { useEffect, useState } from 'react';
import apiClient from '../../services/api';
import {
  ApiResponse,
  CategoryTotal,
  CategoryTotalReport,
  PersonTotal,
  PersonTotalReport,
} from '../../types';

/**
 * Seção de relatórios.
 * Consome os endpoints:
 * - GET /api/relatorios/por-pessoa
 * - GET /api/relatorios/por-categoria
 */
const ReportsSection: React.FC = () => {
  const [personReport, setPersonReport] = useState<PersonTotalReport | null>(null);
  const [categoryReport, setCategoryReport] = useState<CategoryTotalReport | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadReports = async () => {
    setLoading(true);
    setError(null);
    try {
      const [personRes, catRes] = await Promise.all([
        apiClient.get<ApiResponse<PersonTotalReport>>('/relatorios/por-pessoa'),
        apiClient.get<ApiResponse<CategoryTotalReport>>('/relatorios/por-categoria'),
      ]);

      if (personRes.data.success && personRes.data.data) {
        setPersonReport(personRes.data.data);
      } else {
        setError(personRes.data.message || 'Erro ao carregar totais por pessoa');
      }

      if (catRes.data.success && catRes.data.data) {
        setCategoryReport(catRes.data.data);
      } else {
        setError(prev => prev ?? catRes.data.message ?? 'Erro ao carregar totais por categoria');
      }
    } catch (err: any) {
      setError(err.response?.data?.message ?? 'Erro inesperado ao carregar relatórios');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadReports();
  }, []);

  const renderPersonRow = (p: PersonTotal) => (
    <tr key={p.personId}>
      <td>{p.personName}</td>
      <td>R$ {p.totalIncome.toFixed(2)}</td>
      <td>R$ {p.totalExpense.toFixed(2)}</td>
      <td>R$ {p.balance.toFixed(2)}</td>
    </tr>
  );

  const renderCategoryRow = (c: CategoryTotal) => (
    <tr key={c.categoryId}>
      <td>{c.categoryDescription}</td>
      <td>R$ {c.totalIncome.toFixed(2)}</td>
      <td>R$ {c.totalExpense.toFixed(2)}</td>
      <td>R$ {c.balance.toFixed(2)}</td>
    </tr>
  );

  return (
    <>
      <div className="card">
        <div className="card-title">Relatório de Totais por Pessoa</div>
        <div className="card-description">
          Lista todas as pessoas com o total de receitas, despesas e saldo individual,
          além dos totais gerais de todas as pessoas.
        </div>
        {loading && !personReport ? (
          <div>Carregando...</div>
        ) : !personReport ? (
          <div className="info-text">Nenhum dado disponível.</div>
        ) : (
          <>
            <table className="table">
              <thead>
                <tr>
                  <th>Pessoa</th>
                  <th>Total Receitas</th>
                  <th>Total Despesas</th>
                  <th>Saldo</th>
                </tr>
              </thead>
              <tbody>
                {personReport.people.map(renderPersonRow)}
                <tr className="totals-row">
                  <td>Total Geral</td>
                  <td>R$ {personReport.grandTotalIncome.toFixed(2)}</td>
                  <td>R$ {personReport.grandTotalExpense.toFixed(2)}</td>
                  <td>R$ {personReport.grandTotalBalance.toFixed(2)}</td>
                </tr>
              </tbody>
            </table>
          </>
        )}
      </div>

      <div className="card">
        <div className="card-title">Relatório de Totais por Categoria</div>
        <div className="card-description">
          Lista todas as categorias com o total de receitas, despesas e saldo,
          além dos totais gerais de todas as categorias.
        </div>
        {loading && !categoryReport ? (
          <div>Carregando...</div>
        ) : !categoryReport ? (
          <div className="info-text">Nenhum dado disponível.</div>
        ) : (
          <>
            <table className="table">
              <thead>
                <tr>
                  <th>Categoria</th>
                  <th>Total Receitas</th>
                  <th>Total Despesas</th>
                  <th>Saldo</th>
                </tr>
              </thead>
              <tbody>
                {categoryReport.categories.map(renderCategoryRow)}
                <tr className="totals-row">
                  <td>Total Geral</td>
                  <td>R$ {categoryReport.grandTotalIncome.toFixed(2)}</td>
                  <td>R$ {categoryReport.grandTotalExpense.toFixed(2)}</td>
                  <td>R$ {categoryReport.grandTotalBalance.toFixed(2)}</td>
                </tr>
              </tbody>
            </table>
          </>
        )}
        {error && <div className="error-text">{error}</div>}
      </div>
    </>
  );
};

export default ReportsSection;
