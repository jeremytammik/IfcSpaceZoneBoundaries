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

      // Access linked IFC document

      Document ifcdoc = null;
      DocumentSet docs = app.Documents;
      int n = docs.Size;
      Debug.Print( "{0} open documents", n );
      foreach(Document d in docs)
      {
        string s = d.PathName;
        if(s.EndsWith( ".ifc.RVT" ) )
        {
          ifcdoc = d;
        }
      }

      Debug.Print( "Linked-in IFC document: " + ifcdoc.PathName );

      RoomZoneExporter a = new RoomZoneExporter( ifcdoc );

      return Result.Succeeded;
    }
  }
}
