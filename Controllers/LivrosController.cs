using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using consultaLivrosService.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace consultaLivrosService.Controllers
{
    [Route("api/[controller]")]
    public class LivrosController : Controller
    {
        private static string BASE_AUTHENTICATION_URL = "http://localhost:9001/";
        private static IDictionary<int, Livro> livros = new Dictionary<int, Livro>();

        static LivrosController() {
            livros.Add(1, new Livro(1, "A General Gazetteer.", "Richard BROOKES ", 20.0));
            livros.Add(2, new Livro(2, "The Aztec Templo Mayor: A SymZium at Dumbarton Oaks", "Elizabeth Hill Boone", 35.50));
            livros.Add(3, new Livro(3, "Pokemon: Deluxe Essential Handbook", "‎Cris Silvestri", 50.0));
        }

        // GET api/livros
        [HttpGet]
        public IActionResult Get()
        {
            if (!isAuthenticated().Result) {
                // return Forbid(); TODO: Verificar: retorna erro no servidor
                return Unauthorized();
            }
            return Ok(livros.Values.ToList());
        }

        // GET api/livros/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!isAuthenticated().Result) {
                // return Forbid(); TODO: Verificar: retorna erro no servidor
                return Unauthorized();
            }

            Livro l = null;
            livros.TryGetValue(id, out l);
            if (l == null) {
                return NotFound("NotFound - O livro não existe.");
            }
            return Ok(l);
        }

        
        private async Task<bool> isAuthenticated() {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_AUTHENTICATION_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response =  client.GetAsync("api/isauthenticated").Result;
                string result = await response.Content.ReadAsStringAsync();

                return result.Equals("true");
            }
        }

    }
}
