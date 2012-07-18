using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadBehavior.Logging;


namespace BadBehavior.Admin
{
    public class Pager
    {
        private LogQuery baseQuery;
        private LogResultSet results;

        public Pager(LogQuery baseQuery, LogResultSet results)
        {
            this.baseQuery = baseQuery;
            this.results = results;
        }


        public IEnumerable<int> GetPageNumbers()
        {
            yield return 1;
            if (results.TotalPages > 1) {
                var firstPage = results.Page - 2;
                var lastPage = results.Page + 2;
                if (firstPage < 2) firstPage = 2;
                if (lastPage > results.TotalPages - 1) lastPage = results.TotalPages - 1;
                if (firstPage > 2) yield return 0;
                for (int i = firstPage; i <= lastPage; i++) {
                    yield return i;
                }
                if (lastPage < results.TotalPages - 1) yield return 0;
                yield return results.TotalPages;
            }
        }


        public string GetHtml()
        {
            if (results.TotalPages == 1) return null;

            return String.Empty;
        }
    }
}
