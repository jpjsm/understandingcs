using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Text;

namespace morse_mnemonics
{
    public readonly record struct MorseCode(char letter, string code);

    public static class MorseAlphabet
    {
        private static Dictionary<char, char> accented2base = new Dictionary<char, char>()
            {
                {'Á', 'a' },
                {'É', 'e' },
                {'Í', 'i' },
                {'Ó', 'o' },
                {'Ú', 'u' },
                {'Ü', 'u' },
                {'á', 'a' },
                {'é', 'e' },
                {'í', 'i' },
                {'ó', 'o' },
                {'ú', 'u' },
                {'ü', 'u' },
            };

        public static char ToBaseLetter(char letter)
        {

            char l = Char.ToLowerInvariant(letter);

            if (accented2base.ContainsKey(l))
                l = accented2base[l];

            return l;
        }

        private static readonly MorseCode[] codes = new[] 
        { 
            new MorseCode('a',".-"),
            new MorseCode('b',"-..."),
            new MorseCode('c',"-.-."),
            new MorseCode('d',"-.."),
            new MorseCode('e',"."),
            new MorseCode('f',"..-."),
            new MorseCode('g',"--."),
            new MorseCode('h',"...."),
            new MorseCode('i',".."),
            new MorseCode('j',".---"),
            new MorseCode('k',"-.-"),
            new MorseCode('l',".-.."),
            new MorseCode('m',"--"),
            new MorseCode('n',"-."),
            new MorseCode('ñ',"--.--"),
            new MorseCode('o',"---"),
            new MorseCode('p',".--."),
            new MorseCode('q',"--.-"),
            new MorseCode('r',".-."),
            new MorseCode('s',"..."),
            new MorseCode('t',"-"),
            new MorseCode('u',"..-"),
            new MorseCode('v',"...-"),
            new MorseCode('w',".--"),
            new MorseCode('x',"-..-"),
            new MorseCode('y',"-.--"),
            new MorseCode('z',"--.."),
        };

        private readonly static Dictionary<char, string> ToCode;
        private readonly static Dictionary<string, char> ToLetter;

        static MorseAlphabet()
        {
            ToCode = codes.ToDictionary(c => c.letter, c => c.code);
            ToLetter = codes.ToDictionary(c => c.code, c => c.letter);
        }

        public static string Code(char letter) { return ToCode[ToBaseLetter(letter)]; }

        public static char Letter(string code) { return ToLetter[code]; }

        public static string Encode(string text)
        {
            StringBuilder msg = new StringBuilder();
            string[] words = text.Split(" ");
            foreach (string word in words) 
            {
                foreach (char letter in word)
                {
                    msg.Append(ToCode[ToBaseLetter(letter)]);
                    msg.Append(" ");
                }

                msg.Append("  ");
            }

            return msg.ToString();
        }

        public static string Decode(string msg)
        {
            StringBuilder text = new StringBuilder();
            string[] codes = msg.Split(" ",StringSplitOptions.None); // Keep multiple empty entris as word separator
            foreach(string code in codes)
            {
                if (String.IsNullOrEmpty(code))
                {
                    text.Append(" ");
                    continue;
                }

                text.Append(ToLetter[code]);
            }

            return text.ToString();
        }
    }


    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine($"Buenas salenas!");

            //Load words
            Dictionary<char, string[]> palabras = new Dictionary<char, string[]>();

            int total_palabras = 0;
            foreach (string filename in Directory.EnumerateFiles("./starting_letter/", "*.txt"))
            {
                Console.Write($"reading: {Path.GetFileName(filename)}");
                char entry = Path.GetFileNameWithoutExtension(filename)[0];
                string[] words = File.ReadAllLines(filename);
                palabras.Add(entry, words);
                Console.WriteLine($"\t{words.Length:N0} palabras leídas.");
                total_palabras += words.Length;
            }

            Console.WriteLine($"Total palabras diccionario: {total_palabras:N0}");

            Dictionary<char, Tuple<string, List<string>>> results = new Dictionary<char, Tuple<string, List<string>>>();
            Dictionary<char, string> vocales2morse = new Dictionary<char, string>() 
            {
                {'a',"." },
                {'e',"." },
                {'i',"." },
                {'o',"-" },
                {'u',"." },
            };
            // Search for mnemonics... brut force
            foreach (KeyValuePair<char, string[]> kvp in palabras)
            {
                List<string> mnemonics = new List<string>();
                char letter = kvp.Key;
                string code = MorseAlphabet.Code(letter);
                string[] entradas = kvp.Value;
                foreach (string entrada in entradas) 
                {
                    StringBuilder candidate = new StringBuilder();
                    char letra_anterior = ' ';

                    foreach (char letra in entrada)
                    {
                        char base_letter = MorseAlphabet.ToBaseLetter(letra);

                        // skip mute u in 'qu' (i.e. queso) or 'gu' (guitarra)
                        if (vocales2morse.ContainsKey(base_letter)
                            && !((letra_anterior == 'Q' || letra_anterior == 'q' || letra_anterior == 'G' || letra_anterior == 'g') && (letra == 'u' || letra == 'U')))
                        {

                            candidate.Append(vocales2morse[base_letter]);
                        }

                        letra_anterior = letra;
                    }

                    if (code == candidate.ToString()) 
                    {
                        mnemonics.Add(entrada);
                    }
                }

                results[letter]= new Tuple<string, List<string>>(code, mnemonics);
            }

            foreach (char letter in results.Keys.OrderBy(k => k))
            {
                Console.WriteLine($"'{letter}' ({results[letter].Item1}): posibles mnemónicos {results[letter].Item2.Count:n0}");
                using (StreamWriter sw = File.AppendText("summary.txt"))
                {
                    sw.WriteLine($"{letter}\t{results[letter].Item1}\t{results[letter].Item2.Count:n0}");
                }

                File.WriteAllLines($"candidate_{letter}.txt", results[letter].Item2, Encoding.UTF8);
            }
        }
    }
}