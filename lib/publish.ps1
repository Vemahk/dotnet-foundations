$nuget_out = '.\nuget-out'
if (Test-Path -LiteralPath $nuget_out) {
  rm -LiteralPath $nuget_out -Recurse
}

$projs = Get-ChildItem .\**\src -Recurse -Filter *.csproj | % { $_.FullName | Resolve-Path -Relative };
if ($projs.Count -eq 0) {
    echo "Did not find any projects to pack and publish.";
    exit -1;
}

$options = @("all") + $projs;

echo "Please choose an option to pack and publish.";
echo "";
for($i = 0; $i -lt $options.count; $i++){
    Write-Host "  $($i)) $($options[$i])";
}

echo "";

[int]$index = Read-Host -Prompt "Choose the number of the option to select";

$selected_projects = @();

if ($index -eq 0) {
    $selected_projects = $projs;
}
else {
    $choice = $options[$index];

    if ($choice -eq $NULL) {
        echo "Invalid choice. Try again, sweaty.";
        exit -1;
    }

    if (-not $(Test-Path $choice -PathType Leaf) ) {
        echo "Could not find '${choice}'. Either the number chosen option was invalid, or something else has gone wrong."
        exit -1;
    }

    $selected_projects = @($choice);
}

$selected_projects | % {
    dotnet pack $_ --output .\nuget-out --configuration Release --include-source;
}

dotnet nuget push "${nuget_out}\*.nupkg" --source "github" --skip-duplicate