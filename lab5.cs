using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System;
using static System.Math;
using System.Security.Cryptography;


namespace SandBox
{
    class Elgamal
    {
        BigInteger p, q, g;

        static BigInteger RandomBigInteger(BigInteger N)
        {
            // returns random BigInteger from 1 to N.
            Random rand = new Random();
            BigInteger result = 0;
            do
            {
                int length = (int)Math.Ceiling(BigInteger.Log(N, 2));
                int numBytes = (int)Math.Ceiling(length / 8.0);
                byte[] data = new byte[numBytes];
                rand.NextBytes(data);
                result = new BigInteger(data);
            } while (result >= N || result <= 0);
            return result;
        }
        static BigInteger binpow(BigInteger a, BigInteger m, BigInteger n)
        {
            // returns a^m (mod n)
            BigInteger res = 1;
            while (m != 0)
                if ((m & 1) != BigInteger.Zero)
                {
                    res = mod(res * a, n);
                    m--;
                }
                else
                {
                    a = mod(a * a, n);
                    m /= 2;
                }
            return res;
        }
        static BigInteger mod(BigInteger n, BigInteger d)
        {
            BigInteger result = n % d;
            if (result * d < 0)
                result += d;
            return result;
        }
        public static BigInteger Gcd(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            //x == a^(-1) (mod b)
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

            BigInteger gcd = Gcd(b % a, a, out x, out y);

            BigInteger newY = x;
            BigInteger newX = y - (b / a) * x;

            x = newX;
            y = newY;
            return gcd;
        }
        public BigInteger GetBigHash (string M)
        {
            byte[] data = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(M));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString());
            }
            return BigInteger.Parse(sBuilder.ToString());
        }

        public (BigInteger, BigInteger) Gen(BigInteger q)
        {
            this.q = q;
            BigInteger R_help, R;
            do
            {
                R_help = RandomBigInteger(2 * (q + 1) - 1);
                R = 2 * R_help;
                p = q * R + 1;
            } while (binpow(2, q * R, p) != 1 || binpow(2, R, p) == 1);
            BigInteger x;
            do
            {
                x = RandomBigInteger(p - 1);
                g = binpow(x, R, p);
            } while (g == 1);
            BigInteger d = RandomBigInteger(p - 1);
            BigInteger e = binpow(g, d, p);
            return (e, d);
        }

        public (BigInteger, BigInteger) Sign(BigInteger d, string M)
        {
            BigInteger m = GetBigHash(M);
            BigInteger k = RandomBigInteger(q - 1);
            BigInteger rev_k, y;
            BigInteger gcd = Gcd(k, q, out rev_k, out y);
            if (gcd != 1) { throw new Exception(); }
            rev_k = mod(rev_k, q);
            BigInteger r = binpow(g, k, p);
            BigInteger s = mod(rev_k * (m - d * r), q);
            return (r, s);
        }

        public string Verify(BigInteger e, string M, BigInteger r, BigInteger s)
        {
            if (r <= 0 || r >= p || s < 0 || s >= q)
            {
                return "FALSE";
            }
            BigInteger m = GetBigHash(M);
            if (mod(binpow(e, r, p) * binpow(r, s, p), p) == binpow(g, m, p))
            {
                return "TRUE";
            }
            return "FALSE";
        }
    }
    class MyClass
    {

        static void Main(string[] args)
        {
            try
            {
                string M = "I, Ivan Lozovskij, love MiKOZI";
                BigInteger q = BigInteger.Parse("228620023921267193730928153886743793396324452340577138987972760236418208443847");
                Elgamal elgamal = new Elgamal();
                (BigInteger e, BigInteger d) genOut = (-1, -1);
                (BigInteger r, BigInteger s) signOut = (-1, -1);
                bool isContinued = true;
                while (isContinued)
                {
                    Console.WriteLine("choose operation: ");
                    Console.WriteLine("1) Gen      2) Sign     3) Verify     4) Exit");
                    int op = Convert.ToInt32(Console.ReadLine());
                    switch (op)
                    {
                        case 1:
                            genOut = elgamal.Gen(q);
                            Console.WriteLine($"(e, d) = ({genOut.e}, {genOut.d})");
                            break;
                        case 2:
                            if (genOut.d == -1) { throw new Exception(); }
                            signOut = elgamal.Sign(genOut.d, M);
                            Console.WriteLine($"(r, s) = ({signOut.r}, {signOut.s})");
                            break;
                        case 3:
                            if (signOut.r == -1) { throw new Exception(); }
                            Console.WriteLine(elgamal.Verify(genOut.e, M, signOut.r, signOut.s));
                            break;
                        case 4:
                            isContinued = false;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
