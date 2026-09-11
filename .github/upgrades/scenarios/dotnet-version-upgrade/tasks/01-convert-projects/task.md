# 01-convert-projects: Convertir proyectos al formato SDK

Convertir Common\Common.csproj a SDK-style (nuevo csproj), eliminar imports legacy y migrar referencias de packages.config a PackageReference si aplica.

**Scope**: Common\Common.csproj
**Assessment context**: Proyecto clásico (net452), SDK-style = False, AutoGenerateBindingRedirects no establecido.
**Known risks**: Cambios en comportamiento de build, diferencias en targets importados, posibles ajustes en props/targets.

**Done when**: El proyecto usa SDK-style csproj, compila en net10.0 localmente sin errores.

## Research findings
- Proyecto detectado: E:\VSG\VC_Net\Visoproy\Common\Common.csproj (Classic project, TargetFrameworkVersion v4.5.2).
- Referencias assembly explícitas: System, System.Data, System.Xml
- Archivos de código actuales (3): Cache\ConfCache.cs, Cache\UserCache.cs, Properties\AssemblyInfo.cs
- No se detectaron paquetes NuGet en el assessment para este proyecto.

## Planned changes (implementation steps)
1. Crear un nuevo SDK-style csproj mínimo que apunte a net10.0 y conserve AssemblyName/RootNamespace.
2. Mantener Properties\AssemblyInfo.cs estableciendo <GenerateAssemblyInfo>false> para evitar duplicados.
3. Eliminar referencias explícitas a System.* (no necesarias en .NET 10) y confiar en assemblies del framework.
4. Ejecutar `dotnet restore` y `dotnet build` en el proyecto; compilar la solución si procede.
5. Corregir errores de compilación (ajustar usings, API incompatibilidades) y volver a compilar.

## Affected files
- Common\Common.csproj (se reemplaza por SDK-style)
- (posible) Common\Properties\AssemblyInfo.cs — se mantiene o se ajusta según sea necesario

## Next actions
- Aplicar cambio en Common\Common.csproj (con commit según estrategia After Each Task)
- Ejecutar build y reportar resultados en progress-details.md
