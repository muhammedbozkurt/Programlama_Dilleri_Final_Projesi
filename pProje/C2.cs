using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Collections;
using System.Text.RegularExpressions;

namespace pProje
{
    class C2
    {
        public static void M5()
        {
            // Metot 5 - Anlatım Kontrolü
            Console.WriteLine("\nC2:M5 ANLATIM KONTROLÜ\n");
            ArrayList cumleListesi = new ArrayList();
            
            string cumle = string.Empty;
            PdfReader reader = new PdfReader(Environment.CurrentDirectory + @"\tez.pdf");
            string text = string.Empty;
            for (int page = 1; page < reader.NumberOfPages; page++)
            {
                Console.WriteLine(page.ToString() + ". sayfa taranıyor... ");
                text = PdfTextExtractor.GetTextFromPage(reader, page);
                string replacement = Regex.Replace(text, @"\t|\n|\r", "");
                for (int i = 0; i < replacement.Length - 1; i++)
                {
                    while (true)
                    {
                        try
                        {
                            if (Char.IsUpper(replacement[i]) && Char.IsLower(replacement[i + 1]) && Char.IsLower(replacement[i + 2]))
                            {
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            break;
                        }
                        i++;
                    }
                    try
                    {
                        
                        cumle += replacement[i].ToString();
                    }
                    catch (Exception e) { }
                    while (i < replacement.Length -1)
                    {
                        string a = replacement[i].ToString();

                        if (a.Equals(".") && Char.IsLower(replacement[i-1]) && (Char.IsUpper(replacement[i + 1]) || Char.IsUpper(replacement[i + 2])))
                        {
                            if (Char.IsUpper(replacement[i - 3]) || Char.IsUpper(replacement[i - 4]) || Char.IsUpper(replacement[i - 2]))
                            {
                                break;
                            }
                            else
                            {
                                cumleListesi.Add(cumle);
                                cumle = string.Empty;
                                break;
                            }
                        }
                        else
                        {
                            i++;
                            cumle += replacement[i].ToString();
                            try
                            {
                                if (Char.IsLower(cumle[cumle.Length]) && Char.IsUpper(cumle[cumle.Length - 1]))
                                {
                                    cumle = string.Empty;
                                }
                            }
                            catch (Exception e)
                            {

                            }
                            
                        }
                    }
                    
                }
                for (int i = 0; i < cumleListesi.Count; i++)
                {
                    string a = cumleListesi[i].ToString();
                    for (int j = 1; j < a.Length - 1; j++)
                    {
                        
                        char x = a[j];
                        char y = a[j + 1];
                        char z = a[j - 1];
                        if (Char.IsLower(x) && Char.IsUpper(y) && Char.IsUpper(z))
                        {
                            a = a.Remove(0, j + 1);
                            cumleListesi[i] = a;
                        }
                    }
                }
            }
            Console.WriteLine("\nToplamda " + reader.NumberOfPages.ToString() + " sayfa tarandı. Bulunan cümleler %10 hata payı ile doğrudur.\nBulunan toplam cümle sayısı: "+ cumleListesi.Count.ToString());
            Console.Write("Bulunan cümleleri görüntülemek istiyor iseniz 1'e basınız: ");
            int girisDegeri = int.Parse(Console.ReadLine());
            if (girisDegeri == 1)
            {
                Console.WriteLine("");
                for (int i = 0; i < cumleListesi.Count; i++)
                {
                    Console.WriteLine((i + 1).ToString() + ".cümle: " + cumleListesi[i].ToString());
                }
            }
            reader.Close();

        }

        public static void M6()
        {
            // Metot 6 - Satır Aralıkları Kontrolü -- YAPILAMADI
        }

        public static void M7()
        {
            // Metot 7 - Sayfa Numarası Kontrolü
            Console.WriteLine("\nC2:M7 SAYFA NUMARASI KONTROLÜ\n");
            string sayfaNumarasi;
            ArrayList sayfaNo = new ArrayList(); // sayfa sayacı
            ArrayList sayfalar = new ArrayList(); // sayfalarda bulunan sayfa numaraları
            bool sayfaNoKontrol = true;
            PdfReader reader = new PdfReader(Environment.CurrentDirectory + @"\tez.pdf");
            string text = string.Empty;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                sayfaNumarasi = string.Empty;
                sayfalar.Add(page.ToString());
                text = PdfTextExtractor.GetTextFromPage(reader, page);
                text = text.Trim();
                for (int i = text.Length - 1; i > 1; i--)
                {
                    if (Char.IsNumber(text[i]))
                    {
                        sayfaNumarasi += text[i].ToString();
                        sayfaNoKontrol = false;
                    }
                    else if (sayfaNoKontrol)
                    {
                        Console.WriteLine(page + ". sayfasında sayfa numarası bulunamadı.");
                        sayfaNoKontrol = true;
                    }
                    else
                    {
                        break;
                    }
                }
                sayfaNo.Add(sayfaNumarasi);
            }
            reader.Close();
            for (int i = 0; i < sayfaNo.Count; i++)
            {
                string indisDeger = sayfaNo[i].ToString();
                char[] myArr = indisDeger.ToCharArray();
                Array.Reverse(myArr);
                indisDeger = string.Empty;
                for (int j = 0; j < myArr.Length; j++)
                {
                    indisDeger += myArr[j].ToString();
                }
                sayfaNo[i] = indisDeger;
            }

            // KONTROL AŞAMASI
            if (sayfalar.Count == sayfaNo.Count)
            {
                double dogrulukPayiP = 0;
                double dogrulukPayiN = 0;
                Console.WriteLine("Sayfa numaraları eşittir. Tezde bulunan sayfa sayısı: " + sayfalar.Count.ToString());
                for (int i = 0; i < sayfalar.Count; i++)
                {
                    if (sayfalar[i].Equals(sayfaNo[i].ToString()))
                    {
                        Console.WriteLine("Sayfa numarası = " + sayfalar[i].ToString() + " - Bulunan sayfa numarası: " + sayfaNo[i].ToString() + " - Sonuç = Aynı");
                        dogrulukPayiP++;
                    }
                    else
                    {
                        Console.WriteLine("Sayfa numarası = " + sayfalar[i].ToString() + " - Bulunan sayfa numarası: " + sayfaNo[i].ToString() + " - Sonuç = Farklı");
                        dogrulukPayiN++;
                    }
                }

                if (dogrulukPayiN == 0)
                {
                    Console.WriteLine("\nSayfa numarası kontrolü sonucu (1 üzerinden): 1");
                }
                else
                {
                    Console.WriteLine("\nSayfa numarası kontrolü sonucu (1 üzerinden): " + (dogrulukPayiP / dogrulukPayiN).ToString());
                }
            }
            else
            {
                Console.WriteLine("Sayfa numaraları eşit değildir.");
            }

        }

        public static void M8()
        {
            // Metot 8 - Bölüm ve Altbölüm Kontrolü (ve, veya, ile)
            Console.WriteLine("\nC2:M8 YAZIM PLANI KONTROLÜ\n");
            PdfReader reader = new PdfReader(Environment.CurrentDirectory + @"\tez.pdf");
            string text = string.Empty;
            int veSayaci = 0;
            int veSayaciN = 0;
            int veyaSayaci = 0;
            int veyaSayaciN = 0;
            int ileSayaci = 0;
            int ileSayaciN = 0;
            for (int page = 1; page < reader.NumberOfPages; page++)
            {
                text = PdfTextExtractor.GetTextFromPage(reader, page);
                for (int i = 0; i < text.Length - 1; i++)
                {
                    if (text[i].Equals('v') && text[i + 1].Equals('e'))
                    {
                        if (text[i - 1].ToString().Equals(" ") && text[i + 2].ToString().Equals(" "))
                        {
                            if (Char.IsLower(text[i]) && Char.IsLower(text[i + 1]))
                            {
                                veSayaci++;
                            }
                            else
                                veSayaciN++;
                        }
                    }


                    if (text[i].Equals('v') && text[i + 1].Equals('e') && text[i + 2].Equals('y') && text[i + 3].Equals('a'))
                    {
                        if (text[i - 1].Equals(' ') && text[i + 4].Equals(' '))
                        {
                            if (Char.IsLower(text[i]) && Char.IsLower(text[i + 1]) && Char.IsLower(text[i + 2]) && Char.IsLower(text[i + 3]))
                                veyaSayaci++;
                            else
                                veyaSayaciN++;
                        }
                    }

                    if ((text[i].Equals('i') || text[i].Equals('İ')) && (text[i + 1].Equals('l') || text[i + 1].Equals('L')) && (text[i + 2].Equals('e') || text[i + 2].Equals('E')))
                    {
                        if (text[i - 1].Equals(' ') && text[i + 3].Equals(' '))
                        {
                            if (Char.IsLower(text[i]) && Char.IsLower(text[i + 1]) && Char.IsLower(text[i + 2]))
                                ileSayaci++;
                            else
                                ileSayaciN++;
                        }
                    }
                }
            }
            Console.WriteLine("Tez dosyasında bulunan yazımı doğru olan 've' sayısı: " + veSayaci.ToString());
            Console.WriteLine("Tez dosyasında bulunan yazımı yanlış olan 've' sayısı: " + veSayaciN.ToString());
            Console.WriteLine("Tez dosyasında bulunan yazımı doğru olan 'veya' sayısı: " + veyaSayaci.ToString());
            Console.WriteLine("Tez dosyasında bulunan yazımı yanlış olan 'veya' sayısı: " + veyaSayaciN.ToString());
            Console.WriteLine("Tez dosyasında bulunan yazımı doğru olan 'ile' sayısı: " + ileSayaci.ToString());
            Console.WriteLine("Tez dosyasında bulunan yazımı yanlış olan 'ile' sayısı: " + ileSayaciN.ToString());
            reader.Close();
        }
    }
}