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
        public string condition;
        public string s;
        public string C { get; private set; }
        public string S
        {
            get
            {
                return s;
            }
            set
            {
                condition = value;
                s = new string(value.Reverse().ToArray());
            }
        }
        public LFSR (string c, string s)
        {
            C = c;
            S = s;
        }
        public string GetNext()
        {
            string output = S[0] + "";
            string next_S_rev = "";

            int CharToInt(char ch) => ch - '0';
            bool CharToBool(char ch) => ch == '1';
            bool IntToBool(int i) => i == 1;

            for (int i = 0; i <= S.Length - 2; i++)
            {
                next_S_rev += Convert.ToInt32(IntToBool(CharToInt(S[0]) * CharToInt(C[i])) ^ CharToBool(S[i + 1]));
            }
            next_S_rev += CharToInt(S[0]) * CharToInt(C[C.Length - 1]);


            S = new string(next_S_rev.Reverse().ToArray());


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
                Stopwatch sw = new Stopwatch();
                sw.Start();

                string HexToBin(string hexStr) => Convert.ToString(Convert.ToInt32(hexStr, 16), 2);
                LFSR one = new LFSR(HexToBin("0x400062"), HexToBin("0x6DB8F9"));
                LFSR two = new LFSR(HexToBin("0x100000DF"), HexToBin("0x167EE042"));
                LFSR three = new LFSR(HexToBin("0x40000097"), HexToBin("0x55B7BC02"));

                int CharToInt(char ch) => ch - '0';
                bool CharToBool(char ch) => ch == '1';
                bool StrToBool(string str) => Convert.ToInt32(str) == 1;
                bool IntToBool(int i) => i == 1;

                int n = 1250000; //1250000
                //string part2() => ()
                string getGamma() {
                    string one_next = one.GetNext();
                    string two_next = two.GetNext();
                    string three_next = three.GetNext();
                    bool part1() => IntToBool(Convert.ToInt32(one_next) * Convert.ToInt32(two_next));
                    bool part2() => IntToBool(Convert.ToInt32(StrToBool(one_next) ^ true) * Convert.ToInt32(three_next));
                    return Convert.ToInt32(part1()^part2()).ToString();
                }

                string gammaSeq = "";
                for (int i = 0; i < n; i++)
                {
                    gammaSeq += getGamma();
                }

                string path = @"C:\Users\user\Desktop\result.txt";
                File.WriteAllText(path, string.Empty);//but you can use: File.Delete(path);
                using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
                {
                    writer.Write(gammaSeq.Select(item => Convert.ToByte(item)).ToArray());
                }

                
                
                sw.Stop();
                Console.WriteLine(((sw.ElapsedMilliseconds / 100.0).ToString()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Exception");
            }
        }
    }
}
