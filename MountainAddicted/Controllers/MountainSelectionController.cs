using MountainAddicted.Library.GCodeGenerator;
using MountainAddicted.Library;
using MountainAddicted.Library.Models;
using MountainAddicted.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapConverterToGCode.Controllers;
using System.IO;

namespace MountainAddicted.Controllers
{
    public class MountainSelectionController : Controller
    {
        // GET: MountainSelection
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(MountainViewModel mountain)
        {
            var service = new MountainService();
            var data = service.SaveMountain(mountain);
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetMountains()
        {
            var service = new MountainService();
            var data = service.GetMountains();
            return Json(data);
        }

        [HttpPost]
        public ActionResult SaveAndRequestHeight(MountainViewModel mountain)
        {
            var service = new MountainService();
            var data = service.SaveAndRequestHeight(mountain);
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetMountainData(MountainViewModel mountain)
        {
            var service = new MountainService();
            var data = service.GetMountainData(mountain.id);
            data.MountainData = null;
            data.detailsHeights = null;
            return Json(data);
        }
        public JsonResult GetMountainGCode(MountainViewModel mountain)
        {
            var service = new MountainService();
            var mountainData = new MountainData();
            var mData = service.GetMountainData(mountain.id);
            var processor = new GCodeProcessor();
            var gCode = processor.GetMountainGCode(mData.MountainData, new GCodeConfiguration
            {
                FormSizeMMHeight = 40,
                FormSizeMMLength = 70,
                FormSizeMMWidth = 70,
                CleanSpinSizeMM = 6
            });
            ExportGCodeMovesToFile(gCode.PreparationMoves, @"C:\tfs\k2_prep.txt");
            ExportGCodeMovesToFile(gCode.DirtyMoves, @"C:\tfs\k2_dirty.txt");
            ExportGCodeMovesToFile(gCode.CleanMoves, @"C:\tfs\k2_clean.txt");
            return Json("OK");

        }
        public void ExportGCodeMovesToFile(List<IGCodeMove> gCodeMoves, string destination)
        {
            using (var file = new StreamWriter(System.IO.File.Create(destination)))
            {
                foreach (var move in gCodeMoves)
                {
                    file.WriteLine(move.GCodeMoveAsString);
                }
            }
        }
    }
}