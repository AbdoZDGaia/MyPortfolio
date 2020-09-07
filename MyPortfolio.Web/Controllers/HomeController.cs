using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Core.Entities;
using MyPortfolio.Core.Interfaces;
using MyPortfolio.Web.ViewModels;

namespace MyPortfolio.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork<Owner> _owner;
        private readonly IUnitOfWork<PortfolioItem> _portfolio;

        public HomeController(IUnitOfWork<Owner> owner, IUnitOfWork<PortfolioItem> portfolio)
        {
            _owner = owner;
            _portfolio = portfolio;
        }

        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                Owner = _owner.Entity.GetAll().First(),
                PortfolioItems = _portfolio.Entity.GetAll().ToList()
            };

            return View(homeViewModel);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
