$rootDirectory = '.'

$csprojFiles = Get-ChildItem -Path $rootDirectory -Filter '*.csproj' -Recurse

foreach ($csprojFile in $csprojFiles) 
{
    $csprojPath = $csprojFile.FullName
    dotnet list $csprojPath package --outdated --format json
}