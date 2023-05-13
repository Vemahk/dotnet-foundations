$paths = Get-ChildItem -Directory *
$paths | %{ dotnet new uninstall ".\$($_.Name)" }
pause
