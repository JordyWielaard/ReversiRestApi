using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{gameToken}")]
        public ActionResult<Spel> GetGame(string gameToken)
        {
            var spel = iRepository.GetSpel(gameToken);                
            return new ObjectResult(new SpelTbvJson(spel));
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

        // GET api/Beurt/<speltoken>
        [HttpGet("Beurt/{speltoken}")]
        public ActionResult<Kleur> GetGameTurn(string speltoken)
        {
            if (!string.IsNullOrWhiteSpace(speltoken))
            {
                var spel = iRepository.GetSpel(speltoken);
                return new ObjectResult(spel.AandeBeurt);
            }
            return null;

        }

        // Put api/Spel/Zet
        [HttpPut("Zet")]
        public ActionResult<bool> PlacePiece([FromBody] PlacePiece data)
        {
            var game = (from value in iRepository.GetSpellen()
                        where value.Token.Equals(data.gameToken)
                        select value).First();

            //Check if the playerToken is on its turn
            if (game.GetPlayerColour(data.playerToken) != game.AandeBeurt)
            {
                return StatusCode(403);
            }

            //Check if player wants to pass its turn
            if (!data.pass)
            {
                return game.DoeZet(data.y, data.x);
            }
            else
            {
                return game.Pas();
            }

        }

        // Put api/Spel/Opgeven
        [HttpPut("Opgeven")]
        public ActionResult<bool> Surrender([FromBody] Surrender data)
        {
            var game = (from value in iRepository.GetSpellen()
                        where value.Token.Equals(data.gameToken)
                        select value).First();

            return game.Surrender(data.playerToken);
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
        public void SpelVerwijderen(string Id)
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
    }
}