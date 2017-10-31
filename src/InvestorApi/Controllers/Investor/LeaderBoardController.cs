using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Extensions;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Controllers.Investor
{
    /// <summary>
    /// The API controller provides the leader board.
    /// </summary>
    [Route("api/1.0/leaderBoard")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.InvestorsGroup)]
    public class LeaderBoardController : Controller
    {
        private ILeaderBoardService _leaderBoardService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaderBoardController"/> class.
        /// </summary>
        /// <param name="leaderBoardService">Injected instance of <see cref="ILeaderBoardService"/>.</param>
        public LeaderBoardController(ILeaderBoardService leaderBoardService)
        {
            _leaderBoardService = leaderBoardService;
        }

        /// <summary>
        /// Get the leader board users.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to get the current leader board users.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="pageNumber">The page number to return. (Default = 1)</param>
        /// <param name="pageSize">The page size to apply. (Default = 100)</param>
        /// <returns>The action response.</returns>
        [HttpGet]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(ListResult<LeaderBoardUser>))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        public IActionResult GetUsers(
            [FromQuery][Range(1, 1000)]int? pageNumber,
            [FromQuery][Range(1, 100)]int? pageSize)
        {
            var users = _leaderBoardService.GetUsers(Request.GetUserId(), pageNumber ?? 1, pageSize ?? 100);
            return Ok(users);
        }

        /// <summary>
        /// Get the leader board for the current user.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to get the current leader board for the current user including its neighbors.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="neighborCount">The number of neighbors to include on both sides. (Default = 2)</param>
        /// <returns>The action response.</returns>
        [HttpGet("me")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(LeaderBoardUser))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        public IActionResult GetUsers([FromQuery][Range(0, 50)]int? neighborCount)
        {
            var users = _leaderBoardService.GetUsers(Request.GetUserId(), neighborCount ?? 2);
            return Ok(users);
        }
    }
}
