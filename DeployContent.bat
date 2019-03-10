@echo off
echo Updating content...
cmd /C robocopy %1 %2 /MIR /NJH /NJS /NDL
if %ERRORLEVEL% LEQ 7 (exit /b 0) else (exit /b 1)