using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System;

namespace SandBox
{
    class GCD
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
    class RSA
    {
        public BigInteger n;
        public BigInteger fi;
        public BigInteger d;
        public BigInteger Gen(BigInteger p ,BigInteger q, BigInteger e)
        {
            n = p * q;
            Console.WriteLine($"n = {n}");
            fi = (p - 1) * (q - 1);
            Console.WriteLine($"fi = {fi}");
            BigInteger helpVar;
            BigInteger gcd = GCD.Gcd(e, fi, out d, out helpVar);//e^(-1) (mod fi) == d
            if (gcd != 1){ throw new Exception(); }
            d = (d%fi + fi) % fi;
            Console.WriteLine($"d = {d}");
            return d;
        }
        BigInteger binpow(BigInteger a, BigInteger m)
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
        public BigInteger Encr(BigInteger X, BigInteger e, BigInteger n)
        {
            if (X >= n) { throw new Exception(); }
            return binpow(X, e) % n;
        }
        public BigInteger Encr(BigInteger X, BigInteger e)
        {
            if (X >= n) { throw new Exception(); }
            return binpow(X, e) % n;
        }
        public BigInteger Decr(BigInteger Y, BigInteger d, BigInteger n)
        {
            if (Y >= n) { throw new Exception(); }
            return binpow(Y, d) % n;
        }
        public BigInteger Decr(BigInteger Y)
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
                    X1 = BigInteger.Parse("201229993267158788910642144722"),
                    Y2 = BigInteger.Parse("474981332560572636448191786787");
                RSA rsa = new RSA();
                rsa.Gen(p, q, e);
                var encrMsg = rsa.Encr(X1, e);
                var decrMsg = rsa.Decr(Y2, rsa.d, e);


                Console.WriteLine(encrMsg);
                Console.WriteLine(rsa.Decr(encrMsg));
                
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}

