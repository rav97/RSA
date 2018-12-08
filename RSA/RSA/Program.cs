using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Numerics;

namespace RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            string message = "Testowy tekst na Podstawy Ochrony Danych-test RSA.";
            Console.WriteLine("Plain text:\n\"{0}\"\n",message);
            RSA rsa = new RSA();
            rsa.WhoIAm();
            BigInteger encrypt = rsa.Encrypt(message);
            Console.WriteLine("\nEncrypted message:\n{0}", encrypt);
            string decrypt = rsa.Decrypt(encrypt);
            Console.WriteLine("\nDecrypted message:\n{0}", decrypt);
        }
    }
    class RSA
    {
        static Random rand = new Random();
        int p;
        int q;
        ulong n;
        ulong phi;
        ulong e;
        ulong d;
        public RSA()
        {
            p = GeneratePrime();
            q = GeneratePrime();
            n = (ulong)p * (ulong)q;
            phi = (ulong)(p - 1) * (ulong)(q - 1);
            e = GenerateE();
            d = GenerateD();
        }
        ulong NWD(ulong a, ulong b)
        {
            while (a != b)
            {
                if (a < b)
                    b -= a;
                else
                    a -= b;
            }
            return a;
        }
        bool IsPrime(ulong l)
        {
            bool tn = true;
            for (ulong i = 2; i < l - 1; i++)
            {
                if (l % i == 0)
                {
                    tn = false;
                    break;
                }
            }

            return tn;
        }
        bool IsPrime(int l)
        {
            bool tn = true;
            for (int i = 2; i < l - 1; i++)
            {
                if (l % i == 0)
                {
                    tn = false;
                    break;
                }
            }

            return tn;
        }
        int GeneratePrime()
        {
            int l = rand.Next(1000, 9972);
            bool tn = true;
            while (tn)
            {
                for (int i = l; i < 9974; i++)
                {
                    if(IsPrime(i))
                    {
                        l = i;
                        tn = false;
                        break;
                    }
                }
            }
            return l;
        }
        ulong GenerateE()
        {
            ulong e = 0, p = this.phi;
            for(ulong i=2;i<9999;i++)
            {
                ulong pom = i;
                if(NWD(p,(ulong)pom)==1 && IsPrime(pom))
                {
                    e = i;
                    break;
                }
            }
            return e;
        }
        ulong GenerateD()
        {
            ulong e1 = this.e;
            ulong d = 0;
            for(ulong i = 1; i < 999999999; i++)
            {
                if(((e1*i)-1)%this.phi==0)
                {
                    d = i;
                    break;
                }
            }
            return d;
        }
        public void WhoIAm()
        {
            Console.WriteLine("RSA o parametrach:\n\tp = {0}\n\tq = {1}\n\tn = {2}\n\tphi = {3}\n\te = {4}\n\td = {5}",p,q,n,phi,e,d);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nKlucz publiczny:\n\te = {0}\n\tn = {1}",e,n);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\nKlucz prywatny:\n\td = {0}\n\tn = {1}", d, n);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
       
        private BigInteger MakeBig(string Text)
        {
            BigInteger l = new BigInteger(0);
            for(int i=0;i<Text.Length;i++)
            {
                l += new BigInteger(Convert.ToInt32(Text[i]));
                if(i!=Text.Length-1)
                    l *= 1000;
            }
            return l;
        }
        public BigInteger EncryptBlock(string block)
        {
            BigInteger message = MakeBig(block);
            BigInteger pow = BigInteger.ModPow(message, this.e, this.n);
            return pow;
        }
        public string DecryptBlock(BigInteger block)
        {
            string decrypted = "";
            BigInteger message = BigInteger.ModPow(block, this.d, this.n);
            while(message>0)
            {
                decrypted = (char)(message%1000) + decrypted;
                message /= 1000;
            }
            return decrypted;
        }
        public BigInteger Encrypt(string plainText)
        {
            BigInteger encrypted = new BigInteger();
            for(int i=0;i<plainText.Length;i+=2)
            {
                string pom = plainText.Substring(i, 2);
                encrypted += EncryptBlock(pom);
                if (i != plainText.Length - 2)
                    encrypted *= 100000000;
            }
            return encrypted;
        }
        public string Decrypt(BigInteger encrypted)
        {
            string decrypted = "";
            while(encrypted>0)
            {
                decrypted = DecryptBlock(encrypted % 100000000) + decrypted;
                encrypted /= 100000000;
            }
            return decrypted;
        }
        public void print(ulong[] tab)
        {
            Console.WriteLine();
            for(int i=0;i<tab.Length;i++)
            {
                Console.Write(tab[i]);
            }
            Console.WriteLine();
        }
    }
}