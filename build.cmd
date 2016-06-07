@echo off

set solution=%1%

set FrameworkVersion=v4.0.30319
set FrameworkDir=%SystemRoot%\Microsoft.NET\Framework

if exist "%SystemRoot%\Microsoft.NET\Framework64" (
  set FrameworkDir=%SystemRoot%\Microsoft.NET\Framework64
)

set msbuild_exe="%FrameworkDir%\%FrameworkVersion%\msbuild.exe"
set msbuild_params=/p:Configuration=Release /t:Clean;Rebuild /verbosity:normal /nologo 

echo build the solution... .. .
%msbuild_exe% %msbuildparams% %solution%