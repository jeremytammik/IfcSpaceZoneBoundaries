#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

namespace IfcSpaceZoneBoundaries
{
  [Transaction( TransactionMode.ReadOnly )]
  public class Command : IExternalCommand
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

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;
      Application app = uiapp.Application;
      Document doc = uidoc.Document;

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
          Debug.Print( e.Name );
        }
      }
      return Result.Succeeded;
    }
  }
}
