using System;
using System.Collections.Generic;
using System.Text;
using BadBehavior.Logging;

namespace BadBehavior.Admin
{
    public class Pager
    {
        private Uri baseUrl;
        private LogQuery baseQuery;
        private LogResultSet results;

        public Pager(Uri baseUrl, LogQuery baseQuery, LogResultSet results)
        {
            this.baseUrl = baseUrl;
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

            var sb = new StringBuilder();
            foreach (var number in GetPageNumbers()) {
                if (sb.Length > 0) sb.Append(" ");
                if (number == results.Page)
                    sb.AppendFormat("<strong>{0}</strong>", number);
                else if (number == 0)
                    sb.Append("...");
                else {
                    var query = this.baseQuery.Clone();
                    query.PageNumber = number;
                    var uri = new Uri(baseUrl, "?" + query.ToString());
                    sb.AppendFormat("<a href=\"{0}\">[{1}]</a>", uri, number);
                }
            }

            return sb.ToString();
        }
    }
}
