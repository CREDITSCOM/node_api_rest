using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS.Db.Context;
using Microsoft.AspNetCore.Mvc;

namespace CS.WebApi.Controllers
{
    public class StatisticsController : Controller
    {
        private AppDbContext _appDbCoontext;

        public StatisticsController(AppDbContext appDbContext)
        {
            _appDbCoontext = appDbContext;
        }


        [Route("stats")]
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index", _appDbCoontext.Statistics.ToList());
        }

        [Route("stats/{any:alpha}")]
        [HttpGet]
        public IActionResult Action()
        {
            string key = (string) ControllerContext.RouteData.Values["any"];
            var item = _appDbCoontext.Statistics.Find(key);
            return View("Item", item);
        }
    }
}