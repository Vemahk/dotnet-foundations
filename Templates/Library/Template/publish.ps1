$nuget_out = '.\nuget-out'
if (Test-Path -LiteralPath $nuget_out) {
  rm -LiteralPath $nuget_out -Recurse
}
dotnet pack .\Main\Template.csproj -o .\nuget-out --configuration Release
dotnet nuget push "${nuget_out}\*.nupkg" --source "github" --skip-duplicate