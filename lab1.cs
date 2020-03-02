using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace SandBox
{
    enum Operation { D, E }

    abstract class Cipher
    {
        public const int m = 33;
        public string alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        public string text;
        public Cipher(string text) {
            this.text = text;
        }
        public string DoOperation(Operation op)
        {
            if (op == Operation.D) { D(); }
            else { E(); }
            return text;
        }
        abstract public void D();
        abstract public void E();
    }

    class Affine : Cipher
    {
        private readonly int a;
        private readonly int b;
        public Affine(string text, int a = 31, int b = 25) : base (text)
        {
            if (a < 0 || b < 0) { throw new Exception(); }
            this.a = a;
            this.b = b;
        }
        public override void D()
        {
            int x, y, gcd = Gcd(a, m, out x,out y);
            if (gcd != 1) { throw new Exception(); }
            int a_rev = x;
            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                result += alphabet[a_rev *(alphabet.IndexOf(text[i]) - b + m) % m];
            }
            text = result;
        }
        public override void E()
        {
            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                result += alphabet[(a * alphabet.IndexOf(text[i]) + b) % m];
            }
            text = result;
        }
        public int Gcd(int a, int b, out int x, out int y)
        {
            if (b < a)
            {
                var t = a;
                a = b;
                b = t;
            }

            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            int gcd = Gcd(b % a, a, out x, out y);

            int newY = x;
            int newX = y - (b / a) * x;

            x = newX;
            y = newY;
            return gcd;
        }
    }

    class Vijner : Cipher
    {
        private readonly string key = "";
        public Vijner(string text, string rawKey) : base(text)
        {
            int i = 0;
            while (key.Length != text.Length)
            {
                key += rawKey[i];
                i++; i = i % rawKey.Length;
            }
        }   
        public override void E()
        {
            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                result += alphabet[(alphabet.IndexOf(text[i]) + alphabet.IndexOf(key[i])) % m];
            }
            text = result;
        }
        public override void D()
        {
            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                result += alphabet[(alphabet.IndexOf(text[i]) - alphabet.IndexOf(key[i]) + m) % m];
            }
            text = result;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            try
            {
                Cipher cipher = new Affine("НАРУШЕНИЕ");
                Console.WriteLine(cipher.DoOperation(Operation.E));
                Console.WriteLine(cipher.DoOperation(Operation.D));

                cipher = new Vijner("КУЙАНЬМТ", "КДЕФС");
                Console.WriteLine(cipher.DoOperation(Operation.D));
                Console.WriteLine(cipher.DoOperation(Operation.E));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Exception");
            }
        }
    }
}
