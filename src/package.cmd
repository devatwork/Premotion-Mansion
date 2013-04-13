.nuget\NuGet.exe update -self

%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe Premotion.Mansion.sln /t:Clean,Rebuild /p:Configuration=Release /fileLogger
.nuget\NuGet.exe pack Premotion.Mansion.Amazon\Premotion.Mansion.Amazon.nuspec
.nuget\NuGet.exe pack Premotion.Mansion.Core\Premotion.Mansion.Core.nuspec
.nuget\NuGet.exe pack Premotion.Mansion.KnowledgeOrganization\Premotion.Mansion.KnowledgeOrganization.nuspec
.nuget\NuGet.exe pack Premotion.Mansion.Repository.ElasticSearch\Premotion.Mansion.Repository.ElasticSearch.nuspec
.nuget\NuGet.exe pack Premotion.Mansion.Repository.SqlServer\Premotion.Mansion.Repository.SqlServer.nuspec
.nuget\NuGet.exe pack Premotion.Mansion.Web\Premotion.Mansion.Web.nuspec
.nuget\NuGet.exe pack Premotion.Mansion.Web.Portal\Premotion.Mansion.Web.Portal.nuspec
.nuget\NuGet.exe pack Premotion.Mansion.Web.Social\Premotion.Mansion.Web.Social.nuspec

# %windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe Premotion.Mansion.sln /t:Clean,Rebuild /p:Configuration=Debug /fileLogger
# .nuget\NuGet.exe pack Premotion.Mansion.Amazon\Premotion.Mansion.Amazon.symbols.nuspec -Symbols
# .nuget\NuGet.exe pack Premotion.Mansion.Core\Premotion.Mansion.Core.symbols.nuspec -Symbols
# .nuget\NuGet.exe pack Premotion.Mansion.KnowledgeOrganization\Premotion.Mansion.KnowledgeOrganization.symbols.nuspec -Symbols
# .nuget\NuGet.exe pack Premotion.Mansion.Repository.ElasticSearch\Premotion.Mansion.Repository.ElasticSearch.symbols.nuspec -Symbols
# .nuget\NuGet.exe pack Premotion.Mansion.Repository.SqlServer\Premotion.Mansion.Repository.SqlServer.symbols.nuspec -Symbols
# .nuget\NuGet.exe pack Premotion.Mansion.Web\Premotion.Mansion.Web.symbols.nuspec -Symbols
# .nuget\NuGet.exe pack Premotion.Mansion.Web.Portal\Premotion.Mansion.Web.Portal.symbols.nuspec -Symbols
# .nuget\NuGet.exe pack Premotion.Mansion.Web.Social\Premotion.Mansion.Web.Social.symbols.nuspec -Symbols