$dirs = Get-ChildItem .\ -include bin,obj,packages,Output,Output_WorkerService,Download -Recurse

foreach ($dir in $dirs) 
{ 
	Write-Host "Removing $dir"
	Remove-Item $dir.FullName -Force -Recurse 
}

$myBuildNumber = $(get-date).ToString("yyyy.MM.dd.HHmm");

git add .
git commit -m "v$myBuildNumber"
git push --force

dotnet publish .\MyAccess.WorkerService\MyAccess.WorkerService.csproj /property:Version=$myBuildNumber /p:EnvironmentName=Production --output Output_WorkerService