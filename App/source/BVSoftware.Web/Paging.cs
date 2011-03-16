using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web
{
    public class Paging
    {
        public static int TotalPages(int totalRecords, int pageSize)
        {
            int result = 1;

            if (totalRecords < pageSize)
            {
                return result;
            }
            else
            {                
                int wholePages = (int)Math.Floor((double)totalRecords / (double)pageSize);
                int partialPages = (int)((double)totalRecords % (double)pageSize);
                if (partialPages > 0)
                {
                    result = wholePages + 1;
                }
                else
                {
                    result = wholePages;
                }
            }

            return result;
        }

        public static int StartRowIndex(int pageNumber, int pageSize)
        {
            int pageIndex = pageNumber - 1;
            if (pageIndex < 0) pageIndex = 0;
            return pageIndex * pageSize;
        }

        public static string RenderPager(string link, int currentPage, int totalRecords, int pageSize)
        {
            return RenderPager(link, currentPage, TotalPages(totalRecords, pageSize));
        }
        
        public static string RenderPager(string link, int currentPage, int pages)
        {
            return RenderPagerWithLimits(link, currentPage, pages, 10);
        }

        public static string RenderPagerWithLimits(string link, int currentPage, int totalRecords, int pageSize, int maxPagesToShow)
        {
            return RenderPagerWithLimits(link, currentPage, TotalPages(totalRecords, pageSize), maxPagesToShow);
        }
        public static string RenderPagerWithLimits(string link, int currentPage, int pages, int maxPagesToShow)
        {
            // don't render a pager if we don't need one
            if (pages < 2) return string.Empty;

            // Make sure current page is within limits
            int current = currentPage;
            if (current < 1) current = 1;
            if (current > pages) current = pages;

            // Truncate to 10 pages at a time
            decimal limitToShow = (decimal)maxPagesToShow;            
            decimal pageOfPages = Math.Floor((decimal)(currentPage-1) / limitToShow);
            int startPage = (int)((limitToShow * pageOfPages) + 1);
            int endingPage = startPage + ((int)limitToShow - 1);
            if (endingPage > pages) endingPage = pages;

            
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"pager\"><ul>");

            // Render previous page groups
            if (startPage > limitToShow)
            {
                sb.Append("<li><a href=\"" + string.Format(link, (startPage - 1).ToString()) + "\">...</a></li>");
            }

            for (int i = startPage; i <= endingPage; i++)
            {
                sb.Append("<li");
                if (current == i)
                {
                    sb.Append(" class=\"current\"");
                }
                sb.Append(">");
                sb.Append("<a href=\"" + string.Format(link, i.ToString()) + "\">" + i.ToString() + "</a></li>");
            }

            // Render more pages if available
            if (pages > endingPage)
            {
                sb.Append("<li><a href=\"" + string.Format(link, (endingPage + 1).ToString()) + "\">...</a></li>");
            }

            sb.Append("</ul></div>");

            return sb.ToString();
        }
    }
}
