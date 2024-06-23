$rootDirectory = '.'

$csprojFiles = Get-ChildItem -Path $rootDirectory -Filter '*.csproj' -Recurse

foreach ($csprojFile in $csprojFiles) 
{
    $csprojPath = $csprojFile.FullName
    $commandOutput = dotnet list $csprojPath package --outdated --format json

    $commandOutput

    $jsonString = $commandOutput -join ''
    
    $jsonObject = ConvertFrom-Json $jsonString

    foreach ($project in $jsonObject.projects) {
        foreach ($framework in $project.frameworks) {
            foreach ($package in $framework.topLevelPackages) {
                $packageId = $package.id
                $requestedVersion = $package.requestedVersion
                $resolvedVersion = $package.latestVersion

                Write-Warning "[MarceloCastelo.io] Update package $packageId from $requestedVersion version to $resolvedVersion version"
                dotnet add $csprojPath package $packageId
            }
        }
    }
}
