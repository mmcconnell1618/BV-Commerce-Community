using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Migration
{
    public static class LineItemHelper
    {
        public static void SplitTaxesAcrossItems(decimal taxTotal, decimal subTotal, List<MigrationServices.LineItemDTO> items)
        {
            // Total Tax for all items on this schedule is calculated
            // Now, we assign the tax parts to each line item based on their
            // linetotal value. The last item should get the remainder of the tax
            decimal RoundedTotal = Math.Round(taxTotal, 2);

            decimal TotalApplied = 0M;

            for (int i = 0; i < items.Count(); i++)
            {
                MigrationServices.LineItemDTO li = items[i];

                li.TaxPortion = 0m;                
                if (i == items.Count() - 1)
                {
                    // last item
                    li.TaxPortion = (RoundedTotal - TotalApplied);
                }
                else
                {
                    decimal percentOfTotal = 0;
                    if (LineTotalForItem(li) != 0)
                    {
                        percentOfTotal = LineTotalForItem(li) / subTotal;
                    }
                    decimal part = Math.Round(percentOfTotal * subTotal, 2);
                    li.TaxPortion = part;
                    TotalApplied += part;
                }
            }
        }

        public static decimal LineTotalForItem(MigrationServices.LineItemDTO li)
        {                        
            decimal result = li.BasePricePerItem * li.Quantity;
            result += SumUpDiscounts(li);
            return result;            
        }

        private static decimal SumUpDiscounts(MigrationServices.LineItemDTO li)
        {
            if (li.DiscountDetails != null)
            {
                return (li.DiscountDetails).Sum(y => y.Amount);
            }
            return 0;
        }

    }
}
