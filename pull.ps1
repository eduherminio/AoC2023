# Install-Module -Name Microsoft.PowerShell.SecretManagement
# Install-Module -Name Microsoft.PowerShell.SecretStore

# Import-Module Microsoft.PowerShell.SecretManagement
# Register-SecretVault -Name SecretStore -ModuleName Microsoft.PowerShell.SecretStore -DefaultVault

# Set-Secret -Name AoCSessionId -Secret "..."

if ($args.Count -eq 0) {
	Write-Host "Please provide the day number"
	exit 1
}

cat .\Vault | ConvertTo-SecureString -AsPlainText -Force | Unlock-SecretStore

$year = "2023"
$day = $args[0]

$outputFile = If ($day -ge 10) { "$day.txt" } Else { "0$day.txt" }
$outputPath = "./src/AoC_$year/Inputs/$outputFile"

$url = "https://adventofcode.com/$year/day/$day/input"

$cookie = New-Object System.Net.Cookie
$cookie.Name = "session"
$cookie.Value = (Get-Secret AoCSessionId -AsPlainText)
$cookie.Domain = "adventofcode.com"

$session = New-Object Microsoft.PowerShell.Commands.WebRequestSession
$session.Cookies.Add($cookie);
$session.Headers.Add("User-Agent", "github.com-eduherminio-AoC2023")


Invoke-WebRequest $url -WebSession $session -TimeoutSec 5 -OutFile $outputPath

if (Test-Path $outputPath) {
	Write-Host "Input of Day $day downloaded to $outputPath"
	cat $outputPath
}

notepad++ $outputPath
