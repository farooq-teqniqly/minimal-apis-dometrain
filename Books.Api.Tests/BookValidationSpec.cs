// <copyright file="BookValidationSpec.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

using TechTalk.SpecFlow;

namespace Books.Api.Tests
{
	using FakeItEasy;
	using FluentAssertions;
	using Microsoft.AspNetCore.TestHost;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using Models;
	using Repositories;
	using System.Net;

	[Binding]
	public class BookValidationSpec : IClassFixture<CustomWebApplicationFactory>, IDisposable
	{
		private readonly ApiTestClient client;
		private readonly IBookRepository bookRepository;
		private Book bookToAdd = default!;
		private HttpResponseMessage addBookResponse = default!;

		public BookValidationSpec(CustomWebApplicationFactory webApplicationFactory)
		{
			var customWebApplicationFactory = webApplicationFactory.WithWebHostBuilder(b =>
			{
				b.ConfigureTestServices(s =>
				{
					s.RemoveAll<IBookRepository>();
					s.AddSingleton(_ => A.Fake<IBookRepository>());
				});
			});

			client = new ApiTestClient(customWebApplicationFactory.CreateClient());
			bookRepository = customWebApplicationFactory.Services.GetRequiredService<IBookRepository>();
		}

		[Given(@"the book request body contains an invalid isbn")]
		public void GivenTheBookRequestBodyContainsAnInvalidIsbn()
		{
			bookToAdd = new Book
			{
				Isbn = "123",
				Title = "A Clockwork Orange",
				Author = "Anthony Burgess",
				PageCount = 240,
				ReleaseDate = new DateOnly(2019, 5, 21)
			};

			A.CallTo(() => bookRepository.AddBookAsync(bookToAdd)).Returns(true);
		}

		[When(@"the post request is made")]
		public async Task WhenThePostRequestIsMade()
		{
			addBookResponse = await client.PostAsync("books", bookToAdd);
			addBookResponse.Should().NotBeNull();
		}

		[Then(@"a (.*) bad request status is returned")]
		public async Task ThenABadRequestStatusIsReturned(int p0)
		{
			addBookResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			var content = await addBookResponse.Content.ReadAsStringAsync();
		}

		public void Dispose() => client.Dispose();
	}
}
