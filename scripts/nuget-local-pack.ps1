param (
    [string]$csproj,
    [string]$configuration,
    [string]$packageVersion
)

Write-Host "[MarceloCastelo.IO] Packing MarceloCasteloIO.BuildingBlocks.OutputEnvelop"

dotnet pack `
    -c $configuration $csproj `
    /p:Version=$packageVersion `
    -o ./local-packages `
    --include-symbols `
    --include-source