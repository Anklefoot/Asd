using System;
using System.Collections.Generic;

namespace LegkisebbSulyozottMerevitofa
{
    class Program
    {
        // Változók deklarálása.
        static int[] id;    // id, itt tárolom majd a "kapcsolatokat"
        static int csucsokSzama, elekSzama; // csúcsok és élek száma
        static List<EgyEl> elek = new List<EgyEl>();    // Lista a gráfoknak

        static void Main(string[] args)
        {
            Console.WriteLine("***************************************************************************************");
            Console.WriteLine("**   Készítette: Császár Zsolt Neptun: JIH1SA                                        **");
            Console.WriteLine("***************************************************************************************\n");

            Console.WriteLine("Adja meg egy gráf csúcsainak és éleinek a számát a következő formátumban:\nx y");
            // Gráf adatainak bekérése.

            while(true)
            {
                var sor = Console.ReadLine();
                var adat = sor.Split(' ');
                try    // Adatbekérés ellenőzése miatt, így már csak jó adatokat fogad el.
                {
                    csucsokSzama = int.Parse(adat[0]);
                    elekSzama = int.Parse(adat[1]);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\nHibás érték vagy formátum, hibaüzenet:\n\"{e.Message}\"\n\nAdjon meg egy új értéket: csúcsokSzáma élekSzáma");
                }
            }

            Console.WriteLine("\nAdja meg sorban az éleket a következő formátumban:\nx y suly\n");
            // Gráf éleinek bekérése és rögzítése a listába.
            for (int i = 0; i < elekSzama; i++)
            {
                while(true)
                {
                    var sor2 = Console.ReadLine();
                    var data2 = sor2.Split(' ');
                    try    // Adatbekérés ellenőzése miatt, így már csak jó adatokat fogad el.
                    {
                        elek.Add(new EgyEl { Suly = long.Parse(data2[2]), x = int.Parse(data2[0]), y = int.Parse(data2[1]) });
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Hibás érték vagy formátum, hibaüzenet:\n\"{e.Message}\"\n\nAdjon meg egy új értéket: x y suly");
                    }
                }
            }

            init(); // ID tömb inicializálása, számozás.
           
            // Élek lista rendezése növekvő sorrendben a súly alapján. (Legkisebb súly lesz a 0. elem)
            elek.Sort((x, y) => x.Suly.CompareTo(y.Suly));

            // Számolás majd eredmény kiírása konzolba.
            long minAr;
            minAr = LegkisebbMerevito(elek);
            Console.Write($"\n*************************************************************\n" +
                          $"A gráf legkisebb súlyozott merevítőfájának összege: {minAr}\n" +
                          $"***************************************************************\n\n");

            //TestKiir();

            // Futás megállítása.
            Console.WriteLine("\n\nA kilépéshez nyomjon meg egy gombot...");
            Console.ReadLine();
        }

        /// <summary>
        /// Egy Gráf legkisebb súlyozott merevítőfájának összegét számolja ki
        /// </summary>
        /// <param name="elek">Egy gráf éleinek a listája.</param>
        /// <returns>Legkisebb merevítőfa</returns>
        static long LegkisebbMerevito(List<EgyEl> elek)
        {
            int x, y;
            long ar, minAr = 0;
            foreach (EgyEl el in elek)  // Végigmegyek az élek listán.
            {
                // felveszem az él értékeit változókba
                x = el.x;
                y = el.y;
                ar = el.Suly;

                // Ezzel vizsgálom, hogy a csúcsok már csatlakozva vannak e, 
                // ha nincsenek hozzáadám az él súlyát az összes súlyhoz, és uniózom az élt.
                if(ered(x) != ered(y))
                {
                    minAr += ar;
                    unio(x, y);
                }
            }
            return minAr;   //Visszatérek a minimum árral.
        }

        /// <summary>
        /// Visszakeresi az él "eredetét", innen tudom meg, hogy unióban vannak e már.
        /// </summary>
        /// <param name="x">Él egy tagja</param>
        /// <returns>Az él "eredete"</returns>
        static int ered(int x)
        {
            //Amíg nem találja meg hol van az ID tömben az él addig fut
            while(id[x] != x)
            {
                id[x] = id[id[x]];
                x = id[x];  // ha talált egyet felveszi az értékét és visszaadja.
            }

            return x;
        }

        /// <summary>
        /// Uniózza az élt a már megrajzolt gráfba.
        /// </summary>
        /// <param name="x">él első tagja</param>
        /// <param name="y">él második tagja</param>
        static void unio(int x, int y)
        {
            int p = ered(x);
            int q = ered(y);
            id[p] = id[q];
        }

        /// <summary>
        /// A tömböt inicializálja 0 ra.
        /// </summary>
        static void init()
        {
            /*
             * Itt probléma volt, hogy 1000 ig fut, most megkeresi a legnagyobb lehetséges csúcsot, és az alapján foglalja le az id tömböt.
             */
            int legnagyobbCsucs = getLegnagyobbCsucs() + 1;	// tömb 0 számozás miatt
            id = new int[legnagyobbCsucs];
            for (int i = 0; i < legnagyobbCsucs; ++i)
            {
                id[i] = i;
            }
        }

        /// <summary>
        /// Megkeresei a legnagyobb számú csúcsot.
        /// </summary>
        /// <returns>A legnagyobb csúcs értéke</returns>
        static int getLegnagyobbCsucs()
        {
            int max = 0;

            foreach (EgyEl el in elek)
            {
                if(max < el.x) max = el.x;
                if(max < el.y) max = el.y;
            }
            return max;
        }

        static void TestKiir()
        {
            foreach (EgyEl el in elek)
            {
                Console.Write(el.x);
                Console.Write(el.y);
                Console.Write(el.Suly);
                Console.WriteLine();
            }
        }
    }

    /// <summary>
    /// Él osztály, egy darab élnek a tulajdonságait tárolom benne.
    /// </summary>
    public class EgyEl
    {
        public long Suly { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}
