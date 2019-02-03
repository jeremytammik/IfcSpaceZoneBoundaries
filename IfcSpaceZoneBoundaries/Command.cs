#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.IFC.Import;
using Revit.IFC.Import.Data;
using IfcSpaceZoneBoundaries.Exporter;
#endregion

namespace IfcSpaceZoneBoundaries.Addin
{
  [Transaction( TransactionMode.Manual )]
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
    /// Retrieve and return all linked-in IFC documents.
    /// </summary>
    List<Document> GetLinkedInIfcDocs( Application app )
    {
      List<Document> ifcdocs = null;
      DocumentSet docs = app.Documents;
      int n = docs.Size;

      JtLogger.Log( string.Format( "{0} open document{1}",
        n, Util.PluralSuffix( n ) ) );

      foreach( Document d in docs )
      {
        string s = d.PathName;
        if( s.EndsWith( ".ifc.RVT" ) )
        {
          if( null == ifcdocs )
          {
            ifcdocs = new List<Document>();
          }
          ifcdocs.Add( d );
        }
      }
      return ifcdocs;
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

      // Retrieve all linked-in IFC documents

      List<Document> ifcdocs = GetLinkedInIfcDocs( app );

      if( null == ifcdocs || 0 == ifcdocs.Count )
      {
        // If no IFC links are present, create one

        string path = JtSettings.Instance.IfcInputFilePath;

        if( CreateIfcLink( doc, path ) )
        {
          ifcdocs = GetLinkedInIfcDocs( app );
        }
      }

      int n = ifcdocs.Count;

      JtLogger.Log( string.Format(
        "{0} linked-in IFC document{1} found.",
        n, Util.PluralSuffix( n ) ) );

      foreach( Document ifcdoc in ifcdocs )
      {
        JtLogger.Log( "Linked-in IFC document: "
          + ifcdoc.PathName );

        RoomZoneExporter.Export( ifcdoc );
      }
      return ( 0 < n )
        ? Result.Succeeded
        : Result.Failed;
    }
  }
}
