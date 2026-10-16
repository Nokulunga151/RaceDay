using Microsoft.AspNetCore.Http;
using RaceDay.Api.Data;
using RaceDay.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace RaceDay.Api.Tests
{
    public class UnitTest1
    {
        private static RaceDayDBContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDBContext>()
                .UseInMemoryDatabase(
                    databaseName: System.Guid.NewGuid().ToString())
                .Options;

            return new RaceDayDBContext(options);
        }

        private static void SetSession(
            ControllerBase controller,
            int? userId = null,
            string? role = null)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();

            if (userId.HasValue)
                httpContext.Session.SetInt32("UserId", userId.Value);

            if (role != null)
                httpContext.Session.SetString("Role", role);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task Register_MissingEmail_ReturnsBadRequest()
        {
            using var db = CreateContext();
            var controller = new AuthController(db);


            var request = new RegisterRequest
            {
                Email = "",
                Password = "MyPassword@15",
                Role = "Participant"
            };

            var result = await controller.Register(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_InvalidRole_ReturnsBadRequest()
        {
            using var db = CreateContext();
            var controller = new AuthController(db);

            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "Password123!",
                Role = "Admin"
            };

            var result = await controller.Register(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_MissingCredentials_ReturnsBadRequest()
        {
            using var db = CreateContext();
            var controller = new AuthController(db);

            var request = new LoginRequest
            {
                Email = "",
                Password = ""
            };

            var result = await controller.Login(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_UnknownEmail_ReturnsUnauthorized()
        {
            using var db = CreateContext();
            var controller = new AuthController(db);

            var request = new LoginRequest
            {
                Email = "unknown@example.com",
                Password = "Password123!"
            };

            var result = await controller.Login(request);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GetEvents_NoEvents_ReturnsOk()
        {
            using var db = CreateContext();
            var controller = new EventController(db);

            var result = await controller.GetEvents();

            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task GetEvent_EventDoesNotExist_ReturnsNotFound()
        {
            using var db = CreateContext();
            var controller = new EventController(db);

            var result = await controller.GetEvent("missing-event");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateEvent_NoSession_ReturnsUnauthorized()
        {
            using var db = CreateContext();
            var controller = new EventController(db);

            SetSession(controller);

            var request = new CreateEventRequest();

            var result = await controller.CreateEvent(request);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task CreateEvent_ParticipantRole_ReturnsForbid()
        {
            using var db = CreateContext();
            var controller = new EventController(db);

            SetSession(controller, 1, "Participant");

            var request = new CreateEventRequest();

            var result = await controller.CreateEvent(request);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task GetProfile_NoSession_ReturnsUnauthorized()
        {
            using var db = CreateContext();
            var controller = new ProfileController(db);

            SetSession(controller);

            var result = await controller.GetProfile();

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetMyEnrolments_NoSession_ReturnsUnauthorized()
        {
            using var db = CreateContext();
            var controller = new EnrolmentsController(db);

            SetSession(controller);

            var result = await controller.GetMyEnrolments();

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetMyResults_NoSession_ReturnsUnauthorized()
        {
            using var db = CreateContext();
            var controller = new ResultsController(db);

            SetSession(controller);

            var result = await controller.GetMyResults();

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetCategories_NoCategories_ReturnsNotFound()
        {
            using var db = CreateContext();
            var controller = new CategoriesController(db);

            var result = await controller.GetCategories("missing-event");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        private sealed class TestSession : ISession
        {
            private readonly Dictionary<string, byte[]> _data = new();

            public bool IsAvailable => true;
            public string Id => "test-session";
            public IEnumerable<string> Keys => _data.Keys;

            public void Clear() => _data.Clear();

            public Task LoadAsync(
                CancellationToken cancellationToken = default)
                => Task.CompletedTask;

            public Task CommitAsync(
                CancellationToken cancellationToken = default)
                => Task.CompletedTask;

            public void Remove(string key) => _data.Remove(key);

            public void Set(string key, byte[] value)
                => _data[key] = value;

            public bool TryGetValue(string key, out byte[] value)
            {
                if (_data.TryGetValue(key, out var stored))
                {
                    value = stored;
                    return true;
                }

                value = null!;
                return false;
            }
        }

    }
}
