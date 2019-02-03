using System.Collections.Generic;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;

namespace IfcSpaceZoneBoundaries.Exporter
{
  public class RoomZoneExporter
  {
    /// <summary>
    /// Retrieve and return all linked-in IFC documents.
    /// </summary>
    public static List<Document> GetLinkedInIfcDocs(
      Application app )
    {
      List<Document> ifcdocs = null;
      DocumentSet docs = app.Documents;
      int n = docs.Size;

      JtLogger.Log( string.Format(
        "{0} open document{1}",
        n, Util.PluralSuffix( n ) ) );

      foreach( Document d in docs )
      {
        string s = d.PathName;
        if( s.EndsWith( ".ifc.RVT" ) )
        {
          if( null == ifcdocs )
          {
            ifcdocs = new List<Document>();
          }
          ifcdocs.Add( d );
        }
      }
      return ifcdocs;
    }

    /// <summary>
    /// Retrieve the rooms and areas, i.e., the spaces
    /// and zones, from the linked-in IFC document.
    /// </summary>
    /// <param name="ifcdoc">Linked-in IFC document</param>
    public static int Export( Document ifcdoc )
    {
      //logger.Log( string.Format( "Logging level {0}", 
      //  App.Settings.LoggingLevel ) );

      string path = ifcdoc.PathName;

      JtLogger.Log( string.Format(
        "Processing {0}...", path ) );

      // IFC room and zones are represented by
      // generic model direct shape elements.

      FilteredElementCollector col
        = new FilteredElementCollector( ifcdoc )
          .OfClass( typeof( DirectShape ) )
          .OfCategory( BuiltInCategory.OST_GenericModel );

      JtLogger.Log( string.Format( "Extracting zones "
        + "and spaces from {0} direct shapes.",
        col.GetElementCount() ) );

      path = Path.ChangeExtension( path, "csv" );

      int n = 0;

      using( StreamWriter csv_out
        = new StreamWriter( path ) )
      {
        foreach( Element e in col )
        {
          RoomZoneData d = new RoomZoneData( e );

          if( d.IsRoomOrZone )
          {
            csv_out.WriteLine( d.AsString() );
            ++n;
          }
        }
        csv_out.Close();
      }
      JtLogger.Log( string.Format(
        "{0} zones and spaces written to {1}.",
        n, path ) );

      return n;
    }

    public static int ExportAll(
      Application app )
    {
      List<Document> ifcdocs = GetLinkedInIfcDocs(
        app );

      int n = ifcdocs.Count;

      JtLogger.Log( string.Format(
        "{0} linked-in IFC document{1} found.",
        n, Util.PluralSuffix( n ) ) );

      foreach( Document ifcdoc in ifcdocs )
      {
        JtLogger.Log( "Linked-in IFC document: "
          + ifcdoc.PathName );

        n = RoomZoneExporter.Export( ifcdoc );
      }

      JtLogger.Log( string.Format(
        "{0} linked-in IFC document{1} exported.",
        n, Util.PluralSuffix( n ) ) );

      return n;
    }
  }
}
