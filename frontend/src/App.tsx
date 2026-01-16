import React, { useState } from 'react';
import PeopleSection from './components/People/PeopleSection';
import CategoriesSection from './components/Categories/CategoriesSection';
import TransactionsSection from './components/Transactions/TransactionsSection';
import ReportsSection from './components/Reports/ReportsSection';

/**
 * Enum interno para representar as abas da aplicação.
 */
type Tab = 'people' | 'categories' | 'transactions' | 'reports';

/**
 * Componente raiz da aplicação.
 * Responsável por:
 * - Exibir o cabeçalho
 * - Controlar qual aba está ativa
 * - Renderizar a seção correspondente (pessoas, categorias, transações, relatórios)
 */
const App: React.FC = () => {
  const [activeTab, setActiveTab] = useState<Tab>('people');

  return (
    <div className="app-container">
      <h1 className="app-title">Controle de Gastos Residenciais</h1>
      <p className="app-subtitle">
        Cadastre pessoas, categorias, registre transações e visualize totais por pessoa e categoria.
      </p>

      {/* Navegação por abas */}
      <div className="tabs">
        <button
          className={`tab-button ${activeTab === 'people' ? 'tab-active' : ''}`}
          onClick={() => setActiveTab('people')}
        >
          Pessoas
        </button>
        <button
          className={`tab-button ${activeTab === 'categories' ? 'tab-active' : ''}`}
          onClick={() => setActiveTab('categories')}
        >
          Categorias
        </button>
        <button
          className={`tab-button ${activeTab === 'transactions' ? 'tab-active' : ''}`}
          onClick={() => setActiveTab('transactions')}
        >
          Transações
        </button>
        <button
          className={`tab-button ${activeTab === 'reports' ? 'tab-active' : ''}`}
          onClick={() => setActiveTab('reports')}
        >
          Relatórios
        </button>
      </div>

      {/* Renderização condicional das seções */}
      {activeTab === 'people' && <PeopleSection />}
      {activeTab === 'categories' && <CategoriesSection />}
      {activeTab === 'transactions' && <TransactionsSection />}
      {activeTab === 'reports' && <ReportsSection />}
    </div>
  );
};

export default App;
