using System.IO;
using System.Web.Script.Serialization;

namespace IfcSpaceZoneBoundaries
{
  // grabbed from http://stackoverflow.com/questions/453161/best-practice-to-save-application-settings-in-a-windows-forms-application
  public class AppSettings<T> where T : new()
  {
    private const string _default_filename
      = "RvtFader.json";

    public void Save(
      string fileName = _default_filename )
    {
      File.WriteAllText(
        Path.Combine( App.Path, fileName ),
        ( new JavaScriptSerializer() ).Serialize(
          this ) );
    }

    public static void Save(
      T pSettings,
      string fileName = _default_filename )
    {
      string path = Path.Combine( App.Path, fileName );
      File.WriteAllText( path,
        ( new JavaScriptSerializer() ).Serialize(
          pSettings ) );
    }

    public static T Load(
      string fileName = _default_filename )
    {
      string path = Path.Combine( App.Path, fileName );
      T t = new T();
      if( File.Exists( path ) )
      {
        t = ( new JavaScriptSerializer() ).Deserialize<T>(
          File.ReadAllText( path ) );
      }
      return t;
    }
  }

  public class JtSettings : AppSettings<JtSettings>
  {
    public int DebugLevel = 3;
  }
}
