using System;
using System.Data;
using Npgsql;
using MarketAutomation.Models;

namespace MarketAutomation.Services
{
    /// <summary>
    /// Veritabanı işlemlerini yöneten servis sınıfı
    /// </summary>
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Veritabanı bağlantısını test eder
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// SQL sorgusu çalıştırır ve sonuçları DataTable olarak döndürür
        /// </summary>
        public DataTable ExecuteQuery(string query, params NpgsqlParameter[] parameters)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    var dataTable = new DataTable();
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    return dataTable;
                }
            }
        }

        /// <summary>
        /// SQL komutu çalıştırır ve etkilenen satır sayısını döndürür
        /// </summary>
        public int ExecuteNonQuery(string query, params NpgsqlParameter[] parameters)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Transaction içinde SQL komutlarını çalıştırır (void)
        /// </summary>
        public void ExecuteInTransaction(Action<NpgsqlConnection, NpgsqlTransaction> action)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        action(connection, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Transaction içinde SQL komutlarını çalıştırır ve değer döndürür (generic)
        /// </summary>
        public T ExecuteInTransaction<T>(Func<NpgsqlConnection, NpgsqlTransaction, T> func)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        T result = func(connection, transaction);
                        transaction.Commit();
                        return result;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
