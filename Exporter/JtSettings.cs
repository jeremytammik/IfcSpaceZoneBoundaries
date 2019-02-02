using System.Diagnostics;
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
    static string _filename;

    /// <summary>
    /// Initialise setting class with filename
    /// </summary>
    public static void Init( string filename )
    {
      _filename = filename;
    }

    /// <summary>
    /// Load settings from file, if found
    /// </summary>
    /// <returns></returns>
    public static JtSettings Load()
    {
      Debug.Assert( null != _filename,
        "did you forget to call Init?" );

      JtSettings settings = new JtSettings();

      if( File.Exists( _filename ) )
      {
        settings = ( new JavaScriptSerializer() )
          .Deserialize<JtSettings>(
            File.ReadAllText( _filename ) );
      }
      return settings;
    }

    /// <summary>
    /// Save current settings to file
    /// </summary>
    public void Save()
    {
      File.WriteAllText( _filename,
        ( new JavaScriptSerializer() )
          .Serialize( this ) );
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
