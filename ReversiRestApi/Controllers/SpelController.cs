using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReversiRestApi.Model;

namespace ReversiRestApi.Controllers
{

    [ApiController]
    [Route("api/Spel")]
    public class SpelController : ControllerBase
    {
        private readonly ISpelRepository iRepository;

        public SpelController(ISpelRepository repository)
        {
            iRepository = repository;
        }
        // GET api/spel
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler()
        {
            //Check if player2 token is set
            return new ObjectResult(
                (from value
                 in iRepository.GetSpellen()
                 where string.IsNullOrWhiteSpace(value.Speler2Token)
                 select new { value.Omschrijving, value.Speler1Token, value.Token})
                .ToList());
        }

        // GET api/spel
        [HttpGet("{spelToken}")]
        public ActionResult<Spel> GetSpel(string spelToken)
        {
            var spel = iRepository.GetSpel(spelToken);                
            return new ObjectResult(new SpelTbvJson(spel));
        }

        // GET api/spel
        [HttpGet("AfgelopenSpellen")]
        public ActionResult<List<SpelTbvJson>> GetAfgelopenSpellen()
        {
            var spellen = iRepository.GetSpellen();
            List<SpelTbvJson> afgelopenSpellen = new List<SpelTbvJson>();
            foreach (var spel in spellen)
            {
                if (spel.Afgelopen)
                {
                    afgelopenSpellen.Add(new SpelTbvJson(spel));
                }              
            }
            if (afgelopenSpellen != null)
            {
                return new ObjectResult(afgelopenSpellen);
            }
            return null;
        }

        [HttpGet("Speler/{spelertoken}")]
        public ActionResult<Spel> GetGamePlayer(string spelertoken)
        {
            if (!string.IsNullOrWhiteSpace(spelertoken))
            {
                var spel = iRepository.GetSpellen().Where(spel => (spel.Speler1Token == spelertoken || spel.Speler2Token == spelertoken) &&  spel.Afgelopen == false).FirstOrDefault();

                if (spel != null)
                {
                    return new ObjectResult(new SpelTbvJson(spel));
                }
            }
            return null;
        }

        [HttpGet("PieceHistory/{speltoken}")]
        public ActionResult<List<int>> GetPieceHistory(string speltoken)
        {
            if (!string.IsNullOrWhiteSpace(speltoken))
            {
                List<List<int>> spelHistory = new List<List<int>>();

                var spel = iRepository.GetSpel(speltoken);

                if (spel == null)
                {
                    return null;
                }
                if (spel.Speler2Token != "")
                {
                    var speler1History = iRepository.GetPieceHistory(spel.Token, spel.Speler1Token);
                    var speler2History = iRepository.GetPieceHistory(spel.Token, spel.Speler2Token);
                    spelHistory.Add(speler1History);
                    spelHistory.Add(speler2History);
                    return new ObjectResult(JsonConvert.SerializeObject(spelHistory));
                }
            }
            return null;

        }

        // Put api/Spel/Zet
        [HttpPost("Zet")]
        public void PlacePiece([FromBody] [Bind("X,Y,SpelerToken,SpelToken,Pas")] PlaatsFiche data)
        {
            var spel = iRepository.GetSpel(data.SpelToken);
            if (!spel.Afgelopen)
            {
                if (spel.CheckAandeBeurt(data.SpelerToken))
                {
                    spel.UpdateNieuweFiches();
                    if (!data.Pas)
                    {
                        if (spel.DoeZet(data.Y, data.X))
                        {
                            if (spel.SpelAfgelopen())
                            {
                                spel.Afgelopen = true;
                                spel.WinnaarSpel(spel.OverwegendeKleur());                            
                            }
                            iRepository.AddPieceHistoryteSpel(spel.Token, spel.SpelerPieces(Kleur.Wit), spel.SpelerPieces(Kleur.Zwart));
                            iRepository.UpdateSpel(spel);
                        }
                    }
                    else
                    {
                        iRepository.AddPieceHistoryteSpel(spel.Token, spel.SpelerPieces(Kleur.Wit), spel.SpelerPieces(Kleur.Zwart));
                        spel.Pas();
                        iRepository.UpdateSpel(spel);
                    }
                }
            }
        }

        // Put api/Spel/Opgeven
        [HttpPost("Opgeven")]
        public void Surrender([FromBody] [Bind("SpelerToken,SpelToken")] Surrender data)
        {
            var spel = iRepository.GetSpel(data.SpelToken);
            if (!spel.Afgelopen)
            {
                spel.Surrender(data.SpelerToken);
                iRepository.OpgevenSpel(spel);
            }
        }

        [HttpPut("SpelerToevoegen")]
        public void SpelerToevoegen([FromBody] JoinGame data)
        {
            iRepository.JoinSpel(data);
        }

        // POST api/CreateGame
        [HttpPost]
        public ObjectResult CreateGame([FromBody] CreateGame data)
        {
            Spel NieuwSpel = new Spel
            {
                Speler1Token = data.Speler1Token,
                Omschrijving = data.Omschrijving,
                Token = Guid.NewGuid().ToString(),
                Afgelopen = false
            };

            iRepository.AddSpel(NieuwSpel);
            return StatusCode(201, NieuwSpel.Token);
        }

        [HttpDelete("{Id}")]
        public void SpelSpelerVerwijderen(string Id)
        {
            List<Spel> spellen;
            spellen = iRepository.GetSpellen();

            if (spellen != null)
            {
                foreach (var item in spellen)
                {
                    if (item.Speler1Token.ToLower().Equals(Id.ToLower()) || item.Speler2Token.ToLower().Equals(Id.ToLower()))
                    {
                        iRepository.DeleteSpel(item.Token);
                    }

                }
            }
        }

        [HttpDelete("Afgelopen/{Id}")]
        public void SpelVerwijderen(string spelToken)
        {
            var spel = iRepository.GetSpel(spelToken);

            if (spel != null)
            {
                iRepository.DeleteSpel(spel.Token);
            }
        }
    }
}