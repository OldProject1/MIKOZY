using System.Linq;
using System;

namespace SandBox
{
    class MyClass
    {
        public static string GetBinaryStr(int num, int length)
        {
            return Convert.ToString(num, 2).PadLeft(length, '0');
        }
        public static string GetSquareSum(string first, string second, int length)
        {
            int result = Convert.ToInt32(first, 2) + Convert.ToInt32(second, 2);
            return GetBinaryStr(result % (int)Math.Pow(2, length), length);
        }
        public static string E(string X, string k)
        {
            string k_1, k_2, k_3;
            var GetStrByPos = (Func<string, int[], string>) ((str, arr) => new string(arr.Select(num => str[num - 1]).ToArray()));
            k_1 = GetStrByPos(k, new int[] { 10, 12, 2, 5, 8, 6, 9, 4 });
            k_2 = GetStrByPos(k, new int[] { 2, 9, 10, 5, 1, 12, 6, 4 });
            k_3 = GetStrByPos(k, new int[] { 7, 1, 2, 6, 12, 3, 9, 11 });
            string[] roundKeys = new string[] { k_1, k_2, k_3 };
            string[] S1 = "B 5 1 9 8 D F 0 E 4 2 3 C 7 A 6".Split(' ');
            string[] S2 = "E 7 A C D 1 3 9 0 2 B 4 F 8 5 6".Split(' ');
            S1 = (from item in S1 select GetBinaryStr(Convert.ToInt32(item, 16), 4)).ToArray();
            S2 = (from item in S2 select GetBinaryStr(Convert.ToInt32(item, 16), 4)).ToArray();
            var P = (Func<string, int, string>)((data, step) => data.Substring(0, step) + data.Substring(step, data.Length - step));
            for (int i = 0; i < 3; i++)
            {
                X = GetSquareSum(X, roundKeys[i], X.Length);
                string T1, T2;
                T1 = X.Substring(0, X.Length / 2);
                T2 = X.Substring(X.Length / 2, X.Length / 2);
                string N1, N2;
                N1 = S1[Convert.ToInt32(T1, 2)];
                N2 = S2[Convert.ToInt32(T2, 2)];
                X = N1 + N2;
                X = P(X, 6);
                Console.WriteLine("X = " + X);
            }
            return X;
        }
        static void Main(string[] args)
        {
            try
            {
                int N = 14;
                int q = 4, r = 9;
                string X = GetBinaryStr(7 * N, 8), key = GetBinaryStr(4096 - 11 * q * r, 12);
                Console.WriteLine("X = " + X);
                Console.WriteLine("key = " + key);
                E(X, key);
                Console.WriteLine();
                E("01100000", key);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
