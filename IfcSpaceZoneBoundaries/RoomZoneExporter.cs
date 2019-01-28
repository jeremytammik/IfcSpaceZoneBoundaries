using System.Diagnostics;
using Autodesk.Revit.DB;
using System.IO;

namespace IfcSpaceZoneBoundaries
{
  class RoomZoneExporter
  {
    public RoomZoneExporter( Document doc )
    {
      Debug.Print( "Logging level {0}", App.Settings.LoggingLevel );

      // IFC room and zones are represented by
      // generic model direct shape elements.

      FilteredElementCollector col
        = new FilteredElementCollector( doc )
          .OfClass( typeof( DirectShape ) )
          .OfCategory( BuiltInCategory.OST_GenericModel );

      string path = Path.ChangeExtension(
        doc.PathName, "csv" );

      App.Log( string.Format( 
        "Writing {0} zones and spaces to {1}.",
        col.GetElementCount(), path ) );

      using( StreamWriter csv_out 
        = new StreamWriter( path ) )
      {
        foreach( Element e in col )
        {
          RoomZoneData d = new RoomZoneData( e );

          if( d.IsRoomOrZone )
          {
            csv_out.WriteLine( d.AsString() );
          }
        }
        csv_out.Close();
      }
    }
  }
}
