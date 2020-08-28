using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSPA
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class MenuController:ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public ActionResult GetItems()
        {
            List<string> menus = new List<string>();
            menus.Add("Menu 1");
            menus.Add("Menu 2");
            return Ok(menus);
        }
    }
}