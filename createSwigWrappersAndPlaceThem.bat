REM runs swig and copies files around automatically
REM note that I hard code a path to swig.exe.  This will obviously fail if it isn't there...

C:\src\swigwin-4.0.2\swig.exe -c++ -csharp -namespace pagmo -dllimport pagmoWrapper.dll swigInterfaceFileAndPagmoHeaders\pagmoSharpSwigInterface.i
copy swigInterfaceFileAndPagmoHeaders\swig_wrap.cxx "pagmoWrapper\pagmoSharpSwigInterface_wrap.cxx"
xcopy /I /Y swigInterfaceFileAndPagmoHeaders\*.cs "pagmoSharp\pygmoWrappers\"

del "swigInterfaceFileAndPagmoHeaders\pagmoSharpSwigInterface_wrap.cxx"
del /s /q /f "swigInterfaceFileAndPagmoHeaders\*.cs"