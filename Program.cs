using System;
using System.Collections.Generic;
using System.IO;

namespace eutazas
{
    struct Utasok
    {
        public int megallo, fev, fhonap, fnap, lev, lhonap, lnap,ingyen;
        public string kod;
        public bool ervenyes;
        public Utasok(string s)
        {
            string[] db = s.Split(' ');
            megallo = int.Parse(db[0]);
            fev = int.Parse(db[1].Substring(0, 4));
            fhonap = int.Parse(db[1].Substring(4, 2));
            fnap = int.Parse(db[1].Substring(6, 2));
            kod = db[2];
            
            if (db[4].Length > 2)
            {
                lev = int.Parse(db[4].Substring(0, 4));
                lhonap = int.Parse(db[4].Substring(4, 2));
                lnap = int.Parse(db[4].Substring(6, 2));
            }
            else lev = lhonap = lnap = int.Parse(db[4]);
            
            ervenyes = true;
            if(db[4]=="0")ervenyes = false;
            else
            if ( string.Compare(db[4], db[1].Substring(0, 8))<0 && db[4].Length>2) ervenyes = false;

            ingyen = 0;
            if(ervenyes)
                switch (db[3])
            {
                case "TAB":
                case "NYB": ingyen = -1;break;
                case "NYP":
                case "RVS":
                case "GYK": ingyen = 1;break;
            }        
        }
    }
    class Program
    {
        static int napokszama(int e1,int h1,int n1,int e2,int h2,int n2)
        {
            h1 = (h1 + 9) % 12;
            e1 = e1 - h1 / 10;
            int d1 = 365 * e1 + e1 / 4 - e1 / 100 + e1 / 400 + (h1 * 306 + 5) / 10 + n1 - 1;
            h2 = (h2 + 9) % 12;
            e2 = e2 - h2 / 10;
            int d2 = 365 * e2 + e2 / 4 - e2 / 100 + e2 / 400 + (h2 * 306 + 5) / 10 + n2 - 1;
            return d2 - d1;
        }

        static void Main(string[] args)
        {
            List<Utasok> adatok = new List<Utasok>();
            foreach (var i in File.ReadAllLines(@"M:\14a\c#\eUtazás\eUtazas\utasadat.txt"))
                adatok.Add(new Utasok(i));

            //2. feladat
            Console.WriteLine($"2. feladat:\nA buszra {adatok.Count} utas akart felszállni.");

            //3. feladat
            int o = 0;
            foreach (var i in adatok)
            {
                if(!i.ervenyes)o++;                
            }
            Console.WriteLine($"\n3. feladat:\nA buszra {o} utas nem szállhatott fel: ");

            //4. feladat
            int[] mszam = new int[30];
            foreach (var i in adatok)
            {
                mszam[i.megallo]++;
            }
            
            int max = 0;
            for (int i = 0; i < 30; i++)
                if (max < mszam[i]) max = mszam[i];
            
            o = 0;
            while (max != mszam[o]) o++;
            Console.WriteLine($"\n4. feladat:\nA legtöbb utas ({max} fő) a {o}. megállóban akart felszállni.");

            //5. feladat
            int ingyen = 0, kedvezmény = 0;
            foreach (var i in adatok)
            {
                if (i.ingyen < 0) kedvezmény++;
                else ingyen+=i.ingyen;
            }
            Console.WriteLine($"\n5. feladat:\nIngyenesen utazók száma: {ingyen} fő.\nA kedvezményesen utazók száma: { kedvezmény} fő.");

            //6. feladat
            StreamWriter ki = new StreamWriter(@"M:\14a\c#\eUtazás\eUtazas\figyelmeztetes.txt");
            foreach (var i in adatok)
            {
                int nap = napokszama(i.fev, i.fhonap, i.fnap, i.lev, i.lhonap, i.lnap);
                if (i.lev > 10 &&  nap<= 3 && i.ervenyes)
                    ki.WriteLine($"{i.kod} {i.lev}-{i.lhonap}-{i.lnap}");
            }
            ki.Close();

            Console.ReadKey();
        }
    }
}

