@echo off
setlocal EnableDelayedExpansion

REM runs swig and copies files around automatically
REM note that I hard code a path to swig.exe.  This will obviously fail if it isn't there...

set "SWIG_EXE=C:\Programs\swigwin-4.4.0\swig.exe"
set "SWIG_OUT=swigInterfaceFileAndPagmoHeaders"

"%SWIG_EXE%" -c++ -csharp -namespace pagmo -dllimport pagmoWrapper -I..\..\pagmoWrapper -I.\swigInterfaceFileAndPagmoHeaders swigInterfaceFileAndPagmoHeaders\pagmoSharpSwigInterface.i
if errorlevel 1 exit /b 1

copy /Y "%SWIG_OUT%\pagmoSharpSwigInterface_wrap.cxx" "pagmoWrapper\GeneratedWrappers.cxx" >nul
if errorlevel 1 exit /b 1

call :retry_copy "%SWIG_OUT%\pagmoSharpSwigInterface_wrap.h" "pagmoWrapper\pagmoSharpSwigInterface_wrap.h" 8
if errorlevel 1 (
    echo Warning: could not overwrite pagmoWrapper\pagmoSharpSwigInterface_wrap.h after retries.
)

rmdir "pagmoSharp\pygmoWrappers\" /S /Q 2>nul
mkdir "pagmoSharp\pygmoWrappers\" >nul 2>nul
xcopy /I /Y "%SWIG_OUT%\*.cs" "pagmoSharp\pygmoWrappers\" >nul

del "%SWIG_OUT%\pagmoSharpSwigInterface_wrap.cxx" >nul 2>nul
del "%SWIG_OUT%\pagmoSharpSwigInterface_wrap.h" >nul 2>nul
del /s /q /f "%SWIG_OUT%\*.cs" >nul 2>nul
exit /b 0

:retry_copy
set "SRC=%~1"
set "DST=%~2"
set "TRIES=%~3"
for /l %%I in (1,1,!TRIES!) do (
    copy /Y "!SRC!" "!DST!" >nul 2>nul && exit /b 0
    ping -n 2 127.0.0.1 >nul
)
exit /b 1
