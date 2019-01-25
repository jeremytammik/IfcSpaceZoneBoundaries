#region Namespaces
using System;
using System.IO;
using System.Reflection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
#endregion

namespace IfcSpaceZoneBoundaries
{
  class App : IExternalDBApplication
  {
    static JtLogger _logger;
    static JtSettings _settings;

    public static void Log( string msg )
    {
      _logger.Log( msg );
    }

    /// <summary>
    /// Return the user-defined settings
    /// </summary>
    public static JtSettings Settings
    {
      get
      {
        return _settings;
      }
    }

    ///// <summary>
    ///// Return the full add-in assembly folder path.
    ///// </summary>
    //public static string Path
    //{
    //  get
    //  {
    //    return System.IO.Path.GetDirectoryName(
    //      Assembly.GetExecutingAssembly().Location );
    //  }
    //}

    void OnApplicationInitialized(
      object sender,
      ApplicationInitializedEventArgs e )
    {
      throw new NotImplementedException();
    }

    public ExternalDBApplicationResult OnStartup( 
      ControlledApplication a )
    {
      string path = Assembly.GetExecutingAssembly().Location;

      _logger = new JtLogger();
      _logger.Init( Path.ChangeExtension( path, "log" ) );

      JtSettings.Init( Path.ChangeExtension( path, "config" ) );
      _settings = JtSettings.Load();

      a.ApplicationInitialized += OnApplicationInitialized;
      return ExternalDBApplicationResult.Succeeded;
    }

    public ExternalDBApplicationResult OnShutdown( 
      ControlledApplication a )
    {
      _settings.Save();
      _logger.Done();
      return ExternalDBApplicationResult.Succeeded;
    }
  }
}
