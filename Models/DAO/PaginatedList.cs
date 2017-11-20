using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.DAO
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public int PageMax { get; private set; }
        public int StartPageIndex { get; private set; }
        public int EndPageIndex { get; private set; }


        public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            PageSize = pageSize;

            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            PageIndex = pageIndex < 1 ? 1
                : pageIndex > TotalPages ? TotalPages
                : pageIndex;


            PageMax = 8;
            StartPageIndex = Math.Max(1, PageIndex - (PageMax / 2));
            EndPageIndex = Math.Min(TotalPages, PageIndex + (PageMax / 2));

            this.AddRange(source.Skip((PageIndex - 1) * PageSize).Take(PageSize + 1).ToList());
        }

        public bool HasPreviousPage
        {
            get { return PageIndex > 1; }
        }

        public bool HasNextPage
        {
            get { return PageIndex < TotalPages; }
        }

        public string Paging(string link)
        {
            StringBuilder p = new StringBuilder();

            if (TotalCount > PageSize)
            {
                p.Append("<ul class='pagination'>");

                if (HasPreviousPage)
                {
                    if (PageIndex > PageMax / 2)
                    {
                        p.Append("<li><a href='" + link + "?page=" + (1) + "' data-page='" + (1) + "'><i class='fa fa-angle-double-left'></i></a><li>");
                    }
                    p.Append("<li><a href='" + link + "?page=" + (PageIndex - 1) + "' data-page='" + (PageIndex - 1) +  "'><i class='fa fa-angle-left'></i></a><li>");
                }

                for (int i = StartPageIndex; i <= EndPageIndex; i++)
                {
                    if (PageIndex == i)
                        p.Append("<li class='active'><a>" + i + "</a><li>");
                    else
                        p.Append("<li><a href='" + link + "?page=" + i + "' data-page='" + (i) + "'>" + i + "</a><li>");
                }

                if (PageIndex < TotalPages - (PageMax / 2))
                {
                    p.Append("<li><a>...</a><li>");
                }
                if (HasNextPage)
                {
                    p.Append("<li><a href='" + link + "?page=" + (PageIndex + 1) + "' data-page='" + (PageIndex + 1) + "'><i class='fa fa-angle-right'></i></a><li>");
                    p.Append("<li><a href='" + link + "?page=" + TotalPages + "' data-page='" + (TotalPages) + "'><i class='fa fa-angle-double-right'></i></a><li>");
                }

                p.Append("</ul>");
            }

            return p.ToString();
        }
    }
}
