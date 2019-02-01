﻿using Autodesk.Revit.DB;
using System.IO;

namespace IfcSpaceZoneBoundaries
{
  class RoomZoneExporter
  {
    /// <summary>
    /// Retrieve the rooms and areas, i.e., the spaces
    /// and zones, from the linked-in IFC document.
    /// </summary>
    /// <param name="ifcdoc">Linked-in IFC document</param>
    public RoomZoneExporter( Document ifcdoc )
    {
      App.Log( string.Format( "Logging level {0}", 
        App.Settings.LoggingLevel ) );

      // IFC room and zones are represented by
      // generic model direct shape elements.

      FilteredElementCollector col
        = new FilteredElementCollector( ifcdoc )
          .OfClass( typeof( DirectShape ) )
          .OfCategory( BuiltInCategory.OST_GenericModel );

      string path = Path.ChangeExtension(
        ifcdoc.PathName, "csv" );

      App.Log( string.Format( "Extracting zones "
        + "and spaces from {0} direct shapes.",
        col.GetElementCount() ) );

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
      App.Log( string.Format(
        "{0} zones and spaces written to {1}.",
        n, path ) );
    }
  }
}
