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
- Request & Url Helpers


## Getting Started

Just download the solution and build project. This way you can add libraries directly to your project.

### Prerequisites

Project is built with .NET 4.6.1. There are no other library dependencies.

## Using Namespaces

### Configuration

First of all it will be wise to edit .config file to include Nhea configurations. Inside configSections we're gonna add Nhea's sectionGroup.

```
<configSections>
    <sectionGroup name="nhea" type="Nhea.Configuration.GenericConfigSection, Nhea">
      <section name="application" type="Nhea.Configuration.GenericConfigSection.ApplicationSection.ApplicationConfigSection, Nhea"/>
      <section name="data" type="Nhea.Configuration.GenericConfigSection.DataSection.DataConfigSection, Nhea"/>
      <section name="log" type="Nhea.Configuration.GenericConfigSection.LogSection.LogConfigSection, Nhea"/>
      <section name="web" type="Nhea.Configuration.GenericConfigSection.WebSection.WebConfigSection, Nhea"/>
    </sectionGroup>
  </configSections>
```