param(
    [String] $majorMinor = "3.0",  # 2.1
    [String] $patch = "0",         # $env:APPVEYOR_BUILD_VERSION
    [String] $customLogger = "",   # C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll
    [Switch] $notouch
)

function Set-AssemblyVersions($informational, $assembly)
{
    (Get-Content assets/CommonAssemblyInfo.cs) |
        ForEach-Object { $_ -replace """0.0.0.0""", """$assembly""" } |
        ForEach-Object { $_ -replace """0.0.0""", """$informational""" } |
        ForEach-Object { $_ -replace """0.1.1.1""", """$($informational).0""" } |
        Set-Content assets/CommonAssemblyInfo.cs
}

function Install-NuGetPackages($solution)
{
    nuget restore "$solution"
}

function Invoke-MSBuild($solution, $customLogger)
{
    if ($customLogger)
    {
        msbuild "$solution" /verbosity:minimal /p:Configuration=Release /logger:"$customLogger"
    }
    else
    {
        msbuild "$solution" /verbosity:minimal /p:Configuration=Release
    }
}

function Invoke-NuGetPackSpec($nuspec, $version)
{
    nuget pack $nuspec -Version $version
}

function Invoke-Build($majorMinor, $patch, $customLogger, $notouch)
{
    $project = "serilog-sinks-amazonkinesis"

    $solution = "$project.sln"
    $package="$majorMinor.$patch"

    Write-Output "Building $project $package"

    if (-not $notouch)
    {
        $assembly = "$majorMinor.0.0"

        Write-Output "Assembly version will be set to $assembly"
        Set-AssemblyVersions $package $assembly
    }

    Install-NuGetPackages $solution

    Invoke-MSBuild $solution $customLogger

    Invoke-NuGetPackSpec "src/Serilog.Sinks.AmazonKinesis.nuspec" $package
}

$ErrorActionPreference = "Stop"
Invoke-Build $majorMinor $patch $customLogger $notouch
