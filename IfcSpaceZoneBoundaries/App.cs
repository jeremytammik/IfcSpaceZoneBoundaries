#region Namespaces
using System;
using System.Reflection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
#endregion

namespace IfcSpaceZoneBoundaries
{
  class App : IExternalDBApplication
  {
    /// <summary>
    /// Return the full add-in assembly folder path.
    /// </summary>
    public static string Path
    {
      get
      {
        return System.IO.Path.GetDirectoryName(
          Assembly.GetExecutingAssembly().Location );
      }
    }

    void OnApplicationInitialized(
      object sender,
      ApplicationInitializedEventArgs e )
    {
      throw new NotImplementedException();
    }

    public ExternalDBApplicationResult OnStartup( 
      ControlledApplication a )
    {
      a.ApplicationInitialized += OnApplicationInitialized;
      return ExternalDBApplicationResult.Succeeded;
    }

    public ExternalDBApplicationResult OnShutdown( 
      ControlledApplication a )
    {
      return ExternalDBApplicationResult.Succeeded;
    }
  }
}
