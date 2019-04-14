#tool "nuget:?package=xunit.runner.console&version=2.4.0"
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

var buildDir = Directory("./Artifacts") + Directory(configuration);
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

Task("Default")
	.IsDependentOn("GitVersion")
    .IsDependentOn("Build");    

RunTarget(target);