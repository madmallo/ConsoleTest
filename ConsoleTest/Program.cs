using System;
using System.IO;

namespace ConsoleTest
{
    class Program
    {
        #region UtilityClass
        public static class Utility
        {
            public static void Separatore()
            {
                Console.WriteLine();
                Console.WriteLine("*********************");
                Console.WriteLine();
            }

            public static void MostraEccezione<T>(T e) where T : Exception
            {
                Console.WriteLine();
                Console.WriteLine("##############");
                Console.WriteLine("### ERRORE ###");
                Console.WriteLine("##############");
                Console.WriteLine("Eccezione di tipo \"{0}\"\nDettagli: {1}", e.GetType().FullName, e);
            }
        }
        #endregion

        #region ClassRettangoloForOperatorOverloadTest
        public class Rettangolo
        {
            public int L { get; set; }
            public int H { get; set; }

            public static Rettangolo operator +(Rettangolo A, Rettangolo B) => new Rettangolo(A.L + B.L, A.H + B.H);

            public Rettangolo(int l, int h)
            {
                L = l;
                H = h;
            }

            public int GetArea()
            {
                return L * H;
            }
        }
        #endregion

        #region DeclarationOfDelegateFunctionAndTestClass
        delegate int NumberChanger(int n);
        public static class TestDelegate
        {
            static int num = 10;

            public static int AddNum(int p)
            {
                num += p;
                return num;
            }

            public static int MultNum(int m)
            {
                num *= m;
                return num;
            }

            public static int GetNum()
            {
                return num;
            }
        }
        delegate void PrintString(string s);
        public static class TestDelegateLog
        {
            public static void VideoOutput(string s)
            {
                Console.WriteLine(s);
            }
            public static void FileOutput(string s)
            {
                using (FileStream FS = new FileStream("log.txt", FileMode.OpenOrCreate, FileAccess.Write))
                using (StreamWriter SW = new StreamWriter(FS))
                {
                    SW.WriteLine(s);
                }
            }
        }
        #endregion

        static void Main(string[] args)
        {
            try
            {
                #region TestUsageOfOperatorOverload
                Rettangolo A = new Rettangolo(3, 4);
                Rettangolo B = new Rettangolo(5, 5);
                Rettangolo C = A + B;
                Console.WriteLine("Area rettangolo A ({0}x{1}): {2}\nArea rettangolo B ({3}x{4}): {5}\nArea A+B ({6}x{7}): {8}", A.L, A.H, A.GetArea(), B.L, B.H, B.GetArea(), C.L, C.H, C.GetArea());
                #endregion

                #region TestOfUsageOfDelegateFunction
                Utility.Separatore();
                Console.WriteLine("Test di utilizzo di una funzione delegata:");
                NumberChanger NC1 = new NumberChanger(TestDelegate.AddNum);
                NumberChanger NC2 = new NumberChanger(TestDelegate.MultNum);
                Console.WriteLine("- valore iniziale: {0}", TestDelegate.GetNum());
                int plus = 25;
                NC1(plus);
                Console.WriteLine("- dopo aggiunta di {0}: {1}", plus, TestDelegate.GetNum());
                int multi = 5;
                NC2(multi);
                Console.WriteLine("- dopo moltiplicazione per {0}: {1}", multi, TestDelegate.GetNum());
                /* resetto il numero */
                NC2(0);
                Console.WriteLine("- dopo aver resettato il numero: {0}", TestDelegate.GetNum());
                NumberChanger NC;
                NC = NC1 + NC2;
                NC(5);
                Console.WriteLine("- dopo multicasting (NC = NC1 + NC2): {0}", TestDelegate.GetNum());
                Console.WriteLine();
                Console.WriteLine("Altro test di utilizzo di un delegato:");
                string SDel = "Logga questo";
                PrintString PSO = new PrintString(TestDelegateLog.VideoOutput);
                PrintString PSF = new PrintString(TestDelegateLog.FileOutput);
                PrintString Log = PSO + PSF;
                Log(SDel);
                Console.WriteLine("(la stringa qui sopra è stata mostrata a video e loggata in un file simultaneamente attraverso l'utilizzo di un delegato multicast)");
                #endregion
            }
            #region catch
            catch (Exception e)
            {
                Utility.MostraEccezione<Exception>(e);
            }
            #endregion

            Console.ReadKey();
        }
    }
}
