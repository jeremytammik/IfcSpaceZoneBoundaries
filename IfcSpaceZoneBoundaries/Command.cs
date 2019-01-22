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
          .OfClass( typeof( DirectShape ) );

      foreach( Element e in col )
      {
        Debug.Print( e.Name );
      }
      return Result.Succeeded;
    }
  }
}
