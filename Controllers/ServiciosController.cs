﻿using Microsoft.AspNetCore.Mvc;
using SpaWebApp.Models;
using SpaWebApp.Data;
using System.Linq;

namespace SpaWebApp.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly SpaContext _context;

        public ServiciosController(SpaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var comentarios = _context.Comentarios.ToList();
            return View(comentarios);
        }
    }
}
