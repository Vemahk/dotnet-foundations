$paths = Get-ChildItem -Directory *
$paths | %{ dotnet new install ".\$($_.Name)" }
pause
