﻿using System.Diagnostics;
using System.IO;
using System.Web.Script.Serialization;

namespace IfcSpaceZoneBoundaries.Exporter
{
  // grabbed from http://stackoverflow.com/questions/453161/best-practice-to-save-application-settings-in-a-windows-forms-application
  public class AppSettings<T> where T : new()
  {
    static string _filename;

    public AppSettings( string filename )
    {
      _filename = filename;
    }

    public void Save()
    {
      File.WriteAllText( _filename,
        ( new JavaScriptSerializer() ).Serialize(
          this ) );
    }

    public static void Save( T pSettings )
    {
      File.WriteAllText( _filename,
        ( new JavaScriptSerializer() ).Serialize(
          pSettings ) );
    }

    public static T Load()
    {
      T t = new T();
      if( File.Exists( _filename ) )
      {
        t = ( new JavaScriptSerializer() ).Deserialize<T>(
          File.ReadAllText( _filename ) );
      }
      return t;
    }
  }

  /// <summary>
  /// User defined settings class;
  /// Demonstrate defining input parameters 
  /// for DA4R app via input parameter file
  /// </summary>
  public class JtSettings
  {
    /// <summary>
    /// File name and path to configurable user settings
    /// </summary>
    static string _filename;

    /// <summary>
    /// Singleton instance
    /// </summary>
    static JtSettings _instance = null;

    /// <summary>
    /// Access the one and only singleton instance
    /// </summary>
    public static JtSettings Instance
    {
      get { return _instance;  }
    }

    /// <summary>
    /// Initialise user defined settings from specific file
    /// </summary>
    public static void Init( string path )
    {
      _filename = path;

      if( File.Exists( _filename ) )
      {
        _instance = ( new JavaScriptSerializer() )
          .Deserialize<JtSettings>(
            File.ReadAllText( _filename ) );
      }
      else
      {
        _instance = new JtSettings();
      }
    }

    /// <summary>
    /// Save current settings to file
    /// </summary>
    static public void Save()
    {
      File.WriteAllText( _filename,
        ( new JavaScriptSerializer() )
          .Serialize( _instance ) );
    }

    /// <summary>
    /// Private constructor
    /// </summary>
    JtSettings()
    {
    }

    /// <summary>
    /// If no IFC file has yet been linked in to the 
    /// current project, link this one in.
    /// </summary>
    public string IfcInputFilePath 
      = "Z:/a/special/bouygues/2019_bim_surface_info/test/02"
      + "/010-123xx3-arc-bat01-apt01_2_2018-12-27_1507.ifc";

    /// <summary>
    /// Log message level of detail
    /// </summary>
    public int LoggingLevel = 3;

    /// <summary>
    /// Filter for specific level or floor
    /// </summary>
    public string LevelOrFloorRegex = "";

    /// <summary>
    /// Filter for specific zone
    /// </summary>
    public string ZoneRegex = "APT04.*"; // only fourth floor
  }
}