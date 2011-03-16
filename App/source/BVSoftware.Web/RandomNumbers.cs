using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace BVSoftware.Web
{
    public class RandomNumbers
    {

        // Min value will alway be zero based
        public static int RandomInteger(int MaxValue)
        {
            // Create a byte array to hold the random value.
            byte[] randomNumber = new byte[1];

            // Create a new instance of the RNGCryptoServiceProvider.
            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

            // Fill the array with a random value.
            Gen.GetBytes(randomNumber);

            // Convert the byte to an integer value to make the modulus operation easier.
            int rand = Convert.ToInt32(randomNumber[0]);

            // Return the random number mod the number max value
            if (MaxValue != 0)
            {
                return rand % MaxValue;
            }
            else
            {
                return 0;
            }
        }

        public static int RandomInteger(int MaxValue, int MinValue)
        {
            int maxZeroBased = MaxValue - MinValue;
            int zeroBased = RandomInteger(maxZeroBased);
            return zeroBased + MinValue;
        }
       
    }
}

