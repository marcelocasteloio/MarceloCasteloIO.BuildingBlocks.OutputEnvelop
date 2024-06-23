$rootLocation = Get-Location

#########################
### Install tools
#########################

Write-Host "[MarceloCastelo.IO] Install ReportGenerator CLI Tool"
$reportgeneratorFolderPath = "./.reportgenerator/reportgenerator"

if (-not (Test-Path $reportgeneratorFolderPath -PathType Container))
{
    New-Item -Path $reportgeneratorFolderPath -ItemType Directory
    dotnet tool update dotnet-reportgenerator-globaltool --tool-path $reportgeneratorFolderPath
}

Write-Host "[MarceloCastelo.IO] Install stryker CLI Tool"
$strykerfolderPath = "./.stryker/stryker"
if (-not (Test-Path $strykerfolderPath -PathType Container))
{
    New-Item -Path $strykerfolderPath -ItemType Directory
    dotnet tool update dotnet-stryker --tool-path $strykerfolderPath
}

#########################
### Restore and build
#########################

Write-Host "[MarceloCastelo.IO] Restore"
dotnet restore

Write-Host "[MarceloCastelo.IO] Build Release"
dotnet build -c Release --no-restore

#########################
### Unit tests and Mutation Tests
#########################

Write-Host "[MarceloCastelo.IO] Run Unit Tests"
dotnet test -c Release --no-build --verbosity normal /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="Coverage/" ./tst/UnitTests/UnitTests.csproj

Write-Host "[MarceloCastelo.IO] Run MutationTests"

Set-Location ./tst/UnitTests
$result = ../../.stryker/stryker/dotnet-stryker --reporter "html" --output ".stryker-output"
Set-Location $rootLocation

$result

if ($result -like "*The final mutation score is 100.00 %*") {
    Write-Host "[MarceloCastelo.IO] Stryker mutant check passed. No surviving mutants found."
} else {
    Write-Error "[MarceloCastelo.IO] Stryker detected surviving mutants. Pipeline failed."
}

#########################
### Reports
#########################

Write-Host "[MarceloCastelo.IO] Generating Unit Tests Report"
if ($env:OS -like '*Windows*') {
    ./.reportgenerator/reportgenerator/reportgenerator.exe "-reports:./tst/UnitTests/Coverage/coverage.opencover.xml" "-targetdir:./tst/UnitTests/Coverage/"
} else {
    ./.reportgenerator/reportgenerator/reportgenerator "-reports:./tst/UnitTests/Coverage/coverage.opencover.xml" "-targetdir:./tst/UnitTests/Coverage/"
}

Start-Sleep -Seconds 1

Write-Host "[MarceloCastelo.IO] Open Coverage Report"
./tst/UnitTests/Coverage/index.html

Start-Sleep -Seconds 1

Write-Host "[MarceloCastelo.IO] Open Mutation Report"
./tst/UnitTests/.stryker-output/reports/mutation-report.html

Start-Sleep -Seconds 1