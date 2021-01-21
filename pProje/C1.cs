using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using System.Threading;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections;
namespace pProje
{

    public class LocationTextSizeExtractionStrategy : LocationTextExtractionStrategy
    {
        //Hold each coordinate
        public List<SizeAndTextAndFont> myChunks = new List<SizeAndTextAndFont>();

        //Automatically called for each chunk of text in the PDF
        public override void RenderText(TextRenderInfo wholeRenderInfo)
        {
            base.RenderText(wholeRenderInfo);
            GraphicsState gs = (GraphicsState)GsField.GetValue(wholeRenderInfo);
            myChunks.Add(new SizeAndTextAndFont(gs.FontSize, wholeRenderInfo.GetText(), wholeRenderInfo.GetFont().PostscriptFontName));
        }

        FieldInfo GsField = typeof(TextRenderInfo).GetField("gs", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    }

    public class SizeAndTextAndFont
    {
        public float Size;
        public String Text;
        public String Font;
        public SizeAndTextAndFont(float size, String text, String font)
        {
            this.Size = size;
            this.Text = text;
            this.Font = font;
        }
    }

    class C1
    {

        public static void M1()
        {
            // Metot 1 - Yazım Ortamı Kontrolü
            try
            {
                PdfReader reader = new PdfReader(Environment.CurrentDirectory + @"\tez.pdf");
                reader.Close();
                Console.WriteLine("C1:M1 YAZIM ORTAMI KONTROLÜ\n\nHerhangi bir hata bulunamamıştır.\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("Tez dosyasının yolunu veya uzantısını kontrol edin!\nProgram 5 saniye içinde kapatılıyor...");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        public static void M2()
        {
            // Metot 2 - Yazıların Nitelik Kontrolü
            Console.WriteLine("C1:M2 YAZILARIN NİTELİK KONTROLÜ\n");
            PdfReader reader = new PdfReader(Environment.CurrentDirectory + @"\tez.pdf");
            string text = string.Empty;
            bool kontrol = false;
            bool kontrol2 = false;
            string fontTuru = string.Empty;
            float fontBoyut = 0;
            for (int page = 1; page < reader.NumberOfPages; page++)
            {
                LocationTextSizeExtractionStrategy strategy = new LocationTextSizeExtractionStrategy();
                PdfTextExtractor.GetTextFromPage(reader, page, strategy);

                foreach (SizeAndTextAndFont p in strategy.myChunks)
                {
                    //TimesNewRoman && 12 punto
                    if (p.Font[0].Equals('T') && p.Font[5].Equals('N') && p.Font[8].Equals('R'))
                    {
                        kontrol = true;
                    }
                    else if (p.Font[0].Equals('T') && p.Font[6].Equals('N') && p.Font[10].Equals('R'))
                    {
                        kontrol = true;
                    }
                    else
                    {
                        fontTuru = p.Font.ToString();
                        kontrol = false;
                    }
                    if (p.Size == 12)
                    {
                        kontrol2 = true;
                    }
                    else
                    {
                        fontBoyut = p.Size;
                        kontrol2 = false;
                    }
                }
            }
            if (kontrol)
                Console.WriteLine("Tez dosyasının kullandığı font türü doğrudur. Font: Times New Roman");
            else
                Console.WriteLine("Tez dosyasının kullandığı font türü yanlıştır. Font: " + fontTuru.ToString());
        
            if (kontrol2)
                Console.WriteLine("Tez dosyasının kullandığı font boyutu doğrudur. Font boyutu: 12");
            else
                Console.WriteLine("Tez dosyasının kullandığı font boyutu yanlıştır. Font boyutu: " + fontBoyut.ToString());

            reader.Close();
        }

        public static void M3()
        {

            // Metot 3 - Kenar Boşlukları ve Sayfadüzeni Kontrolü -- YAPILAMADI
        }

        public static void M4()
        {
            // Metot 4 Başlıklar ortalı, altbaşlık düzeni, paragraf başı Kontrolü -- YAPILAMADI
        }
    }
}