using Api.Services;
using Common.Entities;
using Common.Models;

namespace Api.Controllers;

public class PersonController : BaseController<PersonModel, Person, PersonService>
{
    public PersonController(PersonService service) : base(service, "/persons")
    {
    }
}
