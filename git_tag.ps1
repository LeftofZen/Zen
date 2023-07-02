$versionFilePath = Join-Path -Path $PSScriptRoot -ChildPath 'VERSION.txt'
$version = (Get-Content -Path $versionFilePath -Raw).Trim()

Write-Host "Attempting to tag $version"

# Add a check to ensure the version file exists and contains a valid version number
if (-not (Test-Path $versionFilePath) -or (-not $version -as [version]))
{
    Write-Host "Error: version.txt file not found or contains an invalid version number."
    exit 1
}

# Check if a Git tag already exists with the specified version
$tagExistsCommand = "git rev-parse --quiet --verify refs/tags/$version"
$tagExists = Invoke-Expression -Command $tagExistsCommand

if ($tagExists)
{
    Write-Host "Error: Git tag with version $version already exists."
    exit 1
}

# Execute git tag command
$tagCommand = "git tag -a $version -m `"Auto-tagging version $version`""
Write-Host $tagCommand
Invoke-Expression -Command $tagCommand
Write-Host "Successfully tagged $version"

$pushTagToOriginCommand = "git push origin $version"
Invoke-Expression -Command $pushTagToOriginCommand
Write-Host "Successfully pushed $version to origin"
