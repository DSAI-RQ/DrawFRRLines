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
            IList<Wall> wall000 = new List<Wall>();
            IList<Wall> wall045 = new List<Wall>();
            IList<Wall> wall060 = new List<Wall>();
            IList<Wall> wall090 = new List<Wall>();
            IList<Wall> wall120 = new List<Wall>();
            IList<Wall> wall180 = new List<Wall>();

            //Sorting walls by parameter value
            foreach (Element wallElem in wallsView)
            {
                Wall wallInst = wallElem as Wall;
                Parameter fireRatingParam = wallInst.WallType.LookupParameter("Fire Rating");
                String paramValue = fireRatingParam.AsString();
                if (paramValue == "00")
                {
                    wall000.Add(wallInst);
                }
                if (paramValue == "45")
                {
                    wall045.Add(wallInst);
                }
                if (paramValue == "60")
                {
                    wall060.Add(wallInst);
                }
                if (paramValue == "90")
                {
                    wall090.Add(wallInst);
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

            ///Getting the Category and GraphicsStyle can probably be turned into a function

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

            //Curve storage list
            IList<Curve> wallCurve000 = new List<Curve>();
            IList<Curve> wallCurve045 = new List<Curve>();
            IList<Curve> wallCurve060 = new List<Curve>();
            IList<Curve> wallCurve090 = new List<Curve>();
            IList<Curve> wallCurve120 = new List<Curve>();
            IList<Curve> wallCurve180 = new List<Curve>();

            ///Getting location curves can probably be turned into a function
            //FRR NON-RATED: Get location curves
            foreach (Element wallElem in wall000)
            {
                Wall wallInst = wallElem as Wall;
                LocationCurve lcurve = wallInst.Location as LocationCurve;
                Curve curve = lcurve.Curve;
                wallCurve000.Add(curve);
            }

            //FRR 45 MIN: Get location curves
            foreach (Element wallElem in wall045)
            {
                Wall wallInst = wallElem as Wall;
                LocationCurve lcurve = wallInst.Location as LocationCurve;
                Curve curve = lcurve.Curve;
                wallCurve045.Add(curve);
            }

            //FRR 1H: Get location curves
            foreach (Element wallElem in wall060)
            {
                Wall wallInst = wallElem as Wall;
                LocationCurve lcurve = wallInst.Location as LocationCurve;
                Curve curve = lcurve.Curve;
                wallCurve060.Add(curve);
            }

            //FRR 1.5H: Get location curves
            foreach (Element wallElem in wall090)
            {
                Wall wallInst = wallElem as Wall;
                LocationCurve lcurve = wallInst.Location as LocationCurve;
                Curve curve = lcurve.Curve;
                wallCurve090.Add(curve);
            }

            //FRR 2H: Get location curves
            foreach (Element wallElem in wall120)
            {
                Wall wallInst = wallElem as Wall;
                LocationCurve lcurve = wallInst.Location as LocationCurve;
                Curve curve = lcurve.Curve;
                wallCurve120.Add(curve);
            }

            //FRR 3H: Get location curves
            foreach (Element wallElem in wall180)
            {
                Wall wallInst = wallElem as Wall;
                LocationCurve lcurve = wallInst.Location as LocationCurve;
                Curve curve = lcurve.Curve;
                wallCurve180.Add(curve);
            }

            //Transaction Reference
            Transaction trans = new Transaction(doc);
            trans.Start("Draw FRR Lines on Active View");
            
            ///Create new lines for each curve list
            //FRR NON-RATED
            foreach (Curve curve in wallCurve000)
            {
                //Create new line
                DetailCurve frrLine = doc.Create.NewDetailCurve(activeView, curve);

                //Set line style
                frrLine.LineStyle = gs000;
            }

            //FRR 45 MIN
            foreach (Curve curve in wallCurve045)
            {
                DetailCurve frrLine = doc.Create.NewDetailCurve(activeView, curve);
                frrLine.LineStyle = gs045;
            }

            //FRR 1H
            foreach (Curve curve in wallCurve060)
            {
                DetailCurve frrLine = doc.Create.NewDetailCurve(activeView, curve);
                frrLine.LineStyle = gs060;
            }

            //FRR 1.5H
            foreach (Curve curve in wallCurve090)
            {
                DetailCurve frrLine = doc.Create.NewDetailCurve(activeView, curve);
                frrLine.LineStyle = gs090;
            }

            //FRR 2H
            foreach (Curve curve in wallCurve120)
            {
                DetailCurve frrLine = doc.Create.NewDetailCurve(activeView, curve);
                frrLine.LineStyle = gs120;
            }

            //FRR 3H
            foreach (Curve curve in wallCurve180)
            {
                DetailCurve frrLine = doc.Create.NewDetailCurve(activeView, curve);
                frrLine.LineStyle = gs120;
            }

            trans.Commit();

            return Result.Succeeded;
        }
    }
}
