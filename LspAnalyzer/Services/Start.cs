using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using LspAnalyzer.Services;
using static System.Int32;

// ReSharper disable once CheckNamespace
namespace LspAnalyzer.Analyze
{
    public static class Start
    {
        /// <summary>
        /// Open the file according to column name with the editor.
        /// - Start VS Code: 'Code [fileName]:[lineNumber]:[characterPosition] -g'
        /// - Copies the name (Column 'Name') to Clipboard (if Name column exists)
        /// Column and Line are relative 0, VS Code handles the numbers relative 1
        /// </summary>
        /// <param name="sender">Grid row with 'SelectionMode' = FullRowSelect</param>
        /// <param name="workSpacePath"></param>
        /// <param name="columnName">Name of the Grid column</param>
        /// <param name="lineNumberName">Name of the Grid column for start line number</param>
        /// <param name="characterPositionName">Name of the Grid column for start character</param>
        public static void StartCodeFile(object sender, string workSpacePath, string columnName, string lineNumberName = "", string characterPositionName  = "")
        {
            // Get the control that is displaying this context menu
            DataGridView grid = GetDataGridViewOfContextMenu(sender);
            if (grid.SelectedRows.Count > 0)
            {
                var row = grid.SelectedRows[0];
                string filePath = row.Cells[columnName].Value.ToString();
                // relative path
                if (! Path.IsPathRooted(filePath))
                    filePath = Path.Combine(workSpacePath, filePath);

                // Character position in line relative  1
                string characterPosition = characterPositionName != ""
                    ? $":{Parse(row.Cells[characterPositionName].Value.ToString()) + 1}"
                    : "";
                string lineNumber = lineNumberName != ""
                    ? $":{Parse(row.Cells[lineNumberName].Value.ToString()) + 1}{characterPosition} -g"
                    : "";

                StartApp(@"Code", $@"""{filePath}""{lineNumber}");
                if (grid.Columns.Contains("Name"))
                {
                    // Copy Function name to Clipboard
                    string functionName = row.Cells["Name"].Value.ToString().Trim();
                    Clipboard.SetText(functionName);
                }
            }



        }
        /// <summary>
        /// Start a program with parameters. Make sure the '%PATH%' Environment variable is correctly set.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="par"></param>
        public static void StartApp(string app, string par)
        {
            var p = new Process
            {
                StartInfo =
                {
                    FileName = app,
                    Arguments = par,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            try
            {
                p.Start();

            }
            catch (Exception e)
            {
                MessageBox.Show(p.StartInfo.FileName + " " +
                                p.StartInfo.Arguments + "\n\n" +
                                "Have you set the %path% environment variable?\n\n" + e,
                    $"Can't start '{app}'!");
            }
        }

        public static void StartFile(string path)
        {
            if (File.Exists(path) || Directory.Exists(path) )
            {
                try
                {

                    Process.Start(path);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Can't start file '{path}'!\n\n{e}",
                        $"Can't start file '{path}'!");
                }
            }
        }
        /// <summary>
        /// Start Code for a position
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="pos"></param>
        public static void StartFile(string filePath, Position pos)
        {
            // make position line/character relative  1
            string position = $":{pos.Line + 1}:{pos.Character + 1}";
            StartApp(@"Code", $@"""{filePath}""{position} -g");

        }

        /// <summary>
        /// Get the DatagridView associated to the context menu
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static DataGridView GetDataGridViewOfContextMenu(object sender)
        {
            // Try to cast the sender to a ToolStripItem
            if (sender is ToolStripItem menuItem)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                if (menuItem.Owner is ContextMenuStrip owner)
                {
                    // Get the control that is displaying this context menu
                    return (DataGridView) owner.SourceControl;
                }
            }
            return null;
        }
    }
}
