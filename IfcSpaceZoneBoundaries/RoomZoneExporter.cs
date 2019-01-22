using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.DB;

namespace IfcSpaceZoneBoundaries
{
  class RoomZoneExporter
  {
    const string _pname_export_as = "IfcexportAs";
    const string _export_as_room = "IfcSpace.INTERNAL";
    const string _export_as_zone = "Ifczone";

    static string GetStringParamValue(
      Element e,
      string pname )
    {
      IList<Parameter> ps = e.GetParameters( pname );

      int n = ps.Count;

      if( 1 < n )
      {
        throw new ArgumentException(
          "expected maximum one parameter named "
            + pname );
      }

      return ( 1 == n )
        ? ps[0].AsString()
        : null;
    }

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
        string export_as = GetStringParamValue(
          e, _pname_export_as );

        if( export_as.Equals( _export_as_room )
          || export_as.Equals( _export_as_zone ) )
        {
          RoomZoneData d = new RoomZoneData( e );
          Debug.Print( d.AsString() );
        }
      }
    }
  }
}
