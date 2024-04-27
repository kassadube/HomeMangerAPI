using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Toolkit.Extensions;
using Toolkit.Serialization;
using Dapper;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace HomeManger.Data
{
    public class BaseRepository
    {
        private string? _connectionString;
        private string? _sqliteConnectionString;
        private int _timeout;
        readonly IConfiguration _configuration;
        public ILogger _logger;

        public BaseRepository(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        #region CONNECTION_STRING
        protected string? ConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(_connectionString)) return _connectionString;
                _connectionString = _configuration.GetConnectionString("FleetConnection")!;
                return _connectionString;
            }
        }
        protected string? SqliteConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(_sqliteConnectionString)) return _sqliteConnectionString;
                _sqliteConnectionString = _configuration.GetConnectionString("SqliteConnection")!;
                return _sqliteConnectionString;
            }
        }      
       

        protected int Timeout
        {
            get
            {
                if (_timeout != default(int)) return _timeout;
                _timeout = _configuration.GetSection("AppSettings")["DB_TIMEOUT"].StringToInt();
                return _timeout;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandType"></param>
        /// <param name="sql"></param>
        /// <param name="commandParams"></param>
        /// <returns></returns>
        public List<T> GetTable<T>(CommandType commandType, string sql, SqlParameterList commandParams) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _logger.LogTrace("Before Execute {@sql} {@commandParams}", sql, commandParams.Params);
            List<T> result = new List<T>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, commandType, sql, commandParams.ToArray()))
                {
                    while (dr.Read())
                    {
                        result.Add(Serializer.DeSerialize<T>(dr));
                    }
                }

            }
            stopwatch.Stop();
            _logger.LogTrace($"After Execute {sql}, time = {stopwatch.Elapsed}");
            return result;
        }
        public List<T> GetTableDapper<T>(CommandType commandType, string sql, object commandParams) where T : class
        {
            _logger.LogTrace("Before Execute {@sql} {@commandParams}", sql, commandParams);
            List<T> result = new List<T>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using (var conn = new SqlConnection(ConnectionString))
            {
                var qResult = conn.Query<T>(sql, commandParams, commandType: commandType, commandTimeout: SqlHelper.TIMEOUT);
                result = qResult.ToList();
            }
            stopwatch.Stop();
            _logger.LogTrace($"After Execute {sql}, time = {stopwatch.Elapsed}");
            return result;
        }

        public int ExecuteDapper(CommandType commandType, string sql, object commandParams)
        {
            _logger.LogTrace("Before Execute {@sql} {@commandParams}", sql, commandParams);
            int result = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using (var conn = new SqlConnection(ConnectionString))
            {
                var qResult = conn.Execute(sql, commandParams, commandType: commandType, commandTimeout: SqlHelper.TIMEOUT);
                result = qResult;
            }
            stopwatch.Stop();
            _logger.LogTrace($"After Execute {sql}, time = {stopwatch.Elapsed}, res = {result}");
            return result;
        }
        public T GetTableItemDapper<T>(CommandType commandType, string sql, object commandParams) where T : class
        {
            _logger.LogTrace("Before Execute {@sql} {@commandParams}", sql, commandParams);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            T result;
            using (var conn = new SqlConnection(ConnectionString))
            {
                var qResult = conn.QueryFirstOrDefault<T>(sql, commandParams, commandType: commandType, commandTimeout: SqlHelper.TIMEOUT);
                result = qResult;
            }
            stopwatch.Stop();
            _logger.LogTrace($"After Execute {sql}, time = {stopwatch.Elapsed}");
            return result;
        }

        public List<T> GetTable<T>(string sql, object commandParams) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _logger.LogTrace("Before Execute {@sql} {@commandParams}", sql, commandParams);
            List<T> result = new List<T>();
            using (var conn = new SqliteConnection(SqliteConnectionString))
            {

                var qResult = conn.Query<T>(sql, commandParams, commandTimeout: SqlHelper.TIMEOUT);
                result = qResult.ToList();
                conn.Close();
            }
            stopwatch.Stop();
            _logger.LogTrace($"After Execute {sql}, time = {stopwatch.Elapsed}");
            return result;
        }
        public T GetTableItem<T>(string sql, object commandParams) where T : class
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _logger.LogTrace($"Before Execute {sql}");
            T result;
            using (var conn = new SqliteConnection(SqliteConnectionString))
            {
                var qResult = conn.QueryFirstOrDefault<T>(sql, commandParams, commandTimeout: SqlHelper.TIMEOUT);
                result = qResult;
                conn.Close();
            }
            stopwatch.Stop();
            _logger.LogTrace($"After Execute {sql}, time = {stopwatch.Elapsed}");
            return result;
        }

    }
}
