Visoproy — Instrucciones de uso y despliegue del ensamblado Interop.AutoCAD

Resumen
- Este repositorio contiene la solución Visoproy (.NET Framework 4.5.2). Durante la modernización se añadieron cambios para restaurar la compilación y gestionar la dependencia COM de AutoCAD mediante un ensamblado de interoperabilidad generado (tlbimp).

Interop (AutoCAD)
- Archivo interop generado: Interop.AutoCAD_A.dll
- Ubicación de referencia por defecto (local de desarrollo): E:\VSG\VC_Net\Interop\Interop.AutoCAD_A.dll
- El proyecto DataAccess usa una propiedad MSBuild parametrizada $(InteropPath) para localizar el ensamblado. Valor por defecto en el proyecto: $(MSBuildThisFileDirectory)..\..\Interop

Despliegue local (script)
- Script: scripts\deploy-interop.ps1
- Copiar el interop a la ruta de trabajo y opcionalmente a los outputs de los proyectos:
  pwsh -ExecutionPolicy Bypass -File .\scripts\deploy-interop.ps1 -Source 'E:\VSG\VC_Net\Interop\Interop.AutoCAD_A.dll' -CopyToProjectOutputs -Force
- Parámetros relevantes:
  -Source: ruta origen del DLL
  -Destination: ruta destino (por defecto ./libs/Interop.AutoCAD_A.dll)
  -CopyToProjectOutputs: copia también a bin\<Configuration> de Presentation, DataAccess y Domain

CI / Build
- En entornos CI establezca la propiedad MSBuild InteropPath antes del build o ejecute el script deploy-interop.ps1 para colocar el DLL en la ruta esperada.
  Ejemplo de MSBuild/CI:
	msbuild Visoproy.sln /t:Rebuild /p:Configuration=Release /p:InteropPath="\\server\share\Interop"

Post-build
- Los proyectos incluyen un target que copia el ensamblado desde $(InteropPath) al directorio de salida si existe. Esto facilita que el bin contenga el interop para ejecución local.

Notas finales
- Si prefiere no mantener el DLL en el repo, la convención usada aquí es mantenerlo fuera (E:\VSG\VC_Net\Interop) y parametrizar $(InteropPath).
- Para cualquier duda sobre la integración AutoCAD o el uso del script, indíquelo y puedo generar un README más detallado o un script de instalación para máquinas cliente.
