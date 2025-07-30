using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RegexMatchingGroups
{
    class Program
    {
        public const string QuotedStringPattern = "\"(?<text>(\\\\.|[^\\\"])*)\"";
        public const string UnquotedSafeStringPattern = "[^;:,\"]*";
        public const string ParamValuePattern = "(?<paramvalue>" + QuotedStringPattern + "|" + UnquotedSafeStringPattern + ")";
        public const string ParamNameValuePattern = "(?<paramname>[-A-Za-z0-9]+)(?<paramnamevalues>=" + ParamValuePattern + "(," + ParamValuePattern + ")*" + ")?";
        public const string ContentLinePatternStandard =
            @"^" +
            @"(?<group>[-A-Za-z0-9]+\.)?" +
            @"(?<name>BEGIN|VERSION|END|SOURCE|KIND|FN|N|NICKNAME|PHOTO|BDAY|ANNIVERSARY|GENDER|ADR|TEL|EMAIL|IMPP|LANG|TZ|GEO|TITLE|ROLE|LOGO|ORG|MEMBER|RELATED|CATEGORIES|NOTE|PRODID|REV|SOUND|UID|CLIENTPIDMAP|URL|KEY|FBURL|CALADRURI|CALURI|XML)" +
            @"(?<params>;" + ParamNameValuePattern + ")*" +
            @":(?<value>.*)?" +
            @"$";
        public const string ContentLinePatternNonStandard =
            @"^" +
            @"(?<group>[-A-Za-z0-9]+\.)?" +
            @"(?<name>[-A-Za-z0-9]+)" +
            @"(?<params>;" + ParamNameValuePattern + ")*" +
            @":(?<value>.*)?" +
            @"$";
        public const string FoldedLinePattern = @"^ (?<foldedline>.*)$";

        public const string SingletonCardNamePattern = "BEGIN|VERSION|END";

        static void Main(string[] args)
        {
            string[] lines =
            {
                @"BEGIN:VCARD",
                @"VERSION:3.0",
                @"N;CHARSET=UTF-8;NAME:saintemarie;Titi Linda;;;",
                @"FN;CHARSET=UTF-8:Titi Linda saintemarie",
                @"NICKNAME;CHARSET=UTF-8:Titi",
                @"X-PHONETIC-FIRST-NAME;CHARSET=UTF-8:",
                @"X-PHONETIC-LAST-NAME;CHARSET=UTF-8:",
                @"ADR;CHARSET=UTF-8;HOME:;;Ave San JoseMaria Escriva de Balaguer 9435, Dep 802  \nVitacura;Santiago;;;Chile",
                @"TEL;CHARSET=UTF-8;TYPE=HOME:+56227927014",
                @"TEL;CHARSET=UTF-8,UTF-7,UTF-32,ASCII;TYPE=CELL:+56954121321",
                @"item0.EMAIL;CHARSET=UTF-8:xsaintemarie@hotmail.com",
                @"item0.X-ABLabel:",
                @"TITLE;CHARSET=UTF-8:",
                @"ORG;CHARSET=UTF-8:;",
                @"NOTE;CHARSET=UTF-8:This contact is read-only. To make changes, tap the link above to edit in Outlook.\n\nThis contact is read-only. To make changes, tap the link above to edit in Outlook.\n\nThis contact is read-only. To make changes, tap the link above to edit in Outlook.",
                @"item1.URL;CHARSET=UTF-8:ms-outlook://people/AQMkADAwATE0YzEwLWUxMGEtZjQ2OS0wMAItMDAKAEYAAANxL-XdGp9cRbBGmFCsjcJ3BwAOXpOvvBpqR4MQJjIV6njYAAACAQ4AAAAOXpOvvBpqR4MQJjIV6njYAAACGkwAAAA=?accountKey=63fb3a92f77d736e3ee1517477a784c5&accountExportedAt=566905691.852914",
                @"item1.X-ABLabel:_$!<HomePage>!$_",
                @"item2.X-IM;CHARSET=UTF-8:xsaintemarie@hotmail.com",
                @"item2.X-ABLabel;CHARSET=UTF-8:IM",
                @"END:VCARD"
            };

            foreach(string line in lines)
            {
                Console.WriteLine("Line                : {0}", line);
                Match matchedContentLine = Regex.Match(line, ContentLinePatternNonStandard, RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);
                if(matchedContentLine.Success)
                {
                    Console.WriteLine("Line group          : {0}", matchedContentLine.Groups["group"].Value);
                    Console.WriteLine("Line Name           : {0}", matchedContentLine.Groups["name"].Value);
                    Console.WriteLine("Line Value          : {0}", matchedContentLine.Groups["value"].Value);
                    foreach (Capture capturedParam in matchedContentLine.Groups["params"].Captures)
                    {
                        Console.WriteLine("Line Name parameters: {0}", capturedParam.Value);
                        Match capturedParamNameValues = Regex.Match(capturedParam.Value, ParamNameValuePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                        Console.WriteLine("Line Name param-name: {0}", capturedParamNameValues.Groups["paramname"].Value);

                        foreach (Capture capturedValue in capturedParamNameValues.Groups["paramvalue"].Captures)
                        {
                            Console.WriteLine("Line Name param-name-values: {0}", capturedValue.Value);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Not a matched line");
                }

                Console.WriteLine();
            }
        }
    }
}
