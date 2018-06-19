using System;
using System.IO;
using System.Threading;

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
        delegate void NumberPrinter(int n);
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
        public delegate void PrintString(string s);
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

        #region EventDeclarationWithDelegates
        public class ExampleEventArgs : System.EventArgs
        {
            /* cerca di chiamare la classe come <qualcosa>EventArgs */

            public ExampleEventArgs()
            {
                Console.WriteLine("*Instanziato un oggetto di classe ExampleEventArgs!*");
            }
        }
        /* delegato per l'handler dell'evento <Example> ;
           cerca di chiamare il delegato come <NomeClasseEvento>EventHandler */
        //public delegate void ExampleEventHandler(object sender, ExampleEventArgs e);
        public delegate void ExampleEventHandler(object sender, EventArgs e);
        public class EventTest
        {
            /* dichiarazione degli eventi (chiamali con nome di "azioni") */
            public event ExampleEventHandler OnPrint;

            public EventTest()
            {
                
            }
            public void Print(string s)
            {
                Console.WriteLine(s);
                OnPrint?.Invoke(this, EventArgs.Empty);
            }
        }
        static void EventTest_OnPrint(object sender, EventArgs e)
        {
            Console.WriteLine("*Testo stampato!*");
        }
        #endregion

        #region Generics
        public class GenericArray<T>
        {
            private T[] Array { get; set; }
            public int Size { get; }

            public GenericArray(int size)
            {
                Size = size;
                Array = new T[Size + 1];
            }

            public void Add(int Index, T Item)
            {
                Array[Index] = Item;
            }

            public T Get(int Index)
            {
                return Array[Index];
            }

            public string TypeOf()
            {
                Type TT = typeof(T);
                return TT.FullName.Replace(TT.Namespace+".", "");
            }
        }
        #endregion

        #region UnsafeFunctions
        public static unsafe void Swap(int* p1, int* p2)
        {
            int temp = *p1;
            *p1 = *p2;
            *p2 = temp;
        }
        #endregion

        public static void ThreadSingolo()
        {
            Console.WriteLine("*Questo output è stato richiamato da un thread figlio di quello main!*");
        }

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
                NumberPrinter NP = delegate (int n)
                {
                    Console.WriteLine("Altro test: il numero {0} è stato scritto con un delegato anonimo!", n);
                };
                NP(0);
                NP(1);
                NP(2);
                #endregion

                #region TestOfUsageOfEvents
                Utility.Separatore();
                Console.WriteLine("Test di utilizzo di eventi (con delegati):");
                EventTest EV = new EventTest();
                EV.OnPrint += EventTest_OnPrint;
                EV.Print("Uallabalooza");
                #endregion

                #region TestOfUsageOfGenerics
                Utility.Separatore();
                GenericArray<string> SArr = new GenericArray<string>(5);
                Console.WriteLine("Test di utilizzo dei GENERICS di tipo \"{0}\":", SArr.TypeOf());
                SArr.Add(0, "Ualla");
                SArr.Add(1, "Balooza");
                SArr.Add(2, "In");
                SArr.Add(3, "The");
                SArr.Add(4, "Sky");
                for(var i=0; i<SArr.Size; i++)
                {
                    Console.WriteLine("- indice {0}: \"{1}\"", i, SArr.Get(i));
                }
                #endregion

                #region TestOfUsageOfUnsafeCode
                /* i puntatori vanno usati solo nei blocchi di codice "unsafe";
                 * puoi dichiarare unsafe anche una funzione o un metodo, se deve usare puntatori;
                 * occorre permettere la compilazione di codice unsafe dalle impostazioni del progetto */
                unsafe
                {
                    Utility.Separatore();
                    Console.WriteLine("Test di utilizzo di puntatori in codice UNSAFE:");
                    int SUns = 12345;
                    int* pSUns = &SUns;
                    Console.WriteLine("- valore: {0}", SUns);
                    Console.WriteLine("- valore da puntatore: {0}", pSUns->ToString());
                    Console.WriteLine("- indirizzo della variabile: {0}", (int)pSUns);
                    Console.WriteLine();
                    int SUns2 = 54321;
                    int* pSUns2 = &SUns2;
                    Console.WriteLine("Prima dello SWAP dei due puntatori: {0} - {1}", SUns, SUns2);
                    Swap(pSUns, pSUns2);
                    Console.WriteLine("Dopo lo SWAP dei due puntatori: {0} - {1}", SUns, SUns2);
                    Console.WriteLine();
                    Console.WriteLine("Test di accesso array con puntatori:");
                    int[] Arr = new int[] { 10, 20, 30 };
                    fixed (int* pArr = Arr)
                    for(int i=0; i<Arr.Length; i++)
                    {
                        Console.WriteLine("- Indirizzo di Arr[{0}]: {1}; Valore di Arr[{0}]: {2}", i, (int)(pArr+i), *(pArr+i));
                    }
                }
                #endregion

                #region TestOfUsageOfThreads
                Utility.Separatore();
                Console.WriteLine("Test di utilizzo dei THREAD");
                Thread TH = Thread.CurrentThread;
                TH.Name = "ThreadMain";
                Console.WriteLine("Nome del thread corrente: {0}", TH.Name);
                ThreadStart TS = new ThreadStart(ThreadSingolo);
                Console.WriteLine("Creazione in corso di un thread figlio...");
                Thread Child = new Thread(TS);
                Child.Start();
                int SleepTime = 5000;
                Console.WriteLine("Metto in pausa per {0} secondi...", SleepTime/1000);
                Thread.Sleep(SleepTime);
                Console.WriteLine("Riprendo l'esecuzione!");
                /*Console.WriteLine("Esco dall'esecuzione del thread (metodo Abort())...");
                Child.Abort();*/ /* non usare Child.Abort! usa Child.Interrupt quando è in stato di sleep-wait-join */
                #endregion
            }
            #region catch
            /*catch(ThreadAbortException e)
            {
                Console.WriteLine("Thread distrutto! (metodo Abort())");
            }*/
            catch (Exception e)
            {
                Utility.MostraEccezione<Exception>(e);
            }
            #endregion

            Console.ReadKey();
        }
    }
}
