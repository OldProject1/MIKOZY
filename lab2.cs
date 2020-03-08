using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using static SandBox.SP;

namespace SandBox
{
    class SP {
        public static string GetBinaryStr(int num, int length)
        {
            return Convert.ToString(num, 2).PadLeft(length,'0');
        }
        public static string GetStrByPos (string baseStr, int[] pos)
        {
            string result = "";
            foreach (int position in pos)
            {
                result += baseStr[position - 1];
            }
            return result;
        }
        public static string GetSquareSum(string first, string second, int length)
        {
            int result = Convert.ToInt32(first, 2) + Convert.ToInt32(second, 2);
            return GetBinaryStr(result % (int)Math.Pow(2, length), length);
        }
        public static void SplitIntoBlocks(string source, out string T1, out string T2)
        {
            T1 = source.Substring(0, source.Length / 2);
            T2 = source.Substring(source.Length / 2, source.Length / 2);
        }
        public static string P(string source)
        {
            string prt1, prt2;
            int step = 6;
            prt1 = source.Substring(0, step);
            prt2 = source.Substring(step, source.Length - step);
            return prt2 + prt1;
        }
        public static string E(string X, string k)
        {
            string k_1, k_2, k_3;
            k_1 = GetStrByPos(k, new int[] { 10, 12, 2, 5, 8, 6, 9, 4 });
            k_2 = GetStrByPos(k, new int[] { 2, 9, 10, 5, 1, 12, 6, 4 });
            k_3 = GetStrByPos(k, new int[] { 7, 1, 2, 6, 12, 3, 9, 11 });
            string[] roundKeys = new string[] { k_1, k_2, k_3 };
            string[] S1 = "B 5 1 9 8 D F 0 E 4 2 3 C 7 A 6".Split(' ');
            string[] S2 = "E 7 A C D 1 3 9 0 2 B 4 F 8 5 6".Split(' ');
            S1 = (from item in S1 select GetBinaryStr(Convert.ToInt32(item, 16), 4)).ToArray();
            S2 = (from item in S2 select GetBinaryStr(Convert.ToInt32(item, 16), 4)).ToArray();
            for (int i = 0; i < 3; i++)
            {
                X = GetSquareSum(X, roundKeys[i], X.Length);
                string T1, T2;
                SplitIntoBlocks(X, out T1, out T2);
                string N1, N2;
                N1 = S1[Convert.ToInt32(T1, 2)];
                N2 = S2[Convert.ToInt32(T2, 2)];
                X = N1 + N2;
                X = P(X);
                Console.WriteLine("X = " + X);
            }
            return X;
        }
    }
    class MyClass {
        static void Main(string[] args)
        {
            try
            {
                int N = 14;
                int q = 4, r = 9;
                string X = GetBinaryStr(7 * N, 8), 
                    key = GetBinaryStr(4096 - 11 * q * r, 12);
                Console.WriteLine("X = " + X);
                Console.WriteLine(key);
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
