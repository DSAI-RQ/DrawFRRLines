///This is a record of the code creation process prior to cleaning up the code.
///This is where methods in the Revit API are tested as proof of concept.
///December 31, 2020

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Load Autodesk
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
//Not sure if UI Selection needed
using Autodesk.Revit.UI.Selection;

namespace DrawFRRLines
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet element)
        {
            //Get application and document
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;

            //Get active view (THIS CAN BE REMOVED BECAUSE OF doc.ActiveView.Id in the FilteredElementCollector below)
            Autodesk.Revit.DB.View activeView = doc.ActiveView;

            //Get elements in active view
            FilteredElementCollector activeViewElements = new FilteredElementCollector(doc, doc.ActiveView.Id);

            //Filter wall elements from active view elements
            ICollection<Element> wallsView = activeViewElements.OfCategory(BuiltInCategory.OST_Walls).ToElements();

            //Get line styles in project
            Category linesCat = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
            Category l060 = linesCat.SubCategories.get_Item("FIRE-060 1 Hour");
            GraphicsStyle gs060 = l060.GetGraphicsStyle(GraphicsStyleType.Projection);

            //Location sotrage list
            IList<Location> loctest = new List<Location>();

            //Location curve storage list
            IList<LocationCurve> locCurve = new List<LocationCurve>();

            //Line storage list
            IList<Line> wallLine = new List<Line>();

            //Curve storage list
            IList<Curve> wallCurve = new List<Curve>();

            //Get location curves
            foreach (Element wallElem in wallsView)
            {
                Wall wallInst = wallElem as Wall;
                //Autodesk.Revit.DB.LocationCurve position = wallElem.Location;
                //loctest.Add(position);
                LocationCurve lcurve = wallInst.Location as LocationCurve;
                Curve curve = lcurve.Curve;
                wallCurve.Add(curve);
            }

            ////Wall storage lists
            //IList<Wall> wallNone = new List<Wall>();
            //IList<Wall> wall00 = new List<Wall>();
            //IList<Wall> wall45 = new List<Wall>();
            //IList<Wall> wall60 = new List<Wall>();
            //IList<Wall> wall90 = new List<Wall>();
            //IList<Wall> wall120 = new List<Wall>();
            //IList<Wall> wall180 = new List<Wall>();

            ////Get parameter
            //foreach (Element wallElem in wallsView)
            //{
            //    Wall wallInst = wallElem as Wall;
            //    Parameter fireRatingParam = wallInst.WallType.LookupParameter("Fire Rating");
            //    //Task dialog is for confirming that the parameter value is being extracted
            //    //TaskDialog.Show("FRR: ", fireRatingParam.AsString());
            //    String paramValue = fireRatingParam.AsString();
            //    //Sorting walls in active view
            //    if (paramValue == "00")
            //    {
            //        wall00.Add(wallInst);
            //    }
            //    if (paramValue == "45")
            //    {
            //        wall45.Add(wallInst);
            //    }
            //    if (paramValue == "60")
            //    {
            //        wall60.Add(wallInst);
            //    }
            //    if (paramValue == "90")
            //    {
            //        wall90.Add(wallInst);
            //    }
            //    if (paramValue == "120")
            //    {
            //        wall120.Add(wallInst);
            //    }
            //    if (paramValue == "180")
            //    {
            //        wall180.Add(wallInst);
            //    }
            //    else
            //    {
            //        wallNone.Add(wallInst);
            //    }
            //}

            //Transaction Reference
            Transaction trans = new Transaction(doc);
            trans.Start("Draw FRR Lines on Active View");
            foreach (Curve i in wallCurve)
            {
                //Create new line
                DetailCurve frrLine = doc.Create.NewDetailCurve(activeView, i);

                //Get Lline style parameter
                //Parameter lineStyle = frrLine.LookupParameter("Line Style");

                //Set line style
                frrLine.LineStyle = gs060;
            }
            trans.Commit();

            return Result.Succeeded;
        }
    }
}
