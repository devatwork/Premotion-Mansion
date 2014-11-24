@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

set nuget=
if "%nuget%" == "" (
    set nuget=src\.nuget\NuGet.exe
)

set nunit="src\packages\NUnit.Runners.2.6.2\tools\nunit-console.exe"

echo Update self %nuget%
%nuget% update -self
if %errorlevel% neq 0 goto failure

echo Restore packages
%nuget% install "src\.nuget\packages.config" -OutputDirectory src\packages -NonInteractive
if %errorlevel% neq 0 goto failure

echo Build
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild src\Premotion.Mansion.sln /t:Rebuild /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
if %errorlevel% neq 0 goto failure

echo Unit tests
rem %nunit% src\Premotion.Mansion.Core.Tests\bin\Premotion.Mansion.Core.Tests.dll /framework:net-4.5
rem if %errorlevel% neq 0 goto failure
rem %nunit% src\Premotion.Mansion.Amazon.Tests\bin\Premotion.Mansion.Amazon.Tests.dll /framework:net-4.5
rem if %errorlevel% neq 0 goto failure
rem %nunit% src\Premotion.Mansion.Repository.SqlServer.Tests\bin\Premotion.Mansion.Repository.SqlServer.Tests.dll /framework:net-4.5
rem if %errorlevel% neq 0 goto failure
rem %nunit% src\Premotion.Mansion.Web.Tests\bin\Premotion.Mansion.Web.Tests.dll /framework:net-4.5
rem if %errorlevel% neq 0 goto failure
rem %nunit% src\Premotion.Mansion.Web.Portal.Tests\bin\Premotion.Mansion.Web.Portal.Tests.dll /framework:net-4.5
rem if %errorlevel% neq 0 goto failure

echo Package
mkdir Build
cmd /c %nuget% pack "src\Premotion.Mansion.Core\Premotion.Mansion.Core.csproj" -symbols -o Build -p Configuration=%config% %version%
if %errorlevel% neq 0 goto failure
cmd /c %nuget% pack "src\Premotion.Mansion.Amazon\Premotion.Mansion.Amazon.csproj" -symbols -o Build -p Configuration=%config% %version%
if %errorlevel% neq 0 goto failure
cmd /c %nuget% pack "src\Premotion.Mansion.KnowledgeOrganization\Premotion.Mansion.KnowledgeOrganization.csproj" -symbols -o Build -p Configuration=%config% %version%
if %errorlevel% neq 0 goto failure
cmd /c %nuget% pack "src\Premotion.Mansion.Repository.ElasticSearch\Premotion.Mansion.Repository.ElasticSearch.csproj" -symbols -o Build -p Configuration=%config% %version%
if %errorlevel% neq 0 goto failure
cmd /c %nuget% pack "src\Premotion.Mansion.Repository.SqlServer\Premotion.Mansion.Repository.SqlServer.csproj" -symbols -o Build -p Configuration=%config% %version%
if %errorlevel% neq 0 goto failure
cmd /c %nuget% pack "src\Premotion.Mansion.Scheduler\Premotion.Mansion.Scheduler.csproj" -symbols -o Build -p Configuration=%config% %version%
if %errorlevel% neq 0 goto failure
cmd /c %nuget% pack "src\Premotion.Mansion.Web\Premotion.Mansion.Web.csproj" -symbols -o Build -p Configuration=%config% %version%
if %errorlevel% neq 0 goto failure
cmd /c %nuget% pack "src\Premotion.Mansion.Web.Portal\Premotion.Mansion.Web.Portal.csproj" -symbols -o Build -p Configuration=%config% %version%
if %errorlevel% neq 0 goto failure
cmd /c %nuget% pack "src\Premotion.Mansion.Web.Social\Premotion.Mansion.Web.Social.csproj" -symbols -o Build -p Configuration=%config% %version%
if %errorlevel% neq 0 goto failure

:success
echo Success
goto end

:failure
echo Failed

:end