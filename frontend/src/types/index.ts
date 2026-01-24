/**
 * Tipos e interfaces TypeScript compartilhadas em toda a aplicação
 * Estes tipos representam as estruturas de dados da API
 */

/**
 * Enum para tipos de transação
 */
export enum TransactionType {
  Expense = 0,
  Income = 1,
}

/**
 * Enum para finalidade da categoria
 */
export enum CategoryPurpose {
  Expense = 0,
  Income = 1,
  Both = 2,
}

/**
 * Interface para uma pessoa
 */
export interface Person {
  id: string;
  name: string;
  age: number;
}

/**
 * Interface para requisição de criação de pessoa
 */
export interface CreatePersonRequest {
  name: string;
  age: number;
}

/**
 * Interface para uma categoria
 */
export interface Category {
  id: string;
  description: string;
  purpose: CategoryPurpose;
}

/**
 * Interface para requisição de criação de categoria
 */
export interface CreateCategoryRequest {
  description: string;
  purpose: CategoryPurpose;
}

/**
 * Interface para uma transação
 */
export interface Transaction {
  id: string;
  description: string;
  amount: number;
  type: TransactionType;
  createdAt: string;
  personId: string;
  personName: string;
  categoryId: string;
  categoryDescription: string;
}

/**
 * Interface para requisição de criação de transação
 */
export interface CreateTransactionRequest {
  description: string;
  amount: number;
  type: TransactionType;
  personId: string;
  categoryId: string;
}

/**
 * Interface para totalização de pessoa
 */
export interface PersonTotal {
  personId: string;
  personName: string;
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

/**
 * Interface para relatório de totais por pessoa
 */
export interface PersonTotalReport {
  people: PersonTotal[];
  grandTotalIncome: number;
  grandTotalExpense: number;
  grandTotalBalance: number;
}

/**
 * Interface para totalização de categoria
 */
export interface CategoryTotal {
  categoryId: string;
  categoryDescription: string;
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

/**
 * Interface para relatório de totais por categoria
 */
export interface CategoryTotalReport {
  categories: CategoryTotal[];
  grandTotalIncome: number;
  grandTotalExpense: number;
  grandTotalBalance: number;
}

/**
 * Interface genérica para resposta da API
 */
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
  errorCode?: string;
}
