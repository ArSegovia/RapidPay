using System;
using System.Text;

namespace RapidPay.Utilities
{
    public static class Utils
    {
        public static string GenerateCardNumber()
        {
            Random random = new Random();

            StringBuilder bld = new StringBuilder();
            for (int i = 0; i < 15; i++)
            {
                int digit = random.Next(0, 10); 
                bld.Append(digit.ToString());
            }

            return bld.ToString();
        }
            //=> Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
    }
}
