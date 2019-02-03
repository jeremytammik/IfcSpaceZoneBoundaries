#define FORGE_DA4R_TEST_LOCALLY

#region Namespaces
using System;
using System.IO;
using System.Reflection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using DesignAutomationFramework;
using IfcSpaceZoneBoundaries.Exporter;
#endregion

namespace IfcSpaceZoneBoundaries.AppBundle
{
  class App : IExternalDBApplication
  {
    /// <summary>
    /// Export all linked-in IFC document rooms and zones
    /// </summary>
    int ExportLinkedInIfcDocs( Application app )
    {
      if( 0 == app.Documents.Size )
      {
        string path = JtSettings.Instance
          .IfcRvtInputFilePath;

        Document doc = app.OpenDocumentFile( path );

        if( doc == null )
        {
          string s = string.Format(
            "Could not open document {0}.", path );

          JtLogger.Log( s );

          throw new InvalidOperationException( s );
        }
      }
      return RoomZoneExporter.ExportAll( app );
    }

#if FORGE_DA4R_TEST_LOCALLY
    void OnApplicationInitialized(
      object sender,
      ApplicationInitializedEventArgs e )
    {
      // `sender` is an Application instance:

      Application app = sender as Application;

      ExportLinkedInIfcDocs( app );
    }
#else // if not FORGE_DA4R_TEST_LOCALLY
    private void OnDesignAutomationReadyEvent( 
      object sender, 
      DesignAutomationReadyEventArgs e )
    {
      // `sender` is an Application instance:

      Application app = sender as Application;

      ExportLinkedInIfcDocs( app );
    }
#endif // FORGE_DA4R_TEST_LOCALLY

    public ExternalDBApplicationResult OnStartup(
      ControlledApplication a )
    {
      string path = Assembly.GetExecutingAssembly().Location;

      JtLogger.Init( Path.ChangeExtension( path, "log" ) );

      JtSettings.Init( Path.ChangeExtension( path, "json" ) );

#if FORGE_DA4R_TEST_LOCALLY
      a.ApplicationInitialized += OnApplicationInitialized;
#else // if not FORGE_DA4R_TEST_LOCALLY
      DesignAutomationBridge.DesignAutomationReadyEvent 
        += OnDesignAutomationReadyEvent;
#endif // FORGE_DA4R_TEST_LOCALLY

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
