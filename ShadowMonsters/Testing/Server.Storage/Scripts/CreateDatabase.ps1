##Import SQL Server Module called SQLPS
#Import-Module SQLPS -DisableNameChecking

$CurrentScriptName = "CreateDatabase.ps1"
$FullPath = $MyInvocation.MyCommand.Path
$Path = $FullPath.Replace($CurrentScriptName, "")
$Instance = "localhost\SQLEXPRESS"
$DatabaseName = "ShadowMonsters"

Write-Host "Working path is: " $Path

Invoke-Sqlcmd -ServerInstance $Instance -InputFile $Path\CreateDatabase.sql | Out-File $Path\CreateDatabase.out

#Create Tables
$TablesPath = $Path+"Tables"
Write-Host "Tables path is: "  $TablesPath

Invoke-Sqlcmd -ServerInstance $Instance -InputFile $TablesPath\Users.Account.sql | Out-File $TablesPath\Users.Account.out
Invoke-Sqlcmd -ServerInstance $Instance -InputFile $TablesPath\Users.Characters.sql | Out-File $TablesPath\Users.Characters.out
Invoke-Sqlcmd -ServerInstance $Instance -InputFile $TablesPath\BattleInstance.BattleInstance.sql | Out-File $TablesPath\BattleInstance.BattleInstance.out

#Create Procedures

Invoke-Sqlcmd -ServerInstance $Instance -InputFile $Path\Procedures\GetCharacters.sql



