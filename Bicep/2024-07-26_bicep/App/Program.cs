using System.CommandLine;
using App;

var command = new RootCommand();

var nameOption = new Option<string>("--name") { IsRequired = true };
var emailOption = new Option<string>("--email");
var stateOption = new Option<string>("--state") { IsRequired = true };
var countryOption = new Option<string>("--country") { IsRequired = true };

command.AddOption(nameOption);
command.AddOption(emailOption);
command.AddOption(stateOption);
command.AddOption(countryOption);

/*
command.SetHandler(
    CosmosHandler.ManageCustomerAsync,
    nameOption,
    emailOption,
    stateOption,
    countryOption
);
await command.InvokeAsync(args);
*/

/*
command.SetHandler(
    CosmosHandler.GetCustomerQueryAsync,
    nameOption,
    emailOption,
    stateOption,
    countryOption
);
await command.InvokeAsync(args);
*/

/*
command.SetHandler(
    CosmosHandler.ReadCustomerAsync,
    nameOption,
    emailOption,
    stateOption,
    countryOption
);
await command.InvokeAsync(args);
*/

command.SetHandler(
    CosmosHandler.CreateCustomerCartAsync,
    nameOption,
    emailOption,
    stateOption,
    countryOption
);
await command.InvokeAsync(args);
