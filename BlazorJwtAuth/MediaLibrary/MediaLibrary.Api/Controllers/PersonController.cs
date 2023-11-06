using MediaLibrary.Api.Services;
using MediaLibrary.Common.Entities;
using MediaLibrary.Common.Models;

namespace MediaLibrary.Api.Controllers;

public class PersonController : BaseController<PersonModel, Person, PersonService>
{
    public PersonController(PersonService service) : base(service, "/persons")
    {
    }
}
