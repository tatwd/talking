using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Talking.Api.Data;
using Talking.Api.Models;
using Talking.Api.Repository;

namespace Talking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger _logger;

        public CommentsController(ICommentRepository commentRepository, ILogger<CommentsController> logger)
        {
            _commentRepository = commentRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string post_url,
                                 [FromQuery] int? page,
                                 [FromQuery] int limit = 10)
        {
            try
            {
                IList<CommentDto> list;
                long total = 0;

                if (string.IsNullOrEmpty(post_url))
                {
                    list = _commentRepository
                        .GetComments(limit: 10)
                        .Select(i => new CommentDto(i))
                        .ToList();
                    total = list.Count;
                    return Ok(HttpResponseFactory.CreateOk(detail: new { total, list }));
                }

                total = _commentRepository.GetCount(post_url);

                var data = !page.HasValue ?
                    _commentRepository.GetComments(post_url) :
                    _commentRepository.GetComments(post_url, page.Value, limit);

                list = data.Select(i => new CommentDto(i)).ToList();
                return Ok(HttpResponseFactory.CreateOk(detail: new { total, list }));
            }
            catch (System.Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return BadRequest(HttpResponseFactory.CreateKo(
                    code: 5,
                    message: "exception",
                    detail: ex.Message));
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Comment comment)
        {
            try
            {
                var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(ip))
                    ip = HttpContext.Connection.RemoteIpAddress.ToString();
                comment.Owner.IPv4 = ip;
                _logger.LogInformation("Request Model: {0}", JsonConvert.SerializeObject(comment));
                _commentRepository.InsertComment(comment);
                return Ok(HttpResponseFactory.CreateOk(
                    message: "created",
                    detail: new CommentDto(comment)));
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return BadRequest(HttpResponseFactory.CreateKo(
                    code: 5,
                    message: "exception",
                    detail: ex.Message));
            }
        }
    }
}
