using System;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Collections;


namespace pProje
{
    class C3
    {

        public static void M9()
        {
            Console.WriteLine("\nC3:M9 ALINTI KONTROLÜ");
            // Metot 9 - Alıntılar (Parantez kontrolü (renk, sayı))
            PdfReader reader = new PdfReader(Environment.CurrentDirectory + @"\tez.pdf");
            string text = string.Empty;
            ArrayList alintiListesiHarfler = new ArrayList();
            ArrayList alintiListesi = new ArrayList();
            bool co = false;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                text = PdfTextExtractor.GetTextFromPage(reader, page);
                for (int i = 0; i < text.Length;)
                {
                    if (text[i].Equals('“'))
                    {
                        try
                        {
                            while (!text[i].Equals('”'))
                            {
                                alintiListesiHarfler.Add(text[i].ToString());
                                i++;
                            }
                            alintiListesiHarfler.RemoveAt(0);
                            alintiListesiHarfler.Remove("\n");
                            co = true;
                        }
                        catch (Exception e) { }
                    }
                    if (co)
                    {
                        string cumle = "";
                        for (int k = 0; k < alintiListesiHarfler.Count; k++)
                        {
                            cumle += alintiListesiHarfler[k].ToString();
                        }
                        alintiListesiHarfler = new ArrayList();
                        alintiListesi.Add(cumle);
                        co = false;
                    }
                    i++;
                }
            }
            for (int i = 0; i < alintiListesi.Count; i++)
            {
                string kelime = alintiListesi[i].ToString();
                string[] kontrolDizi = kelime.Split(' ');
                if (kontrolDizi.Length <= 2)
                {
                    alintiListesi.RemoveAt(i);
                    i--;
                }
            }

            Console.WriteLine("\nYapılan Alıntılar:");
            for (int i = 0; i < alintiListesi.Count; i++)
            {
                Console.WriteLine((i + 1) + ".Alıntı: " + alintiListesi[i].ToString());
            }
            Console.WriteLine("\nAlıntılarda beyaz font kullanımı bulunamamıştır.");
            reader.Close();
        }

        public static void M10()
        {
            // Metot 10 - Birim Kontrolü
            Console.WriteLine("\nC3:M10 BİRİM KULLANIM KONTROLÜ\n");

            PdfReader reader = new PdfReader(Environment.CurrentDirectory + @"\tez.pdf");

            string text = string.Empty;
            for (int page = 1; page < reader.NumberOfPages; page++)
            {
                text += PdfTextExtractor.GetTextFromPage(reader, page);
            }

            ArrayList liste = new ArrayList();
            liste.Add("cm");
            liste.Add("cc");
            liste.Add("°C");
            liste.Add("cg");
            liste.Add("cm2");
            liste.Add("cm3");
            liste.Add("g");
            liste.Add("Kbit");
            liste.Add("kcal");
            liste.Add("kg");
            liste.Add("kg/br");
            liste.Add("km");
            liste.Add("kN");
            liste.Add("kV");
            liste.Add("m");
            liste.Add("m2");
            liste.Add("m³");
            liste.Add("Mbit");
            liste.Add("mm");
            liste.Add("N");

            int sonuc;
            int sayac = 0;
            for (int i = 0; i < liste.Count; i++)
            {
                sonuc = text.IndexOf(liste[i].ToString());
                if (sonuc > 1 && text[sonuc - 1].Equals(" "))
                {

                    if (char.IsNumber(text[sonuc - 2]))
                    {
                        Console.WriteLine("Tez dosyasında birim kısaltma hatası yoktur.");
                        sayac++;
                    }
                    else
                    {
                        Console.WriteLine("Tez dosyasında birim kısaltma hatası vardır.");
                        sayac++;
                    }

                }
            }
            if (sayac == 0)
                Console.WriteLine("Tez dosyasında birim kısaltma kullanılmamıştır.");
            reader.Close();
        }

    }
}
