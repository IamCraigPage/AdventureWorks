using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreScaffoldexample.AdventureWorksEntities;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AdventureWorksLT2019Context _context;

        public ProductsController() //AdventureWorksLT2019Context context) // TO DO, why is my dependency injection not working?
        {
            //_context = context;
            _context = new AdventureWorksLT2019Context();
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<List<System.Dynamic.ExpandoObject>>> GetProducts()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }

            return Models.ProductsLogic.GetProducts(_context,"","");
        }

        // GET: api/Products - http://localhost:5058/api/products/18/the or http://localhost:5058/api/products/null/null
        [HttpGet("{categoryIDs}/{descriptionFilter}")]
        public async Task<ActionResult<List<System.Dynamic.ExpandoObject>>> GetProducts(string categoryIDs = "", string descriptionFilter = "")
        {
            if (_context.Products == null)
            {
                return NotFound();
            }

            if (categoryIDs == "null")
                categoryIDs = "";
            if (descriptionFilter == "null")
                descriptionFilter = "";

            return Models.ProductsLogic.GetProducts(_context, descriptionFilter, categoryIDs);
        }

        // GET: api/Products - http://localhost:5058/api/products/null/null/3/10/false
        [HttpGet("{categoryIDs}/{descriptionFilter}/{pageNumber}/{perPage}/{sortAscending}")]
        public async Task<ActionResult<List<System.Dynamic.ExpandoObject>>> GetProducts(string categoryIDs, string descriptionFilter, int pageNumber, int perPage, bool sortAcsending)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }

            if (categoryIDs == "null")
                categoryIDs = "";
            if (descriptionFilter == "null")
                descriptionFilter = "";

            var products = Models.ProductsLogic.GetProducts(_context, descriptionFilter, categoryIDs);

            // Sort them, they're expando objects so have to use IDictionary...
            if (sortAcsending)
                products = products.OrderBy(x => ((IDictionary<string, object>)x)["name"]).ToList();
            else
                products = products.OrderByDescending(x => ((IDictionary<string, object>)x)["name"]).ToList();

            // get paginated!!
            products = products.Skip((pageNumber - 1) * perPage).Take(perPage).ToList();

            return products;
        }
    }
}
