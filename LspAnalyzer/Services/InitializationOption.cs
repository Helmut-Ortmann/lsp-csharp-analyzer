using System;
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
	    public InitializationOptions(string resourceDirectory, string cacheDirectory, string compilationDatabaseDirectory,
	        string projectRoot)
	    {
	        ProjectRoot = projectRoot;
	        CacheDirectory = @"d:/temp/cquery/cacheDirectory";
             
            // Using 'ExtraClangArguments' leads to clang AST error.
	        //ExtraClangArguments = new string[] {"clang++","-xc", "-std=c11", @"-Wno-unknown-warning-option"
	            //"-resource-dir=d:/hoData/Development/GitHub/LSP/cquery1/build/release/bin/lib/LLVM-4.0.0-win64/lib/clang/5.0.1/",
                //@"-working-directory=d:\hoData\Projects\00Current\ZF\Work\Source\"
	            
	        //};

		}
	}
}
