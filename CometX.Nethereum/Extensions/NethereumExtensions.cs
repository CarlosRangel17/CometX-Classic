using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using CometX.Nethereum.Models;

namespace CometX.Nethereum.Extensions
{
    public static class NethereumExtensions
    {
        public static string DecimalToHexadecimal(this int number)
        {
            var hex = "";
            var result = new List<byte> { 0 };

            var sNumber = number.ToString() + "000000000000000000";
            foreach (char c in sNumber)
            {
                var val = (int)(c - '0');
                for (int i = 0; i < result.Count; i++)
                {
                    int digit = result[i] * 10 + val;
                    result[i] = (byte)(digit & 0x0F);
                    val = digit >> 4;
                }

                if (val != 0) result.Add((byte)val);
            }

            result.ForEach(b => hex = "0123456789ABCDEF"[b] + hex);

            return hex.HexToHexadecimal();
        }

        public static string HexToHexadecimal(this string number)
        {
            return "00000000000000000000000000000000000000000000000" + number;
        }

        public static async Task<RopstenRequest> GetAsyncRopstenRequest(this string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var jsonString = await reader.ReadToEndAsync();
                var ropstenRequest = JsonConvert.DeserializeObject<RopstenRequest>(jsonString);
                return ropstenRequest;
            }
        }
    }
}
