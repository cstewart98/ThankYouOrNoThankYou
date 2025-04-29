using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ThankYouOrNoThankYou.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ThankYouOrNoThankYou.Controllers;

public class TYONTYController : Controller
{

    private List<string> validDogNames = new List<string>(){"Fido", "Maxwell", "Lucky", "Daisy", "Buddy", "Ralph", "Scotty", "Maggie", "Cutie", "Rex", "Spot", "Coco", "Rover", "Spudnik", "Lassie", "Shadow", "Marco", "Sparkie", "Missy", "Little-Bit", "Bear", "Spike", "Junior", "Alpha", "Astro", "Ember"};
    public List<int> prizeValue = new List<int>() { 1, 10, 25, 100, 300, 500, 600, 800, 2500, 1000, 1200, 1500, 4000, 5000, 7000, 8000, 14000, 17000, 20000, 25000, 40000, 45000, 50000, 100000, 500000, 600000 };
    private MyContext db;
    public TYONTYController(MyContext context)
    {
        db = context;
    }
    [HttpGet("")]
    public IActionResult Index()
    {
        HttpContext.Session.Clear();
        return View("Index");

    }

    [HttpPost("/tyonty/contestant")]
    public IActionResult NewPlayer(Contestant newContestant)
    {
        if (!ModelState.IsValid) //If you don't insert a name
        {
            return View("Index");
        }

        // If you do enter a name
        HttpContext.Session.SetInt32("UUID", newContestant.ContestantId);
        HttpContext.Session.SetString("Name", newContestant.Name);

        // Setting up the prizes
        // Setting the List of prizes
        List<string> Prizes = new List<string>() { "Gas for a Year", "Dollar", "1 Acre of the Moon", "$25 iTunes Card", "$100 Amazon Card", "Flat Screen TV", "PS5", "5-Day Aspen Ski Trip", "Tour of NASA", "Visit Every MLB Stadium", "$1k", "Bahama Cruise for 4", "New iPhone", "Grand Piano", "Guadeloupe Shark Diving Trip for 2", "A Week at Disney World for 4", "Super Bowl Ticket", "Harley Davidson", "$20k College Scholarship", "Swimming Pool", "Rolex", "RV", "$50k", "$100k", "$500k", "Lamborghini" };
 

        // Setting up sessions for the prizes that'll hold their price vaule 
        HttpContext.Session.SetInt32("Prize1", 1);
        HttpContext.Session.SetInt32("Prize2", 10);
        HttpContext.Session.SetInt32("Prize3", 25);
        HttpContext.Session.SetInt32("Prize4", 100);
        HttpContext.Session.SetInt32("Prize5", 300);
        HttpContext.Session.SetInt32("Prize6", 500);
        HttpContext.Session.SetInt32("Prize7", 600);
        HttpContext.Session.SetInt32("Prize8", 800);
        HttpContext.Session.SetInt32("Prize9", 2500);
        HttpContext.Session.SetInt32("Prize10", 1000);
        HttpContext.Session.SetInt32("Prize11", 1200);
        HttpContext.Session.SetInt32("Prize12", 1500);
        HttpContext.Session.SetInt32("Prize13", 4000);
        HttpContext.Session.SetInt32("Prize14", 5000);
        HttpContext.Session.SetInt32("Prize15", 7000);
        HttpContext.Session.SetInt32("Prize16", 8000);
        HttpContext.Session.SetInt32("Prize17", 14000);
        HttpContext.Session.SetInt32("Prize18", 17000);
        HttpContext.Session.SetInt32("Prize19", 20000);
        HttpContext.Session.SetInt32("Prize20", 25000);
        HttpContext.Session.SetInt32("Prize21", 40000);
        HttpContext.Session.SetInt32("Prize22", 45000);
        HttpContext.Session.SetInt32("Prize23", 50000);
        HttpContext.Session.SetInt32("Prize24", 100000);
        HttpContext.Session.SetInt32("Prize25", 500000);
        HttpContext.Session.SetInt32("Prize26", 600000);

        // Shuffle the prizes list
        var rnd = new Random();
        Prizes = Prizes.OrderBy(c => rnd.Next()).ToList();

        // Distribute prizes among "dogs"
        for (var x = 0; x <= 25; x++)
        {
            HttpContext.Session.SetString(validDogNames[x], Prizes[0]);
            Prizes.RemoveAt(0);
        }

        return RedirectToAction("Intro");
    }

    [SessionCheck]
    [HttpGet("/tyonty/intro")]
    public IActionResult Intro()
    {
        HttpContext.Session.SetInt32("dogsLeft", 26);
        return View("Intro");
    }

[SessionCheck]
[HttpGet]
[Route("tyonty/pick/{dogName}")]
public ActionResult Pick(string dogName)
{

    if (validDogNames.Contains(dogName))
    {
        HttpContext.Session.SetString("yourDog", dogName);

        // setting up yourPrize session 
        string yourPrize = HttpContext.Session.GetString($"{dogName}");
        HttpContext.Session.SetString("yourPrize", yourPrize);
        HttpContext.Session.SetString($"{dogName}", "Pick");
        prizeValue.RemoveAt(0);

        return RedirectToAction("Game");
    }
    else
    {
        return RedirectToAction("Intro");
    }
}

[SessionCheck]
[HttpGet]
[Route("tyonty/{pickedDog}/drop")]
public IActionResult Drop(string pickedDog)
{
    if(validDogNames.Contains(pickedDog))
    {
        HttpContext.Session.SetString("dropIt", pickedDog); // Set up dog's name that you picked to drop
        string showPrize = HttpContext.Session.GetString($"{pickedDog}"); // This'll retrieve the not your prize
        HttpContext.Session.SetString("revealPrize", showPrize);
        
        HttpContext.Session.SetString(pickedDog, "Pick"); // This'll set the dog to "Pick" and will eliminate from future pickings 
         return View("DropIt");
    }
    else
    {
        return View("Game");
    }
}

[SessionCheck]
[HttpGet]
[Route("/tyonty/eliminate")]
public IActionResult Eliminate()
{
    int minusOneDog = HttpContext.Session.GetInt32("dogsLeft") ?? 0;
    minusOneDog -= 1; // Change the dogsLeft count by one
    HttpContext.Session.SetInt32("dogsLeft", minusOneDog); // update remaining dog count 

    // Setting the prize bar to 0
    string noMore = HttpContext.Session.GetString("revealPrize");
    if(noMore == "Dollar")
    {
        HttpContext.Session.SetInt32("Prize1", 0);
    }
    if(noMore == "1 Acre of the Moon")
    {
        HttpContext.Session.SetInt32("Prize2", 0);
    }
    if(noMore == "$25 iTunes Card")
    {
        HttpContext.Session.SetInt32("Prize3", 0);
    }
    if(noMore == "$100 Amazon Card")
    {
        HttpContext.Session.SetInt32("Prize4", 0);
    }
    if(noMore == "Flat Screen TV")
    {
        HttpContext.Session.SetInt32("Prize5", 0);
    }
    if(noMore == "PS5")
    {
        HttpContext.Session.SetInt32("Prize6", 0);
    }
    if(noMore == "5-Day Aspen Ski Trip")
    {
        HttpContext.Session.SetInt32("Prize7", 0);
    }
    if(noMore == "Tour of NASA")
    {
        HttpContext.Session.SetInt32("Prize8", 0);
    }
    if(noMore == "Visit Every MLB Stadium")
    {
        HttpContext.Session.SetInt32("Prize9", 0);
    }
    if(noMore == "$1k")
    {
        HttpContext.Session.SetInt32("Prize10", 0);
    }
    if(noMore == "Bahama Cruise for 4")
    {
        HttpContext.Session.SetInt32("Prize11", 0);
    }
    if(noMore == "New iPhone")
    {
        HttpContext.Session.SetInt32("Prize12", 0);
    }
    if(noMore == "Gas for a Year")
    {
        HttpContext.Session.SetInt32("Prize13", 0);
    }
    if(noMore == "Grand Piano")
    {
        HttpContext.Session.SetInt32("Prize14", 0);
    }
    if(noMore == "Guadeloupe Shark Diving Trip for 2")
    {
        HttpContext.Session.SetInt32("Prize15", 0);
    }
    if(noMore == "A Week at Disney World for 4")
    {
        HttpContext.Session.SetInt32("Prize16", 0);
    }
    if(noMore == "Super Bowl Ticket")
    {
        HttpContext.Session.SetInt32("Prize17", 0);
    }
    if(noMore == "Harley Davidson")
    {
        HttpContext.Session.SetInt32("Prize18", 0);
    }
    if(noMore == "$20k College Scholarship")
    {
        HttpContext.Session.SetInt32("Prize19", 0);
    }
    if(noMore == "Swimming Pool")
    {
        HttpContext.Session.SetInt32("Prize20", 0);
    }
    if(noMore == "Rolex")
    {
        HttpContext.Session.SetInt32("Prize21", 0);
    }
    if(noMore == "RV")
    {
        HttpContext.Session.SetInt32("Prize22", 0);
    }
    if(noMore == "$50k")
    {
        HttpContext.Session.SetInt32("Prize23", 0);
    }
    if(noMore == "$100k")
    {
        HttpContext.Session.SetInt32("Prize24", 0);
    }
    if(noMore == "$500k")
    {
        HttpContext.Session.SetInt32("Prize25", 0);
    }
    if(noMore == "Lamborghini")
    {
        HttpContext.Session.SetInt32("Prize26", 0);
    }
    
    if(minusOneDog == 20 || minusOneDog == 15 || minusOneDog == 11 || minusOneDog == 8 || minusOneDog == 6 || minusOneDog == 5 || minusOneDog == 4 || minusOneDog == 3)
    {
        return RedirectToAction("Offer");
    }
    if(minusOneDog == 2)
    {
        return RedirectToAction("Switch");
    }
    else
    {
        return RedirectToAction("Game");
    }
}


[SessionCheck]
[HttpGet("/tyonty/game")]
public IActionResult Game()
{
    int dogCount = HttpContext.Session.GetInt32("dogsLeft") ?? 0;
    Console.WriteLine(dogCount);
    return View("Game");
}

[SessionCheck]
[HttpGet("/tyonty/offer")]
public IActionResult Offer()
{
    // Get the offer 
    int totalSum = 0;
    int dogsRemaining = 0;
for (var x = 1; x < 27; x++) // This'll get the sum of all aviable prizes
{
    int caseValue = HttpContext.Session.GetInt32($"Prize{x}") ?? 0;
    totalSum += caseValue;

    if(caseValue > 0)
    {
        dogsRemaining += 1;
    }
}

int offerIs = totalSum / dogsRemaining;
double twentyFive = offerIs * 0.25;
int roundResult = (int)Math.Round(twentyFive);
HttpContext.Session.SetInt32("Offer", roundResult);


    return View("Offer");
}

[SessionCheck]
[HttpGet("tyonty/switch")]
public IActionResult Switch()
{
    return View("Switch");
}

[SessionCheck]
[HttpGet("/tyonty/final")]
public IActionResult Final()
{
    return View("Final");
}

[SessionCheck]
[HttpGet]
[Route("tyonty/{swappedDog}/swap")]
public IActionResult Swap(string swappedDog)
{
    HttpContext.Session.SetString("newDog", swappedDog);
    string newPrize = HttpContext.Session.GetString(swappedDog);
    HttpContext.Session.SetString("yourPrize", newPrize);

    return View("Swap");
}

[SessionCheck]
[HttpPost("tyonty/thank-you")]
public IActionResult ThankYou()
{
    return View("ThankYou");
}





    //Goes to invalid page if enter wrong route
    [HttpGet("{**route}")]
    public IActionResult Unknown()
    {
        Console.WriteLine("Invalid route :(");
        return View("404");
    }
}


public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;

        // Check if the required session data exists (you can modify this based on your session setup)
        if (session.GetInt32("UUID") == null || string.IsNullOrEmpty(session.GetString("Name")))
        {
            // Redirect to another action or handle the case when session data is missing
            context.Result = new RedirectToActionResult("Index", "TYONTY", null);
        }

        base.OnActionExecuting(context);
    }
}