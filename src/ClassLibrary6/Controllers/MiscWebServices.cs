using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Nop.Plugin.Api.Topic.Controllers
{
    public class MiscWebServices : Controller
    {
        public ActionResult Configure()
        {
            return Content("worked");
        }
    }
}
