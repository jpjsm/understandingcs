// Text used in this sample is taken from the English version of [Disney's Mulan (2020)](https://movies.disney.com/mulan-2020)
// The Chinese text is a translation obtained using Google's translator
// The Spanish text is a translation started with Google's translator and manually adjusted
//
using System.Text;
using System.Text.RegularExpressions;

namespace words_and_characters
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string español_extendido = español.Normalize(System.Text.NormalizationForm.FormKD);

            (string language, string phrase)[] phrases = {
                (nameof(test2),test2),
                (nameof(test),test),
                (nameof(español_extendido),español_extendido),
                (nameof(chinese_traditional), chinese_traditional), 
                (nameof(english), english), 
                (nameof(español), español)
            };

            Regex rx_word = new Regex(word_pattern, RegexOptions.Compiled);
            Regex rx_sentence = new Regex(sentence_pattern, RegexOptions.Compiled|RegexOptions.Multiline);

            using (TextWriter writer = new StreamWriter(outputFileName, append: false))
            {
                foreach ((string language, string phrase) item in phrases)
                {
                    foreach (var textWriter in new TextWriter[] { Console.Out, writer })
                    {
                        textWriter.WriteLine($"Sentence:\n{item.phrase}");
                        textWriter.WriteLine();
                        textWriter.WriteLine($"Total {item.language} characters: {item.phrase.Length}");
                        textWriter.WriteLine(new string('-', 80));
                        textWriter.WriteLine();

                        textWriter.WriteLine("Sentences:");
                        textWriter.WriteLine();
                        Match match_rx_sentence = rx_sentence.Match(item.phrase);
                        while(match_rx_sentence.Success)
                        {
                            string v = match_rx_sentence.Groups[0].Value;
                            textWriter.WriteLine($">\t{v}<");
                            match_rx_sentence = match_rx_sentence.NextMatch();
                        }

                        textWriter.WriteLine(new string('-', 80));
                        textWriter.WriteLine();

                        textWriter.WriteLine("Words:");
                        Match match_rx_word = rx_word.Match(item.phrase);
                        while (match_rx_word.Success)
                        {
                            string v = match_rx_word.Groups[0].Value;
                            textWriter.WriteLine($"\t{v}");
                            match_rx_word = match_rx_word.NextMatch();
                        }

                        textWriter.WriteLine(new string('-', 80));
                        textWriter.WriteLine();
                    }
                }
            }
        }

        static string outputFileName = "words_and_characers.txt";
        // static string word_pattern = @"(\p{L}\p{M}*+)+?";
        static string word_pattern = @"(\p{L}\p{M}*)+";
        // static string sentence_pattern = @"^(\\p{L}\\p{M}*)((\\p{L}\\p{M}*)|\\p{N}|\\p{Pd}|\\p{Pi}|\\p{Pf}|\\p{Pc}|,|.|;|:|\\p{Zs})*";
        static string sentence_pattern = @"^(\p{L}\p{M}*)((\p{L}\p{M}*)|\p{N}|\p{Pd}|\p{Pi}|\p{Pf}|\p{Pc}|,|.|;|:|\p{Zs})*$";
        static string chinese_traditional = @"我對這個無法估量的邀請深感榮幸，但我無法接受，我很抱歉。
我在黑暗的掩護下離開了家，背叛了家人的信任。
我做出了我知道會冒著丟臉的風險的選擇。
從那時起……我發誓：忠誠、勇敢、真實。
為了履行這個誓言，我必須回家，為我的家人做出補償。";

        static string english = @"I'm deeply honored by this immesurable invitation but with humble apologies I cannot accept it.
I left home under cover of darkness and betrayed my family's trust.
I made choices I knew would risk their dishonor.
Since then... I have pledged an oath: to be loyal, brave, and true.
In order to fulfill this oath, I must return home and make amends to my family.";

        static string español = @"Me siento profundamente honrada por esta inconmensurable invitación pero con humildad me disculpo por no poder aceptarla.
Salí de casa al amparo de la oscuridad y traicioné la confianza de mi familia; tomé decisiones que sabía que arriesgarían su deshonra.
Desde entonces... he hecho un juramento: ser leal, valiente y sincera.
Para cumplir este juramento, debo regresar a casa y hacer las paces con mi familia.";

        static string test = "\u01b5\u0327\u0308\u0061\u0328\u0301\u0058\u0302\u0065\u0300\u0320oe\u035c";
        static string test2 = @"product name: 世界
Description: 我對這個無法估量的邀請深感榮幸，但我無法接受，我很抱歉。";
    }
}