#region Namespaces
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace IfcSpaceZoneBoundaries
{
  [Transaction( TransactionMode.ReadOnly )]
  public class Command : IExternalCommand
  {
    /// <summary>
    /// Retrieve and return the first linked-in 
    /// IFC document, if any is found.
    /// </summary>
    Document GetLinkedInIfcDoc( Application app )
    {
      Document ifcdoc = null;
      DocumentSet docs = app.Documents;
      int n = docs.Size;
      App.Log( string.Format( "{0} open documents", n ) );
      foreach( Document d in docs )
      {
        string s = d.PathName;
        if( s.EndsWith( ".ifc.RVT" ) )
        {
          ifcdoc = d;
        }
      }
      return ifcdoc;
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

      // Access linked-in IFC document

      Document ifcdoc = GetLinkedInIfcDoc( app );

      if( null == ifcdoc )
      {
        Debug.Assert( false, "not yet implemented" );
        // todo: create new RVT and link in IFC here

        ifcdoc = GetLinkedInIfcDoc( app );
      }

      App.Log( "Linked-in IFC document: " + ifcdoc.PathName );

      RoomZoneExporter a = new RoomZoneExporter( ifcdoc );

      return Result.Succeeded;
    }
  }
}
