using DevExpress.Persistent.Base;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Exceptions;
using DevExpress.Xpo.DB.Helpers;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDNOriginacion.Module
{
    public class SafePostgreSqlConnectionProvider : PostgreSqlConnectionProvider, IDisposable
    {
        private Dictionary<string, string> safeColumnNameCache = new Dictionary<string, string>();
        private Dictionary<string, string> safeTableNameCache = new Dictionary<string, string>();
        private Dictionary<string, string> safeConstraintNameCache = new Dictionary<string, string>();

        private PostgreSqlConnectionProvider InnerDataStore { get; set; }
        new private string ConnectionString { get; set; }
        new private AutoCreateOption AutoCreateOption { get; set; }
        new IDbConnection Connection { get; set; }

        public SafePostgreSqlConnectionProvider(IDbConnection connection, AutoCreateOption autoCreateOption)
            : base(connection, autoCreateOption)
        {
            ConnectionString = connection.ConnectionString;
            AutoCreateOption = autoCreateOption;
            DoReconnect();
        }

        ~SafePostgreSqlConnectionProvider()
        {
            Dispose(false);
        }

        private void DoReconnect()
        {
            DoDispose(false);
            Connection = CreateConnection(ConnectionString);
            InnerDataStore = new PostgreSqlConnectionProvider(Connection, AutoCreateOption);
        }

        private void DoDispose(bool closeConnection)
        {
            if (Connection != null)
            {
                if (closeConnection)
                {
                    Connection.Close();
                    Connection.Dispose();
                }
                Connection = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                DoDispose(true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void HandleNullReferenceException(Exception ex)
        {
            if (ex == null) return;
            if (InnerDataStore.Connection.State == ConnectionState.Open)
                DoReconnect();
            else throw ex;
        }

        protected override IDataParameter CreateParameter(IDbCommand command, object value, string name)
        {
            NpgsqlParameter param = (NpgsqlParameter)base.CreateParameter(command, value, name);
            if (param.DbType == DbType.String)
                param.NpgsqlDbType = NpgsqlDbType.Citext;
            return param;
        }

        protected override string GetSqlCreateColumnTypeForString(DBTable table, DBColumn column)
        {
            return "citext";
        }

        public override string ComposeSafeColumnName(string columnName)
        {
            if (!safeColumnNameCache.ContainsKey(columnName))
                safeColumnNameCache.Add(columnName, ConvertCamelCaseName(base.ComposeSafeColumnName(columnName)));
            return safeColumnNameCache[columnName];
        }
        public override string ComposeSafeTableName(string tableName)
        {
            if (!safeTableNameCache.ContainsKey(tableName))
                safeTableNameCache.Add(tableName, ConvertCamelCaseName(base.ComposeSafeTableName(tableName)));
            return safeTableNameCache[tableName];
        }
        public override string ComposeSafeConstraintName(string constraintName)
        {
            if (!safeConstraintNameCache.ContainsKey(constraintName))
                safeConstraintNameCache.Add(constraintName, ConvertCamelCaseName(base.ComposeSafeConstraintName(constraintName)));
            return safeConstraintNameCache[constraintName];
        }

        private string ConvertCamelCaseName(string name)
        {
            return name.ToLower();
        }

        public override ModificationResult ModifyData(params ModificationStatement[] dmlStatements)
        {
            try
            {
                return InnerDataStore.ModifyData(dmlStatements);
            }
            catch (SqlExecutionErrorException ex)
            {
                HandleNullReferenceException(ex.InnerException);
            }
            return InnerDataStore.ModifyData(dmlStatements);
        }

        public override SelectedData SelectData(params SelectStatement[] selects)
        {
            try
            {
                return InnerDataStore.SelectData(selects);
            }
            catch (SqlExecutionErrorException ex)
            {
                HandleNullReferenceException(ex.InnerException);
            }
            return InnerDataStore.SelectData(selects);
        }

        public override UpdateSchemaResult UpdateSchema(bool dontCreateIfFirstTableNotExist, params DBTable[] tables)
        {
            try
            {
                return InnerDataStore.UpdateSchema(dontCreateIfFirstTableNotExist, tables);
            }
            catch (SqlExecutionErrorException ex)
            {
                HandleNullReferenceException(ex.InnerException);
            }
            return InnerDataStore.UpdateSchema(dontCreateIfFirstTableNotExist, tables);
        }

        public new static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
        {
            IDbConnection connection = new NpgsqlConnection(connectionString);
            objectsToDisposeOnDisconnect = new IDisposable[] { connection };
            return CreateProviderFromConnection(connection, autoCreateOption);
        }


        public new static void Register()
        {
            try
            {
                DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, new DataStoreCreationFromStringDelegate(CreateProviderFromString));
            }
            catch (ArgumentException e)
            {
                Tracing.Tracer.LogText(e.Message);
                Tracing.Tracer.LogText("A connection provider with the same name ( {0} ) has already been registered", XpoProviderTypeString);
            }
        }


        public new static IDataStore CreateProviderFromConnection(IDbConnection connection, AutoCreateOption autoCreateOption)
        {
            return new SafePostgreSqlConnectionProvider(connection, autoCreateOption);
        }

        public new const string XpoProviderTypeString = "SafePostgreSqlConnectionProvider";
    }
}
