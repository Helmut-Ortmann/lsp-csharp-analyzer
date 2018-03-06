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

       


        [JsonIgnore]
        public string WorkspaceDirectory
        {
            get => _workspaceDirectory.Replace(@"\", "/");
            set => _workspaceDirectory = value;
        }
        [JsonIgnore]
        public string ServerPath
        {
            get => _serverPath.Replace(@"\", "/");
            set => _serverPath = value;
        }
        [JsonIgnore]
        public string ServerLogFile
        {
            get => _serverLogFile.Replace(@"\", "/");
            set => _serverLogFile = value;
        }
        [JsonIgnore]
        public string ClientLogFile
        {
            get => _clientLogFile.Replace(@"\", "/");
            set => _clientLogFile = value;
        }
        [JsonIgnore]
        public List<string> CqueryLaunchArgs
        {
            get => _cqueryLaunchArgs ?? new List<string>() ;
            set => _cqueryLaunchArgs = value;
        }
        [JsonIgnore]
        public string CqueryCacheDirectory
        {
            get => _cqueryCacheDirectory.Replace(@"\", "/");
            set => _cqueryCacheDirectory = value;
        }
        [JsonIgnore]
        public string CqueryResourceDirectory
        {
            get => _cqueryResourceDirectory.Replace(@"\", "/");
            set => _cqueryResourceDirectory = value;
        }
        [JsonIgnore]
        public List<string> CqueryExtraClangArguments
        {
            get => _cqueryExtraClangArguments ?? new List<string>() ;
            set => _cqueryExtraClangArguments = value;
        }
        [JsonIgnore]
        public string CqueryCompilationDatabaseDirectory
        {
            get => _cqueryCompilationDatabaseDirectory;
            set => _cqueryCompilationDatabaseDirectory = value;
        }
        [JsonIgnore]
        public WorkspaceSymbol WorkspaceSymbol
        {
            get => _workspaceSymbol;
            set => _workspaceSymbol = value;
        }
        
        [JsonProperty("WorkspaceSymbol")]
        private WorkspaceSymbol _workspaceSymbol;


        [JsonProperty("WorkspaceDirectory")]
        private string _workspaceDirectory;

        [JsonProperty(@"ServerPath")]
        private string _serverPath { get; set; }

        [JsonProperty(@"ServerLogFile")]
        private string _serverLogFile { get; set; }
        [JsonProperty(@"ClientLogFile")]
        private string _clientLogFile { get; set; }

        // Cquery specific
        [JsonProperty(@"cquery.launch.args")]
        private List<string> _cqueryLaunchArgs { get; set; }
        [JsonProperty(@"cquery.cacheDirectory")]
        private string _cqueryCacheDirectory { get; set; }
        [JsonProperty(@"cquery.resourceDirectory")]
        private string _cqueryResourceDirectory { get; set; }
        [JsonProperty(@"cquery.extraClangArguments")]
        private List<string> _cqueryExtraClangArguments { get; set; }
        [JsonProperty(@"cquery.compilationDatabaseDirectory")]
        private string _cqueryCompilationDatabaseDirectory { get; set; }


    }
}
