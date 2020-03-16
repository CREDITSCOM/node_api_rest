using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CS.WebPublic.Controllers
{
   
    [ApiController]
    public abstract class AbstractApiController : ControllerBase
    {
        public IServiceProvider ServiceProvider { get; }
        public AbstractApiController(IServiceProvider provider) : base()
        {
            ServiceProvider = provider;
        }
    }
}