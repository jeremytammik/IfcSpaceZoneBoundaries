﻿using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using System.Diagnostics;

namespace IfcSpaceZoneBoundaries
{
  class Util
  {
    #region Geometrical Comparison
    public const double _eps = 1.0e-9;

    public static double Eps
    {
      get
      {
        return _eps;
      }
    }

    public static double MinLineLength
    {
      get
      {
        return _eps;
      }
    }

    public static double TolPointOnPlane
    {
      get
      {
        return _eps;
      }
    }

    public static bool IsZero(
      double a,
      double tolerance = _eps )
    {
      return tolerance > Math.Abs( a );
    }

    public static bool IsEqual(
      double a,
      double b,
      double tolerance = _eps )
    {
      return IsZero( b - a, tolerance );
    }

    public static int Compare(
      double a,
      double b,
      double tolerance = _eps )
    {
      return IsEqual( a, b, tolerance )
        ? 0
        : ( a < b ? -1 : 1 );
    }

    public static int Compare(
      XYZ p,
      XYZ q,
      double tolerance = _eps )
    {
      int d = Compare( p.X, q.X, tolerance );

      if( 0 == d )
      {
        d = Compare( p.Y, q.Y, tolerance );

        if( 0 == d )
        {
          d = Compare( p.Z, q.Z, tolerance );
        }
      }
      return d;
    }

    /// <summary>
    /// Implement a comparison operator for lines 
    /// in the XY plane useful for sorting into 
    /// groups of parallel lines.
    /// </summary>
    public static int Compare( Line a, Line b )
    {
      XYZ pa = a.GetEndPoint( 0 );
      XYZ qa = a.GetEndPoint( 1 );
      XYZ pb = b.GetEndPoint( 0 );
      XYZ qb = b.GetEndPoint( 1 );
      XYZ va = qa - pa;
      XYZ vb = qb - pb;

      // Compare angle in the XY plane

      double ang_a = Math.Atan2( va.Y, va.X );
      double ang_b = Math.Atan2( vb.Y, vb.X );

      int d = Compare( ang_a, ang_b );

      if( 0 == d )
      {
        // Compare distance of unbounded line to origin

        double da = ( qa.X * pa.Y - qa.Y * pa.Y )
          / va.GetLength();

        double db = ( qb.X * pb.Y - qb.Y * pb.Y )
          / vb.GetLength();

        d = Compare( da, db );

        if( 0 == d )
        {
          // Compare distance of start point to origin

          d = Compare( pa.GetLength(), pb.GetLength() );

          if( 0 == d )
          {
            // Compare distance of end point to origin

            d = Compare( qa.GetLength(), qb.GetLength() );
          }
        }
      }
      return d;
    }

    /// <summary>
    /// Predicate to test whewther two points or 
    /// vectors can be considered equal with the 
    /// given tolerance.
    /// </summary>
    public static bool IsEqual(
      XYZ p,
      XYZ q,
      double tolerance = _eps )
    {
      return 0 == Compare( p, q, tolerance );
    }

    /// <summary>
    /// Return true if the given bounding box bb
    /// contains the given point p in its interior.
    /// </summary>
    public bool BoundingBoxXyzContains(
      BoundingBoxXYZ bb,
      XYZ p )
    {
      return 0 < Compare( bb.Min, p )
        && 0 < Compare( p, bb.Max );
    }

    /// <summary>
    /// Return true if the vectors v and w 
    /// are non-zero and perpendicular.
    /// </summary>
    bool IsPerpendicular( XYZ v, XYZ w )
    {
      double a = v.GetLength();
      double b = v.GetLength();
      double c = Math.Abs( v.DotProduct( w ) );
      return _eps < a
        && _eps < b
        && _eps > c;
      // c * c < _eps * a * b
    }

    public static bool IsParallel( XYZ p, XYZ q )
    {
      return p.CrossProduct( q ).IsZeroLength();
    }

    public static bool IsCollinear( Line a, Line b )
    {
      XYZ v = a.Direction;
      XYZ w = b.Origin - a.Origin;
      return IsParallel( v, b.Direction )
        && IsParallel( v, w );
    }

    public static bool IsHorizontal( XYZ v )
    {
      return IsZero( v.Z );
    }

    public static bool IsVertical( XYZ v )
    {
      return IsZero( v.X ) && IsZero( v.Y );
    }

    public static bool IsVertical( XYZ v, double tolerance )
    {
      return IsZero( v.X, tolerance )
        && IsZero( v.Y, tolerance );
    }

    public static bool IsHorizontal( Edge e )
    {
      XYZ p = e.Evaluate( 0 );
      XYZ q = e.Evaluate( 1 );
      return IsHorizontal( q - p );
    }

    public static bool IsHorizontal( PlanarFace f )
    {
      return IsVertical( f.FaceNormal );
    }

    public static bool IsVertical( PlanarFace f )
    {
      return IsHorizontal( f.FaceNormal );
    }

    public static bool IsVertical( CylindricalFace f )
    {
      return IsVertical( f.Axis );
    }

    /// <summary>
    /// Minimum slope for a vector to be considered
    /// to be pointing upwards. Slope is simply the
    /// relationship between the vertical and
    /// horizontal components.
    /// </summary>
    const double _minimumSlope = 0.3;

    /// <summary>
    /// Return true if the Z coordinate of the
    /// given vector is positive and the slope
    /// is larger than the minimum limit.
    /// </summary>
    public static bool PointsUpwards( XYZ v )
    {
      double horizontalLength = v.X * v.X + v.Y * v.Y;
      double verticalLength = v.Z * v.Z;

      return 0 < v.Z
        && _minimumSlope
          < verticalLength / horizontalLength;

      //return _eps < v.Normalize().Z;
      //return _eps < v.Normalize().Z && IsVertical( v.Normalize(), tolerance );
    }

    /// <summary>
    /// Return the maximum value from an array of real numbers.
    /// </summary>
    public static double Max( double[] a )
    {
      Debug.Assert( 1 == a.Rank, "expected one-dimensional array" );
      Debug.Assert( 0 == a.GetLowerBound( 0 ), "expected zero-based array" );
      Debug.Assert( 0 < a.GetUpperBound( 0 ), "expected non-empty array" );
      double max = a[0];
      for( int i = 1; i <= a.GetUpperBound( 0 ); ++i )
      {
        if( max < a[i] )
        {
          max = a[i];
        }
      }
      return max;
    }
    #endregion // Geometrical Comparison

    const double _inchToMm = 25.4;
    const double _footToMm = 12 * _inchToMm;

    /// <summary>
    /// Convert a given length in feet to millimetres,
    /// rounded to the closest millimetre.
    /// </summary>
    public static int FootToMmInt( double length )
    {
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

    static Options _geo_opt = new Options();

    /// <summary>
    /// Return the topmost horizontal face, if found, or null
    /// </summary>
    PlanarFace GetTopHorizontalFace( GeometryElement geo )
    {
      PlanarFace top_face = null;

      foreach( GeometryObject obj in geo )
      {
        Solid solid = obj as Solid;

        if( null != solid )
        {
          foreach( Face f in solid.Faces )
          {
            PlanarFace pf = f as PlanarFace;

            if( null != pf
              && IsHorizontal( pf )
              && (null == top_face || top_face.Origin.Z < pf.Origin.Z ) )
            {
              top_face = pf;
            }
          }
        }
      }
      return top_face;
    }

    /// <summary>
    /// Return the XY coordinates of the top horizontal
    /// face of the given element, scaled to millimetres, 
    /// in a string of space-separated integer values.
    /// </summary>
    public static string GetTopFaceBoundaryString( 
      Element e )
    {
      GeometryElement geo = e.get_Geometry( _geo_opt );


      return null;
    }
  }
}
