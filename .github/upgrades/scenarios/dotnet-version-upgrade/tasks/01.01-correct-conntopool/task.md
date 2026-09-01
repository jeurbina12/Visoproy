# 01.01-correct-conntopool: Corregir errores de sintaxis y compilación en ConnToPool.cs

## Objective
Corregir los errores de compilación en DataAccess/PostgreSQL/ConnToPool.cs detectados tras la actualización de paquetes.

## Scope
- Archivo: DataAccess/PostgreSQL/ConnToPool.cs
- Proyecto: DataAccess
- Cambios permitidos: correcciones de sintaxis, cierre de llaves, visibilidad de miembros básicos, ajustes menores necesarios para que el archivo compile.

## Done when
1. La solución E:\VSG\VC_Net\Visoproy\Visoproy.sln compila sin errores (msbuild).
2. Los errores CS1001/CS1002/CS1022/CS0106 relacionados con ConnToPool.cs se han resuelto.
3. Se ha escrito tasks/01-fix-conntopool/progress-details.md describiendo los cambios realizados.

## Risks
- Cambios pueden afectar comportamiento de conexión; se harán mínimos y documentados.

## Notes
- Aplicar correcciones locales y sintácticas preferentemente.
