tools\nuget.exe update -self

if not exist package mkdir package
if not exist package mkdir package
if not exist package\lib mkdir package\lib
if not exist package\lib\net461 mkdir package\lib\net461

copy JWT\bin\Release\JWT.dll package\lib\net461

tools\nuget.exe pack JWT.nuspec -BasePath package