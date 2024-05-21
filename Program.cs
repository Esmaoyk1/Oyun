using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odev3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("isminiz:");
            string isim = Console.ReadLine();
            Console.Write("soyisim");
            string soyisim = Console.ReadLine();
            Console.Write("dogum gununuz:");
            int dogum_gunu = int.Parse(Console.ReadLine());

            Oyuncu oy0 = new Oyuncu(isim);
            Oyuncu oy1 = new Oyuncu(soyisim);

            Oyun oyn = new Oyun(oy0, oy1, dogum_gunu);

            Console.ReadKey();
        }

        class Kart
        {
            public int tip;
            public int deger;

            public Kart(int tip, int deger)
            {
                this.tip = tip;
                this.deger = deger;
            }
            public string ismi()
            {
                if (tip == 0)
                {
                    return "sinek";
                }
                else if (tip == 1)
                {
                    return "karo";
                }
                else if (tip == 2)
                {
                    return "kupa";
                }
                else if (tip == 3)
                {
                    return "maço";
                }
                else
                {
                    return "HATA";
                }
            }
        }
        class Oyuncu
        {
            public string adi;
            public List<Kart> el = new List<Kart>();
            public List<Kart> alınanlar = new List<Kart>();
            public int puan = 0;

            public Oyuncu(string isim)
            {
                adi = isim;
            }
            public Kart KartAt()
            {
                Kart son = el[el.Count - 1];
                el.Remove(son);
                return son;
            }
        }
        class Oyun
        {
            Oyuncu oyuncu0;
            Oyuncu oyuncu1;
            List<Kart> deste = new List<Kart>();
            List<Kart> masa = new List<Kart>();

            public Oyun(Oyuncu oyuncu0, Oyuncu oyuncu1, int karma_sayısı)
            {
                this.oyuncu0 = oyuncu0;
                this.oyuncu1 = oyuncu1;

                DesteOlustur();
                for (int i = 0; i < karma_sayısı; i++) { KartlarıKarıstır(); }
                KartlarıDagıt();
                OyunDongusu();
                PuanlarıHesapla();
                OyunSonuMesajları();
            }
            void DesteOlustur()
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 1; j < 14; j++)
                    {
                        deste.Add(new Kart(i, j));
                    }
                }
            }
            void ListeYazdır(List<Kart> liste)
            {
                foreach (Kart k in liste)
                {
                    Console.WriteLine(k.ismi() + " " + k.deger);
                }
            }
            void KartlarıKarıstır()
            {
                Random rng = new Random();
                int n = deste.Count;
                while (n > 1)
                {
                    n--;
                    int i = rng.Next(n + 1);
                    Kart ilk = deste[i];
                    Kart iki = deste[n];
                    deste[i] = iki;
                    deste[n] = ilk;
                }
            }

            void KartlarıDagıt()
            {
                for (int i = 0; i < 4; i++)
                {
                    masa.Add(deste[deste.Count - 1]);
                    deste.RemoveAt(deste.Count - 1);
                }
                for (int i = 0; i < (52 - 4); i++)
                {
                    if (i < (52 - 4) / 2)
                    {
                        oyuncu0.el.Add(deste[i]);
                    }
                    else
                    {
                        oyuncu1.el.Add(deste[i]);
                    }
                }
                deste.Clear();
            }
            void OyunDongusu()
            {
                int turn = 0;
                while (oyuncu0.el.Count > 0)
                {
                    Oyuncu SimdikiOyuncu = (turn == 0) ? oyuncu0 : oyuncu1;

                    Kart atılan = SimdikiOyuncu.KartAt();
                    masa.Add(atılan);
                    if (masa[masa.Count - 2].deger == atılan.deger)
                    {
                        SimdikiOyuncu.puan += 10;
                        for (int i = masa.Count - 1; i > 0; i--)
                        {
                            SimdikiOyuncu.alınanlar.Add(masa[i]);
                            masa.RemoveAt(i);
                        }
                    }
                    else if (atılan.deger == 11)
                    {
                        for (int i = masa.Count - 1; i > 0; i--)
                        {
                            SimdikiOyuncu.alınanlar.Add(masa[i]);
                            masa.RemoveAt(i);
                        }
                    }
                    else
                    {

                    }
                    turn = (turn == 0) ? 1 : 0;
                }
                Console.WriteLine("\nOyun Bitti");
            }
            void PuanlarıHesapla()
            {
                _PuanHesapla(oyuncu0);
                _PuanHesapla(oyuncu1);

                if (oyuncu0.alınanlar.Count > oyuncu1.alınanlar.Count)
                {
                    oyuncu0.puan += 3;
                }
                else
                {
                    oyuncu1.puan += 3;
                }
            }
            void _PuanHesapla(Oyuncu oy)
            {
                foreach (Kart k in oy.alınanlar)
                {
                    if (k.deger == 1)
                    {
                        oy.puan += 1;
                    }
                    else if (k.tip == 0 && k.deger == 2)
                    {
                        oy.puan += 2;
                    }
                    else if (k.tip == 1 && k.deger == 10)
                    {
                        oy.puan += 3;
                    }
                    else if (k.deger == 11)
                    {
                        oy.puan += 1;
                    }
                }
            }

            void OyunSonuMesajları()
            {
                _osm(oyuncu0);
                _osm(oyuncu1);
                Oyuncu kazanan = (oyuncu0.puan > oyuncu1.puan) ? oyuncu0 : oyuncu1;
                Console.WriteLine($"Kazanan oyuncu  {kazanan.adi},{kazanan.puan} puan ile kazandı.");
            }
            void _osm(Oyuncu oy)
            {
                Console.Write($"\n{oy.adi}'ın aldıgı kartlar = [");
                foreach (Kart k in oy.alınanlar)
                {
                    Console.Write(k.ismi() + k.deger + " ");
                }
                Console.WriteLine("]");
                Console.WriteLine($"{oy.adi} aldıgı puan = {oy.puan}\n");
            }
        }
    }
}
