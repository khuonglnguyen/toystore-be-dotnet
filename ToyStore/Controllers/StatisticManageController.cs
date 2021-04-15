using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    public class StatisticManageController : Controller
    {
        private IStatisticTypeService _statisticTypeService;
        private IOrderService _orderService;
        public StatisticManageController(IStatisticTypeService statisticTypeService, IOrderService orderService)
        {
            _statisticTypeService = statisticTypeService;
            _orderService = orderService;
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