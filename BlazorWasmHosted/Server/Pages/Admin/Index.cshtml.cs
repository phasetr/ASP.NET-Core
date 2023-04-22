﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorWasmHosted.Server.Pages.Admin;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        _logger.LogInformation("TEST");
    }
}
