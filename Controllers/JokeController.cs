using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;

namespace JokePhoneAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class JokeController : ControllerBase {

    private readonly ILogger<JokeController> _logger;

    public JokeController(ILogger<JokeController> logger) {
        _logger = logger;
    }

    [HttpGet(Name = "GetJoke")]
    public async Task<Items> Get() {
        Items items = new Items();
        for (int i = 0; i < 7; i++) {
            Response joke;
            HttpClient httpClient = new HttpClient();
            string jokeAPI = "https://v2.jokeapi.dev/joke/!?blacklistFlags=nsfw,religious,political,racist,sexist,explicit";
            switch(i) {
                case 0:
                    jokeAPI = jokeAPI.Replace("!", "Any");
                    break;
                case 1:
                    jokeAPI = jokeAPI.Replace("!", "Programming");
                    break;
                case 2:
                    jokeAPI = jokeAPI.Replace("!", "Miscellaneous");
                    break;
                case 3:
                    jokeAPI = jokeAPI.Replace("!", "Dark");
                    break;
                case 4:
                    jokeAPI = jokeAPI.Replace("!", "Pun");
                    break;
                case 5:
                    jokeAPI = jokeAPI.Replace("!", "Spooky");
                    break;
                case 6:
                    jokeAPI = jokeAPI.Replace("!", "Christmas");
                    break;
            }
            string json = await httpClient.GetStringAsync(jokeAPI);
            joke = JsonConvert.DeserializeObject<Response>(json);
            if (joke == null || joke.error) continue;
            Console.WriteLine(joke.error);
            Console.WriteLine(joke.type);
            switch(joke.type) {
                case "twopart":
                    items.items.Add(new Joke() {
                        title = i == 0 ? "Any" : joke.category,
                        detail = joke.setup + "... " + joke.delivery
                    });
                    break;
                case "single":
                    items.items.Add(new Joke() {
                        title = i == 0 ? "Any" : joke.category,
                        detail = joke.joke
                    });
                    break;
            }
        }
        return items;
    }
}
