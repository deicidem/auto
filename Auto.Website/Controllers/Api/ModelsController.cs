using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auto.Website.Models;
using Auto.Data;
using Auto.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Auto.Website.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase {
        private readonly IAutoDatabase db;

        public ModelsController(IAutoDatabase db) {
            this.db = db;
        }

        [HttpGet]
        public IEnumerable<Model> Get() {
            return db.ListModels();
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            var vehicleModel = db.FindModel(id);
            if (vehicleModel == default) return NotFound();
            return Ok(vehicleModel);

        }

        public static string ParseModelId(dynamic href) {
            var tokens = ((string)href).Split("/");
            return tokens.Last();
        }
    }
}
