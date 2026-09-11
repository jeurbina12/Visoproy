# Plan for dotnet-version-upgrade

## Tasks
- 01-fix-conntopool: Arreglar ConnToPool.cs — corregir errores de compilación para restaurar la build de la solución.
  - 01.01-correct-conntopool: Corregir errores de sintaxis y compilación en ConnToPool.cs

## Overview
El objetivo inmediato es corregir los errores de compilación en DataAccess/PostgreSQL/ConnToPool.cs detectados tras la verificación inicial. Mantener cambios mínimos y seguros.

---

# Upgrade Plan: Migración a .NET 10 (All-at-Once)

## Strategy Declaration
**Selected**: All-at-Once

## Overview
Actualizar todos los proyectos del repositorio a net10.0 en una única pasada. Este plan aplica al proyecto detectado en el análisis (Common\Common.csproj). Incluye conversión a SDK-style, actualización de paquetes, corrección de binding redirects y validación completa de solución.

## Phases & Tasks

### 01-convert-projects: Convertir proyectos al formato SDK
Convertir Common\Common.csproj a SDK-style (nuevo csproj), eliminar imports legacy y migrar referencias de packages.config a PackageReference si aplica.

**Scope**: Common\Common.csproj
**Assessment context**: Proyecto clásico (net452), SDK-style = False, AutoGenerateBindingRedirects no establecido.
**Known risks**: Cambios en comportamiento de build, diferencias en targets importados, posibles ajustes en props/targets.

**Done when**: El proyecto usa SDK-style csproj, compila en net10.0 localmente sin errores.

### 02-update-tfm: Actualizar TargetFramework a net10.0
Actualizar la propiedad TargetFramework/TargetFrameworks a net10.0 en los proyectos convertidos.

**Scope**: Common\Common.csproj
**Assessment context**: Propuesto target net10.0.
**Known risks**: APIs obsoletas o incompatibles; requiere corrección de breaking changes.

**Done when**: El proyecto compila con TargetFramework net10.0 y no hay errores de compilación.

### 03-update-packages: Actualizar paquetes NuGet y resolver vulnerabilidades
Actualizar paquetes a versiones compatibles con net10.0. Incluir fixes de seguridad detectados en el assessment por defecto.

**Scope**: Common\Common.csproj
**Assessment context**: No paquetes detectados como incompatibles en el análisis inicial; ejecutar restore y actualizar según sea necesario.
**Known risks**: Cambios en API entre versiones de paquetes.

**Done when**: No paquetes incompatibles pendientes y auditoría de vulnerabilidades resuelta.

### 04-fix-binding-redirects: Configurar AutoGenerateBindingRedirects o agregar redirects
Habilitar AutoGenerateBindingRedirects o añadir manualmente binding redirects para resolver advertencias/errores de carga de ensamblados.

**Scope**: Common\Common.csproj
**Assessment context**: AutoGenerateBindingRedirects no establecido.

**Done when**: No problemas de binding en tiempo de ejecución al ejecutar pruebas o al iniciar la app que dependa del proyecto.

### 05-validate-solution: Compilar y ejecutar pruebas
Compilar solución completa, ejecutar pruebas unitarias y validar ausencia de warnings críticos.

**Scope**: Solución completa
**Done when**: Build sin errores, tests pasan, y no hay warnings introducidos por los cambios.

### 06-finalize: Commit y documentación
Aplicar commits según la estrategia de commits confirmada (After Each Task por ahora), actualizar README/notes con pasos realizados.

**Done when**: Todos los cambios están comiteados en la rama upgrade-dotnet-10 y la documentación actualizada.

## Execution Constraints
- Validar compilación completa tras cada tarea crítica (convert + tfm update + packages)
- Mantener branch upgrade-dotnet-10 como rama de trabajo
- Commit Strategy: After Each Task (se puede ajustar si se prefiere un commit único para All-at-Once)

## Notes
- Si se detectan proyectos adicionales o dependencias incompatibles, se pausará y se propondrán opciones (skip, replan, o conversión manual previa).


---
Plan generado automáticamente a partir del assessment. En Automatic mode continuaré con la etapa de ejecución y comenzaré con la primera tarea: 01-convert-projects. Si prefiere revisar o modificar algo, responda ahora "detener" o indique el cambio.
