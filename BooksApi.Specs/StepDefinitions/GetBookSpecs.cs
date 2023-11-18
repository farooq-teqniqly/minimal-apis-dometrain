// <copyright file="GetBookSpecs.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace BooksApi.Specs.StepDefinitions
{
	using Books.Api.Models;
	using Books.Api.Repositories;
	using BooksApi.Tests.Lib;
	using FakeItEasy;
	using Microsoft.AspNetCore.TestHost;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using System.Net;
	using System.Net.Http.Json;

	[Binding]
	public class GetBookSpecs
	{
		private readonly ApiTestClient client;
		private readonly IBookRepository bookRepository;
		private readonly string lookupIsbn = "12345";
		private HttpResponseMessage response = default!;

		public GetBookSpecs(CustomWebApplicationFactory webApplicationFactory)
		{
			var customFactory = webApplicationFactory.WithWebHostBuilder(b =>
				b.ConfigureTestServices(s =>
				{
					s.RemoveAll<IBookRepository>();
					s.AddSingleton(_ => A.Fake<IBookRepository>());
				}));

			client = new ApiTestClient(customFactory.CreateClient());
			bookRepository = customFactory.Services.GetRequiredService<IBookRepository>();
		}

		[Given(@"an existing book")]
		public void GivenAnExistingBook() => A.CallTo(() => bookRepository.GetBookAsync(lookupIsbn)).Returns(new Book() { Isbn = lookupIsbn });

		[When(@"a GET request is made for the book by isbn")]
		public async Task WhenAGETRequestIsMadeForTheBookByIsbn()
		{
			response = await client.GetAsync($"books/{lookupIsbn}");
			response.Should().NotBeNull();
		}

		[Then(@"the a ""([^""]*)"" status is returned")]
		public void ThenTheAStatusIsReturned(string p0) => response.StatusCode.Should().Be(HttpStatusCode.OK);

		[Then(@"the book is returned")]
		public async Task ThenTheBookIsReturned()
		{
			var book = await response.Content.ReadFromJsonAsync<Book>();
			book.Should().NotBeNull();
			book!.Isbn.Should().Be(lookupIsbn);
		}
	}
}