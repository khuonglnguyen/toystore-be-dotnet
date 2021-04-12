using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    public class StatisticController : Controller
    {
        private IStatisticTypeService _statisticTypeService;
        public StatisticController(IStatisticTypeService statisticTypeService)
        {
            _statisticTypeService = statisticTypeService;
        }
        // GET: Statistic
        public ActionResult Index()
        {
            IEnumerable<StatisticType> statisticTypes = _statisticTypeService.GetStatisticTypeList();
            ViewBag.StatisticType = statisticTypes;
            return View();
        }
    }
}