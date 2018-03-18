using System.Collections.Generic;
using LspAnalyzer.Services;
using Newtonsoft.Json;
// ReSharper disable InconsistentNaming

namespace LspAnalyzer.Settings
{
    public class SettingsItem
    {
        public enum ServerTypes
        {
           Cquery 
        }
        public string SqLiteDatabasePath { get; set; }
        public ServerTypes ServerType { get; set; }

       

        /// <summary>
        /// Workspace directory where the C/C++ code is
        /// </summary>
        [JsonIgnore]
        public string WorkspaceDirectory
        {
            get => _workspaceDirectory.Replace(@"\", "/");
            set => _workspaceDirectory = value;
        }
        [JsonProperty("WorkspaceDirectory")]
        private string _workspaceDirectory;
        
        
        /// <summary>
        /// Client log file name to log client activities
        /// </summary>
        [JsonIgnore]
        public string ClientLogFile
        {
            get => _clientLogFile.Replace(@"\", "/");
            set => _clientLogFile = value;
        }
        
        [JsonProperty(@"ClientLogFile")]
        private string _clientLogFile { get; set; }


        /// <summary>
        /// CQuery: CQuery additional launch arguments
        /// </summary>
        [JsonIgnore]
        public List<string> CqueryLaunchArgs
        {
            get => _cqueryLaunchArgs ?? new List<string>() ;
            set => _cqueryLaunchArgs = value;
        }
        [JsonProperty(@"cquery.launch.args")]
        private List<string> _cqueryLaunchArgs { get; set; }


        /// <summary>
        /// CQuery: Cache directory
        /// </summary>
        [JsonIgnore]
        public string CqueryCacheDirectory
        {
            get => _cqueryCacheDirectory.Replace(@"\", "/");
            set => _cqueryCacheDirectory = value;
        }
        [JsonProperty(@"cquery.cacheDirectory")]
        private string _cqueryCacheDirectory { get; set; }


        /// <summary>
        /// CQuery: Cache directory
        /// </summary>
        [JsonIgnore]
        public string CqueryResourceDirectory
        {
            get => _cqueryResourceDirectory.Replace(@"\", "/");
            set => _cqueryResourceDirectory = value;
        }
        [JsonProperty(@"cquery.resourceDirectory")]
        private string _cqueryResourceDirectory { get; set; }


        /// <summary>
        /// CQuery: extra clang arguments
        /// </summary>
        [JsonIgnore]
        public List<string> CqueryExtraClangArguments
        {
            get => _cqueryExtraClangArguments ?? new List<string>() ;
            set => _cqueryExtraClangArguments = value;
        }
        
        [JsonProperty(@"cquery.extraClangArguments")]
        private List<string> _cqueryExtraClangArguments { get; set; }
        

        /// <summary>
        /// CQuery: Compilation directory
        /// </summary>
        [JsonIgnore]
        public string CqueryCompilationDatabaseDirectory
        {
            get => _cqueryCompilationDatabaseDirectory;
            set => _cqueryCompilationDatabaseDirectory = value;
        }
        [JsonProperty(@"cquery.compilationDatabaseDirectory")]
        private string _cqueryCompilationDatabaseDirectory { get; set; }


        /// <summary>
        /// CQuery: WorkspaceSymbol parameter
        /// </summary>
        [JsonProperty("WorkspaceSymbol")]
        private WorkspaceSymbol _workspaceSymbol;

        [JsonIgnore]
        public WorkspaceSymbol WorkspaceSymbol
        {
            get => _workspaceSymbol;
            set => _workspaceSymbol = value;
        }
        
        


       
        /// <summary>
        /// CQuery: XRef parameter
        /// </summary>
        [JsonIgnore]
        public Xref Xref
        {
            get => _xref;
            set => _xref = value;
        }

        [JsonProperty(@"Xref")]
        private Xref _xref;


        /// <summary>
        /// Server path of LSP server
        /// </summary>
        [JsonProperty(@"ServerPath")]
        private string _serverPath { get; set; }
        [JsonIgnore]
        public string ServerPath
        {
            get => _serverPath.Replace(@"\", "/");
            set => _serverPath = value;
        }


        /// <summary>
        /// Path to server log file to see the server activities
        /// </summary>
        [JsonProperty(@"ServerLogFile")]
        private string _serverLogFile { get; set; }
        [JsonIgnore]
        public string ServerLogFile
        {
            get => _serverLogFile.Replace(@"\", "/");
            set => _serverLogFile = value;
        }

        /// <summary>
        /// List of symbols 
        /// </summary>
        [JsonIgnore]
        public List<string> SymbolKindList
        {
            get => _symbolKindList ?? new List<string>() ;
            set => _symbolKindList = value;
        }
        [JsonProperty(@"SymbolKindList")]
        private List<string> _symbolKindList { get; set; }

        // Cquery specific
        
        
       
        
        // BlackList of directories to ignore while generating .CQuery include statements
        // e.g. 
        // IncludeDirectoryBlackList: ["NOSTRING1", "NOSTRING2"]
        // ignores all include directories which contains 'NOSTRING1 or NOSTRING2
        [JsonIgnore]
        public List<string> IncludeDirectoryBlackList
        {
            get => _includeDirectoryBlackList ?? new List<string>() ;
            set => _includeDirectoryBlackList = value;
        }
        [JsonProperty(@"IncludeDirectoryBlackList")]
        private List<string> _includeDirectoryBlackList { get; set; }


    }
}
