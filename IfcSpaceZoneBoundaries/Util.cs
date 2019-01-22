using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace IfcSpaceZoneBoundaries
{
  class Util
  {
    const double _inchToMm = 25.4;
    const double _footToMm = 12 * _inchToMm;

    /// <summary>
    /// Convert a given length in feet to millimetres,
    /// rounded to the closest millimetre.
    /// </summary>
    public static int FootToMmInt( double length )
    {
      //return (int) ( _feet_to_mm * d + 0.5 );
      return (int) Math.Round( _footToMm * length,
        MidpointRounding.AwayFromZero );
    }

    /// <summary>
    /// Read a string property value from a name Revit
    /// parameter. Used to read IFC properties from
    /// shared parameters.
    /// </summary>
    public static string GetStringParamValue(
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
  }
}
