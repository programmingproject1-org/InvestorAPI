using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// Contains the result of a pages query operation.
    /// </summary>
    /// <typeparam name="T">The type of the result items.</typeparam>
    public class ListResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListResult{T}"/> class.
        /// </summary>
        /// <param name="items">The citems.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRowCount">The total row count.</param>
        public ListResult(IReadOnlyCollection<T> items, int pageNumber, int pageSize, int totalRowCount)
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRowCount = totalRowCount;
            TotalPageCount = (int)Math.Ceiling((double)totalRowCount / (double)pageSize);
        }

        /// <summary>
        /// Gets the (double)items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IReadOnlyCollection<T> Items { get; private set; }

        /// <summary>
        /// Gets the page number.
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Gets the total page count.
        /// </summary>
        public int TotalPageCount { get; private set; }

        /// <summary>
        /// Gets the total row count.
        /// </summary>
        public int TotalRowCount { get; private set; }

        /// <summary>
        /// Converts the specified result to a different type.
        /// </summary>
        /// <typeparam name="TOut">The type of the output item.</typeparam>
        /// <param name="converter">The converter function.</param>
        /// <returns>The output result items.</returns>
        public ListResult<TOut> Convert<TOut>(Func<T, TOut> converter)
        {
            var convertedItems = Items.Select(item => converter(item)).ToList();
            return new ListResult<TOut>(convertedItems, PageNumber, PageSize, TotalRowCount);
        }
    }
}
