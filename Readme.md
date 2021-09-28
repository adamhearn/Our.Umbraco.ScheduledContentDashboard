# Umbraco.ScheduledContentDashboard

**Scheduled Content Dashboard** for **Umbraco CMS** (v7) that provides a custom dashboard within the **Content** section that lets you view the list of content items that are both scheduled for release and expiration.

## Features ##

The Scheduled Content Dashboard allows you to view the list of scheduled items within your Umbraco site.

# Build

- Visual Studio 2019
- NuGet package references for Umbraco 7, Ensure.That

# NuGet Installation

When you use NuGet the custom dasboard must be added to Umbraco by adding the following to the Dashboard.config file:

```
  <section alias="scheduledContentDashboard">
    <areas>
      <area>content</area>
    </areas>
    <tab caption="Scheduled Content">
      <control>/App_Plugins/ScheduledContentDashboard/dashboard.html</control>
    </tab>
  </section>
  ```
