﻿using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace IfcSpaceZoneBoundaries
{
  class RoomZoneData
  {
    /// <summary>
    /// These are the data fields exported for each 
    /// room and zone. The first is simply 'S' or 'Z'.
    /// The zone and layer properties are only set 
    /// on room object records.
    /// </summary>
    public string Space_or_Zone;
    public string GUID;
    public string Name;
    public string Zone;
    public string Layer;
    public string Pset;
    public string Boundary;

    /// <summary>
    /// Predicate indicating a valid room or zone
    /// </summary>
    public bool IsRoomOrZone
    {
      get { return null != Space_or_Zone; }
    }

    /// <summary>
    /// Private constant strings for retrieving IFC
    /// properties
    /// </summary>
    const string _pname_export_as = "IfcExportAs";
    const string _pname_guid = "IfcGUID";
    const string _pname_name = "IfcName";
    const string _pname_layer = "IfcPresentationLayer";
    const string _pname_pset = "IfcPropertySetList";
    const string _pname_zone = "IfcZone";
    const string _export_as_room = "IfcSpace.INTERNAL";
    const string _export_as_zone = "Ifczone";

    /// <summary>
    /// Export CSV format using comma separated fields 
    /// with no other delimiters
    /// </summary>
    const string _format_string = "{0},{1},{2},{3},{4},{5},{6}";

    /// <summary>
    /// Read a string property value from a name Revit
    /// parameter. Used to read IFC properties from
    /// shared parameters.
    /// </summary>
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

    /// <summary>
    /// Instantiate a room or zone data object from
    /// a given Revit element `e`
    /// </summary>
    public RoomZoneData( Element e )
    {
      string export_as = GetStringParamValue(
        e, _pname_export_as );

      if( export_as.Equals( _export_as_room ) )
      {
        Space_or_Zone = "S";
      }
      else if( export_as.Equals( _export_as_zone ) )
      {
        Space_or_Zone = "Z";
      }

      if( IsRoomOrZone )
      {
        GUID = GetStringParamValue( e, _pname_guid );
        Name = GetStringParamValue( e, _pname_name );
        Zone = GetStringParamValue( e, _pname_zone );
        Layer = GetStringParamValue( e, _pname_layer );
        Pset = GetStringParamValue( e, _pname_pset );
        Boundary = null;
      }
    }

    /// <summary>
    /// Return a string to export room or zone data 
    /// to CSV
    /// </summary>
    public string AsString()
    {
      return string.Format( _format_string,
        Space_or_Zone,
        GUID,
        Name,
        Zone,
        Layer,
        Pset,
        Boundary );
    }
  }
}
