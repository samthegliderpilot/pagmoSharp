REM runs swig and copies files around automatically
REM note that I hard code a path to swig.exe.  This will obviously fail if it isn't there...

C:\Programs\swigwin-4.0.2\swig.exe -c++ -csharp -namespace pagmo -dllimport pagmoWrapper.dll swigInterfaceFileAndPagmoHeaders\pagmoSharpSwigInterface.i
copy swigInterfaceFileAndPagmoHeaders\pagmoSharpSwigInterface_wrap.cxx "pagmoWrapper\GeneratedWrappers.cxx"
copy swigInterfaceFileAndPagmoHeaders\pagmoSharpSwigInterface_wrap.h "pagmoWrapper\pagmoSharpSwigInterface_wrap.h"

rmdir "pagmoSharp\pygmoWrappers\" /S /Q
mkdir "pagmoSharp\pygmoWrappers\"
xcopy /I /Y swigInterfaceFileAndPagmoHeaders\*.cs "pagmoSharp\pygmoWrappers\"

del "swigInterfaceFileAndPagmoHeaders\pagmoSharpSwigInterface_wrap.cxx"
del "swigInterfaceFileAndPagmoHeaders\pagmoSharpSwigInterface_wrap.h"
del /s /q /f "swigInterfaceFileAndPagmoHeaders\*.cs"