$pathList = @("bin", "obj", "Coverage", ".reportgenerator", ".stryker", ".stryker-output", ".specflow", "output")

$foldersToDelete = Get-ChildItem -Recurse -Directory -Force | Where-Object {$_.Name -in $pathList}

$foldersToDelete

$foldersToDelete | ForEach-Object {
    Remove-Item $_.FullName -Recurse -Force
}
