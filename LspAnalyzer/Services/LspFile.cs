using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

// ReSharper disable once CheckNamespace
namespace LspAnalyzer.Services
{
    public static class LspFile
    {
        /// <summary>
        /// Read the LSP location from file, pass string for position
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="startLine"></param>
        /// <param name="startPosition"></param>
        /// <param name="endLine"></param>
        /// <param name="endPosition"></param>
        /// <returns></returns>
        public static string ReadLocation(string fileName, string startLine, string startPosition, string endLine, string endPosition)
        {


            try
            {
                var intStartLine = int.Parse(startLine);
                var intEndLine = int.Parse(endLine);
                var intStartPosition = int.Parse(startPosition);
                var intEndPosition = int.Parse(endPosition);
                return ReadLocation(fileName, intStartLine, intStartPosition, intEndLine, intEndPosition);

            }
            catch (Exception e)
            {
                MessageBox.Show($@"File: '{fileName}'\r\nLine: {startLine} to {endLine}\r\nLine: {startPosition} to {endPosition}\r\n Exception:{e}", "Can't read location of file");
                return "";
            }

        }
        /// <summary>
        /// Read the LSP location from file, pass integer for position. It handles multi lines.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="startLine"></param>
        /// <param name="startPosition"></param>
        /// <param name="endLine"></param>
        /// <param name="endPosition"></param>
        /// <returns></returns>
        public static string ReadLocation(string fileName, int startLine, int startPosition, int endLine, int endPosition)
        {
           
            
            try
            {

                if (startLine == endLine)
                {
                    return File.ReadLines(fileName).Skip(startLine).Take(endLine - startLine + 1).First().Substring(startPosition, endPosition - startPosition );
                }
                else
                {
                    var multiLine = File.ReadLines(fileName).Skip(startLine).Take(endLine - startLine + 1).ToArray();
                    var s = "";
                    var delimiter = "";
                    for (var i = 0; i < multiLine.Count(); i++)
                    {
                        // last line copy only to endPosition
                        s = i == multiLine.Count() - 1
                            ? $"{s}{delimiter}{multiLine[i].Substring(0, endPosition)}"
                            : $"{s}{delimiter}{multiLine[i]}";
                        delimiter = "\r\n";
                    }

                    return s;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show($"File: '{fileName}'\r\nLineStart: {startLine} to {endLine}\r\nLineEnd: {startPosition} to {endPosition}\r\n Exception:{e}", "Can't read location of file");
                return "";
            }

        }
        
    }
}
