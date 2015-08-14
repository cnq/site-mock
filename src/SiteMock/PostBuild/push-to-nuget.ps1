$THIS_SCRIPTS_DIRECTORY = Split-Path $script:MyInvocation.MyCommand.Path

# The NuGet gallery to upload to.
$sourceToUploadTo = "https://nuget.org/api/v2/package"

# Specify any additional NuGet Pack options to pass to nuget.exe.
$pushOptions = ""

if (![string]::IsNullOrWhiteSpace($sourceToUploadTo)) { $pushOptions += " -Source ""$sourceToUploadTo"" " }

# Create the new NuGet package.
& "$THIS_SCRIPTS_DIRECTORY\New-NuGetPackage.ps1" -NuGetExecutableFilePath "$THIS_SCRIPTS_DIRECTORY\..\..\.nuget\NuGet.exe" -PushOptions "$pushOptions"