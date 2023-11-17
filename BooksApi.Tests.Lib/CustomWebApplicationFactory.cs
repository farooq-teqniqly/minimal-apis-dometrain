// <copyright file="CustomWebApplicationFactory.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace BooksApi.Tests.Lib
{
	using Books.Api;
	using Books.Api.Repositories;
	using Fakes.Repositories;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc.Testing;
	using Microsoft.AspNetCore.TestHost;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;

	public class CustomWebApplicationFactory : WebApplicationFactory<Program>
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
			=> builder.ConfigureTestServices(s =>
			{
				s.RemoveAll<IBookRepository>();
				s.AddSingleton<IBookRepository, FakeBookRepository>();
			});
	}
}
