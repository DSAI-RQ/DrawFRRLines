using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;

using System.Reflection;
using System.Windows.Media.Imaging;

namespace DrawFRRLines
{

    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            RibbonPanel panel = ribbonPanel(a);

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButton button = panel.AddItem(new PushButtonData("FRR Lines", "Draw FRR Lines", thisAssemblyPath, "DrawFRRLines.Command")) as PushButton;

            button.ToolTip = "This will draw Fire Resistance Rating Lines in the active view.";
            var globePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "FRR Lines.png");

            Uri uriImage = new Uri(globePath);
            BitmapImage largeImage = new BitmapImage();
            button.LargeImage = largeImage;

            return Result.Succeeded;
        }

        //Start here
        public RibbonPanel ribbonPanel(UIControlledApplication a)
        {
            string tab = "DSAI";
            string pnam = "DSAI Tools";
            RibbonPanel ribbonPanel = null;
            try
            {
                a.CreateRibbonTab(tab);
            }
            catch { }

            try
            {
                RibbonPanel panel = a.CreateRibbonPanel(tab, pnam);
            }
            catch { }

            List<RibbonPanel> panels = a.GetRibbonPanels(tab);

            foreach (RibbonPanel p in panels)
            {
                if (p.Name == pnam)
                {
                    ribbonPanel = p;
                }
            }
            return ribbonPanel;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
