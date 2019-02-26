using Autodesk.Revit.DB;

namespace IfcSpaceZoneBoundaries.Exporter
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
    public string Level;
    public string Area;
    public string Z;
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
    const string _export_as_zone = "IfcZone";

    /// <summary>
    /// Export CSV format using comma separated fields 
    /// with no other delimiters
    /// </summary>
    const string _format_string = "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}";

    /// <summary>
    /// Instantiate a room or zone data object from
    /// a given Revit element `e`
    /// </summary>
    public RoomZoneData( Element e )
    {
      string export_as = Util.GetStringParamValue(
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
        GUID = Util.GetStringParamValue( e, _pname_guid );
        Name = Util.GetStringParamValue( e, _pname_name );
        Zone = Util.GetStringParamValue( e, _pname_zone );
        Layer = Util.GetStringParamValue( e, _pname_layer );
        Pset = Util.GetStringParamValue( e, _pname_pset );
        Level = Util.GetLevelName( e );
        Boundary = Util.GetBottomFaceBoundaryStringZArea( 
          e, out Z, out Area );
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
        Level,
        Area,
        Z,
        Boundary );
    }
  }
}
