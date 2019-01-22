using System.Diagnostics;
using Autodesk.Revit.DB;

namespace IfcSpaceZoneBoundaries
{
  class RoomZoneExporter
  {
    public RoomZoneExporter( Document doc )
    {
      // IFC room and zones are represented by
      // DirectShape elements.

      FilteredElementCollector col
        = new FilteredElementCollector( doc )
          .OfClass( typeof( DirectShape ) )
          .OfCategory( BuiltInCategory.OST_GenericModel );

      foreach( Element e in col )
      {
        RoomZoneData d = new RoomZoneData( e );
        if( d.IsRoomOrZone )
        {
          Debug.Print( d.AsString() );
        }
      }
    }
  }
}
