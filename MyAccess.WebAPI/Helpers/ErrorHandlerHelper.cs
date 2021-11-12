// <copyright file="ErrorHandlerHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.WebAPI.Helpers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;

    public static class ErrorHandlerHelper
    {
        public static void UseErrorHandlerHelper(this IApplicationBuilder app, IHostEnvironment environment)
        {
            app.UseExceptionHandler(
               options =>
               {
                   options.Run(
                       async context =>
                       {
                           // Try and retrieve the error from the ExceptionHandler middleware
                           var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
                           var ex = exceptionDetails?.Error;

                           // ProblemDetails has it's own content type
                           context.Response.ContentType = "application/problem+json";

                           if (ex != null)
                           {
                               // Get the details to display, depending on whether we want to expose the raw exception
                               var title = !environment.IsDevelopment() ? "An error occured: " + ex.Message : "An error occured";
                               var details = environment.IsDevelopment() ? ex.ToString() : null;

                               var problem = new ProblemDetails
                               {
                                   Status = 500,
                                   Title = title,
                                   Detail = details,
                               };

                               // This is often very handy information for tracing the specific request
                               var traceId = Activity.Current?.Id ?? context?.TraceIdentifier;
                               if (traceId != null)
                               {
                                   problem.Extensions["traceId"] = traceId;
                               }

                               await context.Response.WriteAsJsonAsync(problem).ConfigureAwait(false);
                           }
                       });
               });
        }
    }
}
