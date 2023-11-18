// <copyright file="AddBookSpecs.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace BooksApi.Specs.StepDefinitions
{
	using Books.Api.Models;
	using Books.Api.Repositories;
	using FakeItEasy;
	using Microsoft.AspNetCore.TestHost;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using System.Net;
	using Tests.Lib;

	[Binding]
	public class AddBookSpecs : IClassFixture<CustomWebApplicationFactory>, IDisposable
	{
		private readonly ApiTestClient client;
		private readonly IBookRepository bookRepository;
		private HttpResponseMessage addBookResponse = default!;
		private readonly Book expectedBook = new() { Isbn = "12345" };

		public AddBookSpecs(
			CustomWebApplicationFactory webApplicationFactory)
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

		[Given(@"when the book with the same ISBN does not exist")]
		public void GivenWhenTheBookWithTheSameIsbnDoesNotExist()
		{
			var book = new Book { Isbn = "12345" };
			A.CallTo(() => bookRepository.GetBookAsync(book.Isbn)).Returns(Task.FromResult<Book?>(null));
		}

		[When(@"a valid POST request is made")]
		public async Task WhenAValidPostRequestIsMade() => addBookResponse = await client.PostAsync("books", expectedBook);

		[Then(@"a ""([^""]*)"" status is returned")]
		public void ThenAStatusIsReturned(string p0)
		{
			addBookResponse.Should().NotBeNull();
			addBookResponse.StatusCode.Should().Be(HttpStatusCode.Created);
		}

		[Then(@"the location header will be set to the book's location")]
		public void ThenTheLocationHeaderWillBeSetToTheBooksLocation()
		{
			var locationHeader = addBookResponse.Headers.Location;

			locationHeader.Should().NotBeNull();
			locationHeader!.ToString().Should().EndWith($"books/{expectedBook.Isbn}");
		}

		[Then(@"the book is in the response body")]
		public async Task ThenTheBookIsInTheResponseBody()
		{
			var book = await client.ReadFromJsonAsync<Book>(addBookResponse);
			book.Should().NotBeNull();
			book.Isbn.Should().Be(expectedBook.Isbn);
		}

		public void Dispose() => client.Dispose();
	}
}