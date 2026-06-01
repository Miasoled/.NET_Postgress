using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudProductos.Data;
using CrudProductos.Models;

namespace CrudProductos.Controllers
{
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Productos
        // public async Task<IActionResult> Index()
        // {
        //     return View(await _context.Productos
        //     .OrderByDescending(p => p.Precio)
        //     .ToListAsync());
        // }

        //Filtrar por stock menor a 5
       public async Task<IActionResult> Index()
        {
            string palabra = "ram";
            var productos = await _context.Productos
                .Where( p => EF.Functions.ILike(p.Descripcion, $"%{palabra}%") || EF.Functions.ILike(p.Nombre, $"%{palabra}%"))
                .OrderBy( p => p.Precio)
                .Take(5)
                .ToListAsync();
            return View(productos);
                //Ejercicios de practica
                // .FromSqlRaw("SELECT * FROM \"Productos\" WHERE \"Precio\" BETWEEN 500 AND 1000")
                // .Select(p => new Producto
                // .Where(p => p.Stock > 0)
                // {
                //     Id = p.Id,
                //     Nombre = p.Nombre,
                //     Precio = p.Precio,
                //     Stock = p.Stock
                // })
                // .OrderBy(p => p.Nombre)
                // .Where(p => p.Precio >= 500 && p.Precio <= 1000)
                // //trae todos los registros que quiera
                // .Take(3)
                // //tomar los siguientes tres productos
                // .Skip(3)
        }
        /*public async Task<IActionResult> Index()
        {
            string nombre = "laptop lenovo";

            var productos = await _context.Productos
                .Where(p => EF.Functions.Collate(p.Nombre, "C") == nombre)
                .ToListAsync();

            return View(productos);
        }*/
        // public async Task<IActionResult> Index()
        // {
        //     var productos = await _context.Productos
        //         .Where(p => Regex.IsMatch(p.Nombre, "^L"))
        //         .OrderBy(p => p.Nombre)
        //         .ToListAsync();

        //     return View(productos);
        // }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Stock")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.FechaRegistro = DateTime.UtcNow;

                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Stock")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Productos.Any(e => e.Id == producto.Id))
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

            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
