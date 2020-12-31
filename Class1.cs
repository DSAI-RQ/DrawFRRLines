using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Load Autodesk
//using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
//using Autodesk.Revit.UI.Selection;

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

            //Get active view
            View activeView = doc.ActiveView;

            //Get elements in active view
            FilteredElementCollector activeViewElements = new FilteredElementCollector(doc, doc.ActiveView.Id);

            //Filter wall elements from active view elements
            ICollection<Element> wallsView = activeViewElements.OfCategory(BuiltInCategory.OST_Walls).ToElements();

            //Wall storage lists
            IList<Wall> wallNone = new List<Wall>();
            IList<Wall> wall00 = new List<Wall>();
            IList<Wall> wall45 = new List<Wall>();
            IList<Wall> wall60 = new List<Wall>();
            IList<Wall> wall90 = new List<Wall>();
            IList<Wall> wall120 = new List<Wall>();
            IList<Wall> wall180 = new List<Wall>();

            //Get parameter
            foreach (Element wallElem in wallsView)
            {
                Wall wallInst = wallElem as Wall;
                Parameter fireRatingParam = wallInst.WallType.LookupParameter("Fire Rating");
                String paramValue = fireRatingParam.AsString();
                //Sorting walls in active view
                if (paramValue == "00")
                {
                    wall00.Add(wallInst);
                }
                if (paramValue == "45")
                {
                    wall45.Add(wallInst);
                }
                if (paramValue == "60")
                {
                    wall60.Add(wallInst);
                }
                if (paramValue == "90")
                {
                    wall90.Add(wallInst);
                }
                if (paramValue == "120")
                {
                    wall120.Add(wallInst);
                }
                if (paramValue == "180")
                {
                    wall180.Add(wallInst);
                }
                else
                {
                    wallNone.Add(wallInst);
                }
            }

            //Get line styles in project
            Category linesCat = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

            ///Getting the Category and GraphicsStyle can probably be turned into a method

            //FRR NON-RATED
            Category l000 = linesCat.SubCategories.get_Item("FIRE-000 Non-Rated Fire Separation");
            GraphicsStyle gs000 = l000.GetGraphicsStyle(GraphicsStyleType.Projection);

            //FRR 45 MIN
            Category l045 = linesCat.SubCategories.get_Item("FIRE-045 0.75 Hour");
            GraphicsStyle gs045 = l045.GetGraphicsStyle(GraphicsStyleType.Projection);

            //FRR 1H
            Category l060 = linesCat.SubCategories.get_Item("FIRE-060 1 Hour");
            GraphicsStyle gs060 = l060.GetGraphicsStyle(GraphicsStyleType.Projection);

            //FRR 1.5H
            Category l090 = linesCat.SubCategories.get_Item("FIRE-090 1.5 Hour");
            GraphicsStyle gs090 = l090.GetGraphicsStyle(GraphicsStyleType.Projection);

            //FRR 2H
            Category l120 = linesCat.SubCategories.get_Item("FIRE-120 2 Hour");
            GraphicsStyle gs120 = l120.GetGraphicsStyle(GraphicsStyleType.Projection);

            //FRR 3H
            Category l180 = linesCat.SubCategories.get_Item("FIRE-180 3 Hour");
            GraphicsStyle gs180 = l180.GetGraphicsStyle(GraphicsStyleType.Projection);

            //Get location curves
            foreach (Element wallElem in wallsView)
            {
                Wall wallInst = wallElem as Wall;
                LocationCurve lcurve = wallInst.Location as LocationCurve;
                Curve curve = lcurve.Curve;
                wallCurve.Add(curve);
            }

            //Transaction Reference
            Transaction trans = new Transaction(doc);
            trans.Start("Draw FRR Lines on Active View");
            foreach (Curve i in wallCurve)
            {
                //Create new line
                DetailCurve frrLine = doc.Create.NewDetailCurve(activeView, i);

                //Set line style
                frrLine.LineStyle = gs060;
            }
            trans.Commit();

            return Result.Succeeded;
        }
    }
}
