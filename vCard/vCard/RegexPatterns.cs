using System;
using System.Collections.Generic;
using System.Text;

namespace vCard
{
    public static class RegexPatterns
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
        public const string N_pattern = @""; 
    }
}
