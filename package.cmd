tools\nuget.exe update -self

if not exist package mkdir package
if not exist package mkdir package
if not exist package\lib mkdir package\lib
if not exist package\lib\4.6.1 mkdir package\lib\4.6.1

copy JWT\bin\Release\JWT.dll package\lib\4.6.1\

tools\nuget.exe pack JWT.nuspec -BasePath package