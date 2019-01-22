using Autodesk.Revit.DB;

namespace IfcSpaceZoneBoundaries
{
  class RoomZoneData
  {
    const string _format_string = "{0},{1},{2},{3},{4},{5},{6}";

    public string Space_or_Zone;
    public string GUID;
    public string Name;
    public string Zone;
    public string Layer;
    public string Pset;
    public string Boundary;

    public RoomZoneData( Element e )
    {

    }

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
