@echo off
setlocal EnableDelayedExpansion

REM runs swig and copies files around automatically

if not defined SWIG_EXE (
    for /f "delims=" %%I in ('where swig.exe 2^>nul') do (
        set "SWIG_EXE=%%I"
        goto :swig_found
    )
)

if not defined SWIG_EXE if defined SWIG_HOME if exist "%SWIG_HOME%\swig.exe" set "SWIG_EXE=%SWIG_HOME%\swig.exe"

:swig_found
if not defined SWIG_EXE (
    echo Error: SWIG executable not found. Set SWIG_EXE, set SWIG_HOME, or add swig.exe to PATH.
    exit /b 1
)

set "SWIG_OUT=swig"

"%SWIG_EXE%" -c++ -csharp -namespace pagmo -dllimport PagmoWrapper -I..\..\pagmoWrapper -I.\swig swig\PagmoNETSwigInterface.i
if errorlevel 1 exit /b 1

copy /Y "%SWIG_OUT%\PagmoNETSwigInterface_wrap.cxx" "pagmoWrapper\GeneratedWrappers.cxx" >nul
if errorlevel 1 exit /b 1

call :retry_copy "%SWIG_OUT%\PagmoNETSwigInterface_wrap.h" "pagmoWrapper\PagmoNETSwigInterface_wrap.h" 8
if errorlevel 1 (
    echo Warning: could not overwrite pagmoWrapper\PagmoNETSwigInterface_wrap.h after retries.
)

rmdir "Pagmo.NET\pygmoWrappers\" /S /Q 2>nul
mkdir "Pagmo.NET\pygmoWrappers\" >nul 2>nul
xcopy /I /Y "%SWIG_OUT%\*.cs" "Pagmo.NET\pygmoWrappers\" >nul

del "%SWIG_OUT%\PagmoNETSwigInterface_wrap.cxx" >nul 2>nul
del "%SWIG_OUT%\PagmoNETSwigInterface_wrap.h" >nul 2>nul
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
