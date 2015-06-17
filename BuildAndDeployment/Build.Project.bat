REM @ECHO OFF
cd %~dp0

SET minorVersion="1.2"
SET majorVersion="0"

SET projectID=ElasticSearch.Client
SET project1="..\src\\%projectID%\\%projectID%.csproj"

%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe "%project1%" /verbosity:m /target:Rebuild /p:platform=AnyCPU /p:TargetFrameworkVersion="v4.5.1" /p:Configuration=Release /p:OutputPath="%cd%\pack\lib\net451" /p:DebugSymbols=false /p:DebugType=none /P:SignAssembly=False
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe "%project1%" /verbosity:m /target:Rebuild /p:platform=AnyCPU /p:TargetFrameworkVersion="v4.5" /p:Configuration=Release /p:OutputPath="%cd%\pack\lib\net45" /p:DebugSymbols=false /p:DebugType=none /P:SignAssembly=False

%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe EditNuspec.msbuild /p:File="%projectID%.nuspec" /p:projectID="%projectID%" /p:majorVersion=%majorVersion% /p:minorVersion=%minorVersion%

echo Try to clean up
del %projectID%.nupkg
echo Clean up finished

echo Try to remove necessary dll's

del pack\lib\net451\Newtonsoft.Json.dll
del pack\lib\net451\Newtonsoft.Json.xml
del pack\lib\net45\Newtonsoft.Json.dll
del pack\lib\net45\Newtonsoft.Json.xml

echo Necessary dll's removed

echo Creating package
nuget.exe pack .\pack\%projectID%.nuspec

ren %projectID%.%majorVersion%.%minorVersion%.nupkg %projectID%.nupkg
echo Packet renamed to %projectID%.nupkg

echo Try to remove 'pack' folder
rd pack /S /Q
echo 'pack' folder removed

pause