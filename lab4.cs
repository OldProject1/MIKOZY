using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System;

using static SandBox.GCD;
using static SandBox.RSA;


namespace SandBox
{
    static class GCD
    {
        public static BigInteger Gcd(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
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

            BigInteger gcd = Gcd(b % a, a, out x, out y);

            BigInteger newY = x;
            BigInteger newX = y - (b / a) * x;

            x = newX;
            y = newY;
            return gcd;
        }
    }
    static class RSA
    {
        public static BigInteger n;
        public static BigInteger d;
        public static BigInteger Gen(BigInteger p, BigInteger q, BigInteger e)
        {
            n = p * q;
            BigInteger fi = (p - 1) * (q - 1);
            BigInteger helpVar;
            BigInteger gcd = Gcd(e, fi, out d, out helpVar);//e^(-1) (mod fi) == d
            if (gcd != 1) { throw new Exception(); }
            d = (d % fi + fi) % fi;
            return d;
        }
        static BigInteger binpow(BigInteger a, BigInteger m)
        {
            BigInteger res = 1;
            while (m != 0)
                if ((m & 1) != BigInteger.Zero)
                {
                    res = res * a % n;
                    m--;
                }
                else
                {
                    a = a * a % n;
                    m /= 2;
                }
            return res;
        }
        public static BigInteger Encr(BigInteger X, BigInteger e, BigInteger n)
        {
            if (X >= n) { throw new Exception(); }
            return binpow(X, e) % n;
        }
        public static BigInteger Encr(BigInteger X, BigInteger e)
        {
            if (X >= n) { throw new Exception(); }
            return binpow(X, e) % n;
        }
        public static BigInteger Decr(BigInteger Y, BigInteger d, BigInteger n)
        {
            if (Y >= n) { throw new Exception(); }
            return binpow(Y, d) % n;
        }
        public static BigInteger Decr(BigInteger Y, BigInteger d)
        {
            if (Y >= n) { throw new Exception(); }
            return binpow(Y, d) % n;
        }
    }
    class MyClass
    {
        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                BigInteger
                    p = 804288300171659,
                    q = 819139104388459,
                    e = BigInteger.Parse("587862009679843002844824189377"),
                    X1 = BigInteger.Parse("201229993267158788910642144722"), Y1, 
                    Y2 = BigInteger.Parse("474981332560572636448191786787"), X2;
                bool isContinued = true;
                while(isContinued)
                {
                    Console.WriteLine("choose operation: ");
                    Console.WriteLine("1) Gen      2) Encr     3) Decr     4) Exit");
                    int op = Convert.ToInt32(Console.ReadLine());
                    switch (op)
                    {
                        case 1:
                            Gen(p, q, e);
                            Console.WriteLine($"d = {d}");
                            break;
                        case 2:
                            Y1 = Encr(X1, e);
                            Console.WriteLine($"Y1 = {Y1}");
                            break;
                        case 3:
                            X2 = Decr(Y2, d);
                            Console.WriteLine($"X2 = {X2}");
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
