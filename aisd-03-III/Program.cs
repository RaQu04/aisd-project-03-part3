using System;
using System.Diagnostics;
using System.Threading;

namespace aisd_03_III
{
    class Program
    {
        static Random rnd = new Random(Guid.NewGuid().GetHashCode());

        static void qsortRekurencyjny(int[] t, int l, int p)
        {
            int i, j, x;
            i = l;
            j = p;
            x = t[(l + p) / 2]; // (pseudo)mediana
            do
            {
                while (t[i] < x) i++; // przesuwamy indeksy z lewej
                while (x < t[j]) j--; // przesuwamy indeksy z prawej
                if (i <= j) // jeśli nie minęliśmy się indeksami (koniec kroku)
                { // zamieniamy elementy
                    int buf = t[i]; t[i] = t[j]; t[j] = buf;
                    i++; j--;
                }
            }
            while (i <= j);
            if (l < j) qsortRekurencyjny(t, l, j); // sortujemy lewą część (jeśli jest)
            if (i < p) qsortRekurencyjny(t, i, p); // sortujemy prawą część (jeśli jest)
        } /* qsort() */

        static void qsortRekurencyjnyPrawy(int[] t, int l, int p)
        {
            int i, j, x;
            i = l;
            j = p;
            x = t[p]; 
            do
            {
                while (t[i] < x) i++; // przesuwamy indeksy z lewej
                while (x < t[j]) j--; // przesuwamy indeksy z prawej
                if (i <= j) // jeśli nie minęliśmy się indeksami (koniec kroku)
                { // zamieniamy elementy
                    int buf = t[i]; t[i] = t[j]; t[j] = buf;
                    i++; j--;
                }
            }
            while (i <= j);
            if (l < j) qsortRekurencyjnyPrawy(t, l, j); // sortujemy lewą część (jeśli jest)
            if (i < p) qsortRekurencyjnyPrawy(t, i, p); // sortujemy prawą część (jeśli jest)
        } /* qsort() */

        static void qsortRekurencyjnyLosowy(int[] t, int l, int p)
        {
            int i, j, x;
            i = l;
            j = p;
            x = t[rnd.Next(l,p)];
            do
            {
                while (t[i] < x) i++; // przesuwamy indeksy z lewej
                while (x < t[j]) j--; // przesuwamy indeksy z prawej
                if (i <= j) // jeśli nie minęliśmy się indeksami (koniec kroku)
                { // zamieniamy elementy
                    int buf = t[i]; t[i] = t[j]; t[j] = buf;
                    i++; j--;
                }
            }
            while (i <= j);
            if (l < j) qsortRekurencyjnyLosowy(t, l, j); // sortujemy lewą część (jeśli jest)
            if (i < p) qsortRekurencyjnyLosowy(t, i, p); // sortujemy prawą część (jeśli jest)
        } /* qsort() */

        static void qsortInteracyjny(int[] t)
        {
            int i, j, l, p, sp;
            int[] stos_l = new int[t.Length],
            stos_p = new int[t.Length]; // przechowywanie żądań podziału
            sp = 0; stos_l[sp] = 0; stos_p[sp] = t.Length - 1; // rozpoczynamy od całej tablicy
            do
            {
                l = stos_l[sp]; p = stos_p[sp]; sp--; // pobieramy żądanie podziału
                do
                {
                    int x;
                    i = l; j = p; x = t[(l + p) / 2]; // analogicznie do wersji rekurencyjnej
                    do
                    {
                        while (t[i] < x) i++;
                        while (x < t[j]) j--;
                        if (i <= j)
                        {
                            int buf = t[i]; t[i] = t[j]; t[j] = buf;
                            i++; j--;
                        }
                    } while (i <= j);
                    if (i < p) { sp++; stos_l[sp] = i; stos_p[sp] = p; } // ewentualnie dodajemy żądanie podziału
                    p = j;
                } while (l < p);
            } while (sp >= 0); // dopóki stos żądań nie będzie pusty
        } 

        static void GenerateRandomArray(int[] t, Random rnd, int maxValue = int.MaxValue)
        {
            for (int i = 0; i < t.Length; ++i)
                t[i] = rnd.Next(maxValue);
        }

        static void GenerateAshapeArray(int[] t)
        {
            int count = 0;

            for (int i = 0; i < t.Length / 2; i++)
            {
                t[count++] = i;
            }
            for (int i = t.Length / 2; i > 0; i--)
            {
                t[count++] = i;
            }
        }

        static void Tester()
        {
            var watch = new Stopwatch();
            Console.WriteLine("Wartość\tRekurnecyjny\tPrawy\tLosowy\tInteracyjny");
            for (int i = 50000; i <= 200000; i += 10000)
            {
                Console.Write(i+"\t");
                RekurencyjnyPomiar(watch, i);
                InteracyjnyPomiar(watch, i);
                Console.WriteLine();
            }
        }

        private static void InteracyjnyPomiar(Stopwatch watch, int i)
        {
            Int32[] tab = new Int32[i];
            GenerateRandomArray(tab, rnd, int.MaxValue);
            watch.Reset();
            watch.Start();
            qsortInteracyjny(tab);
            
            watch.Stop();
            Console.Write("{0} ms" +"\t", Math.Round(1000.0 * watch.ElapsedTicks / Stopwatch.Frequency,2));
        }

        private static void RekurencyjnyPomiar(Stopwatch watch, int i)
        {
            Int32[] tab = new Int32[i];
            GenerateRandomArray(tab, rnd, int.MaxValue);
            watch.Reset();
            watch.Start();
            qsortRekurencyjny(tab,0,tab.Length-1);
            watch.Stop();
            Console.Write("{0} ms" + "\t", Math.Round(1000.0 * watch.ElapsedTicks / Stopwatch.Frequency, 2));

            GenerateAshapeArray(tab);
            watch.Reset();
            watch.Start();
            qsortRekurencyjnyPrawy(tab, 0, tab.Length - 1);
            watch.Stop();
            Console.Write("{0} ms" + "\t", Math.Round(1000.0 * watch.ElapsedTicks / Stopwatch.Frequency, 2));


            GenerateAshapeArray(tab);
            watch.Reset();
            watch.Start();
            qsortRekurencyjnyLosowy(tab, 0, tab.Length - 1);

            watch.Stop();
            Console.Write("{0} ms" + "\t", Math.Round(1000.0 * watch.ElapsedTicks / Stopwatch.Frequency, 2));


        }

        

        static void Main(string[] args)
        {
            Thread TesterThread = new Thread(Program.Tester, 8 * 1024 * 1024); // utworzenie wątku
            TesterThread.Start(); // uruchomienie wątku
            TesterThread.Join(); // oczekiwanie na zakończenie wątku
            
        }
    }
}
