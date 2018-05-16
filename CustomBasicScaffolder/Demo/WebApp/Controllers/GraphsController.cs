using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PublicPara.CodeText.Data;
using WebApp.Models;

namespace WebApp.Controllers
{ 

    public class GraphsController : Controller
    {
        // GET: graphs/flot
  
        public ActionResult FlotChart() => View();

        // GET: graphs/morris
     
        public ActionResult MorrisCharts() => View();

        // GET: graphs/sparkline-charts
     
        public ActionResult Sparklines() => View();

        // GET: graphs/easypie-charts
    
        public ActionResult EasyPieCharts() => View();

        // GET: graphs/dygraphs
        public ActionResult Dygraphs() => View();

        // GET: graphs/chart-js

        public ActionResult ChartJS() => View();

        // GET: graphs/high-charts
       
        public ActionResult HighChartTable() => View();

        public ActionResult DashboardMarketing() => View();
    }
}
