$nuget_out = '.\nuget-out'
if (Test-Path -LiteralPath $nuget_out) {
  rm -LiteralPath $nuget_out -Recurse
}

Get-ChildItem .\Vemahk\**\*.csproj -Recurse -Exclude *.Test.csproj | % {
    $proj = $_.FullName;
    dotnet pack $proj --output .\nuget-out --configuration Release --include-source --include-symbols;
}

dotnet nuget push "${nuget_out}\*.nupkg" --source "github" --skip-duplicate