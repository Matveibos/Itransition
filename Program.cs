using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace zad_3
{
    class Program
    {
        class Generating_Table{

            public void generation_table(int Count, int rule, params string[] values){
                Rules Ruls = new Rules();
                int i = 0, j = 0;
                string result;
                int t = values[0].Length;
                for (int l = 0; l < Count; l++){
                    if (t < values[l].Length)
                        t = values[l].Length;
                }
                int LeftTab = t + 2;
                Console.Write("  \\User".PadRight(LeftTab, ' '));
                for (; i < Count; i++) Console.Write(" | ".PadRight(values[i].Length + 3, ' ')); i = 0;
                Console.Write("\n PC\\      ".PadRight(LeftTab, ' '));
                for (; j < Count; j++) Console.Write(" | " + values[j]);
                Console.Write("".PadRight(LeftTab, ' '));
                for (; i < Count; i++){
                    Console.Write("\n".PadRight(LeftTab + 2, '-'));
                    for (j = 0; j < Count; j++) Console.Write("+".PadRight(values[j].Length + 3, '-'));
                    Console.Write("\n" + values[i].PadRight(LeftTab, ' '));
                    for (j = 0; j < Count; j++){
                        result = Ruls.Rule(i, j, rule);
                        Console.Write(" | " + result.PadRight(values[j].Length, ' '));
                    }
                }

            }
        }
        class Rules
        {
            public string Rule(int i, int j, int rule){
                if (i == j) return "DRAW";
                if (i < rule){
                    if (i < j & j - i <= rule) return "LOSE";
                    else return "WIN";
                }
                else{
                    if (i > j & i - j <= rule) return "WIN";
                    else return "LOSE";
                }
            }
        }
        public class Generating_Key{
            byte[] secretkey = new Byte[16];
            byte[] PCchoise = new Byte[1];
            public byte[] PCchoise1 = new Byte[1];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            public Generating_Key(int last,HMAC hMAC){
                do{
                    rng.GetBytes(PCchoise);
                } while (PCchoise[0] > last - 1);
                PCchoise1[0] = PCchoise[0];
                rng.GetBytes(secretkey);
                hMAC.Hmac(secretkey, PCchoise);


            }
 
        }

      public class HMAC{
            public string strkey, strPC, result, Hmac_key;

            public void Hmac(byte[] key, byte[] chiose){
                string Hash(string secretKey, string message){
                    var encoding = System.Text.Encoding.UTF8;
                    byte[] msgBytes = encoding.GetBytes(message);
                    var keyBytes = encoding.GetBytes(secretKey);
                    using (HMACSHA256 hmac = new HMACSHA256(keyBytes)){
                        byte[] hashBytes = hmac.ComputeHash(msgBytes);
                        var sb = new StringBuilder();
                        for (int i = 0; i < hashBytes.Length; i++)
                            sb.Append(hashBytes[i].ToString("X2"));
                        return sb.ToString();
                    }
                }
                for (int i = 0; i < key.Length; i++)
                    strkey += key[i].ToString();
                for (int i = 0; i < key.Length; i++)
                    Hmac_key += key[i].ToString("X2");
                strPC = chiose[0].ToString();
                result = Hash(strkey, strPC);
                Console.Write("\nHMAC: " + result + "\n");
            }
        }
        static void Main(string[] args){
            while (true)
            {for (int tt = 0; tt < args.Length-1; tt++)
                {
                    for (int t = tt+1; t < args.Length; t++){
                        if (args[tt] == args[t]){
                            Console.Write("Параметры повторяются");
                            Thread.Sleep(4000);
                            Environment.Exit(0); 
                        }
                    }
                }
                if (args.Length % 2 == 0 || args.Length == 0 || args.Length == 1){
                    Console.Write("Выбрано неверное количество параметров");
                    Thread.Sleep(4000);
                    break;
                }
                HMAC hmac_key = new HMAC();
                Generating_Key key = new Generating_Key(args.Length,hmac_key);
                Generating_Table a = new Generating_Table();
                string g = hmac_key.Hmac_key;
                Rules rul = new Rules();
                int Count = args.Length;
                int rule = Count / 2;
                Console.Write("Available moves:\n");
                for (int i = 0; i < args.Length; i++){
                    Console.WriteLine(i + 1 + "-" + args[i]);
                }
                Console.Write("0 - exit \n");
                Console.Write("? - help \n");
                Console.Write("Enter your move: ");
                char selection = Console.ReadKey().KeyChar;
                Console.Write("\n");
                if (selection == '0') break;
                if (selection == '?') {
                    a.generation_table(Count, rule, args);
                    Thread.Sleep(1000);
                    continue;
                }
                int varic = selection - '0';
                int User = varic - 1;
                byte Pc = key.PCchoise1[0];
                string result = rul.Rule(Pc, User, rule);
                Console.Write("Your move: " + args[varic - 1] + "\n");
                Console.Write("Compputer move: " + args[Pc] + "\n");
                Console.Write("You :" + result + "\n");
                Console.Write("HMAC key : " + g + "\n");
                Console.ReadLine();

            }
        }
    }
}
