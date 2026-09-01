<#
.SYNOPSIS
Copia el ensamblado Interop.AutoCAD_A.dll desde una ubicación central a la carpeta de librerías del proyecto.

.PARAMETER Source
Ruta completa del archivo interop origen. Por defecto: E:\VSG\VC_Net\Interop\Interop.AutoCAD_A.dll

.PARAMETER Destination
Ruta de destino del archivo (archivo completo). Por defecto: ./libs/Interop.AutoCAD_A.dll relativo al root del script.

.PARAMETER Force
Forzar sobrescritura si el archivo existe.

.EXAMPLE
pwsh -ExecutionPolicy Bypass -File .\scripts\deploy-interop.ps1 -Source 'E:\VSG\VC_Net\Interop\Interop.AutoCAD_A.dll' -Destination 'C:\deploy\Interop.AutoCAD_A.dll' -Force

#>
param(
	[string]$Source = 'E:\VSG\VC_Net\Interop\Interop.AutoCAD_A.dll',
	[string]$Destination = "$PSScriptRoot\..\libs\Interop.AutoCAD_A.dll",
	[switch]$Force,
	[string]$SolutionRoot = "$PSScriptRoot\..",
	[string[]]$Projects = @('Presentation','DataAccess','Domain'),
	[string]$Configuration = 'Debug',
	[switch]$CopyToProjectOutputs
)

try {
	$srcFull = Resolve-Path -Path $Source -ErrorAction Stop
} catch {
	Write-Error "Fuente no encontrada: $Source"
	exit 1
}

$destFull = [System.IO.Path]::GetFullPath((Resolve-Path -Path (Split-Path -Path $Destination -Parent) -ErrorAction SilentlyContinue) -join '\\') 2>$null
if (-not $destFull) {
	$destDir = Split-Path -Path $Destination -Parent
	if (-not (Test-Path -Path $destDir)) {
		New-Item -ItemType Directory -Path $destDir -Force | Out-Null
	}
}

$dst = (Resolve-Path -Path (Split-Path -Path $Destination -Parent) -ErrorAction SilentlyContinue).ProviderPath
if (-not $dst) { $dst = Split-Path -Path $Destination -Parent }
$destFile = Join-Path -Path $dst -ChildPath (Split-Path -Path $Destination -Leaf)

if (Test-Path $destFile) {
	if ($Force) {
		Copy-Item -Path $srcFull -Destination $destFile -Force
		Write-Output "Sobrescrito: $destFile"
	}
	else {
		Write-Output "Destino ya existe: $destFile. Use -Force para sobrescribir."
		exit 0
	}
} else {
	Copy-Item -Path $srcFull -Destination $destFile -Force
	Write-Output "Copiado: $destFile"
}

# Ajustar atributos y permisos mínimos
try {
	if (Test-Path $destFile) {
		# Quitar ReadOnly si estaba
		(Get-Item $destFile).IsReadOnly = $false
		Write-Output "Ajustado: atributos de archivo OK"
	}
} catch {
	Write-Warning "No se pudieron ajustar atributos: $_"
}

Write-Output "Despliegue completado."