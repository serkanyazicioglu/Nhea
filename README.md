[![Build Status](https://travis-ci.org/serkanyazicioglu/Nhea.svg?branch=master)](https://travis-ci.org/serkanyazicioglu/Nhea)
[![NuGet](https://img.shields.io/nuget/v/Nhea.svg)](https://www.nuget.org/packages/Nhea/)

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

### Prerequisites

Project is built with .NET Framework Standard. 

Dependencies:
- Newtonsoft.Json (>= 11.0.2)
- System.Configuration.ConfigurationManager (>= 4.5.0)
- System.Data.Common (>= 4.3.0)
- System.Data.SqlClient (>= 4.5.1)
- System.Drawing.Common (>= 4.5.1)

## Using Namespaces

### Configuration

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
	<communication connectionName="MyDbConnectionName" />
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
	<section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
</configSections>
<nhea>
	<application environmentType="Development" />
	<data connectionName="MyDbConnectionName" />
	<communication connectionName="MyDbConnectionName" />
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