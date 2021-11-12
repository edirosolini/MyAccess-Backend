// <copyright file="Index.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.WebAPP.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            this.logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
