﻿using System;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BadBehavior.Util;

namespace BadBehavior.Logging
{
    /* ====== LogQuery class ====== */

    /// <summary>
    ///  Contains the criteria by which we are to filter, sort and page through the logs.
    /// </summary>

    public class LogQuery
    {
        private const int DEFAULT_PAGE_SIZE = 20;

        private string _filter;
        private string _sort;

        /// <summary>
        ///  Creates a new instance of the <see cref="LogQuery"/> class.
        /// </summary>

        public LogQuery()
        {
            PageNumber = 1;
            PageSize = DEFAULT_PAGE_SIZE;
        }

        /// <summary>
        ///  Creates a new instance of the <see cref="LogQuery"/> class,
        ///  based on the query string for an HTTP request.
        /// </summary>
        /// <param name="queryString">
        ///  The query string.
        /// </param>

        public LogQuery(NameValueCollection queryString)
        {
            this.Filter = queryString["filter"];
            this.FilterValue = queryString["filtervalue"];
            this.Sort = queryString["sort"];
            this.Ascending = queryString["order"] != "desc";
            int i;
            if (Int32.TryParse(queryString["page"], out i))
                this.PageNumber = i;
            else
                this.PageNumber = 1;
            if (Int32.TryParse(queryString["pageSize"], out i))
                this.PageSize = i;
            else
                this.PageSize = DEFAULT_PAGE_SIZE;
        }

        /// <summary>
        ///  The number of the page to return.
        /// </summary>

        public int PageNumber { get; set; }

        /// <summary>
        ///  The number of records to return on each page.
        /// </summary>

        public int PageSize { get; set; }

        /// <summary>
        ///  The name of the column on which to filter, or null if none.
        /// </summary>

        public string Filter {
            get { return _filter; }
            set
            {
                value.AssertSafe();
                _filter = value;
            }
        }

        /// <summary>
        ///  The value of the column to be filtered, or null if none.
        /// </summary>

        public string FilterValue { get; set; }

        /// <summary>
        ///  The column to sort on, or null for the default.
        /// </summary>

        public string Sort
        {
            get { return _sort; }
            set
            {
                value.AssertSafe();
                _sort = value;
            }
        }

        /// <summary>
        ///  true to sort in ascending order, otherwise descending.
        /// </summary>

        public bool Ascending { get; set; }

        /// <summary>
        ///  Converts the <see cref="LogQuery"/> instance to a query string format.
        /// </summary>
        /// <returns>
        ///  A query string representation of the log query.
        /// </returns>

        public override string ToString()
        {
            var collection = new NameValueCollection();
            if (Filter != null) collection.Add("filter", Filter);
            if (FilterValue != null) collection.Add("filtervalue", FilterValue);
            if (Sort != null) collection.Add("sort", Sort);
            if (!Ascending) collection.Add("order", "desc");
            if (PageNumber != 1) collection.Add("page", PageNumber.ToString());
            if (PageSize != DEFAULT_PAGE_SIZE) collection.Add("pageSize", PageSize.ToString());

            var sb = new StringBuilder();

            foreach (string key in collection.Keys) {
                if (sb.Length > 0) sb.Append("&");
                sb.Append(HttpUtility.UrlEncode(key));
                sb.Append("=");
                sb.Append(HttpUtility.UrlEncode(collection[key]));
            }

            return sb.ToString();
        }

        /// <summary>
        ///  Creates a copy of this log query, allowing us to construct other queries from it.
        /// </summary>
        /// <returns>
        ///  A new <see cref="LogQuery"/> instance.
        /// </returns>

        public LogQuery Clone()
        {
            return (LogQuery)this.MemberwiseClone();
        }
    }
}
