using System;
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


	    /// <summary>
	    /// Create the initialization options used by CQuery
	    /// </summary>
	    /// <param name="resourceDirectory"></param>
	    /// <param name="cacheDirectory"></param>
	    /// <param name="compilationDatabaseDirectory"></param>
	    /// <param name="projectRoot"></param>
	    /// <param name="extraClangArguments"></param>
	    public InitializationOptions(string resourceDirectory, string cacheDirectory, string compilationDatabaseDirectory,
	        string projectRoot, List<string> extraClangArguments)
	    {
	        ProjectRoot = projectRoot;
	        CacheDirectory = cacheDirectory;
	        ExtraClangArguments = extraClangArguments.ToArray();

	    }
	}
}
