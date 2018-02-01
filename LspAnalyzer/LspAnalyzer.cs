using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using LspAnalyzer.Analyze;
using LspAnalyzer.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using OmniSharp.Extensions.LanguageServer.Client;
using OmniSharp.Extensions.LanguageServer.Client.Dispatcher;
using OmniSharp.Extensions.LanguageServer.Client.Processes;
using OmniSharp.Extensions.LanguageServer.Client.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LspAnalyzer.Services.Db;


// ReSharper disable once CheckNamespace
namespace LspAnalyzer
{
    public partial class LspAnalyzer : Form
    {
        private StdioServerProcess _serverProcess;
        private LanguageClient _client;

        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
        private ILogger _logger;
        readonly BindingSource _bsServerCapabilities = new BindingSource();
        readonly BindingSource _bsClientCapabilities = new BindingSource();
        readonly BindingSource _bsServerSymbols = new BindingSource();
        readonly BindingSource _bsReferences  = new BindingSource();
        readonly BindingSource _bsHighlight  = new BindingSource();

        private AggregateGridFilter _aggregateFilterSymbol;
        private AggregateGridFilter _aggregateFilterServerCapabilities;
        private AggregateGridFilter _aggregateFilterClientCapabilities;
        private AggregateGridFilter _aggregateFilterHighlight;


        DataTable _dtServerCapabilities = new DataTable();
        DataTable _dtClientCapabilities = new DataTable();
        DataTable _dtSymbols = new DataTable();
        DataTable _dtReferences = new DataTable();
        DataTable _dtHighlight = new DataTable();

        private string _dbSymbolPath = @"c:\temp\DbSymbol.sqlite";

        
        private ProcessStartInfo _lspServerProcessStartInfo;
        //string _lspServerPath =
        //    @"d:\hoData\Development\GitHub\LSP\csharp-language-server-protocol\sample\SampleServer\bin\Debug\netcoreapp2.0\win7-x64\SampleServer.exe";
        //string _lspServerPath = @"d:\hoData\Development\GitHub\LSP\cquery\build\release\bin\cquery.exe";
        string _lspServerPath = @"d:/hoData/Development/GitHub/LSP/cquery2/build/release/bin/cquery.exe";

        


        // CQuery
        // The parameter for CQuery Language Server (https://github.com/jacobdufault/cquery)
        // --language-server
        //   Run as a language server. This implements the language server
        //    spec over STDIN and STDOUT.
        // --test-unit   Run unit tests.
        // --test-index <opt_filter_path>
        //     Run index tests. opt_filter_path can be used to specify which
        // test to run. If not provided all tests are run.
        // --log-stdin-stdout-to-stderr
        //    Print stdin and stdout messages to stderr. This is a aid for
        // developing new language clients, as it makes it easier to figure
        // out how the client is interacting with cquery.
        // --log-file <absoulte_path>
        //    Emit diagnostic logging to the given path, which is taken
        //    literally, ie, it will be relative to the current working
        //    directory.
        // --log-all-to-stderr
        //    Write all log messages to STDERR.
        // --clang-sanity-check
        //    Run a simple index test. Verifies basic clang functionality.
        //    Needs to be executed from the cquery root checkout directory.
        // --wait-for-input
        //    If true, cquery will wait for an '[Enter]' before exiting.
        //    Useful on windows.
        // --help        Print this help information.
        //string _lspServerPath = @"d:\hoData\Development\GitHub\LSP\cquery\build\release\bin\cquery.exe";
        string _lspServerParameter = @"--language-server --log-file d:/temp/CQuery.log";
        string _workSpacePath = @"d:/hoData/Projects/00Current/ZF/Work/source";

        private string _lspServerCacheDirectory = @"d:/temp/cquery/cacheDirectory";


        private string _resourceDirectory = @"../lib/LLVM-5.0.1-win64/lib/clang/5.0.1/";
        private readonly string _lspServerCompilationDatabaseDirectory = null;
        
        public LspAnalyzer()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(@"d:\\temp\\lspSampleClient.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Use Serilog
            _loggerFactory.AddSerilog();
            Log.Logger.Information($"Start logging LSP Sample Client PID={Process.GetCurrentProcess().Id}");
            _logger = _loggerFactory.CreateLogger<LspAnalyzer>();

        }
        /// <summary>
        /// Initialize GUI fields
        /// </summary>
        private void InitGui()
        {
            _dtReferences.Clear();
            _dtServerCapabilities.Clear();
            _dtClientCapabilities.Clear();
            _dtSymbols.Clear();
            txtSymbol.Text = "";
            txtDocument.Text = "";
            txtReferenceSymbol.Text = "";
            txtReferencesFilter.Text = "";
            txtReferencesSymbolName.Text = "";
            txtWsSymbolName.Text = "";
        }
        /// <summary>
        /// Initialize Server, receives the client capabilities and outputs the server capabilities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnInitializeServer_Click(object sender, EventArgs e)
        {
            InitGui();
            btnRun.Enabled = false;
            _workSpacePath = txtWorkspace.Text;
            _lspServerPath = txtServerPath.Text;

            // Runs a separate threat
            await Task.Run(RequestServerInitialize);
            if (_client?.ServerCapabilities != null)
            {
                //s = JsonConvert.SerializeObject(_client.ServerCapabilities);
                var serverCapabilities = JObject.FromObject(_client.ServerCapabilities);
                _dtServerCapabilities = JsonUtility.JObjectToDataTable(serverCapabilities);
            }
            if (_client?.ClientCapabilities != null)
            {
                var clientCapabilities = JObject.FromObject(_client.ClientCapabilities);
                _dtClientCapabilities = JsonUtility.JObjectToDataTable(clientCapabilities);
            }


            Log.Information($"Background initialite server ended");

            _bsServerCapabilities.DataSource = _dtServerCapabilities;
            _bsClientCapabilities.DataSource = _dtClientCapabilities;
            ServerProcessState(txtServerState);
            btnRun.Enabled = true;
            tabDocument.SelectedTab = tabCapabilities;
            
 
 
        }

        /// <summary>
        /// Sets the <see cref="ProcessStartInfo"/>, starts the LSP server process and initialize the client.
        /// </summary>
        /// <returns></returns>
        private async Task RequestServerInitialize()
        {
            
            // Connect via process and stdio connection
            _lspServerProcessStartInfo = new ProcessStartInfo(_lspServerPath, _lspServerParameter);

            _serverProcess = new StdioServerProcess(_loggerFactory, _lspServerProcessStartInfo);

            _client = await CreateClient(initialize: true);
        }
        /// <summary>
        /// Start Sever and create server connection for *.exe with stdin/stdout
        /// </summary>
        /// <returns></returns>
        protected async Task<LspConnection> CreateServerConnection()
        {
            if (!_serverProcess.IsRunning)
                await _serverProcess.Start();

            await _serverProcess.HasStarted;

            var connection = new LspConnection(_loggerFactory, input: _serverProcess.OutputStream, output: _serverProcess.InputStream);

            return connection;
        }
        /// <summary>
        /// Initialize Client with current workspace
        /// </summary>
        /// <param name="initialize"></param>
        /// <returns></returns>
        protected async Task<LanguageClient> CreateClient( bool initialize = true)
        {
            var client = new LanguageClient(_loggerFactory, _serverProcess);

            if (initialize)
            {
                var initializationOptions = new InitializationOptions(_resourceDirectory, _lspServerCacheDirectory, _lspServerCompilationDatabaseDirectory, _workSpacePath);
                await client.Initialize(_workSpacePath, initializationOptions);
            }

            return client;
        }

        
        
        private async void btnShutDown_Click(object sender, EventArgs e)
        {

            // backgroundWorker1.RunWorkerAsync(RequestType.Shutdown);
            btnShutDown.Enabled = false;
            _dtServerCapabilities.Clear();
            _dtClientCapabilities.Clear();
            await Task.Run(
                () => {
                    return RequestShutdown();
                }
            );
            Log.Information($"Background Shutdown server ended");
            ServerProcessState(txtServerState);
            btnShutDown.Enabled = true;
        }

        private async Task RequestShutdown()
        {
            if (_client != null) 
                    await _client.Shutdown();
            _client = null;


        }
        /// <summary>
        /// Set the Server Process state in GUI
        /// </summary>
        /// <param name="txtBox"></param>
        /// <returns></returns>
        private void ServerProcessState(TextBox txtBox)
        {
            string serverState = "";
            if (_serverProcess != null)
            {
                serverState = _serverProcess.IsRunning ? "running" : "stop";
            }
            txtBox.Text = serverState;
        }

        private void LspManager_Load(object sender, EventArgs e)
        {
            txtServerPath.Text = _lspServerPath;
            txtWorkspace.Text = _workSpacePath;
            
            //txtDocument.Text = _documentPath.Replace(_workSpacePath, "").TrimStart('\\');
            txtServerState.Text = "";
            grdServerCapabilities.DataSource = _bsServerCapabilities;
            grdClientCapabilities.DataSource = _bsClientCapabilities;
            grdWorkspaceSymbols.DataSource = _bsServerSymbols;

            // Tooltips dynamic
            grdClientCapabilities.ShowCellToolTips = false;
            grdServerCapabilities.ShowCellToolTips = false;
            grdWorkspaceSymbols.ShowCellToolTips = false;
            grdDocument.ShowCellToolTips = false;
            grdReferences.ShowCellToolTips = false;


            // Make an Aggregate filters 
            _aggregateFilterSymbol = new AggregateGridFilter(
                _bsServerSymbols,
                new List<string>(){ "Name", "File", "Kind" },
                new List<TextBox>(new[] {txtWsSymbolName, txtWsSymbolFile, txtWsSymbolKind})
                );
            // Make an Aggregate filters 
            _aggregateFilterServerCapabilities = new AggregateGridFilter(
                _bsServerCapabilities,
                new List<string>(){ "Name", "Value" },
                new List<TextBox>(new[] {txtServerCapabilitiesName, txtServerCapabilitiesValue})
            );
            _aggregateFilterClientCapabilities = new AggregateGridFilter(
                _bsClientCapabilities,
                new List<string>(){ "Name", "Value" },
                new List<TextBox>(new[] {txtClientCapabilitiesName, txtClientCapabilitiesValue})
            );
            _aggregateFilterHighlight = new AggregateGridFilter(
                _bsHighlight,
                new List<string>(){ "Kind"},
                new List<TextBox>(new[] {txtDocumentKind})
            );

            txtWsSymbolKind.Text = " (Kind = 'Variable' OR Kind = 'Function') ";
            txtWsSymbolFile.Text = "((EndLine-StartLine >0) AND (File LIKE '*'))";

        }
        /// <summary>
        /// Form closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LspManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            await Task.Run(
                () => {
                    return RequestShutdown();
                }
            );
        }
        /// <summary>
        /// Get and show Workspace Symbols
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnWsSymbol_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Client not initialized, Break");
                return;
            }
            // Empty string should be allowed to show all possible symbols
            if (txtSymbol.Text.Trim() == "xxxxxxxxx")
            {
                MessageBox.Show("No symbol to search for defined, Break");
                return;
            }

            await RequestSymbol(txtSymbol.Text);


        }

        /// <summary>
        /// Requests the symbols
        /// </summary>
        /// <returns></returns>
        private async Task RequestSymbol(string symbol)
        {
            btnWsSymbol.Enabled = false;
            

            // Request Symbol from server
            SymbolInformationContainer symbols = await  _client.Workspace.Symbol(symbol);
            _dtSymbols = (from rec in symbols
                      orderby rec.Name, rec.Location.Uri.AbsolutePath, rec.Location.Range.Start.Line
                      select new
                      {
                          Intern = rec.Name,
                          Name = GetSymbolNameFromSymbolIntern(rec.Kind.ToString(), rec.Name),
                          Kind = rec.Kind.ToString(),
                          File = rec.Location.Uri.LocalPath.Replace(_workSpacePath,"").TrimStart('/'),
                          StartLine = rec.Location.Range.Start.Line,
                          LineCount = rec.Location.Range.End.Line - rec.Location.Range.Start.Line +1,
                          StartChar = rec.Location.Range.Start.Character,
                          EndLine = rec.Location.Range.End.Line,
                          EndChar = rec.Location.Range.End.Character,
                          ContainerName = rec.ContainerName

                      }
                      ).ToDataTable();

            _bsServerSymbols.DataSource = _dtSymbols;
            btnWsSymbol.Enabled = true;
            tabDocument.SelectedTab = tabWsSymbol;
            grdWorkspaceSymbols.Columns[0].Width = 200; // Intern
            grdWorkspaceSymbols.Columns[1].Width = 300;  // Name
            grdWorkspaceSymbols.Columns[2].Width = 70; // Kind
            grdWorkspaceSymbols.Columns[3].Width = 350; // File
            //grdWorkspaceSymbols.Columns[4].Width = true;
            //grdWorkspaceSymbols.Columns[5].Width = true;
            //grdWorkspaceSymbols.Columns[6].Width = false;
            txtWsSymbolName.Text = $"*{txtSymbol.Text}";
            txtWsCount.Text = grdWorkspaceSymbols.RowCount.ToString("N0");

        }

        
        private void txtWsSymbol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                _aggregateFilterSymbol.FilterGrid();
                txtWsCount.Text = grdWorkspaceSymbols.RowCount.ToString("N0");
                e.Handled = true;
            }
        }

       /// <summary>
       /// Open file in Editor. 
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void openFileInEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Start.StartCodeFile(sender, _workSpacePath, "File",  "StartLine", "StartChar" );
        }
        /// <summary>
        /// Copy the cell according to name of all selected rows to Clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="name"></param>
        private void CopyCellValuesToClipboard(object sender, string name)
        {
            // Get the control that is displaying this context menu
            DataGridView grid = Start.GetDataGridViewOfContextMenu(sender);
            string text = "";
            string delimiter = "";
            var functions = (from f in grid.SelectedRows.Cast<DataGridViewRow>()
                orderby f.Cells[name].Value.ToString()
                select f.Cells[name].Value.ToString()).Distinct();

            foreach (var c in functions)
            {
                var function = c;
                text = $"{text}{delimiter}{function}";
                delimiter = "\r\n";
            }
            Clipboard.SetText(text);
        }

        private void filterSpecificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://msdn.microsoft.com/en-us/library/system.data.datacolumn.expression%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396");
        }

        private void lSPSpecificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://microsoft.github.io/language-server-protocol/specification");
        }

        private void copyFilePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyCellValuesToClipboard(sender, "Name");
        }

        private void grdWorkspaceSymbols_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int indexNextRow = e.RowIndex;
            txtDocument.Text = grdWorkspaceSymbols["File",indexNextRow].Value.ToString(); 

        }

        private void hoverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hover(sender, e);
        }

        private async void Hover(object sender, EventArgs e)
        {
            DataGridView grid = Start.GetDataGridViewOfContextMenu(sender);
            if (grid.SelectedRows.Count > 0)
            {
                var row = grdWorkspaceSymbols.SelectedRows[0];
                string document = Path.Combine(_workSpacePath, row.Cells["File"].Value.ToString());
                int line = int.Parse(row.Cells["StartLine"].Value.ToString());
                int position = int.Parse(row.Cells["StartChar"].Value.ToString());
                Hover hover = await _client.TextDocument.Hover(document, line, position);

                // Show hover content: Language, Function
                if (hover == null)
                {
                    MessageBox.Show($"Hover doesn't return result for symbol", "Hover doesn't return result");
                    return;
                }


                try
                {
                    var hoverValue= JsonConvert.SerializeObject(hover.Contents, Formatting.Indented);
                    //var hoverValue = (hover.Contents.MarkedStrings.Select(
                    //    markedString => $"Language: {markedString.Language.ToUpper()}\r\n {markedString.Value}")).ToArray();

                    MessageBox.Show(hoverValue, "Hover content");
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Exception:\r\n{exception}", "Error Hover content");
                    throw;
                }
            }

        }
        private async void Signature(object sender, EventArgs e)
        {
            DataGridView grid = Start.GetDataGridViewOfContextMenu(sender);
            if (grid.SelectedRows.Count > 0)
            {
                var row = grdWorkspaceSymbols.SelectedRows[0];
                string document = Path.Combine(_workSpacePath, row.Cells["File"].Value.ToString());
                int line = int.Parse(row.Cells["StartLine"].Value.ToString());
                int position = int.Parse(row.Cells["StartChar"].Value.ToString());
                var signature = await _client.TextDocument.SignatureHelp(document, line, position);

                // Show hover content: Language, Function
                if (signature == null)
                {
                    MessageBox.Show($"Signature doesn't return result for symbol", "Signature doesn't return result");
                    return;
                }
                try
                {

                // Make a linefeed separated string
                var signatureValue = JsonConvert.SerializeObject(signature);
                MessageBox.Show(signatureValue, "Signature Help content");
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Exception:\r\n{exception}", "Error SinatureHelp content");
                    throw;
                }

            }

        }


        private async void referencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdWorkspaceSymbols.SelectedRows.Count > 0)
            {
                var row = grdWorkspaceSymbols.SelectedRows[0];
                var document = GetWsDocument(row);
                var symbolName = GetWsSymbolName(row);
                var startLine = GetWsStartLine(row);
                var startPosition = GetWsStartPosition(row);
                var endLine = GetWsEndLine(row);
                var endPosition = GetWsEndPosition(row);

                // estimate position of function name in Function symbol for functions
                var symbolIntern = GetWsSymbolIntern(row);
                var kind = GetWsSymbolKind(row);

                var intern = LspFile.ReadLocation(document, startLine, startPosition, endLine, endPosition);

                // Determine start position of symbol
                Position position = GetSymbolStartPosition(intern, symbolName, new Position{Line=startLine, Character = startPosition});
                txtReferencesSymbolName.Text = symbolName;

                // ReferenceParams: Request
                // Response: LocationContainer
                var locations = await _client.TextDocument.References(document, (int)position.Line, (int)position.Character, includeDeclaration: true);

                
                var dtReference = (from rec in locations
                    orderby rec.Uri.AbsolutePath,  rec.Range.Start.Line
                    select new
                    {

                        File = rec.Uri.LocalPath.Replace(_workSpacePath,"").TrimStart('/'),
                        StartLine = rec.Range.Start.Line,
                        StartChar = rec.Range.Start.Character,
                        EndLine = rec.Range.End.Line,
                        EndChar = rec.Range.End.Character

                    }
                ).ToDataTable();



                _bsReferences.DataSource = dtReference;
                grdReferences.DataSource = _bsReferences;
                tabDocument.SelectedTab = tabReferences;
                grdReferences.Columns[0].Width = 400; // Name
                grdReferences.Columns[1].Width = 45;  // Kind
                grdReferences.Columns[2].Width = 350; // File
                grdReferences.Columns[3].Visible = true;

                txtReferenceSymbol.Text = $"{symbolIntern}";
                txtReferencesFilter.Text = "";
               

            }

        }
         private async void callersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdWorkspaceSymbols.SelectedRows.Count > 0)
            {
                var row = grdWorkspaceSymbols.SelectedRows[0];
                var document = GetWsDocument(row);
                var symbolName = GetWsSymbolName(row);
                var startLine = GetWsStartLine(row);
                var startPosition = GetWsStartPosition(row);
                var endLine = GetWsEndLine(row);
                var endPosition = GetWsEndPosition(row);

                // estimate position of function name in Function symbol for functions
                var symbolIntern = GetWsSymbolIntern(row);
                var kind = GetWsSymbolKind(row);

                var intern = LspFile.ReadLocation(document, startLine, startPosition, endLine, endPosition);

                // Determine start position of symbol
                Position position = GetSymbolStartPosition(intern, symbolName, new Position{Line=startLine, Character = startPosition});
                txtReferencesSymbolName.Text = symbolName;

                // ReferenceParams: Request
                // Response: LocationContainer
                var locations = await _client.CommandCqueryCallers.CqueryCallers(document, (int)position.Line, (int)position.Character);

                
                var dtCallers = (from rec in locations
                    orderby rec.Uri.AbsolutePath,  rec.Range.Start.Line
                    select new
                    {

                        File = rec.Uri.LocalPath.Replace(_workSpacePath,"").TrimStart('/'),
                        StartLine = rec.Range.Start.Line,
                        StartChar = rec.Range.Start.Character,
                        EndLine = rec.Range.End.Line,
                        EndChar = rec.Range.End.Character

                    }
                ).ToDataTable();



                _bsReferences.DataSource = dtCallers;
                grdReferences.DataSource = _bsReferences;
                tabDocument.SelectedTab = tabReferences;
                grdReferences.Columns[0].Width = 400; // Name
                grdReferences.Columns[1].Width = 45;  // Kind
                grdReferences.Columns[2].Width = 350; // File
                grdReferences.Columns[3].Visible = true;

                txtReferenceSymbol.Text = $"{symbolIntern}";
                txtReferencesFilter.Text = "";
               

            }

        }


        private async void txtSymbol_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Client not initialized, Break");
                return;
            }
            if (txtSymbol.Text.Trim() == "")
            {
                MessageBox.Show("No symbol to search for defined, Break");
                return;
            }

            await RequestSymbol(txtSymbol.Text);

        }

        private async void txtSymbol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (_client == null)
                {
                    MessageBox.Show("Client not initialized, Break");
                    e.Handled = true;
                    return;
                }
                if (txtSymbol.Text.Trim() == "xxxxxx")
                {
                    MessageBox.Show("No symbol to search for defined, Break");
                    e.Handled = true;
                    return;
                }
                e.Handled = true;
                await RequestSymbol(txtSymbol.Text);
            }
        }

        private void toolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            Start.StartCodeFile(sender, _workSpacePath, "File",  "StartLine", "StartChar" );
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CopyCellValuesToClipboard(sender, "File");
        }

        private void toolStripMenuHover_Click(object sender, EventArgs e)
        {
            Hover(sender, e);
        }

        private void signitureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Signature(sender, e);
        }
        /// <summary>
        ///     Create an <see cref="LspDispatcher"/> for use in tests.
        /// </summary>
        /// <returns>
        ///     The <see cref="LspDispatcher"/>.
        /// </returns>
        LspDispatcher CreateDispatcher() => new LspDispatcher(new Serializer(ClientVersion.Lsp3));

        private void txtServerCapabilityName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                _aggregateFilterServerCapabilities.FilterGrid();
                e.Handled = true;
            }
        }

        private void txtClientCapabilitiesName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                _aggregateFilterClientCapabilities.FilterGrid();
                e.Handled = true;
            }

        }
        /// <summary>
        /// Highlights the 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void highlightInDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdWorkspaceSymbols.SelectedRows.Count > 0)
            {
                DataGridViewRow row = grdWorkspaceSymbols.SelectedRows[0];
                string symbolName = GetWsSymbolName(row);
                string document = GetAbsoluteFilePath(GetWsDocument(row));
                int startLine = GetWsStartLine(row);
                int startPosition = GetWsStartPosition(row);

                // estimate position of symbol name in symbol intern string
                var symbolIntern = GetWsSymbolIntern(row);
                startPosition += symbolIntern.IndexOf(symbolName, StringComparison.Ordinal);

                txtReferencesSymbolName.Text = symbolName;

                // ReferenceParams: Request
                // Response: LocationContainer
                DocumentHighlightContainer locations = await _client.TextDocument.Highlight(document, startLine, startPosition);

                try
                {
                    _dtHighlight = (from rec in locations
                        orderby rec.Range.Start.Line, rec.Range.Start.Character,   rec.Kind
                        select new
                        {
                            Name = LspFile.ReadLocation(document,(int)rec.Range.Start.Line,(int)rec.Range.Start.Character,(int)rec.Range.End.Line,(int)rec.Range.End.Character),
                            Kind =rec.Kind.ToString(),
                            StartLine = rec.Range.Start.Line,
                            StartChar = rec.Range.Start.Character,
                            EndLine = rec.Range.End.Line,
                            EndChar = rec.Range.End.Character,
                            File = GetReleativFilePath(document)
                        }
                    ).ToDataTable();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(JsonConvert.SerializeObject(locations),"Can't Symbol with LINQ");
                }



                _bsHighlight.DataSource = _dtHighlight;
                grdDocument.DataSource = _bsHighlight;
                tabDocument.SelectedTab = tabDocumentSymbol;
                grdDocument.Columns[0].Width = 200; // Name
                grdDocument.Columns[1].Width = 100;  // Kind
                grdDocument.Columns[2].Width = 100;  // Start
                grdDocument.Columns[3].Width = 100;  // Start


                txtDocumentSymbolName.Text = $"{symbolIntern}";
                txtReferencesFilter.Text = "";
               

            }

        }
        /// <summary>
        /// Get symbol position in internal symbol. 
        /// </summary>
        /// <param name="internSymbol"></param>
        /// <param name="symbol"></param>
        /// <param name="position">Position of internal symbol</param>
        /// <returns>Position of symbol</returns>
        private static Position GetSymbolStartPosition(string internSymbol, string symbol, Position position)
        {
            var lines = 0;
            foreach (var line in internSymbol.Split(new [] {"\r\n"},StringSplitOptions.None))
            {
                int pos = line.IndexOf(symbol);
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
        
        /// <summary>
        /// Get symbol name from LSP server returned symbol, named 'Intern'
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="symbolIntern"></param>
        /// <returns></returns>
        private static string GetSymbolNameFromSymbolIntern(string kind, string symbolIntern)
        {
            string symbolName = symbolIntern;
            switch (kind)
            {
                case "Function":
                    var rxFunctionName = new Regex(@"\s((\w*)\s*\()");
                    var match = rxFunctionName.Match(symbolIntern);
                    if (match.Success && match.Groups.Count == 3)
                    {
                        symbolName = match.Groups[2].Value;
                    }
                    break;

                case "Variable":
                    var rxVariableName = new Regex(@"\w*$");
                    var matchVariable = rxVariableName.Match(symbolIntern);
                    if (matchVariable.Success )
                    {
                        symbolName = matchVariable.Groups[0].Value;
                    }
                    break;
                case "File":
                    symbolName = Path.GetFileName(symbolIntern);
                    break;
                default:
                    symbolName = symbolIntern;
                    break;
            }
            return symbolName;
        }




        private static string GetWsSymbolIntern(DataGridViewRow row)
        {
            return row.Cells["Intern"].Value.ToString();
        }

        private static string GetWsSymbolKind(DataGridViewRow row)
        {
            return row.Cells["Kind"].Value.ToString();
        }

        /// <summary>
        /// Get Workspace start Position
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static int GetWsStartPosition(DataGridViewRow row)
        {
            int position = int.Parse(row.Cells["StartChar"].Value.ToString());
            return position;
        }

        /// <summary>
        /// Get Workspace Start line of selected element
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static int GetWsStartLine(DataGridViewRow row)
        {
            return int.Parse(row.Cells["StartLine"].Value.ToString());
        }
        /// <summary>
        /// Get Workspace start Position
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static int GetWsEndPosition(DataGridViewRow row)
        {
            int position = int.Parse(row.Cells["EndChar"].Value.ToString());
            return position;
        }

        /// <summary>
        /// Get Workspace Start line of selected element
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static int GetWsEndLine(DataGridViewRow row)
        {
            return int.Parse(row.Cells["EndLine"].Value.ToString());
        }

        /// <summary>
        /// Get Workspace document of selected Row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetWsDocument(DataGridViewRow row)
        {
            return Path.Combine(_workSpacePath, row.Cells["File"].Value.ToString());
        }
        /// <summary>
        /// Get Workspace symbol name of selected Row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetWsSymbolName(DataGridViewRow row)
        {
            return row.Cells["Name"].Value.ToString();
        }

        /// <summary>
        /// Gets the relative file patch from an absolute or relative path
        /// </summary>
        /// <param name="absoluteFilePath"></param>
        /// <returns></returns>
        private string GetReleativFilePath(string absoluteFilePath)
        {
            return absoluteFilePath.Replace(_workSpacePath, "").Trim('\\').Trim('/');
        }
        /// <summary>
        /// Gets the absolute file path from a relative or an absolute file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetAbsoluteFilePath(string filePath)
        {
            var relPath = GetReleativFilePath(filePath);
            return Path.Combine(_workSpacePath,relPath);
        }


        private void txtDocumentKind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                _aggregateFilterSymbol.FilterGrid();
                e.Handled = true;
            }

        }
        /// <summary>
        /// Output the Column Header/Technical name as Tooltip. It uses the data column of the associated data source. 
        /// Disable Tooltip in the grid (ShowCellToolTips = false;).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        // Enter a cell
        private void grdInterfaces_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex != -1)

            {
                var grid = (DataGridView) sender;
                var dataPropertyName = grid.Columns[e.ColumnIndex].DataPropertyName;
                toolTip1.SetToolTip((DataGridView) sender, dataPropertyName);
            }
            else
            {
                toolTip1.Hide((DataGridView) sender);
            }

        }

        private void defineCSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    _workSpacePath = fbd.SelectedPath;
                    txtWorkspace.Text = _workSpacePath;

                }
            }
        }

        private void lpoadServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_serverProcess != null && _serverProcess.IsRunning)
            {
                MessageBox.Show("Can't change a running server", "Server is running, Break");
            }
            else
            {
                OpenFileDialog serverPathOpenDialog = new OpenFileDialog();
                serverPathOpenDialog.Filter = @"Exe Files (*.exe)|*.exe|All File (*.*)|*.*";
                serverPathOpenDialog.FilterIndex = 1;

                serverPathOpenDialog.Multiselect = false;
                if (serverPathOpenDialog.ShowDialog() == DialogResult.OK)    
                {     
                    _lspServerPath = serverPathOpenDialog.FileName;
                    txtServerPath.Text = _lspServerPath;

                }
            }

        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Helmut-Ortmann/lsp-csharp-analyzer/wiki");
        }
        /// <summary>
        /// Generate Symbol DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateSymbols_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SymbolDb symbolDb = new SymbolDb(_dbSymbolPath);
            symbolDb.LoadFiles(_workSpacePath);
            var count = symbolDb.LoadItems(_workSpacePath, _dtSymbols);
            Cursor.Current = Cursors.Default;

            MessageBox.Show($"SymbolDB='{_dbSymbolPath}'\r\nWorkspace='{_workSpacePath}'\r\nLoaded symbols={count:N0}", "Symbols added from grid");
        }

        private void btnCreateSSQLiteDB_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SymbolDb symbolDb = new SymbolDb(_dbSymbolPath);
            symbolDb.Create();
            Cursor.Current = Cursors.Default;
            MessageBox.Show($"SymbolDB='{_dbSymbolPath}'", "SymbolDB created");

        }
    }
}
