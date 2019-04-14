#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var toolpath = Argument("toolpath", @"");

///////////////////////////////////////////////////////////////////////////////
// PREPARATION
///////////////////////////////////////////////////////////////////////////////

var buildDir = Directory("./artifacts") + Directory(configuration);
GitVersion gitVersion = null; 

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("GitVersion").Does(() => {
    gitVersion = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
	});
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
	DotNetCoreRestore(new DotNetCoreRestoreSettings
	{
		NoCache = true,
		Verbosity = DotNetCoreVerbosity.Normal
	});

    NuGetRestore("./HkCacheManager.sln", new NuGetRestoreSettings 
	{ 
		NoCache = true,
		Verbosity = NuGetVerbosity.Normal,
		ToolPath = "./tools/nuget.exe"
	});
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
	.IsDependentOn("GitVersion")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./HkCacheManager.sln", settings => {
		settings.ToolPath = String.IsNullOrEmpty(toolpath) ? settings.ToolPath : toolpath;
		settings.ToolVersion = MSBuildToolVersion.VS2017;
        settings.PlatformTarget = PlatformTarget.MSIL;
		settings.SetConfiguration(configuration);
	  });
    }
    else
    {
      // Use XBuild
      XBuild("./HkCacheManager.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .Does(() =>{
     DotNetCoreTest("./tests/Hk.RedisCacheTests/Hk.RedisCacheTests.csproj", new DotNetCoreTestSettings { Configuration = "Debug" });
});

Task("Pack")
    .IsDependentOn("GitVersion")
    .Does(() => 
    {
      DotNetCorePack("./src/Hk.RedisCache/Hk.RedisCache.csproj", new DotNetCorePackSettings {
        OutputDirectory = "./artifacts",
        Configuration = "Release",
        ArgumentCustomization = args => args.Append("/p:PackageVersion=" + gitVersion.NuGetVersionV2)
      });  
    });

Task("Default")
	 .IsDependentOn("GitVersion")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Pack");

RunTarget(target);