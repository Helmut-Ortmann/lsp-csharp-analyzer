using System;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

// ReSharper disable once CheckNamespace
namespace LspAnalyzer.Services
{
    public class LspAnalyzerHelper
    {
        /// <summary>
        /// Get symbol position in internal symbol. 
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="symbol"></param>
        /// <param name="position">Position of internal symbol</param>
        /// <returns>Position of symbol</returns>
        public static Position GetSymbolStartPosition(string signature, string symbol, Position position)
        {
            var lines = 0;
            foreach (var line in signature.Split(new [] {"\r\n"},StringSplitOptions.None))
            {
                int pos = line.IndexOf(symbol, StringComparison.Ordinal);
                if (pos > -1)
                {
                    position.Line = position.Line + lines;
                    if (lines == 0) position.Character = position.Character + pos;
                    else position.Character = pos;
                    return position;
                }

                lines += 1;
            }

            return position; // nothing found
        }
    }
}
