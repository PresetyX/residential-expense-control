import axios, { AxiosInstance } from 'axios';

/**
 * Configuração da URL base da API
 * Por padrão, a API roda em http://localhost:5000
 */
const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

/**
 * Instância do cliente HTTP Axios com configurações padrão
 * Todas as requisições à API passam por este cliente
 */
const apiClient: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default apiClient;
