using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web
{
    public class Conversions
    {
        private Conversions()
        {
        }

        /// <summary>
        /// Converts inches to centimeters
        /// </summary>
        /// <param name="inches">Inches to convert</param>
        /// <returns>Inches converted to centimeters</returns>
        public static decimal InchesToCentimeters(decimal inches)
        {
            decimal centimeters = 0m;
            centimeters = inches * 2.54m;
            return centimeters;
        }

        /// <summary>
        /// Converts centimeters to inches
        /// </summary>
        /// <param name="centimeters">Centimeters to convert</param>
        /// <returns>Centimeters converted to inches</returns>
        public static decimal CentimetersToInches(decimal centimeters)
        {
            decimal inches = 0m;
            inches = centimeters * 0.3937m;
            return inches;
        }

        /// <summary>
        /// Converts pounds to kilograms
        /// </summary>
        /// <param name="pounds">Pound amount to convert</param>
        /// <returns>Pounds converted to Kilograms</returns>
        public static decimal PoundsToKilograms(decimal pounds)
        {
            decimal kilograms = 0m;
            kilograms = pounds * 2.2046m;
            return kilograms;
        }

        /// <summary>
        /// Converts Kilograms to Pounds		
        /// </summary>
        /// <param name="kilograms">Kilogram amount to convert</param>
        /// <returns>Kilograms converted to pounds</returns>
        public static decimal KilogramsToPounds(decimal kilograms)
        {
            decimal pounds = 0m;
            pounds = kilograms * 0.4536m;
            return pounds;
        }

        /// <summary>
        /// Converts the non-whole pounds portion of a decial number to ounces
        /// </summary>
        /// <param name="pounds">The decimal representation of pounds to be converted</param>
        /// <returns>Only the non-whole pound portion of the pounds converted to ounces</returns>
        public static int DecimalPoundsToOunces(decimal pounds)
        {
            // Get only Partial Pounds
            decimal remainder = 0m;
            remainder = pounds % 1;

            decimal ounces = 0m;
            ounces = remainder * 16;
            ounces = Math.Round(ounces, 0);
            return (int)ounces;
        }

        // Converts ounces to decimal pounds
        public static decimal OuncesToDecimalPounds(decimal ounces)
        {
            decimal result = (ounces / 16.0m);             
            return result;
        }
    }
}
