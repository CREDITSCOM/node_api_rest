using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CS.WebApi.Models;
using CS.Db.Context;
using Microsoft.Extensions.DependencyInjection;
using CS.Db.Models.Application;
using System.IO;
using System.Text;

namespace CS.WebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IServiceProvider ServiceProvider { get; }

        public HomeController(ILogger<HomeController> logger, IServiceProvider provider)
        {
            _logger = logger;
            ServiceProvider = provider;
        }


        public IActionResult Index(string extra_info)
        {
            //using (var context = ServiceProvider.GetService<AppDbContext>())
            //{
            //    //    var t = context.RestUser.FirstOrDefault(p => p.ID == 1);
            //    var entity = new SystemLogEntity();
            //    entity.DateCreate = DateTime.Now;
            //    entity.UrlRequest =  // HttpContext.Request.QueryString.Value;

            //    context.SystemLog.Add(entity);
            //    context.SaveChanges();
            //}

            return View();
        }

        [HttpGet]
        public IActionResult suppliment()
        {
            return View();
        }

        [HttpGet]
        public IActionResult circulation()
        {
            return View();
        }

        [HttpGet]
        public async Task<string> Postback()
        {
            await ReadQueryParams(isGet: true);

            return "";
        }

        [HttpPost]
        public async Task<string> Postback(string extra_info)
        {
            await ReadQueryParams(isGet: false);

            return "";
        }


        private async Task ReadQueryParams(bool isGet)
        {
            StringBuilder buildStr = new StringBuilder();


            if (isGet)
            {
                buildStr.Append($"URL [GET]: {Request.Host}?");
            }
            else
            {
                buildStr.Append($"URL [POST]: {Request.Host}?");
            }

            using (var context = ServiceProvider.GetService<AppDbContext>())
            {
                var entity = new SystemLogEntity();

                foreach (var item in Request.Query)
                {
                    buildStr.Append($"{item.Key}={item.Value}&");
                }

                if (!isGet)
                    using (var reader = new StreamReader(HttpContext.Request.Body))
                    {

                        var readRes = await reader.ReadToEndAsync();
                        buildStr.Append($"; [BODY]: {readRes}");

                    }

                entity.DateCreate = DateTime.Now;
                entity.UrlRequest = buildStr.ToString();
                context.SystemLog.Add(entity);
                context.SaveChanges();
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
