using MediaLibrary.Api.Services;
using MediaLibrary.Common.Entities;
using MediaLibrary.Common.Models;

namespace MediaLibrary.Api.Controllers;

public class PersonController(PersonService service)
    : BaseController<PersonModel, Person, PersonService>(service, "/persons");
