using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Core.Entities;
using MyPortfolio.Core.Interfaces;
using MyPortfolio.Infrastructure;
using MyPortfolio.Web.ViewModels;

namespace MyPortfolio.Web.Controllers
{
    public class PortfolioItemsController : Controller
    {
        private readonly IWebHostEnvironment _hosting;
        private readonly IUnitOfWork<PortfolioItem> _unitOfWork;

        public PortfolioItemsController(IWebHostEnvironment hosting, IUnitOfWork<PortfolioItem> unitOfWork)
        {
            _hosting = hosting;
            _unitOfWork = unitOfWork;
        }

        // GET: PortfolioItems
        public IActionResult Index()
        {

            return View(_unitOfWork.Entity.GetAll());
        }

        // GET: PortfolioItems/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioItem = _unitOfWork.Entity.GetById(id);
            if (portfolioItem == null)
            {
                return NotFound();
            }

            return View(portfolioItem);
        }

        // GET: PortfolioItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PortfolioItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PortfolioViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.File != null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, @"img\portfolio");
                    string fullPath = Path.Combine(uploads, viewModel.File.FileName);
                    viewModel.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                }
                PortfolioItem portfolioItem = new PortfolioItem
                {
                    Description = viewModel.Description,
                    ImageUrl = viewModel.File.FileName,
                    ProjectName = viewModel.ProjectName,
                };

                _unitOfWork.Entity.Insert(portfolioItem);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: PortfolioItems/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioItem = _unitOfWork.Entity.GetById(id);
            if (portfolioItem == null)
            {
                return NotFound();
            }

            PortfolioViewModel portfolioView = new PortfolioViewModel
            {
                Description = portfolioItem.Description,
                Id = portfolioItem.Id,
                ImageUrl = portfolioItem.ImageUrl,
                ProjectName = portfolioItem.ProjectName,
            };

            return View(portfolioView);
        }

        // POST: PortfolioItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, PortfolioViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (viewModel.File != null)
                    {
                        string uploads = Path.Combine(_hosting.WebRootPath, @"img\portfolio");
                        string fullPath = Path.Combine(uploads, viewModel.File.FileName);
                        viewModel.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                    }
                    PortfolioItem portfolioItem = new PortfolioItem
                    {
                        Id = viewModel.Id,
                        Description = viewModel.Description,
                        ImageUrl = viewModel.File.FileName,
                        ProjectName = viewModel.ProjectName,
                    };

                    _unitOfWork.Entity.Update(portfolioItem);
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortfolioItemExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: PortfolioItems/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioItem = _unitOfWork.Entity.GetById(id);
            if (portfolioItem == null)
            {
                return NotFound();
            }

            return View(portfolioItem);
        }

        // POST: PortfolioItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _unitOfWork.Entity.Delete(id);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool PortfolioItemExists(Guid id)
        {
            return _unitOfWork.Entity.GetAll().Any(e => e.Id == id);
        }
    }
}
