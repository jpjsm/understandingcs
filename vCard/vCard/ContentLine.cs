using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace vCard
{
    public class ContentLine
    {
        private readonly Dictionary<string, List<string>> nameparams = new Dictionary<string, List<string>>();
        private StringBuilder value = new StringBuilder();

        public string Group { get; }
        public string Name { get; }
        public string Value { get { return value.ToString(); } }
        public IReadOnlyDictionary<string, List<string>> NameParams { get { return nameparams; } }

        public ContentLine(string name, string value, string group, Dictionary<string, List<string>> prms)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            if (!string.IsNullOrEmpty(value))
            {
                this.value.Append(value);
            }

            if (!string.IsNullOrEmpty(group))
            {
                Group = group;
            }

            foreach (KeyValuePair<string, List<string>> kvp in prms)
            {
                if (string.IsNullOrEmpty(kvp.Key))
                {
                    throw new ArgumentNullException(nameof(prms), "Parameter with no name.");
                }

                nameparams.Add(kvp.Key, kvp.Value is null ? new List<string>() : kvp.Value);
            }
        }

        public ContentLine(string name, string value)
            : this(name, value, string.Empty, new Dictionary<string, List<string>>())
        {
        }

        public ContentLine(string group, string name, string value)
            : this(name, value, group, new Dictionary<string, List<string>>())
        {
        }

        public ContentLine(Match matchedContentLine)
        {
            Name = matchedContentLine.Groups["name"].Value;

            this.value.Append(matchedContentLine.Groups["value"].Value);

            Group = matchedContentLine.Groups["group"].Value;

            foreach (Capture capturedParam in matchedContentLine.Groups["params"].Captures)
            {
                List<string> paramvalues = new List<string>();
                Match capturedParamNameValues = Regex.Match(capturedParam.Value, RegexPatterns.ParamNameValuePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                string paramname = capturedParamNameValues.Groups["paramname"].Value;

                foreach (Capture capturedValue in capturedParamNameValues.Groups["paramvalue"].Captures)
                {
                    paramvalues.Add(capturedValue.Value);
                }

                if (!nameparams.ContainsKey(paramname))
                {
                    nameparams.Add(paramname, paramvalues);
                }
                else
                {
                    nameparams[paramname].AddRange(paramvalues);
                }
            }
        }

        public void AddFoldedLIne(string foldedline)
        {
            if (foldedline is null)
                foldedline = string.Empty;

            if (Regex.IsMatch(Name, RegexPatterns.SingletonCardNamePattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
                throw new ApplicationException(string.Format("Can't add folded content to singleton line: {0}", Name));

            this.value.Append(foldedline);

        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(Group))
            {
                str.Append(Group);
                str.Append(".");
            }

            str.Append(Name);
            foreach (KeyValuePair<string, List<string>> kvp in nameparams)
            {
                str.Append(";");
                str.Append(kvp.Key);
                if (kvp.Value.Count > 0)
                {
                    str.Append("=");
                    str.Append(string.Join(",", kvp.Value));
                }
            }

            str.Append(":");
            if (value.Length > 0 )
            {
                str.Append(value.ToString());
            }

            return str.ToString();
        }
    }
}
