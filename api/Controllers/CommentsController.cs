using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Talking.Api.Models;
using Talking.Domain.Data;
using Talking.Domain.Repository;

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
        public IActionResult Get([FromQuery(Name = "post_url")] string postUrl,
                                 [FromQuery] int? page,
                                 [FromQuery] int limit = 10)
        {
            IList<CommentDto> list;
            long total = 0;

            if (string.IsNullOrEmpty(postUrl))
            {
                list = _commentRepository
                    .GetComments(limit: 10)
                    .Select(i => new CommentDto(i))
                    .ToList();
                total = list.Count;
                return Ok(HttpResponseFactory.CreateOk(detail: new { total, list }));
            }

            total = _commentRepository.GetCount(postUrl);

            var data = !page.HasValue ?
                _commentRepository.GetComments(postUrl) :
                _commentRepository.GetComments(postUrl, page.Value, limit);

            list = data.Select(i => new CommentDto(i)).ToList();
            return Ok(HttpResponseFactory.CreateOk(detail: new { total, list }));
        }

        [HttpPost]
        public IActionResult Post([FromBody] Comment comment)
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            comment.Owner.IpV4 = ip;
            _logger.LogInformation("Comment.Owner: {@Owner}", comment.Owner);
            _commentRepository.InsertComment(comment);
            return Ok(HttpResponseFactory.CreateOk(
                message: "created",
                detail: new CommentDto(comment)));
        }
    }
}
