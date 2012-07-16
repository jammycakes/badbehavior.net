using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BadBehavior.Logging.SqlServer
{
    public class ReaderRepository : RepositoryBase
    {
        public ReaderRepository() : base() { }

        public ReaderRepository(string connectionString) : base(connectionString) { }


        private static string GetWhereClause(LogQuery query)
        {
            if (!String.IsNullOrEmpty(query.Filter) && !String.IsNullOrEmpty(query.FilterValue))
                return " where [" + query.Filter + "]=@filter";
            else
                return String.Empty;
        }

        private static string GetOrderByClause(LogQuery query)
        {
            if (!String.IsNullOrEmpty(query.Sort))
                return " order by [" + query.Sort + "]";
            else
                return String.Empty;
        }

        private static SqlParameter[] GetParameters(LogQuery query)
        {
            if (!String.IsNullOrEmpty(query.Filter) && !String.IsNullOrEmpty(query.FilterValue))
                return new SqlParameter[] { new SqlParameter("@filter", query.FilterValue) };
            else
                return new SqlParameter[0];
        }

        public int Count(LogQuery query)
        {
            string sql = "select count(*) from BadBehavior_Log" + GetWhereClause(query);
            return base.ExecuteScalar<int>(sql, GetParameters(query));
        }
    }
}
