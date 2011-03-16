using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BVSoftware.Web.Css
{
    public class Variables
    {

        public static string DefaultDelimiterStart()
        {
            return "/* bv";
        }
        public static string DefaultDelimiterEnd()
        {
            return "*/";
        }
        public static char DefaultDelimiterName()
        {
            return ':';
        }

        public static Queue<string> Tokenize(string input)
        {
            return Tokenize(input, DefaultDelimiterStart(), DefaultDelimiterEnd());
        }
        public static Queue<string> Tokenize(string input, string delimiterStart, string delimiterEnd)
        {
            Queue<string> result = new Queue<string>();

            bool parsing = false;
            int currentPosition = 0;
            while (currentPosition < input.Length)
            {
                if (parsing)
                {
                    int closingIndex = input.IndexOf(delimiterEnd, currentPosition + delimiterStart.Length);
                    if (closingIndex > 0 && closingIndex < input.Length)
                    {
                        int tagLength = closingIndex - currentPosition + delimiterEnd.Length;
                        result.Enqueue(input.Substring(currentPosition, tagLength));
                        currentPosition = closingIndex + delimiterEnd.Length;
                    }
                    else
                    {
                        // no end delimiter found, just dump the text
                        result.Enqueue(input.Substring(currentPosition, input.Length - currentPosition));
                        currentPosition = input.Length;
                    }
                    parsing = false;
                }
                else
                {
                    int nexttag = input.IndexOf(delimiterStart, currentPosition);
                    if (nexttag >= currentPosition && nexttag < input.Length)
                    {
                        if (nexttag > currentPosition)
                        {
                            result.Enqueue(input.Substring(currentPosition, nexttag - currentPosition));
                        }
                        parsing = true;
                        currentPosition = nexttag;
                    }
                    else
                    {
                        // no more tags found so dump everything else
                        result.Enqueue(input.Substring(currentPosition, input.Length - currentPosition));
                        currentPosition = input.Length;
                    }
                }
            }

            return result;
        }

        public static Dictionary<string, CssVariable> ParseVariablesFromFile(string filePath)
        {
            Dictionary<string, CssVariable> result = new Dictionary<string, CssVariable>();

            if (File.Exists(filePath))
            {
                string fullFile = File.ReadAllText(filePath);
                result = ParseVariables(fullFile);
            }

            return result;
        }

        public static Dictionary<string, CssVariable> ParseVariables(string input)
        {
            return ParseVariables(input, DefaultDelimiterStart(), DefaultDelimiterEnd(), DefaultDelimiterName());
        }
        public static Dictionary<string, CssVariable> ParseVariables(string input, string tokenStart, string tokenEnd, char delimiterName)
        {
            Dictionary<string, CssVariable> result = new Dictionary<string, CssVariable>();
            
            Queue<string> parts = Tokenize(input, tokenStart, tokenEnd);

            bool isParsing = false;
            string parsingId = string.Empty;
            string parsingValue = string.Empty;

            foreach (string part in parts)
            {
                if (isParsing)
                {
                    if (part.StartsWith(tokenStart))
                    {
                        // potential closing tag
                        CssVariable closingVar = CssVariable.ParseFromTag(part, tokenStart, tokenEnd, delimiterName);
                        if (closingVar.Id == parsingId)
                        {
                            isParsing = false;
                            if (result.ContainsKey(parsingId))
                            {
                                result[parsingId].CurrentValue = parsingValue;
                            }
                            else
                            {
                                result.Add(parsingId, new CssVariable() { Id = parsingId, CurrentValue = parsingValue });
                            }
                        }
                        else
                        {
                            // not our closing tag
                            parsingValue += part.Trim();
                        }
                    }
                    else
                    {
                        // not closing tag, add to color value
                        parsingValue += part.Trim();
                    }

                }
                else
                {
                    if (part.StartsWith(tokenStart))
                    {
                        CssVariable tempVar = CssVariable.ParseFromTag(part,tokenStart,tokenEnd,delimiterName);

                        // Found a token to start.
                        if (CssVariable.TagContainsName(part, tokenStart, tokenEnd, delimiterName))
                        {
                            // found a friendly name tag
                            if (result.ContainsKey(tempVar.Id))
                            {
                                result[tempVar.Id].Name = tempVar.Name;
                            }
                            else
                            {
                                result.Add(tempVar.Id, tempVar);
                            }
                        }
                        else
                        {
                            // found a tag that needs to be parsed for color
                            isParsing = true;
                            parsingId = tempVar.Id;
                            parsingValue = string.Empty;
                        }
                    }
                    else
                    {
                        // We're not parsing and not in a tag so ignore
                    }
                }

            }

            return result;
        }


        public static string ReplaceCssVariables(string input, Dictionary<string, CssVariable> vars)
        {
            return ReplaceCssVariables(input, vars, DefaultDelimiterStart(), DefaultDelimiterEnd(), DefaultDelimiterName());
        }
        public static string ReplaceCssVariables(string input, Dictionary<string, CssVariable> vars, string tokenStart, string tokenEnd, char delimiterName)
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder();

            Queue<string> parts = Tokenize(input, tokenStart, tokenEnd);

            bool isParsing = false;
            string parsingId = string.Empty;

            foreach (string part in parts)
            {
                if (isParsing)
                {
                    if (part.StartsWith(tokenStart))
                    {
                        // potential closing tag
                        CssVariable closingVar = CssVariable.ParseFromTag(part,tokenStart,tokenEnd, delimiterName);
                        if (closingVar.Id == parsingId)
                        {
                            // found closing tag, write new value
                            isParsing = false;
                            sb.Append(vars[parsingId].CurrentValue);
                            sb.Append(part);
                        }
                    }
                }
                else
                {
                    if (part.StartsWith(tokenStart))
                    {
                        // found a tag that needs to be parsed for color
                        CssVariable tempVar = CssVariable.ParseFromTag(part,tokenStart,tokenEnd,delimiterName);
                        if (vars.ContainsKey(tempVar.Id) && !CssVariable.TagContainsName(part,tokenStart,tokenEnd,delimiterName))
                        {
                            // Yes, we have a replacement for this so parse it
                            isParsing = true;
                            parsingId = tempVar.Id;
                        }
                        sb.Append(part);
                    }
                    else
                    {
                        // We're not parsing and not in a tag so just dump it
                        sb.Append(part);
                    }
                }

            }

            result = sb.ToString();
            return result;
        }
    }
}
