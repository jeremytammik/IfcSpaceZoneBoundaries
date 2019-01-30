#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.IFC.Import;
using Revit.IFC.Import.Data;
#endregion

namespace IfcSpaceZoneBoundaries
{
  [Transaction( TransactionMode.ReadOnly )]
  public class Command : IExternalCommand
  {
    /// <summary>
    /// Create a link to a given IFC file.
    /// Return true on success.
    /// </summary>
    bool CreateIfcLink( 
      Document doc, 
      string ifcpath )
    {
      bool rc = false;

      IDictionary<string, string> options
        = new Dictionary<string, string>( 2 );

      options["Action"] = "Link"; // default is "Open"
      options["Intent"] = "Reference"; // this is the default

      Importer importer = Importer.CreateImporter( 
        doc, ifcpath, options );

      try
      {
        importer.ReferenceIFC( doc, ifcpath, options );
        rc = true;
      }
      catch( Exception ex )
      {
        if( null != Importer.TheLog )
          Importer.TheLog.LogError( 
            -1, ex.Message, false );
      }
      finally
      {
        if( null != Importer.TheLog )
          Importer.TheLog.Close();
        if( null != IFCImportFile.TheFile )
          IFCImportFile.TheFile.Close();
      }
      return rc;
    }

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
        string path = App.Settings.IfcInputFilePath;

        if( CreateIfcLink( doc, path ) )
        {
          ifcdoc = GetLinkedInIfcDoc( app );
        }
      }

      App.Log( "Linked-in IFC document: " 
        + ifcdoc.PathName );

      RoomZoneExporter a = new RoomZoneExporter( 
        ifcdoc );

      return Result.Succeeded;
    }
  }
}
