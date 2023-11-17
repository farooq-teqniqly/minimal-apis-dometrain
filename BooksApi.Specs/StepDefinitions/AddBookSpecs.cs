// <copyright file="AddBookSpecs.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace BooksApi.Specs.StepDefinitions
{
	using Books.Api.Models;
	using Books.Api.Repositories;
	using Microsoft.Extensions.DependencyInjection;
	using System.Net;
	using Tests.Lib;

	[Binding]
	public class AddBookSpecs : IClassFixture<CustomWebApplicationFactory>, IDisposable
	{
		private readonly ApiTestClient client;
		private readonly IBookRepository bookRepository;
		private HttpResponseMessage addBookResponse;
		private readonly Book expectedBook = new() { Isbn = "12345" };

		public AddBookSpecs(
			CustomWebApplicationFactory webApplicationFactory)
		{
			client = new ApiTestClient(webApplicationFactory.CreateClient());
			bookRepository = webApplicationFactory.Services.GetRequiredService<IBookRepository>();
		}

		[Given(@"When the book with the same ISBN does not exist")]
		public async Task GivenWhenTheBookWithTheSameIsbnDoesNotExist()
		{
			var book = await this.bookRepository.GetBookAsync("12345");
			book.Should().BeNull();
		}

		[When(@"A valid POST request is made")]
		public async Task WhenAValidPostRequestIsMade() => addBookResponse = await client.PostAsync("books", expectedBook);

		[Then(@"a ""([^""]*)"" status is returned")]
		public void ThenAStatusIsReturned(string p0)
		{
			addBookResponse.Should().NotBeNull();
			addBookResponse.StatusCode.Should().Be(HttpStatusCode.Created);
		}

		public void Dispose() => client.Dispose();
	}
}