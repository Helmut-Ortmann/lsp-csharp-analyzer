using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace LspAnalyzer.Services
{
// Define other methods and classes here
	[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
	public class InitializationOptions{
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
		public string CacheDirectory {get;set;}
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
		public string CompilationDatabaseDirectory {get;set;}
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
		public string ProjectRoot {get;set;}
	    [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
	    public string ResourceDirectory { get; set; }
	    [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
	    public string[] ExtraClangArguments { get; set; }

	    [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
	        NullValueHandling = NullValueHandling.Ignore)]
	    public WorkspaceSymbol WorkspaceSymbol { get; set; }


	    /// <summary>
	    /// Create the initialization options used by CQuery
	    /// </summary>
	    /// <param name="resourceDirectory"></param>
	    /// <param name="cacheDirectory"></param>
	    /// <param name="compilationDatabaseDirectory"></param>
	    /// <param name="projectRoot"></param>
	    /// <param name="extraClangArguments"></param>
	    /// <param name="workspaceSymbol"></param>
	    public InitializationOptions(string resourceDirectory, string cacheDirectory, string compilationDatabaseDirectory,
	        string projectRoot, List<string> extraClangArguments, WorkspaceSymbol workspaceSymbol)
	    {
	        ProjectRoot = projectRoot;
	        CacheDirectory = cacheDirectory;
	        ExtraClangArguments = extraClangArguments.ToArray();
	        WorkspaceSymbol = workspaceSymbol;

	    }
	}
    /// <summary>
    /// Options to define workspace symbols
    /// </summary>
    public class WorkspaceSymbol {
       public int maxNum = 1000;
       bool sort = true;

    }
       
}

