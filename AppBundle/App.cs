#region Namespaces
using System;
using System.IO;
using System.Reflection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using IfcSpaceZoneBoundaries.Exporter;
using DesignAutomationFramework;
#endregion

namespace IfcSpaceZoneBoundaries.AppBundle
{
  class App : IExternalDBApplication
  {
    void OnApplicationInitialized(
      object sender,
      ApplicationInitializedEventArgs e )
    {
      throw new NotImplementedException();
    }

    private void OnDesignAutomationReadyEvent( 
      object sender, 
      DesignAutomationReadyEventArgs e )
    {
      throw new NotImplementedException();
    }

    public ExternalDBApplicationResult OnStartup(
      ControlledApplication a )
    {
      string path = Assembly.GetExecutingAssembly().Location;

      JtLogger.Init( Path.ChangeExtension( path, "log" ) );

      JtSettings.Init( Path.ChangeExtension( path, "json" ) );

      a.ApplicationInitialized += OnApplicationInitialized;

      DesignAutomationBridge.DesignAutomationReadyEvent 
        += OnDesignAutomationReadyEvent;

      return ExternalDBApplicationResult.Succeeded;
    }

    public ExternalDBApplicationResult OnShutdown(
      ControlledApplication a )
    {
      JtSettings.Save();
      JtLogger.Done();
      return ExternalDBApplicationResult.Succeeded;
    }
  }
}
