using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Diagnostics;

namespace SandBox
{
    class LFSR
    {
        
        public string C { get; private set; }
        public string S { get; private set; }
    
        public LFSR (string c, string s)
        {
            C = c;
            S = s;
        }
        public string GetNext()
        {
            int L = S.Length;
            string output = S[L-1] + "";
            string next_S = "";

            int CharToInt(char ch) => ch - '0';
            bool CharToBool(char ch) => ch == '1';
            bool IntToBool(int i) => i == 1;

            
            next_S += CharToInt(S[L -1]) * CharToInt(C[L -1]);
            for (int i = L - 2; i >= 0; i--)
            {
                next_S += Convert.ToInt32(IntToBool(CharToInt(S[L - 1]) * CharToInt(C[i])) ^ CharToBool(S[L - i - 2]));
            }
            S = next_S;
            
            return output;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();

                string HexToBin(string hexStr) => Convert.ToString(Convert.ToInt32(hexStr, 16), 2);
                //LFSR one = new LFSR(HexToBin("0x400062"), HexToBin("0x6DB8F9"));
                //LFSR two = new LFSR(HexToBin("0x100000DF"), HexToBin("0x167EE042"));
                //LFSR three = new LFSR(HexToBin("0x40000097"), HexToBin("0x55B7BC02"));

                LFSR one = new LFSR(HexToBin("0x400051"), HexToBin("0x6980D6"));
                LFSR two = new LFSR(HexToBin("0x100000C8"), HexToBin("0x1016FD2E"));
                LFSR three = new LFSR(HexToBin("0x4000007F"), HexToBin("0x732DB497"));

                bool StrToBool(string str) => Convert.ToInt32(str) == 1;
                bool IntToBool(int i) => i == 1;
                int n = 32; //1250000

                string getGamma()
                {
                    string one_next = one.GetNext();
                    string two_next = two.GetNext();
                    string three_next = three.GetNext();
                    bool part1() => IntToBool(Convert.ToInt32(one_next) * Convert.ToInt32(two_next));
                    bool part2() => IntToBool(Convert.ToInt32(StrToBool(one_next) ^ true) * Convert.ToInt32(three_next));
                    return Convert.ToInt32(part1() ^ part2()).ToString();
                }

                string gammaSeq = "";
                for (int i = 0; i < n; i++)
                {
                    gammaSeq += getGamma();
                }
                string path = @"C:\Users\user\Desktop\result.txt";
                File.WriteAllText(path, string.Empty);//but you can use: File.Delete(path);
                //File.WriteAllText(path, gammaSeq);
                using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                {
                    writer.Write(10);
                    writer.Write(25);
                    writer.Write(31);
                    writer.Write(5235);
                    writer.Write(2);
                    writer.Write(512);

                }

                timer.Stop();
                Console.WriteLine((timer.ElapsedMilliseconds / 100.0).ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Exception");
            }
        }
    }
}
