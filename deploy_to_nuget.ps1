# Stop script if any part of it fails (ie don't continue)
$ErrorActionPreference = 'Stop'

# Set the solution directory
$solutionDirectory = "."

# Set the path to the configuration file
$configFile = "$solutionDirectory\nuget_api_key.json"

# Read the API key from the configuration file
$config = Get-Content -Path $configFile -Raw | ConvertFrom-Json
$apiKey = $config.NuGetApiKey
Write-Host "Nuget API key: $apiKey"

# Set the NuGet package source
$packageSource = "https://api.nuget.org/v3/index.json"

# Get all the project directories within the solution directory
$projectDirectories = Get-ChildItem -Path $solutionDirectory -Filter "*.csproj" -Recurse | Where-Object { $_.DirectoryName -notlike "*Testing*" } | Select-Object -ExpandProperty DirectoryName

# Delete bin folders
foreach ($projectDirectory in $projectDirectories)
{
    $binDirectories = Get-ChildItem -Path $projectDirectory -Filter "bin" -Directory
    foreach ($binDirectory in $binDirectories)
    {
        Remove-Item -Path $binDirectory.FullName -Recurse -Force
    }
}

# Loop through each project directory and build the project
foreach ($projectDirectory in $projectDirectories)
{
    Write-Host "Building project in directory: $projectDirectory"
    dotnet restore "$projectDirectory"
    dotnet build "$projectDirectory" --configuration Release
}

$versionFilePath = Join-Path -Path $solutionDirectory -ChildPath 'VERSION.txt'
$version = (Get-Content -Path $versionFilePath -Raw).Trim()
Write-Host $version

# Loop through each project directory and pack the project into a NuGet package
foreach ($projectDirectory in $projectDirectories)
{
    Write-Host "Packing project in directory: $projectDirectory"
    dotnet pack "$projectDirectory" -p:PackageVersion=$version --configuration Release --output "$projectDirectory\bin\Release" --no-build
}

# Loop through each project directory and push the generated NuGet packages
foreach ($projectDirectory in $projectDirectories)
{
    $packagePath = Join-Path $projectDirectory "bin\Release\*.$version.nupkg"
    Write-Host "Pushing NuGet package: $packagePath to $packageSource with key $apiKey"
    dotnet nuget push $packagePath --source $packageSource --api-key $apiKey
}
