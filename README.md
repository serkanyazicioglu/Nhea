[![Build Status](https://dev.azure.com/serkanyazicioglu/serkanyazicioglu/_apis/build/status/serkanyazicioglu.Nhea?branchName=master)](https://dev.azure.com/serkanyazicioglu/serkanyazicioglu/_build/latest?definitionId=1&branchName=master)
[![NuGet](https://img.shields.io/nuget/v/Nhea.svg)](https://www.nuget.org/packages/Nhea/)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=serkanyazicioglu_Nhea&metric=alert_status)](https://sonarcloud.io/dashboard?id=serkanyazicioglu_Nhea)

# Nhea Framework

Nhea Framework is a set of tools that helped me over years. This framework consists of following namespaces:
- Communication (Sending E-Mails, Notifications)
- Configuration (Settings for Nhea framework)
- Data (Basic namespace for repository libraries)
- Enumeration (Some helpers for enum usage)
- Globalization (Culture management)
- Localization (Multilanguage)
- Logging (Logging publishers)

Other Helper namespaces:
- Convertion Helpers
- Image Helpers
- Text Helpers


## Getting Started

Nhea is on NuGet. You may install Nhea via NuGet Package manager.

https://www.nuget.org/packages/Nhea/

```
Install-Package Nhea
```

### Prerequisites

Project is built with .NET 6

## Using Namespaces

This project also supports .net core configuration. Documentation will be provided soon.

### .Net Core Configuration

There are 3 different configuration blocks for .Net Core. Logging, mailing and localization.

#### Logging Configuration

Nhea has three different kind of logging: File, Database (SQL) and E-Mail. .Net Core DI logging is supported. 

By default Logging targets File.

For file publishing you can change the file name to provide a unique name. By defaylt log file will be created in the executing folder. If you want to change the path or the file name you can override these settings. Directory will be automatically created by Nhea.

```
services.AddLogging(configure =>
    configure.AddNheaLogger(nheaConfigure =>
    {
        nheaConfigure.PublishType = Nhea.Logging.PublishTypes.File;
        nheaConfigure.FileName = "loggingfile.txt";
    })
);
```
For directory overriding;
```
services.AddLogging(configure =>
    configure.AddNheaLogger(nheaConfigure =>
    {
        nheaConfigure.PublishType = Nhea.Logging.PublishTypes.File;
        nheaConfigure.DirectoryPath = "C:\Projects\";
        nheaConfigure.FileName = "loggingfile.txt";
    })
);
```
You can also format file name dynamically by datetime.
```
services.AddLogging(configure =>
    configure.AddNheaLogger(nheaConfigure =>
    {
        nheaConfigure.PublishType = Nhea.Logging.PublishTypes.File;
        nheaConfigure.DirectoryPath = "C:\Projects\";
        nheaConfigure.FileName = "NheaLog-{0:dd.MM.yyyy-HH:mm}.txt";
    })
);
```

For DB publishing you have to provide a sql connection string. In order to use SQL first you have to create Nhea's <a href=https://github.com/serkanyazicioglu/Nhea/blob/master/SQL/nhea_Log.sql>log table</a>. Execute the nhea_Log.sql which creates the schema of the table.

```
services.AddLogging(configure => configure.AddNheaLogger(nhea =>
{
    nhea.ConnectionString = "sql connection string";
    nhea.PublishType = Nhea.Logging.PublishTypes.Database;
}));
```

#### Mailing Configuration

Nhea has a mailing as a service and logging is directly intergrated with it. In order to use this service first you have to create Nhea's <a href=https://github.com/serkanyazicioglu/Nhea/blob/master/SQL/nhea_MailQueue.sql>mail tables</a>. Execute the nhea_MailQueue.sql which creates the schema of the tables.

```
services.AddMailService(configure =>
{
    configure.ConnectionString = "sql connection string";
});
```
Later you can inject IMailService to access the mail service.
```
private readonly IMailService mailService;
private readonly ILogger<HomeController> logger;

public HomeController(IMailService mailService, ILogger<HomeController> logger)
{
    this.mailService = mailService;
    this.logger = logger;
}
```
By calling the add method a mail record will be inserted to database.
```
mailService.Add(Environment.GetEnvironmentVariable("from@from.com", "to@to.com", "Mail Subject", "mail <b>body</b>");
```
Please refer to the <a href="https://github.com/serkanyazicioglu/Nhea#mailing-job-docker">mailing service section</a> for running the mail service via Docker.

#### Localization Configuration

For configuring the localization service you can use the following sample code. In order to use this service first you have to create Nhea's <a href=https://github.com/serkanyazicioglu/Nhea/blob/master/SQL/Localization.sql>localization table</a>. Execute the Localization.sql which creates the schema of the table.

```
services.AddNheaLocalizationService(configure =>
{
    configure.ConnectionString = "sql connection string";
    configure.DefaultLanguageId = 1;
});
```
Later you can inject ILocalizationService to access the localization service.
```
private readonly ILocalizationService localizationService;
private readonly ILogger<HomeController> logger;

public HomeController(ILocalizationService localizationService, ILogger<HomeController> logger)
{
    this.localizationService = localizationService;
    this.logger = logger;
}
```
Then you can fill the Localization table on the SQL manually or via code.

- To insert global translations;

```
localizationService.SaveLocalization("new translation", "NewTranslation");

var translation = localizationService.GetLocalization("NewTranslation");
Console.WriteLine("Tanslation: " + translation);

localizationService.DeleteLocalization("NewTranslation");
```
- To insert record bound translations;
```
Guid recordId = Guid.NewGuid();
int languageId = 1;
localizationService.SaveLocalization("new translation", "NewTranslation", recordId, "Member", languageId);

var memberTranslation = localizationService.GetLocalization("NewTranslation", recordId, languageId);
Console.WriteLine("Tanslation: " + translation);

localizationService.DeleteLocalization(recordId, "NewTranslation", languageId);
```
TargetEntityId must be Guid in this context. You can directly insert your primary key if it's Guid or can have another localization guid column. TargetEntityName must be table's name.

### .Net Framework Configuration

Important Note: Nhea won't be supporting .Net Framework since .Net Core took a lot of ground on the community.

First of all we will edit .config file to include Nhea configurations. Inside configSections we're gonna add Nhea's sectionGroup.

```
<configSections>
	<sectionGroup name="nhea" type="Nhea.Configuration.GenericConfigSection, Nhea">
		<section name="application" type="Nhea.Configuration.GenericConfigSection.ApplicationSection.ApplicationConfigSection, Nhea"/>
		<section name="data" type="Nhea.Configuration.GenericConfigSection.DataSection.DataConfigSection, Nhea"/>
		<section name="communication" type="Nhea.Configuration.GenericConfigSection.CommunicationSection.CommunicationConfigSection, Nhea" />
		<section name="log" type="Nhea.Configuration.GenericConfigSection.LogSection.LogConfigSection, Nhea"/>
	</sectionGroup>
</configSections>
```

And then we will embed our settings.

```
<nhea>
	<application environmentType="Development" />
	<data connectionName="MyDbConnectionName" />
	<communication />
	<log />
</nhea>
```

At the end .config file should look like this: 

```
...
<configSections>
	<sectionGroup name="nhea" type="Nhea.Configuration.GenericConfigSection, Nhea">
		<section name="application" type="Nhea.Configuration.GenericConfigSection.ApplicationSection.ApplicationConfigSection, Nhea" />
		<section name="data" type="Nhea.Configuration.GenericConfigSection.DataSection.DataConfigSection, Nhea" />
		<section name="communication" type="Nhea.Configuration.GenericConfigSection.CommunicationSection.CommunicationConfigSection, Nhea" />
		<section name="log" type="Nhea.Configuration.GenericConfigSection.LogSection.LogConfigSection, Nhea" />
	</sectionGroup>
</configSections>
<nhea>
	<application environmentType="Development" />
	<data />
	<communication />
	<log />
</nhea>
  ...
```

### Application segment

Inside the code we can get environment type by using:

```
if (Nhea.Configuration.Settings.Application.EnvironmentType == Nhea.Configuration.EnvironmentType.Production)
{
    //PROD
}
else
{
    //NOT PROD
}
```

Inside environmentType following options can be set:
- Development,
- Integration,
- Uat,
- Staging,
- Production

### Log segment

Nhea has three different kind of logging: File, Database (SQL) and E-Mail. By default Logging targets File.

```
Nhea.Logging.Logger.Log("Hello Nhea!");
```

By executing this code a log file will be created in the executing folder. If you want to change the path or the file name you can override these settings. Directory will be automatically created by Nhea.

```
<nhea>
	<application environmentType="Development" />
	<log directoryPath="C:\Projects\" fileName="MyLogFile.txt" />
</nhea>
```
You may also give file name a formatter for datetime.
```
<nhea>
	<application environmentType="Development" />
	<log directoryPath="C:\Projects\" fileName="NheaLog-{0:dd.MM.yyyy-HH:mm}.txt" />
</nhea>
```

Exceptions can easily be logged by the same method.
```
try
{
    throw new Exception("Test Exception");
}
catch (Exception ex)
{
    Nhea.Logging.Logger.Log(ex);
}
```

If you want to add additional details or parameters to exception logs you may use Exception.Data collection.
```
try
{
    throw new Exception("Test Exception");
}
catch (Exception ex)
{
    ex.Data.Add("Id", 1);
    Nhea.Logging.Logger.Log(ex);
}
```

If you have a SQL database you may want to target your DB for logs. It can easily be done by just a few configuration changes. In order to use SQL first you have to create Nhea's <a href=https://github.com/serkanyazicioglu/Nhea/blob/master/SQL/nhea_Log.sql>log table</a>. Execute the nhea_Log.sql which creates the schema of the table.

And then you should change the configuration. Add data to configuration list and set the connection name. Then change the defaultPublishType to 'Database'.

```
<nhea>
    <application environmentType="Development"></application>
    <data connectionName="DbConnectionStringName" />
    <log defaultPublishType="Database" />
  </nhea>
  <connectionStrings>
    <add name="DbConnectionStringName" connectionString="DB connection string" providerName="System.Data.SqlClient" />
  </connectionStrings>
```


### Mailing v2

As of 1.6.1 I've developed new version for mailing system. This will be supporting some more mailing functionalities by using the same table structure. What you have to do is before updating any of your projects you must update the mail sender process.

Please switch nhea_MailQueue Id column's default value to newsequentialid() when you fetch the latest version.

### Mailing Job Docker

You can use the sample project in the solution to create your mailing service or directly use the docker image. Click <b><a target='_blank' href='https://hub.docker.com/r/nhea/mailservice'>here</a></b> to visit repository docker page or run the following command to pull the image.

```
docker pull nhea/mailservice:latest
```
Please refer to the documentation on Docker Hub for running the service.

### Enumeration Helpers

As an old but useful practice you can set additional details for your enums. Simply put Detail attribute on to the header of your enumeraions.

```
public enum StatusType
{
    [Detail("Canceled")]
    Canceled = -10,
    [Detail("Planned")]
    Planned = -2,
    [Detail("In Production")]
    InProduction = -1,
    [Detail("Post Production")]
    PostProduction = 0,
    [Detail("Released")]
    Released = 1
}
```
Later you can use the following enumeration helper methods.
```
List<Nhea.Enumeration.Enumeration> enumerations = Nhea.Enumeration.EnumHelper.GetEnumerations<StatusType>();

foreach (var enumeration in enumerations)
{
    Console.WriteLine("Name: " + enumeration.Name + " .Detail: " + enumeration.Detail + " .Value: " + enumeration.Value.ToString());
}

StatusType enumByValue = Nhea.Enumeration.EnumHelper.GetEnum<StatusType>(1);

string enumDetail = Nhea.Enumeration.EnumHelper.GetDetail<StatusType>(1);
```