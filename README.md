# IfcSpaceZoneBoundaries

Revit C# .NET add-in to retrieve IFC spaces and zones and their boundaries.

As described
by [The Building Coder](https://thebuildingcoder.typepad.com) discussing
how to [retrieve linked `IfcZone` elements using Python](https://thebuildingcoder.typepad.com/blog/2019/01/retrieving-linked-ifczone-elements-using-python.html),
IFC rooms and zones are represented by `DirectShape` elements equipped with custom shared parameters.

IfcSpaceZoneBoundaries retrieves the IFC rooms and zone elements and exports a CSV file listing them, their relationships and boundaries.

Room properties:

- IfcExportAs = IfcSpace.INTERNAL
- IfcGUID = 2QZ$T4_uPCWPddxgtStT47
- IfcName = CHA
- IfcPresentationLayer = M-AREA-____-OTLN
- IfcPropertySetList = "Pset_SpaceCommon";"BI_Parameters";"BaseQuantities"
- IfcZone = APT0102

Zone properties:

- IfcExportAs = IfcZone
- IfcGUID = 2QZ$2QZ$T4_uPCWPddxgtStT7A
- IfcName = APT0102
- IfcPresentationLayer = 
- IfcPropertySetList = "Pset_ZoneCommon";"BI_Parameters"
- IfcZone = 

CSV file format:

- Space or Zone
- GUID
- Name
- Zone
- Layer
- Property set sans quotes
- List of space separated boundary point XY coordinates in millimetres

CSV example for the room and zone properties listed above:

    S, 2QZ$T4_uPCWPddxgtStT47, CHA, APT0102, M-AREA-____-OTLN, Pset_SpaceCommon;BI_Parameters;BaseQuantities, 12 24 12 68 -4 68 -4 12
    Z, 2QZ$2QZ$T4_uPCWPddxgtStT7A, APT0102, , Pset_ZoneCommon;BI_Parameters, 11 23 11 69 -5 69 -5 11



## Author

Jeremy Tammik,
[The Building Coder](http://thebuildingcoder.typepad.com),
[Forge](http://forge.autodesk.com) [Platform](https://developer.autodesk.com) Development,
[ADN](http://www.autodesk.com/adn)
[Open](http://www.autodesk.com/adnopen),
[Autodesk Inc.](http://www.autodesk.com)


## License

This sample is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT).
Please see the [LICENSE](LICENSE) file for full details.
