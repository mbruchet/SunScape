using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace SunScape.Data
{
    public class SqlApplicationLock : IDisposable
    {
        private readonly String _uniqueId;
        private readonly SqlConnection _sqlConnection;
        private Boolean _isLockTaken = false;

        public SqlApplicationLock(
            String uniqueId,
            String connectionString)
        {
            _uniqueId = uniqueId;
            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();
        }

        public int TakeLock(TimeSpan takeLockTimeout)
        {
            int returnValue = -1;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                SqlCommand sqlCommand = new SqlCommand("sp_getapplock", _sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = (int)takeLockTimeout.TotalSeconds;

                sqlCommand.Parameters.AddWithValue("Resource", _uniqueId);
                sqlCommand.Parameters.AddWithValue("LockOwner", "Session");
                sqlCommand.Parameters.AddWithValue("LockMode", "Exclusive");
                sqlCommand.Parameters.AddWithValue("LockTimeout", (Int32)takeLockTimeout.TotalMilliseconds);

                SqlParameter returnValueP = sqlCommand.Parameters.Add("ReturnValue", SqlDbType.Int);
                returnValueP.Direction = ParameterDirection.ReturnValue;
                sqlCommand.ExecuteNonQuery();

                returnValue = Convert.ToInt32(returnValueP.Value);

                transactionScope.Complete();
            }

            return returnValue;
        }

        public void ReleaseLock()
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                SqlCommand sqlCommand = new SqlCommand("sp_releaseapplock", _sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("Resource", _uniqueId);
                sqlCommand.Parameters.AddWithValue("LockOwner", "Session");

                sqlCommand.ExecuteNonQuery();
                _isLockTaken = false;
                transactionScope.Complete();
            }
        }

        public void Dispose()
        {
            if (_isLockTaken)
            {
                ReleaseLock();
            }
            _sqlConnection.Close();
        }
    }
}
